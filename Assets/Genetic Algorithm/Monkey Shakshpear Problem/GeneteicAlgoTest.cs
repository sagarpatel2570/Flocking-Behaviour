using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneteicAlgoTest : MonoBehaviour {

	public string target;
	public int noOfPopulation;
	public float mutationRate;

	public Text targetText;
	public Text generationText;
	public Text averageFitnesstext;
	public Text mutationText;


	Population popullations;
	void Start () {

		popullations = new Population (target, noOfPopulation, mutationRate);

	}

	void Update () {
		if (popullations.isFinished == false) {
			popullations.CalculateFitness ();
			popullations.NaturalSelection ();
			popullations.Generate ();

			popullations.Evaluate ();
		}

		targetText.text = popullations.currestBestTarget;
		generationText.text = "Generation : " + popullations.generation.ToString ();
		averageFitnesstext.text = "Average Fitness : " + popullations.averageFitness.ToString ();
		mutationText.text = "Mutation Rate : " + mutationRate.ToString ();
	}
}
