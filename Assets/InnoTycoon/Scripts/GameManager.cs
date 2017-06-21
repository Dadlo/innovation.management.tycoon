using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe cuja ideia seria cuidar de eventos importantes do jogo e fazer a ligacao com o persistenceActivator
/// </summary>
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	void Awake() {
		instance = this;
	}

	public void GoToNextDay() {

	}

	public void AddNewPlayerProduct(Product newProduct) {
		if (PersistenceActivator.instance.productsDoing == null) PersistenceActivator.instance.productsDoing = new List<Product>();
		PersistenceActivator.instance.productsDoing.Add(newProduct);

		if (PersistenceActivator.instance.productsList == null) PersistenceActivator.instance.productsList = new List<Product>();
		PersistenceActivator.instance.productsList.Add(newProduct);
	}
}
