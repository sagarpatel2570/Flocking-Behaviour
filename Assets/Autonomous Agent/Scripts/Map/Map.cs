using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {


	public Vector2 size;
	public float criticalDstToSeekNextTarget;
	public bool isCyclic;

	public Path path { get; protected set;}
	ShapeCreator pathCreator;

	public FlowMap flowMap { get; protected set;}

	void Start () {
		flowMap = GameObject.FindObjectOfType<FlowMap> ();
		pathCreator = GameObject.FindObjectOfType<ShapeCreator> ();

		if (pathCreator != null) {
			path = new Path (pathCreator.points.ToArray (), criticalDstToSeekNextTarget,isCyclic);
		}	
	}



	void OnDrawGizmos () {

		if (path != null) {
			path.DrawGizmos ();
		}

		Gizmos.color = Color.white;
		Gizmos.DrawLine (new Vector3(-size.x/2,-size.y/2,0),new Vector3(size.x/2,-size.y/2,0));
		Gizmos.DrawLine (new Vector3(size.x/2,-size.y/2,0),new Vector3(size.x/2,size.y/2,0));
		Gizmos.DrawLine (new Vector3(size.x/2,size.y/2,0),new Vector3(-size.x/2,size.y/2,0));
		Gizmos.DrawLine (new Vector3(-size.x/2,size.y/2,0),new Vector3(-size.x/2,-size.y/2,0));


	}


}
