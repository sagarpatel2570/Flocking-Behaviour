using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour {

	public float speed = 1;
	Map map;
	Vector2 size;
	Vector3 randomDir;
	void Start () {
		map = GameObject.FindObjectOfType<Map> ();
		size = map.size;

		StartCoroutine (SetRandomDir ());
	}

	IEnumerator SetRandomDir () {
		while (true) {
			randomDir = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0);
			yield return new WaitForSeconds (5);
		}
	}
	
	void Update () {
		transform.position += randomDir * Time.deltaTime * speed;
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
}
