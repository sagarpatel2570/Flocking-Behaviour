using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAboid {

	public Vector2[] genes;
	public float maxForce;
	public int lifeTime;

	public DNAboid (Vector2[] genes,float maxForce,int lifeTime){
		if (genes != null) {
			this.genes = genes;
		} else {
			this.maxForce = maxForce;
			this.lifeTime = lifeTime;
			this.genes = new Vector2[lifeTime];

			for (int i = 0; i < lifeTime; i++) {
				float angle = Mathf.Deg2Rad * Random.Range (0, 360);
				float magnitude = Random.Range (0, maxForce);
				this.genes [i] = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * magnitude;
			}

			this.genes [0] = this.genes [0].normalized;

		}
	}

	public DNAboid CrossOver (DNAboid patner) {
		Vector2[] childGenes = new Vector2[patner.genes.Length];
		int randomMidPoint = Random.Range (0, childGenes.Length);
		for (int i = 0; i < childGenes.Length; i++) {
			if (i > randomMidPoint) {
				childGenes [i] = this.genes [i];
			} else {
				childGenes [i] = patner.genes [i];
			}
		}
		DNAboid newChild = new DNAboid (childGenes, this.maxForce, this.lifeTime);
		return newChild;
	}

	public void Mutate (float mutateRate){
		for (int i = 0; i < genes.Length; i++) {
			if (Random.value < mutateRate) {
				float angle = Mathf.Deg2Rad * Random.Range (0, 360);
				float magnitude = Random.Range (0, maxForce);
				genes [i] = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)) * magnitude;
			}
		}
	}
}
