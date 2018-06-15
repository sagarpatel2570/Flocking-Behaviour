using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		Vector2 targetPos = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (targetPos.x, targetPos.y, 0);
	}
}
