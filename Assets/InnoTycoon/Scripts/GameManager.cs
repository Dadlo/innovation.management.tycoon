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

    public const float ownRepetitionDetrimentFactor = 0.7f, othersRepetitionDetrimentFactor = 0.2f;

	public ProductOptionsContainer pConcepts, pDevOptions, pMonetOptions;

    public StudyOptionsContainer studies;

	public FeedbackTexts fbTexts;

	public TextAsset productConceptsAsset, productDevOptionsAsset, productMonetizationsAsset, studiesAsset, feedbackTextsAsset;

    public ShowPanels showPanels;

    public List<string> studyNames = new List<string>();

	void Awake() {
		instance = this;

		pConcepts = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productConceptsAsset);
		pDevOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productDevOptionsAsset);
		pMonetOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productMonetizationsAsset);
        studies = PersistenceHandler.LoadFromFile<StudyOptionsContainer>(studiesAsset);
		fbTexts = PersistenceHandler.LoadFromFile<FeedbackTexts>(feedbackTextsAsset);

		fbTexts.GetOrganizedTextsFromList();

        //fill study list names for AI tycoons
        for(int i = 0; i < studies.studyOptionsList.Count; i++)
        {
            studyNames.Add(studies.studyOptionsList[i].title);
        }
    }

	public void GoToNextDay() {
		DevSteps.Instance().ReleaseMarkedBars();

		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

        //update AI tycoons
        for(int i = 0; i < persInstanceSave.AiTycoons.Count; i++)
        {
            persInstanceSave.AiTycoons[i].Tick();
        }

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

        //studies!!!
        //one step forward if we are studying
        if(persInstanceSave.studyDoing != "")
        {
            persInstanceSave.curStudyStep++;
            StudyOption curStudy = GetStudyByName(persInstanceSave.studyDoing);
            if(persInstanceSave.curStudyStep >= curStudy.steps)
            {
                ModalPanel.Instance().OkBox(fbTexts.GetText("studyRdy"), fbTexts.GetText("studyRdyTxt", curStudy.title));
                //we are done studying!! reduce costs and add study to the saved studiesList
                persInstanceSave.curStudyStep = 0;
                persInstanceSave.studyDoing = "";
                if (persInstanceSave.studiesList == null) persInstanceSave.studiesList = new List<string>();
                persInstanceSave.studiesList.Add(curStudy.skillId);
                persInstanceSave.cost -= curStudy.cost;

				switch (curStudy.type) {
					case "Concep\u00E7\u00E3o":
						showPanels.pCreationPanel.RefreshOptionEntriesList(ProductOptionListEntry.OptionType.concept);
                        break;
					case "Desenvolvimento":
						showPanels.pCreationPanel.RefreshOptionEntriesList(ProductOptionListEntry.OptionType.dev);
						break;
					case "Comercializa\u00E7\u00E3o":
						showPanels.pCreationPanel.RefreshOptionEntriesList(ProductOptionListEntry.OptionType.monet);
						break;
				}
				
            }
        }
	}

	/// <summary>
	/// adiciona o produto newProduct a lista de produtos geral (productsList) e a lista de produtos sendo feitos (productsDoing).
	/// tambem atualiza o custo diario para o jogador e mostra esse novo custo adequadamente
	/// </summary>
	/// <param name="newProduct"></param>
	public void AddNewPlayerProduct(Product newProduct) {
        newProduct.owner = "Player";

		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		if (persInstanceSave.productsDoing == null) persInstanceSave.productsDoing = new List<Product>();
		persInstanceSave.productsDoing.Add(newProduct);

		if (persInstanceSave.productsList == null) persInstanceSave.productsList = new List<Product>();
		persInstanceSave.productsList.Add(newProduct);

		persInstanceSave.cost += CalculateProductCost(newProduct);
		PersistenceActivator.instance.RenderAllChanges();
	}

    public void StartNewPlayerStudy(StudyOption theStudy)
    {
        SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
        persInstanceSave.curStudyStep = 0;
        persInstanceSave.studyDoing = theStudy.title;
        persInstanceSave.cost += theStudy.cost;
        PersistenceActivator.instance.RenderAllChanges();
    }

	/// <summary>
	/// metodo chamado quando um produto entra na fase de vendas.
	/// nesse momento, ele tambem deixa de ter custos para a empresa,
	/// passando a dar lucro, e tem a sua nota calculada
	/// </summary>
	/// <param name="theProductSold"></param>
	public void ProductEnteredSales(Product theProductSold) {
        Product.ProductCloneType produtoRepetido = theProductSold.CalculateRating();

        if (theProductSold.MadeByPlayer)
        {
            SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

            persInstanceSave.cost -= CalculateProductCost(theProductSold);

            string messageBoxContent = string.Concat(fbTexts.GetText("criticsGrade"),
                theProductSold.rating.ToString(), "\n", fbTexts.GetText("estimIncome"), theProductSold.rentability.ToString(), "\n",
                fbTexts.GetText("salesPeriod"), theProductSold.saleSteps.ToString(), "\n",
                fbTexts.GetText(string.Concat("fbProdGrade", GetProductRatingIntervalText(theProductSold), "-", Random.Range(1, 4).ToString())));

            if (produtoRepetido != Product.ProductCloneType.notAClone)
            {
                messageBoxContent = string.Concat(messageBoxContent, "\n", fbTexts.GetText("prodRepeatAlert"));
            }


            ModalPanel.Instance().OkBox(fbTexts.GetText("prodEnteredSales"), messageBoxContent);

            persInstanceSave.cost -= theProductSold.rentability;
        }
        else
        {
            AITycoon prodOwner = GetAITycoByName(theProductSold.owner);

            prodOwner.curIncome += CalculateProductCost(theProductSold);
            prodOwner.curIncome += theProductSold.rentability;
        }
		
	}

	/// <summary>
	/// esse metodo e chamado quando um produto encerra sua fase de vendas.
	/// esse produto "morreu", e deixa de dar lucro
	/// </summary>
	/// <param name="doneProduct">o produto que foi terminado</param>
	public void ProductIsDone(Product doneProduct) {
        if (doneProduct.MadeByPlayer)
        {
            //nao podemos remover assim que descobrimos que o produto se encerrou para nao dar problemas com o for que itera sobre os productsDoing
            productsDoneToday.Add(doneProduct);
        }
        else
        {
            AITycoon prodOwner = GetAITycoByName(doneProduct.owner);

            prodOwner.curIncome -= doneProduct.rentability;
            prodOwner.productDoing = null;
        }
		
	}

	public ProductOption GetProductOptionByID(string optionID) {
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
	/// <param name="name">o nome do produto que queremos</param>
	/// <returns>o produto achado, ou null se nao houver</returns>
	public Product GetProductByName(string name) {
		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
		for (int i = 0; i < persInstanceSave.productsList.Count; i++) {
			if(persInstanceSave.productsList[i].name == name) {
				return persInstanceSave.productsList[i];
			}
		}
        for (int i = 0; i < persInstanceSave.AiTycoons.Count; i++)
        {
            for (int j = 0; j < persInstanceSave.AiTycoons[i].products.Count; j++)
            {
                if (persInstanceSave.AiTycoons[i].products[j].name == name)
                {
                    return persInstanceSave.AiTycoons[i].products[j];
                }
            }

        }
        return null;
	}

    /// <summary>
    /// retorna o estudo com o nome desejado "studyName" ou null se nao achar
    /// </summary>
    /// <param name="studyName"></param>
    /// <returns></returns>
    public StudyOption GetStudyByName(string studyName)
    {
        for (int i = 0; i < studies.studyOptionsList.Count; i++)
        {
            if (studies.studyOptionsList[i].title == studyName)
            {
                return studies.studyOptionsList[i];
            }
        }
        return null;
    }

    public AITycoon GetAITycoByName(string tycoName)
    {
        SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
        for (int i = 0; i < persInstanceSave.AiTycoons.Count; i++)
        {
            if(persInstanceSave.AiTycoons[i].name == tycoName)
            {
                return persInstanceSave.AiTycoons[i];
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

    public bool HasProductOptionBeenUnlocked(ProductOption theOption) {
		if (theOption.active) return true;

		SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;

		return persInstanceSave.studiesList.Contains(theOption.id);
	}

    /// <summary>
    /// retorna true caso testedProduct tenha as mesmas opcoes (nem mais nem menos) que outro produto ja lancado
    /// </summary>
    /// <param name="testedProduct"></param>
    /// <returns></returns>
    public Product.ProductCloneType ProductHasRepeatedOptions(Product testedProduct)
    {
        List<Product> existingProducts = PersistenceActivator.instance.curGameData.productsList;
        for (int i = 0; i < existingProducts.Count; i++)
        {
            if (existingProducts[i] == testedProduct || existingProducts[i].currentPhase == Product.ProductPhase.concept ||
                existingProducts[i].currentPhase == Product.ProductPhase.dev)
            {
                continue; //nao precisa testar contra nos mesmos ou contra produtos ainda sendo feitos
            }
            else
            {
                if(testedProduct.pickedOptionIDs.Count == existingProducts[i].pickedOptionIDs.Count)
                {
                    bool isClone = true; 
                    //comecamos pensando que temos um clone aqui, mas o for abaixo vai nos dizer a verdade
                    for (int j = 0; j < testedProduct.pickedOptionIDs.Count; j++)
                    {
                        if (!existingProducts[i].pickedOptionIDs.Contains(testedProduct.pickedOptionIDs[j]))
                        {
                            isClone = false;
                            break;
                        }
                    }

                    //se ainda acreditamos que temos um clone, temos um clone mesmo
                    if (isClone)
                    {
                        Debug.Log(string.Concat("product ", testedProduct.name, " is a clone! detriment should be applied"));
                        return Product.ProductCloneType.ownClone;
                    }
                    else
                    {
                        continue;
                    }
                    
                }
                else
                {
                    continue; //pode ate ter varias igualdades, mas um produto tem mais opcoes que o outro
                }
                
            }

        }

        return Product.ProductCloneType.notAClone;
    }

	public int CalculateProductCost(Product targetProduct) {
		int totalProdCost = 0;
		for(int i = 0; i < targetProduct.pickedOptionIDs.Count; i++) {
            ProductOption theOption = GetProductOptionByID(targetProduct.pickedOptionIDs[i]);
            if(theOption != null)
            {
                totalProdCost += theOption.cost;
            }
			
		}

		return totalProdCost;
	}

	private string GetProductRatingIntervalText(Product targetProduct) {
		float theRating = targetProduct.rating;
		if(theRating < 3) {
			return "0a3";
		}else if(theRating < 5) {
			return "3a5";
		}else if(theRating < 7) {
			return "5a7";
		}else if(theRating < 9) {
			return "7a9";
		}
		else {
			return "9a10";
		}
	}

	public string GetVariableText(string textID) {
		return fbTexts.GetText(textID);
	}

	/// <summary>
	/// adiciona o R$ e o ,00 a um numero fornecido e retorna essa string
	/// </summary>
	/// <param name="number"></param>
	/// <returns></returns>
	public static string ConvertNumberToCoinString(int number) {
		return string.Concat("R$", number.ToString(), ",00");
	}

}
