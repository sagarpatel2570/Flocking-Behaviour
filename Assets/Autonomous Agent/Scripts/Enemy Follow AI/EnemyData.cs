using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy data contains the infomation of enemy
/// </summary>
[CreateAssetMenu]
public class EnemyData : ScriptableObject {

	public float maxSpeed;
	public float turnSpeed;
	public int stoppingDst;
	public float acceleration;
	public float enemyPersonalVisionRadius;
	public float enemyTargetVisionRadius;
	public float viewAngle;
	public LayerMask obstacleLayermask;
	public bool loop;
	public int maxNumberOfNeighbour;
	public float fov;
	public float turnAccleration;
	public float minTurnSpeed;
	public float turnTime;

	[Range(0,10)]
	public float allignPriority;

	[Range(0,10)]
	public float seekPriority;

	[Range(0,10)]
	public float separatePriority;

	[Range(0,10)]
	public float cohesionPriority;

	public float avoidanceRadius;
	[Range(0,100)]
	public float avoidanceProperty;

	[Range(0,10)]
	public float flowMapPriority;
}
