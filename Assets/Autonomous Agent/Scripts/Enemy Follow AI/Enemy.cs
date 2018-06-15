using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy {

	Transform transform;
	EnemyData enemyData;
	Vector2 size;
	Map map;

	Path path;
	int lineIndex;
	bool previousSide;
	bool currentSide;
	Vector2 perpendicularPointOnLine;

	float currentSpeed;
	bool stop;

	bool isTurning;
	float currentTurnTime;


	List<Transform> enemyNeighbourList;
	EnemyController enemysBehaviour;

	public Enemy (Transform enemyTransform,EnemyData enemyData,Vector2 mapSize,Map map,EnemyController enemys) {
		this.transform = enemyTransform;
		this.enemyData = enemyData;
		this.size = mapSize;
		this.map = map;
		enemyNeighbourList = new List<Transform> ();
		this.enemysBehaviour = enemys;

		if (map != null && map.path != null) {
			path = map.path;
			currentSide = path.lines [lineIndex].GetSide (transform.position);
			previousSide = currentSide;

			SetLineIndexAccordingToPath ();
		}
	}

	public void Update (Vector3 targetPos) {
		if (!stop) {

			enemyNeighbourList.Clear ();

			for (int i = 0; i < enemysBehaviour.enemys.Count; i++) {

				if (enemyNeighbourList.Count > enemyData.maxNumberOfNeighbour) {
					break;
				}

				if ((transform.position - enemysBehaviour.enemys [i].transform.position).sqrMagnitude <= enemyData.enemyPersonalVisionRadius) {

					if (Vector3.Angle (transform.right, transform.position - enemysBehaviour.enemys [i].transform.position) <= enemyData.fov) {

						enemyNeighbourList.Add (enemysBehaviour.enemys [i].transform);
					}
				}

			}

			if (map != null && path != null) {
				targetPos = FollowPath ();
			}

			/// according to the priority of each feature's we sum up all the direction and move towards the final normalize direction 
			Vector2 targetDir = Seek (targetPos) * enemyData.seekPriority +
				FlowMapVelocity () * enemyData.flowMapPriority +
				Allign () * enemyData.allignPriority +
				Separate () * enemyData.separatePriority +
				Cohesion () * enemyData.cohesionPriority +
				Avoidance () * enemyData.avoidanceProperty;

			if (targetDir != Vector2.zero) {
				Rotate (targetDir);
			}
		}

		if (path == null) {
			if ((transform.position - targetPos).sqrMagnitude <= enemyData.stoppingDst * enemyData.stoppingDst) {
				Arrive (targetPos);
			} else {
				currentSpeed = Mathf.Lerp (currentSpeed, enemyData.maxSpeed, Time.deltaTime * enemyData.acceleration);
				stop = false;
			}
		} else {
			if (!isTurning) {
				currentSpeed = Mathf.Lerp (currentSpeed, enemyData.maxSpeed, Time.deltaTime * enemyData.acceleration);
			}
		}

		if (isTurning) {
			HandleTurning ();
		}

	}

	/// <summary>
	/// find all the enemey which are in its neighbour list
	/// and find the midpoint if all the neighbours and move toward that point
	/// </summary>
	Vector2 Cohesion () {
		Vector3 cohesionPos = Vector3.zero;
		for (int i = 0; i < enemyNeighbourList.Count; i++) {
			cohesionPos += enemyNeighbourList[i].position;
		}
		cohesionPos /= enemyNeighbourList.Count;
		return (cohesionPos -transform.position).normalized;
	}

	/// <summary>
	/// find all the enemey which are in its neighbour list
	/// and move them in opposute direction
	/// </summary>
	Vector2 Separate () {
		Vector3 separateDir = Vector2.zero;
		for (int i = 0; i < enemyNeighbourList.Count; i++) {
			
			separateDir += (transform.position - enemyNeighbourList[i].position);
		}
		return separateDir.normalized;
	}

	/// <summary>
	/// find all the enemey which are in its neighbour list
	/// and move them in its direction
	/// </summary>
	Vector2 Allign () {

		Vector3 allignDir = Vector2.zero;
		for (int i = 0; i < enemyNeighbourList.Count; i++) {
			
			allignDir += enemyNeighbourList[i].right;
		}
		return allignDir.normalized;

	}
	/// <summary>
	/// find all the enemey which are at it;s avoidance radius 
	/// and sum them up and take the normalize directed and move opposite
	/// </summary>
	Vector2 Avoidance (){
		Vector3 avoidanceDir = Vector2.zero;
		Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position,enemyData.avoidanceRadius,enemyData.obstacleLayermask);

		for (int i = 0; i <obstacles.Length; i++) {
			avoidanceDir += (transform.position - obstacles[i].transform.position);
		}
		return avoidanceDir.normalized;
	}

	Vector2 Seek (Vector2 targetPos) {
		return (targetPos - (Vector2)transform.position).normalized;
	}

	void Arrive (Vector2 targetPos) {

		Vector2 targetDir = (targetPos - (Vector2)transform.position);
		float targetAngle = Mathf.Atan2 (targetDir.y, (targetDir.x + 0.0001f)) * Mathf.Rad2Deg;

		Quaternion finalRot = Quaternion.AngleAxis (targetAngle, Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, finalRot, enemyData.turnSpeed * Time.deltaTime);

		float distance = (targetPos - (Vector2)transform.position).magnitude;

		currentSpeed = Mathf.Lerp (0, currentSpeed, (distance - enemyData.stoppingDst)/(enemyData.enemyTargetVisionRadius  - enemyData.stoppingDst));
		currentSpeed = Mathf.Clamp (currentSpeed,0, enemyData.maxSpeed);

		if (currentSpeed <= 0) {
			stop = true;
		}

	}

	void Rotate (Vector2 targetDir){

		float targetAngle = Mathf.Atan2 (targetDir.y, (targetDir.x + 0.01f)) * Mathf.Rad2Deg;

		Quaternion finalRot = Quaternion.AngleAxis (targetAngle , Vector3.forward);
		transform.rotation = Quaternion.Lerp (transform.rotation,finalRot, enemyData.turnSpeed * Time.deltaTime);
	}

	public void Move (){

		if (stop) {
			return;
		}
		transform.Translate ((Vector3.right) * currentSpeed * Time.deltaTime, Space.Self);
		ClampPos ();
	}

	void ClampPos () {
		float posInX = transform.position.x;
		if (posInX < -size.x/2) {
			posInX = size.x/2;
		}

		if (posInX > size.x/2) {
			posInX = -size.x/2;
		}

		float posInY = transform.position.y;
		if (posInY < -size.y/2) {
			posInY = size.y/2;
		}

		if (posInY > size.y/2) {
			posInY = -size.y/2;
		}

		transform.position = new Vector3 (posInX,posInY, 0);
	}

	Vector2 FlowMapVelocity () {
		if (map.flowMap == null) {
			return Vector2.zero;
		}
		return map.flowMap.getVelocity (transform.position);
	}
		
	/// <summary>
	/// we have created a perpendiculate line at the certain distance from endpoint 
	/// so if the player crosses that point it will seek the endpoint of next line 
	/// this way we get smooth turn movement 
	/// </summary>
	/// <returns>The path.</returns>
	Vector2 FollowPath () {

		// find the  shortest perpendicular to the line ..
		Line line = path.lines[lineIndex];
		currentSide = line.GetSide (transform.position);

		if (currentSide != previousSide) {
			isTurning = true;
			lineIndex++;

			if (lineIndex >= path.lines.Length) {
				if (!enemyData.loop) {
					lineIndex = path.lines.Length - 1;
				}else {

					lineIndex = (lineIndex ) % path.lines.Length;
					currentSide = line.GetSide (transform.position);
					previousSide = currentSide;
				}
			}

			line = path.lines [lineIndex];
			perpendicularPointOnLine = line.starPoint;

			previousSide = currentSide = line.GetSide (transform.position);
		}

		previousSide = currentSide;
		perpendicularPointOnLine = line.endPoint;

		return (perpendicularPointOnLine);

	}

	void HandleTurning () {

		currentTurnTime += Time.deltaTime;
		currentSpeed = Mathf.Lerp (currentSpeed, enemyData.minTurnSpeed, Time.deltaTime * enemyData.turnAccleration);
		if (currentTurnTime >= enemyData.turnTime) {
			currentTurnTime = 0;
			isTurning = false;
		}
	}


	/// <summary>
	/// Sets the line index according to path.
	/// we find the shortest distance which is perpenicular to a line and store the index of that line
	/// </summary>
	void SetLineIndexAccordingToPath () {
		float dst = float.MaxValue;
		for (int i = 0; i < path.lines.Length; i++) {
			float distance = path.lines [i].DistanceFromPointToLine (transform.position);
			if (distance < dst  ) {
				dst = distance;
				lineIndex = i;
			}
		}
	}
}
