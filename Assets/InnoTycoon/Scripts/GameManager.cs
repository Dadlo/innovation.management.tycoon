using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe cuja ideia seria cuidar de eventos importantes do jogo e fazer a ligacao com o persistenceActivator
/// </summary>
public class GameManager : MonoBehaviour {

	public const int baseDailyCost = 2000;

	public static GameManager instance;

	public List<Product> productsDoneToday = new List<Product>();

	public const int baseNumberOfConceptSteps = 1, baseNumberOfDevSteps = 3, baseNumberOfSaleSteps = 3;

	public const int baseConceptProfit = 9000, baseDevProfit = 9000, baseSalesProfit = 9000;

	public const float baseRatingDivisor = 10.5f, salesStepsDivisor = 3;

	public const float luckyFactor = 0.3f;

	public ProductOptionsContainer pConcepts, pDevOptions, pMonetOptions;

	public TextAsset productConceptsAsset, productDevOptionsAsset, productMonetizationsAsset;

	void Awake() {
		instance = this;

		pConcepts = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productConceptsAsset);
		pDevOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productDevOptionsAsset);
		pMonetOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productMonetizationsAsset);
	}

	public void GoToNextDay() {
		DevSteps.Instance().ReleaseMarkedBars();

		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
		persInstanceSave.day++;
		// Take cost from capital
		persInstanceSave.capital -= persInstanceSave.cost;

		//update products/studies... everyone goes to next step
		for(int i = 0; i < persInstanceSave.productsDoing.Count; i++) {
			persInstanceSave.productsDoing[i].OneStep();
		}

		//products marked as "done" are removed from productsDoing
		for(int i = 0; i < productsDoneToday.Count; i++) {
			persInstanceSave.cost += productsDoneToday[i].rentability;
			persInstanceSave.productsDoing.Remove(productsDoneToday[i]);
		}

		productsDoneToday.Clear();
	}

	/// <summary>
	/// adiciona o produto newProduct a lista de produtos geral (productsList) e a lista de produtos sendo feitos (productsDoing).
	/// tambem atualiza o custo diario para o jogador e mostra esse novo custo adequadamente
	/// </summary>
	/// <param name="newProduct"></param>
	public void AddNewPlayerProduct(Product newProduct) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		if (persInstanceSave.productsDoing == null) persInstanceSave.productsDoing = new List<Product>();
		persInstanceSave.productsDoing.Add(newProduct);

		if (persInstanceSave.productsList == null) persInstanceSave.productsList = new List<Product>();
		persInstanceSave.productsList.Add(newProduct);

		persInstanceSave.cost += CalculateProductCost(newProduct);
		PersistenceActivator.instance.RenderAllChanges();
	}

	/// <summary>
	/// metodo chamado quando um produto entra na fase de vendas.
	/// nesse momento, ele tambem deixa de ter custos para a empresa,
	/// passando a dar lucro, e tem a sua nota calculada
	/// </summary>
	/// <param name="theProductSold"></param>
	public void ProductEnteredSales(Product theProductSold) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		persInstanceSave.cost -= CalculateProductCost(theProductSold);

		//TODO a janelinha de aviso de "produto entrou em venda"
		theProductSold.CalculateRating();

		persInstanceSave.cost -= theProductSold.rentability;
	}

	/// <summary>
	/// esse metodo e chamado quando um produto encerra sua fase de vendas.
	/// esse produto "morreu", e deixa de dar lucro
	/// </summary>
	/// <param name="doneProduct"></param>
	public void ProductIsDone(Product doneProduct) {
		//nao podemos remover assim que descobrimos que o produto se encerrou para nao dar problemas com o for que itera sobre os productsDoing
		productsDoneToday.Add(doneProduct);
	}

	ProductOption GetProductOptionByID(string optionID) {
		for(int i = 0; i < pConcepts.productOptionsList.Count; i++) {
			if(pConcepts.productOptionsList[i].id == optionID) {
				return pConcepts.productOptionsList[i];
			}
		}

		for (int i = 0; i < pDevOptions.productOptionsList.Count; i++) {
			if (pDevOptions.productOptionsList[i].id == optionID) {
				return pDevOptions.productOptionsList[i];
			}
		}

		for (int i = 0; i < pMonetOptions.productOptionsList.Count; i++) {
			if (pMonetOptions.productOptionsList[i].id == optionID) {
				return pMonetOptions.productOptionsList[i];
			}
		}

		return null;
	}

	/// <summary>
	/// retorna o produto com o nome desejado (name) ou null se nao achar
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public Product GetProductByName(string name) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
		for (int i = 0; i < persInstanceSave.productsList.Count; i++) {
			if(persInstanceSave.productsList[i].name == name) {
				return persInstanceSave.productsList[i];
			}
		}
		return null;
	}

	public Product.ProductPhase GetProductOptionPhase(ProductOption targetOption) {
		if (pConcepts.productOptionsList.Contains(targetOption)) {
			return Product.ProductPhase.concept;
		}else if (pDevOptions.productOptionsList.Contains(targetOption)) {
			return Product.ProductPhase.dev;
		}else if (pMonetOptions.productOptionsList.Contains(targetOption)) {
			return Product.ProductPhase.sales;
		}

		return Product.ProductPhase.done;
	}

	public int CalculateProductCost(Product targetProduct) {
		int totalProdCost = 0;
		for(int i = 0; i < targetProduct.pickedOptions.Count; i++) {
			totalProdCost += targetProduct.pickedOptions[i].cost;
		}

		return totalProdCost;
	}

}
