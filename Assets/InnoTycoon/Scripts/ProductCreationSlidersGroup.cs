using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductCreationSlidersGroup : MonoBehaviour {

	public Slider[] sliders;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < sliders.Length; i++) {
			ProductCreationSlider pickedSlider = sliders[i].GetComponent<ProductCreationSlider>();
			pickedSlider.mySliderGroupIndex = i;
			pickedSlider.beingHeld = false;
		}

		SetSliderValue(0, 34);
	}


	public void SetSliderValue(int sliderIndex, float value) {
		if(sliders[sliderIndex].value != value) {
			sliders[sliderIndex].value = value;
		}

		BalanceSliders(sliderIndex);
	}

	/// <summary>
	/// altera os valores dos sliders (exceto o do fixedSlider) ate que a soma dos valores dos sliders resulte em 100
	/// </summary>
	/// <param name="fixedSliderIndex"></param>
	public void BalanceSliders(int fixedSliderIndex) {
		int sliderSum = 0, sliderDifference = 0;
		for(int i = 0; i < sliders.Length; i++) {
			sliderSum += (int) sliders[i].value;
		}

		sliderDifference = 100 - sliderSum;

		while (sliderDifference != 0) {
			for (int i = 0; i < sliders.Length; i++) {
				if (sliderDifference == 0) break; //e para de mexer se ja mexeu o suficiente
				if (i == fixedSliderIndex) continue; //nao mexe nesse cara

				else if (sliderDifference > 0) {
					if (sliders[i].value < sliders[i].maxValue) {
						sliders[i].value++;
						sliderDifference--;
					}
					
				}
				else {
					if (sliders[i].value > sliders[i].minValue) {
						sliders[i].value--;
						sliderDifference++;
					}
					
				}
			}

			sliderSum = 0;

			for (int i = 0; i < sliders.Length; i++) {
				sliderSum += (int)sliders[i].value;
			}

			sliderDifference = 100 - sliderSum;
		}
	}
	
}
