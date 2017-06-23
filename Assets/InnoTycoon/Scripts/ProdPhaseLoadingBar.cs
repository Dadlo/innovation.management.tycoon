using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProdPhaseLoadingBar : MonoBehaviour {

	public Image loadBarImg;

	private float currentTransitionTime = 0;


	void Reset() {
		loadBarImg = GetComponent<Image>();
	}

	public void TransitionToValue(float finalValue) {
		StopCoroutine("LoadBarTransitionRoutine");
		StartCoroutine("LoadBarTransitionRoutine", finalValue);
	}

	public IEnumerator LoadBarTransitionRoutine(float finalValue) {
		currentTransitionTime = 0;
		while (!Mathf.Approximately(loadBarImg.fillAmount, finalValue)) {
			loadBarImg.fillAmount = Mathf.Lerp(loadBarImg.fillAmount, finalValue, currentTransitionTime / DevSteps.transitionDuration);
			yield return null;
		}
		
	}
}
