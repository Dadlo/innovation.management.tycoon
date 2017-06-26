using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSteps : MonoBehaviour {

	public ProdPhaseLoadingBar[] loadingBarsCon, loadingBarsDev, loadingBarsMon;

	public static float transitionDuration = 1.5f;

	public static float baseLoadBarOpacity = 0.6f;

	public List<Color> availableLoadBarColors = new List<Color>();

	private List<Color> currentlyUsedLoadBarColors = new List<Color>();

	/// <summary>
	/// lista de barras que serao soltas no dia seguinte.
	/// a gente nao solta na hora para que ainda seja possivel ver a barra cheia ate o dia seguinte
	/// </summary>
	private List<ProdPhaseLoadingBar> barsMarkedForRelease = new List<ProdPhaseLoadingBar>();

	private static DevSteps MainDevSteps;

	public static DevSteps Instance() {
		if (!MainDevSteps) {
			MainDevSteps = FindObjectOfType(typeof(DevSteps)) as DevSteps;
			if (!MainDevSteps) {
				Debug.LogError("There needs to be one active DevSteps script on a GameObject in your scene.");
			}
		}
		return MainDevSteps;
	}

	/// <summary>
	/// tentamos pegar uma load bar que nao esteja sendo usada no momento.
	/// se nao conseguirmos, retornamos null
	/// </summary>
	/// <param name="curProductPhase"></param>
	/// <returns></returns>
	public ProdPhaseLoadingBar GetALoadBar(Product.ProductPhase curProductPhase) {
		switch (curProductPhase) {
			case Product.ProductPhase.concept:
				return LookForUnusedLoadBar(loadingBarsCon);
			case Product.ProductPhase.dev:
				return LookForUnusedLoadBar(loadingBarsDev);
			case Product.ProductPhase.sales:
				return LookForUnusedLoadBar(loadingBarsMon);
		}

		return null;
	}

	ProdPhaseLoadingBar LookForUnusedLoadBar(ProdPhaseLoadingBar[] loadBarArray) {
		for(int i = 0; i < loadBarArray.Length; i++) {
			if (!loadBarArray[i].isBeingUsed) {
				return loadBarArray[i];
			}
		}

		return null;
	}

	/// <summary>
	/// pega uma cor para representar um produto de forma consistente durante as 3 etapas
	/// </summary>
	/// <returns></returns>
	public Color GetALoadBarColor() {
		Color pickedColor = Color.clear;//se nao acharmos nenhuma valida, pegamos uma completamente transparente
		if (availableLoadBarColors.Count > 0) {
			pickedColor = availableLoadBarColors[0];
			currentlyUsedLoadBarColors.Add(pickedColor);
			availableLoadBarColors.Remove(pickedColor);
		}

		return pickedColor;
	}

	/// <summary>
	/// faz a cor alvo voltar para a lista de cores disponiveis para os produtos
	/// </summary>
	/// <param name="targetColor"></param>
	public void ReleaseLoadBarColor(Color targetColor) {
		currentlyUsedLoadBarColors.Remove(targetColor);
		availableLoadBarColors.Add(targetColor);
	}

	public void MarkBarForRelease(ProdPhaseLoadingBar theBar) {
		barsMarkedForRelease.Add(theBar);
	}

	/// <summary>
	/// barras marcadas para serem soltas sao finalmente soltas aqui.
	/// elas somem e ficam disponiveis novamente para outros produtos
	/// </summary>
	public void ReleaseMarkedBars() {
		for(int i = 0; i < barsMarkedForRelease.Count; i++) {
			barsMarkedForRelease[i].isBeingUsed = false;
			barsMarkedForRelease[i].loadBarImg.CrossFadeAlpha(0, transitionDuration, true);
		}

		barsMarkedForRelease.Clear();
	}

}
