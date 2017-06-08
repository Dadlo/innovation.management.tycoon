using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductCreationPanel : MonoBehaviour {

	public struct ProductConcepts {
		public List<Concept> productConcepts;
	}

	public ProductConcepts pConcepts;

	public TextAsset productConceptsAsset;

	// Use this for initialization
	void Start () {

		pConcepts = PersistenceHandler.LoadFromFile<ProductConcepts>(productConceptsAsset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
