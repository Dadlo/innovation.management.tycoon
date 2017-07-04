using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductsPanel : ShowablePanel {

	public Transform productsListParent;

	public GameObject prodListEntryPrefab;

	/// <summary>
	/// cria um prefab de entrada da lista com os dados do targetProduct
	/// </summary>
	/// <param name="targetProduct"></param>
	public void AddProductToList(Product targetProduct) {
		GameObject newEntry = Instantiate(prodListEntryPrefab);
		newEntry.transform.SetParent(productsListParent, false);
		newEntry.GetComponent<ProductsPanelListEntry>().SetContent(targetProduct);
	}

	/// <summary>
	/// destroi todas as entradas da lista.
	/// a ideia seria limpar a lista antes de popular novamente para que nao tenhamos produtos repetidos
	/// </summary>
	public void ClearProductList() {
		for(int i = 0; i < productsListParent.childCount; i++) {
			Destroy(productsListParent.GetChild(i).gameObject);
		}
	}

	void OnEnable() {
		ClearProductList();
		for(int i = 0; i < PersistenceActivator.instance.curGameData.productsList.Count; i++) {
			AddProductToList(PersistenceActivator.instance.curGameData.productsList[i]);
		}
	}
}
