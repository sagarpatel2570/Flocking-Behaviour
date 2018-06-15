using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowMap : MonoBehaviour {

	public Vector2 size;
	public float gridSize;
	public float scale;
	public float speed = 5;
	public bool UpdateFlowmaap = true;

	Vector2 offset;

	Node[,] map;

	void Start () {
		map = new Node[(int)size.x, (int)size.y];
		GenerateMap ();
	}


	/// <summary>
	/// Every update we change the direction with the help of perlin noise 
	/// we use perlin noise so that it's neighbour also get affected with the same direction with some difference in their angle
	/// </summary>
	void Update () {
		if (!UpdateFlowmaap) {
			return;
		}
		offset.x += Time.deltaTime * speed;
		offset.y += Time.deltaTime * speed;
		for (int y = 0; y < (int)size.y; y++) {
			for (int x = 0; x < (int)size.x; x++) {
				float randomAngle =  (Mathf.PerlinNoise ((map[x,y].pos.x + offset.x)/scale, (map[x,y].pos.y + offset.y)/scale) * 2 - 1) * 180f ;
				Vector3 dir = new Vector3 (Mathf.Cos (randomAngle * Mathf.Deg2Rad), Mathf.Sin (randomAngle * Mathf.Deg2Rad), 0);
				map [x, y].dir  = dir;
			}
		}
	}

	/// <summary>
	/// Generates the map in such a way that each point has some direction 
	/// if an enemy visit this point then it will be moved in this direction with its velocity
	/// We used flow map
	/// </summary>
	void GenerateMap () {
		for (int y = 0; y < (int)size.y; y++) {
			for (int x = 0; x < (int)size.x; x++) {
				Vector3 position = Vector3.right * (-size.x * gridSize + gridSize)/2 +  Vector3.right * x * gridSize + Vector3.up *(-size.y * gridSize  + gridSize)/2 + Vector3.up * y * gridSize;
				float randomAngle =  (Mathf.PerlinNoise ((position.x + offset.x)/scale, (position.y + offset.y)/scale) * 2 - 1) * 180f ;
				Vector3 dir = new Vector3 (Mathf.Cos (randomAngle * Mathf.Deg2Rad), Mathf.Sin (randomAngle * Mathf.Deg2Rad), 0);
				map [x, y] = new Node (x, y, position,dir);
			}
		}
	}

	/// <summary>
	/// Gets the velocity direction which we stored in the map while its generation
	/// </summary>
	/// <returns>The velocity.</returns>
	/// <param name="position">Position.</param>
	public Vector3 getVelocity(Vector3 position){
		int x = Mathf.FloorToInt ((position.x + ((size.x  * gridSize + gridSize)/2) ) / gridSize);
		int y = Mathf.FloorToInt ((position.y + ((size.y * gridSize + gridSize)/2) ) / gridSize);

		if (x >= 0 && x < (int)size.x && y >= 0 && y < (int)size.y) {
			return map [x, y].dir;
		} else {
			return Vector3.zero;
		}
	}

	public struct Node {
		public int x;
		public int y;
		public Vector3 pos;
		public Vector3 dir;

		public Node (int x,int y,Vector3 pos,Vector3 dir) {
			this.x = x;
			this.y = y;
			this.pos = pos;
			this.dir = dir;
		}
	}

	void OnDrawGizmos () {
		if (map != null) {
			for (int y = 0; y < (int)size.y; y++) {
				for (int x = 0; x < (int)size.x; x++) {
					Gizmos.DrawSphere (map [x, y].pos, 0.1f);
					Debug.DrawRay (map [x, y].pos, map [x, y].dir * 0.5f , new Color(1,0,0,0.3f));
				}
			}
		}
	}
}
