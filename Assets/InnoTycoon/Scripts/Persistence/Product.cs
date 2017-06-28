using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product {

	public string name;

	public bool isReady;

	public List<string> optionIDs;

	public float rentability;

	public int rating;

	public int curStep;

	public enum ProductPhase {
		concept,
		dev,
		sales,
		done
	}

	public ProductPhase currentPhase = ProductPhase.concept;

	public int conceptSteps, devSteps, saleSteps;

	private ProdPhaseLoadingBar currentLoadBar;

	private Color loadBarsDisplayColor = Color.white;

	public Product() {}

	/// <summary>
	/// retorna o numero de passos da fase em que o produto se encontra; -1 se o produto ja estiver pronto e fora da fase de vendas ("done")
	/// </summary>
	/// <returns></returns>
	public int GetCurrentPhaseSteps() {
		switch (currentPhase) {
			case ProductPhase.concept:
				return conceptSteps;
			case ProductPhase.dev:
				return devSteps;
			case ProductPhase.sales:
				return saleSteps;
			default:
				return -1;
		}
	}

	public string GetCurrentPhaseName() {
		switch (currentPhase) {
			case ProductPhase.concept:
				return "Concep\u00E7\u00E3o";
			case ProductPhase.dev:
				return "Desenvolvimento";
			case ProductPhase.sales:
				return "Comercializa\u00E7\u00E3o";
			default:
				return "Pronto";
        }
	}

	/// <summary>
	/// incrementa o curStep desse produto e altera sua fase caso tenha chegado ao limite de passos da fase atual
	/// </summary>
	public void OneStep() {
		curStep++;

		UpdateLoadBar();

		switch (currentPhase) {
			case ProductPhase.concept:
				if(curStep >= conceptSteps) {
					currentPhase = ProductPhase.dev;
					curStep = 0;
					ReleaseLoadBar();
				}
				break;
			case ProductPhase.dev:
				if (curStep >= devSteps) {
					currentPhase = ProductPhase.sales;
					curStep = 0;
					ReleaseLoadBar();
					UpdateLoadBar(); //uma atualizacao a mais aqui para enchermos essa loadbar
					GameManager.instance.ProductEnteredSales(this);
				}
				break;
			case ProductPhase.sales:
				if(curStep >= saleSteps) {
					currentPhase = ProductPhase.done;
					curStep = -1;
					ReleaseLoadBar();
					DevSteps.Instance().ReleaseLoadBarColor(loadBarsDisplayColor);
					//esse produto esta encerrado entao
					GameManager.instance.ProductIsDone(this);
				}
				break;
			default:
				Debug.LogWarning(string.Concat("Product ", name, ", already completed, is trying to be updated as if it hadnt been completed"));
				break;
		}
	}


	public void UpdateLoadBar() {
		//pegamos uma cor para a gente caso ainda estejamos com o padrao (white com 1.0f alpha)
		if(loadBarsDisplayColor == Color.white) {
			loadBarsDisplayColor = DevSteps.Instance().GetALoadBarColor();
		}

		//tentamos pegar uma load bar para nos se ainda nao tivermos
		if(currentLoadBar == null) {
			currentLoadBar = DevSteps.Instance().GetALoadBar(currentPhase);
			if(currentLoadBar != null) {
				currentLoadBar.isBeingUsed = true;
				currentLoadBar.ResetBarValue();
				currentLoadBar.loadBarImg.CrossFadeAlpha(DevSteps.baseLoadBarOpacity, DevSteps.transitionDuration, true);
				currentLoadBar.loadBarImg.color = loadBarsDisplayColor;
			}
		}

		//se agora temos uma, vamos atualizar o valor dela de acordo com nosso progresso
		if(currentLoadBar != null) {
			switch (currentPhase) {
				case ProductPhase.concept:
					currentLoadBar.TransitionToValue((float) curStep / conceptSteps);
					break;
				case ProductPhase.dev:
					currentLoadBar.TransitionToValue((float) curStep / devSteps);
					break;
				case ProductPhase.sales:
					currentLoadBar.TransitionToValue((float) curStep / saleSteps);
					break;
			}
			
		}
	}

	public void ReleaseLoadBar() {
		DevSteps.Instance().MarkBarForRelease(currentLoadBar);
		currentLoadBar = null;
	}

}
