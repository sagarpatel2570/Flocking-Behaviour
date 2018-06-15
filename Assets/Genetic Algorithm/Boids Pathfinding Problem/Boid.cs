using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Boid : MonoBehaviour {

	public float turnSpeed = 5;
	public LayerMask obstacleLayermask;
	public float boidRadius;

	[HideInInspector]
	public DNAboid dnaBoid;


	public float fitness { get; protected set;}
	public bool hasReachedTarget { get; protected set;}


	int geneCounter = 0;
	int lifeTime;
	bool hasHitObstacle;
	float distanceToTarget= 10000;
	public Vector2 targetpos;
	Vector3 startPos;
	int finishedTime = 0;

	public void Initialize (float maxForce,int lifeTime,Vector2 targetPos,Vector2[] genes,Vector3 startpos) {
		
		dnaBoid = new DNAboid (genes, maxForce, lifeTime);
		this.lifeTime = lifeTime;
		this.targetpos = targetpos;

		transform.position = startpos;
		this.startPos = startPos;
		this.distanceToTarget = 10000;
		finishedTime = 0;
	}


	void Update () {
		if(dnaBoid != null){
			if (hasReachedTarget == false && hasHitObstacle == false) {
				RotateAndMove (dnaBoid.genes [geneCounter]);
			}


			distanceToTarget = CalculateDistanceToTarget ();
			if (distanceToTarget < 0.5f) {
				hasReachedTarget = true;
			} else {
				finishedTime++;
				hasReachedTarget = false;
			}

			if (CheckForCollisions ()) {
				hasHitObstacle = true;
			} else {
				hasHitObstacle = false;
			}


			geneCounter++;

			if (geneCounter == lifeTime) {
				//Generate new Boids..
				geneCounter = 0;
				hasReachedTarget = false;

			}

		}
	}

	public void CalculateFitness () {
		
		fitness = 1 / (  (distanceToTarget)) ;

		fitness = Mathf.Pow (fitness, 2);


		if (CheckForCollisions()) {
			fitness *= 0.1f;

		}

		if (hasReachedTarget) {
			fitness *= 2;

		}



	}

	float CalculateDistanceToTarget () {

		return (this.targetpos - (Vector2)transform.position).magnitude;
	} 

	RaycastHit2D CheckForCollisions () {
		return Physics2D.CircleCast (transform.position, boidRadius, transform.right, 0.2f, obstacleLayermask);
	}

	void RotateAndMove (Vector2 targetDir){
		
		float targetAngle = Mathf.Atan2(targetDir.y ,(targetDir.x + 0.0001f)) * Mathf.Rad2Deg;

		Quaternion finalRot = Quaternion.AngleAxis(targetAngle, Vector3.forward);

		transform.rotation = Quaternion.Slerp (transform.rotation , finalRot,turnSpeed  * Time.deltaTime);

		transform.Translate ((Vector3.right ) * targetDir.magnitude * Time.deltaTime,Space.Self);
	}
}
