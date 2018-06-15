using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population  {

	public DNA[] popullations;
	public string target;
	public float mutationRate;

	public int generation;
	public float bestScore;
	public float averageFitness;
	public string currestBestTarget;
	public bool isFinished;

	List<DNA> mattingPool;

	public Population (string target,int noOfPopulations,float mutationRate) {
		this.target = target;
		popullations = new DNA[noOfPopulations];
		this.mutationRate = mutationRate;

		for (int i = 0; i < noOfPopulations; i++) {
			popullations [i] = new DNA (target.Length);
		}

		isFinished = false;
		bestScore = 1;
		generation = 0;
		averageFitness = 0;
		currestBestTarget = "";


	}

	public void CalculateFitness () {
		for (int i = 0; i < popullations.Length; i++) {
			popullations [i].CalculateFitness (target);
		}
	}

	// Generating a mating Pool ..
	public void NaturalSelection () {
		
		mattingPool = new List<DNA> ();

		for (int i = 0; i < popullations.Length; i++) {
			float fitness = popullations [i].fitness;
			int n = Mathf.FloorToInt (fitness * 100f);
			for (int j = 0; j < n; j++) {
				mattingPool.Add (popullations [i]);
			}

		}

	}

	// Creating a new Generation
	public void Generate () {
		
		for (int i = 0; i < popullations.Length; i++) {
			DNA randomChild_1 = mattingPool [Random.Range (0, mattingPool.Count)];
			DNA randomChild_2 = mattingPool [Random.Range (0, mattingPool.Count)];
			DNA newChild = randomChild_1.CrossOver (randomChild_2);
			newChild.Mutate (mutationRate);
			this.popullations [i] = newChild;

		}
		CalculateFitness ();
		generation++;
	}

	public void Evaluate () {
		int index = 0;
		float maxValue = float.MinValue;
	
		for (int i = 0; i < popullations.Length; i++) {
			if (popullations [i].fitness > maxValue) {
				maxValue = popullations [i].fitness;
				index = i;
			}

		}

		if (maxValue == bestScore) {
			isFinished = true;
		}

		currestBestTarget = new string (popullations [index].currentTarget);

		averageFitness = 0;

		for (int i = 0; i < popullations.Length; i++) {
			averageFitness += popullations [i].fitness;	
		}

		averageFitness /= (float)popullations.Length;
		averageFitness *= 100;

	}
}
