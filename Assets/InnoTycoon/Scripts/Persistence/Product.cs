using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product {

	public string name;

	public bool isReady;

	public List<string> optionIDs;

	public float rentability;

	public int rating;

	public int curStep;

	public enum ProductPhase {
		concept,
		dev,
		sales,
		done
	}

	public ProductPhase currentPhase = ProductPhase.concept;

	public int conceptSteps, devSteps, saleSteps;

	public Product() {}

	/// <summary>
	/// incrementa o curStep desse produto e altera sua fase caso tenha chegado ao limite de passos da fase atual
	/// </summary>
	public void OneStep() {
		curStep++;

		switch (currentPhase) {
			case ProductPhase.concept:
				if(curStep >= conceptSteps) {
					currentPhase = ProductPhase.dev;
					curStep = 0;
				}
				break;
			case ProductPhase.dev:
				if (curStep >= devSteps) {
					currentPhase = ProductPhase.sales;
					curStep = 0;
				}
				break;
			case ProductPhase.sales:
				if(curStep >= saleSteps) {
					currentPhase = ProductPhase.done;
					curStep = -1;
					//esse produto esta encerrado entao
					GameManager.instance.ProductIsDone(this);
				}
				break;
			default:
				Debug.LogWarning(string.Concat("Product ", name, ", already completed, is trying to be updated as if it hadnt been completed"));
				break;
		}
	}
}
