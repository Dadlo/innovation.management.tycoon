using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductsPanelListEntry : MonoBehaviour {

	public Text nameText;

	public Text curStepText;

	public Text phaseText;

	public Text ratingText;

	/// <summary>
	/// define o conteudo dessa entrada de acordo com o produto fornecido (productData)
	/// </summary>
	public void SetContent(Product productData) {
		nameText.text = productData.name;
		int curPhaseSteps = productData.GetCurrentPhaseSteps();

		if(curPhaseSteps != -1) {
			curStepText.text = string.Concat(productData.curStep.ToString(), " / ", curPhaseSteps.ToString());
		}else {
			curStepText.text = " - ";
		}

		phaseText.text = productData.GetCurrentPhaseName();

		if(productData.rating == -1) {
			ratingText.text = "?";
		}
		else {
			ratingText.text = productData.rating.ToString("n2");
		}

	}
	
}
