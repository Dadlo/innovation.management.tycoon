using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductCreationSlider : MonoBehaviour {

	public bool beingHeld = false;

	public ProductCreationSlidersGroup theGroup;

	public int mySliderGroupIndex;

	public Text sliderText;


    /// <summary>
    /// apenas altera o valor da variavel beingHeld, para sabermos quando esse slider esta sendo segurado pelo usuario.
    /// (para que funcione direitinho, o event trigger precisa vir antes do script de slider nesse objeto)
    /// </summary>
    /// <param name="held"></param>
	public void SetBeingHeld(bool held) {
		beingHeld = held;
	}

	public void ValueChanged(float newValue) {
		if (beingHeld) {
			theGroup.SetSliderValue(mySliderGroupIndex, newValue);
		}

		sliderText.text = string.Concat(newValue.ToString(), "%");

	}

}
