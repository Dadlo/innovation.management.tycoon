using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe cuja ideia seria cuidar de eventos importantes do jogo e fazer a ligacao com o persistenceActivator
/// </summary>
public class GameManager : MonoBehaviour {

	public static int baseDailyCost = 2000;

	public static GameManager instance;

	public List<Product> productsDoneToday = new List<Product>();

	void Awake() {
		instance = this;
	}

	public void GoToNextDay() {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
		persInstanceSave.day++;
		// Take cost from capital
		persInstanceSave.capital -= persInstanceSave.cost;

		//update products/studies... everyone goes to next step
		for(int i = 0; i < persInstanceSave.productsDoing.Count; i++) {
			persInstanceSave.productsDoing[i].OneStep();
		}

		//products marked as "done" are removed from productsDoing
		for(int i = 0; i < productsDoneToday.Count; i++) {
			persInstanceSave.productsDoing.Remove(productsDoneToday[i]);
		}

		productsDoneToday.Clear();


		RecalculateDailyCost();
	}



	public void AddNewPlayerProduct(Product newProduct) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		if (persInstanceSave.productsDoing == null) persInstanceSave.productsDoing = new List<Product>();
		persInstanceSave.productsDoing.Add(newProduct);

		if (persInstanceSave.productsList == null) persInstanceSave.productsList = new List<Product>();
		persInstanceSave.productsList.Add(newProduct);

		RecalculateDailyCost();
	}

	/// <summary>
	/// esse metodo e chamado quando um produto encerra sua fase de vendas.
	/// esse produto "morreu"
	/// </summary>
	/// <param name="doneProduct"></param>
	public void ProductIsDone(Product doneProduct) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		//nao podemos remover assim que descobrimos que o produto se encerrou para nao dar problemas com o for que itera sobre os productsDoing
		productsDoneToday.Add(doneProduct);

	}

	public void RecalculateDailyCost() {

	}
}
