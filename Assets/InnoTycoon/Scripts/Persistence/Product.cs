using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product {

	public string name;

	public bool isReady;

	public List<int> optionIDs;

	public float rentability;

	public int rating;

	public int curStep;

	public Product() { }
}
