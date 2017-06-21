using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductsPanelListEntry : MonoBehaviour {

	public Text nameText;

	/// <summary>
	/// define o conteudo dessa entrada de acordo com o produto fornecido (productData)
	/// </summary>
	public void SetContent(Product productData) {
		nameText.text = productData.name;
	}
	
}
