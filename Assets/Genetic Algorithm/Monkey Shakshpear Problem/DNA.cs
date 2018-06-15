using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA  {

	public char[] currentTarget;
	public float fitness;

	string chars = "abcdefghijklmnopqrstuvwxyz ";

	public DNA (int targetLength) {
		char[] c = new char[targetLength];
		for (int i = 0; i < targetLength; i++) {
			c [i] = chars[Random.Range (0, chars.Length)];
		}
		currentTarget = c;
	}

	public void CalculateFitness (string target) {
		float count = 0;
		for (int i = 0; i < currentTarget.Length; i++) {
			if (currentTarget [i] == target [i]) {
				count++;
			}
		}

		this.fitness = count / target.Length;

	}

	public DNA CrossOver (DNA target) {
		DNA child = new DNA (target.currentTarget.Length);
		int midPoint = Random.Range (0, target.currentTarget.Length);
		for (int i = 0; i < target.currentTarget.Length; i++) {
			if (i > midPoint) {
				child.currentTarget [i] = this.currentTarget [i];
			} else {
				child.currentTarget [i] = target.currentTarget[i];
			}
		}
		return child;
	}

	public void Mutate (float mutationRate) {
		char[] c = new char[currentTarget.Length];
		for (int i = 0; i < currentTarget.Length ; i++) {
			if (Random.value < mutationRate) {
				c [i] = chars [Random.Range (0, chars.Length)];
			} else {
				c [i] = currentTarget [i];
			}
		}
		currentTarget = c;
	}
}
