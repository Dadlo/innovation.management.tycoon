using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProdPhaseLoadingBar : MonoBehaviour {

	public Image loadBarImg;

	private float currentTransitionTime = 0;

	public bool isBeingUsed = false;

	public bool startsFull = false;


	//isso roda assim que adicionamos o script num objeto e quando mandamos dar reset no script no inspetor
	void Reset() {
		loadBarImg = GetComponent<Image>();
	}

	/// <summary>
	/// o valor sendo mostrado pela barra e reiniciado, seja para 100 por cento ou pra 0, dependendo da variavel startsFull
	/// </summary>
	public void ResetBarValue() {
		loadBarImg.fillAmount = startsFull ? 1.0f : 0.0f;
	}

	public void TransitionToValue(float finalValue) {
		StopCoroutine("LoadBarTransitionRoutine");
		StartCoroutine("LoadBarTransitionRoutine", startsFull ? 1 - finalValue : finalValue);
	}

	public IEnumerator LoadBarTransitionRoutine(float finalValue) {
		currentTransitionTime = 0;
		while (!Mathf.Approximately(loadBarImg.fillAmount, finalValue)) {
			loadBarImg.fillAmount = Mathf.Lerp(loadBarImg.fillAmount, finalValue, currentTransitionTime / DevSteps.transitionDuration);
			currentTransitionTime += Time.deltaTime;
			yield return null;
		}
		
	}
}
