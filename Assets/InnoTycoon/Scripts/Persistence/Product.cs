using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Product {

	public string name;

	public bool isReady;

	public List<ProductOption> pickedOptions;

	public int rentability;

	public float rating = -1;

	public int curStep;

	public enum ProductPhase {
		concept,
		dev,
		sales,
		done
	}

	public ProductPhase currentPhase = ProductPhase.concept;

	public float conceptFocusPercentage, devFocusPercentage, saleFocusPercentage;

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
					GameManager.instance.ProductEnteredSales(this);
					UpdateLoadBar(); //uma atualizacao a mais aqui para enchermos essa loadbar
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


	public void CalculateRating() {
		//soma dos multiplicadores de cada opcao escolhida para o produto
		float totalConceptModifier = 1, totalDevModifier = 1, totalSalesModifier = 1;

		for(int i = 0; i < pickedOptions.Count; i++) {
			switch (GameManager.instance.GetProductOptionPhase(pickedOptions[i])) {
				case ProductPhase.concept:
					totalConceptModifier += pickedOptions[i].multiplier;
					break;
				case ProductPhase.dev:
					totalDevModifier += pickedOptions[i].multiplier;
					break;
				case ProductPhase.sales:
					totalSalesModifier += pickedOptions[i].multiplier;
					break;
				default:
					Debug.LogWarning(string.Concat("Failed trying to get product phase from product option ", pickedOptions[i].title));
					break;
			}
		}

		//lucky modifier
		totalConceptModifier += Random.Range(0, GameManager.luckyFactor);
		totalDevModifier += Random.Range(0, GameManager.luckyFactor);
		totalSalesModifier += Random.Range(0, GameManager.luckyFactor);

		rating = Mathf.Min((totalConceptModifier / GameManager.baseRatingDivisor) +
						   (totalDevModifier / GameManager.baseRatingDivisor) +
						   (totalSalesModifier / GameManager.baseRatingDivisor), 10);

		//multiplicacao pelos fatores de foco do produto
		totalConceptModifier *= conceptFocusPercentage;
		totalDevModifier *= devFocusPercentage;
		totalSalesModifier *= saleFocusPercentage;

		//TODO DETRIMENTO

		rentability = (int) (GameManager.baseConceptProfit * totalConceptModifier + GameManager.baseDevProfit * totalDevModifier +
			GameManager.baseSalesProfit * totalSalesModifier);

		saleSteps = GameManager.baseNumberOfSaleSteps + Mathf.FloorToInt(totalSalesModifier / GameManager.salesStepsDivisor);
	}

}
