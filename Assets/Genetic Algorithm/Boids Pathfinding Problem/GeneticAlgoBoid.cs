using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgoBoid : MonoBehaviour {

	public Transform target;
	public Transform start;
	public float maxForce;
	public int lifeSpan;
	public int noOfPopullations;
	public float mutationrate;
	public GameObject boidPrefab;


	public Text populationText;
	public Text generationText;
	public Text lifeSpantext;
	public Text isFinishedByOneText;
	public Text hasEveryOneFinishedText;
	public Text averageFitnesstext;


	PopullationBoid popullations;
	Boid[] boids;
	int currentCount;

	void Start () {
		boids = new Boid[noOfPopullations];
		for (int i = 0; i < noOfPopullations; i++) {
			GameObject g = Instantiate (boidPrefab, start.position, Quaternion.identity);
			boids [i] = g.GetComponent<Boid> ();
		}
		popullations = new PopullationBoid (boids, mutationrate,(Vector2) target.position, lifeSpan, maxForce, start.position);
	}

	void Update () {
		if (popullations.finishedByEveyone == false) {
			if (currentCount != lifeSpan) {
				currentCount++;
			} else {
				//generate new generation..
				currentCount = 0;
				popullations.CalculateFitness ();
				popullations.NaturalSelection ();
				popullations.Generate ();
				popullations.Evaluate ();

			}
		}


		populationText.text = "Total Boids is : " + noOfPopullations.ToString ();
		generationText.text = "No Of Generation : " + popullations.generation.ToString ();
		lifeSpantext.text = "Life Span : " + (lifeSpan - currentCount).ToString ();
		if(popullations.isFinishedByOne){
			isFinishedByOneText.text = "One of the boid finsihed the target";
		}

		if (popullations.finishedByEveyone) {
			hasEveryOneFinishedText.text = "Every boids finished the target";
		}

		averageFitnesstext.text = "Average Fitness : " + popullations.averageFitness.ToString ();
	}
}
