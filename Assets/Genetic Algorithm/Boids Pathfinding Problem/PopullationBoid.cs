using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopullationBoid  {

	Vector3 startPos;
	float mutationRate;
	Vector2 targetPos;
	int lifeSpan;
	Boid[] boids;
	public int generation;
	List<Boid> mattingPool;
	float maxForce;


	public bool isFinishedByOne;
	public bool finishedByEveyone;
	public float averageFitness;


	public PopullationBoid (Boid[] boids,float mutationrate,Vector2 targetPos,int lifeSpan,float maxForce,Vector3 startPos) {
		this.mutationRate = mutationRate;
		this.targetPos = targetPos;
		this.lifeSpan = lifeSpan;
		this.boids = boids;
		this.startPos = startPos;
		this.maxForce = maxForce;

		for (int i = 0; i < boids.Length; i++) {
			
			boids [i].Initialize (maxForce, lifeSpan, this.targetPos,null,startPos);
			(boids [i].targetpos) = this.targetPos;
		}
			
	}

	public void CalculateFitness () {
		for (int i = 0; i < boids.Length; i++) {
			boids [i].CalculateFitness ();
		}
	}

	// Generating a mating Pool ..
	public void NaturalSelection () {

		float maxFitness = float.MinValue;
		//calculate maximumfitness
		for (int i = 0; i < boids.Length; i++) {
			if (boids [i].fitness > maxFitness) {
				maxFitness = boids [i].fitness;

			}
		}

		mattingPool = new List<Boid> ();
		;
		for (int i = 0; i < boids.Length; i++) {
			int n = Mathf.FloorToInt ((boids [i].fitness / maxFitness) * 100);



			for (int j = 0; j < n; j++) {
				mattingPool.Add (boids [i]);
			}
		}


	}

	// Creating a new Generation
	public void Generate () {
		
		for (int i = 0; i < boids.Length; i++) {
			Boid randomBoid_1 = mattingPool [Random.Range (0, mattingPool.Count)];
			Boid randomBoid_2 = mattingPool [Random.Range (0, mattingPool.Count)];
			DNAboid newChild = randomBoid_1.dnaBoid.CrossOver (randomBoid_2.dnaBoid);
			newChild.Mutate (mutationRate);
			boids [i].dnaBoid = null;
			boids [i].dnaBoid = newChild;
			boids [i].Initialize (maxForce, lifeSpan, targetPos, boids [i].dnaBoid.genes, startPos);
		}

		generation++;
	}

	public void Evaluate () {
		int noOfFinished = 0;
		for (int i = 0; i < boids.Length; i++) {
			if (boids [i].hasReachedTarget) {
				if (!isFinishedByOne) {
					isFinishedByOne = true;
				}
				noOfFinished++;
			}
		}

		if (boids.Length == noOfFinished) {
			finishedByEveyone = true;
		}
		averageFitness = 0;
		for (int i = 0; i < boids.Length; i++) {
			averageFitness += boids [i].fitness;
		}

	}
}
