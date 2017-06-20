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
		sales
	}

	public ProductPhase currentPhase = ProductPhase.concept;

	public int conceptSteps, devSteps, saleSteps;

	public Product() {}
}
