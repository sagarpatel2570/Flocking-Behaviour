using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public GameObject enemyPrefab;
	public EnemyData enemyData;
	public int noOfEnemy;
	public int noOfSimulationPerFrame;
	public bool useMouseToSpawn;

	[HideInInspector]
	public List<Enemy> enemys;
	Map map;
	int currentNoOfSimulation;
	int enemyNo;

	void Start () {
		map = GameObject.FindObjectOfType<Map> ();

		enemys = new List<Enemy> ();
		StartCoroutine(SpawnEnemy());
	}

	/// <summary>
	/// Spawns the enemy. frame by frame in random direction
	/// </summary>
	/// <returns>The enemy.</returns>
	IEnumerator SpawnEnemy () {
		for (int i = 0; i < noOfEnemy; i++) {
			SpawnEnemy (new Vector3 (Random.Range (-map.size.x/2,map.size.x/2 ), Random.Range (-map.size.y/2, map.size.y/2), 0));
			yield return null;
		}
	}

	void Update () {

		Vector3 targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		targetPos.z = 0;

		if (useMouseToSpawn) {
			if (Input.GetMouseButton (0)) {
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				pos.z = 0;
				SpawnEnemy (pos);
			}
		} 

		currentNoOfSimulation = 0;
		for (int i = 0; i < enemys.Count; i++) {
			currentNoOfSimulation++;
			if(currentNoOfSimulation > noOfSimulationPerFrame){
				break;
			}
			enemyNo++;
			if (enemyNo >= enemys.Count - 1) {
				enemyNo = 0;
			}
			enemys [enemyNo].Update (targetPos);
		}

		for (int i = 0; i < enemys.Count; i++) {
			enemys [i].Move ();
		}

	}

	void SpawnEnemy (Vector3 pos) {

		GameObject enemyGo = Instantiate (enemyPrefab, pos, Quaternion.identity);
		enemyGo.transform.eulerAngles = Vector3.forward * Random.Range(0,360);
		enemyGo.transform.parent = this.transform;

		Enemy enemy = new Enemy (enemyGo.transform, enemyData, map.size, map,this);
		enemys.Add (enemy);
	}

}
