using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ProductOptionsContainer {
	public List<ProductOption> productOptionsList;
}

public class ProductCreationPanel : ShowablePanel {

	public Toggle[] sectionToggles;

	public CanvasGroup[] sectionContents;

	public InputField prodNameInputField;

	/// <summary>
	/// o texto de titulo muda de acordo com o nome do produto
	/// </summary>
	public Text titleText;

    public GameObject prodOptionEntryPrefab;

    public int totalConceptsCost, totalDevCost, totalMonetCost;

    public Text totalConceptsCostText, totalDevCostText, totalMonetCostText;

    public RectTransform conceptsListContainer, devListContainer, monetListContainer;

    public List<ProductOption> pickedConcepts = new List<ProductOption>(), 
        pickedDevOptions = new List<ProductOption>(), 
        pickedMonetOptions = new List<ProductOption>();

	public int totalProductCost;

	public Text totalProductCostText, pCreationWarningsText;

	public Button createProductBtn;


	public void DoneEditingProductName() {
		titleText.text = prodNameInputField.text;
	}

    /// <summary>
    /// reseta os conteudos desse painel, como se o usuario nunca tivesse mexido.
	/// as opcoes de concepcao e etc permanecem selecionadas
    /// </summary>
	public void ResetPanels() {
		titleText.text = "Novo Produto";//ou o padrao, de acordo com a localizacao
		prodNameInputField.text = "Novo Produto";
		SetActivePanel(0);

		ToggleProductFinalizationOption(pickedConcepts.Count > 0);
	}

	public void SetActivePanel(int panelIndex) {
		for(int i = 0; i < sectionContents.Length; i++) {
			if(i == panelIndex) {
				sectionContents[i].interactable = true;
                sectionContents[i].blocksRaycasts = true;
                sectionContents[i].alpha = 1;
				if (!sectionToggles[i].isOn) {
					sectionToggles[i].isOn = true;
				}
			}
			else {
				sectionContents[i].interactable = false;
                sectionContents[i].blocksRaycasts = false;
                sectionContents[i].alpha = 0;
				if (sectionToggles[i].isOn) {
					sectionToggles[i].isOn = false;
				}
			}
		}
	}

	public void OnToldToChangeSections(Transform newSectionToggle) {
		SetActivePanel(newSectionToggle.GetSiblingIndex());
	}

	public void GoToNextSection() {
		for (int i = sectionToggles.Length - 2; i >= 0; i--) {
			if (sectionToggles[i].isOn) {
				SetActivePanel(i + 1);
			}
		}
	}

	void OnEnable() {
		ResetPanels();
	}


	// Use this for initialization
	void Start () {
		
		//nos registramos nos eventos de cada toggle para ficar sabendo quando um foi clicado e fazer o que precisarmos fazer de acordo
		for(int i = 0; i < sectionToggles.Length; i++) {
			sectionToggles[i].GetComponent<ProductCreationSectionToggle>().onToggled += OnToldToChangeSections;
		}

        //preenchemos as listas de opcoes...
        FillOptionsList(GameManager.instance.pConcepts.productOptionsList, ProductOptionListEntry.OptionType.concept, conceptsListContainer);
        FillOptionsList(GameManager.instance.pDevOptions.productOptionsList, ProductOptionListEntry.OptionType.dev, devListContainer);
        FillOptionsList(GameManager.instance.pMonetOptions.productOptionsList, ProductOptionListEntry.OptionType.monet, monetListContainer);

		ToggleProductFinalizationOption(pickedConcepts.Count > 0);
    }

    void FillOptionsList(List<ProductOption> theOptionsList, ProductOptionListEntry.OptionType optionType, RectTransform theOptionsContainer)
    {
        for (int i = 0; i < theOptionsList.Count; i++)
        {
            GameObject newEntry = Instantiate(prodOptionEntryPrefab);
            newEntry.transform.SetParent(theOptionsContainer, false);
            ProductOptionListEntry entryScript = newEntry.GetComponent<ProductOptionListEntry>();
            entryScript.onToggled += OnPickedProdOption;
            entryScript.SetContent(theOptionsList[i], optionType);
        }
    }

	/// <summary>
	/// deixa possivel ou impossivel interagir com o botao de criar produto e 
	/// mostra, ou esconde, um aviso sobre o que esta impedindo de criar (precisa escolher uma opcao de concepcao)
	/// </summary>
	/// <param name="active"></param>
	void ToggleProductFinalizationOption(bool active) {
		createProductBtn.interactable = active;
		pCreationWarningsText.gameObject.SetActive(!active);
	}

	/// <summary>
	/// atualiza o valor da variavel de custo total do produto
	/// e o texto responsavel por mostra-la
	/// </summary>
	void RefreshTotalProductCost() {
		totalProductCost = totalConceptsCost + totalDevCost + totalMonetCost;
		totalProductCostText.text = totalProductCost.ToString();
	}

    public void OnPickedProdOption(ProductOptionListEntry pickedOption)
    {
        switch (pickedOption.myOptionType)
        {
            case ProductOptionListEntry.OptionType.concept:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedConcepts.Add(pickedOption.theOptionRepresented);
                    totalConceptsCost += pickedOption.theOptionRepresented.cost;
                }
                else
                {
                    pickedConcepts.Remove(pickedOption.theOptionRepresented);
                    totalConceptsCost -= pickedOption.theOptionRepresented.cost;                    
                }

				totalConceptsCostText.text = totalConceptsCost.ToString();
				ToggleProductFinalizationOption(pickedConcepts.Count > 0);
				RefreshTotalProductCost();

				break;
            case ProductOptionListEntry.OptionType.dev:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedDevOptions.Add(pickedOption.theOptionRepresented);
                    totalDevCost += pickedOption.theOptionRepresented.cost;
                }
                else
                {
                    pickedDevOptions.Remove(pickedOption.theOptionRepresented);
                    totalDevCost -= pickedOption.theOptionRepresented.cost;
                }

				totalDevCostText.text = totalDevCost.ToString();
				RefreshTotalProductCost();

				break;
            case ProductOptionListEntry.OptionType.monet:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedMonetOptions.Add(pickedOption.theOptionRepresented);
                    totalMonetCost += pickedOption.theOptionRepresented.cost;
                }
                else
                {
                    pickedMonetOptions.Remove(pickedOption.theOptionRepresented);
                    totalMonetCost -= pickedOption.theOptionRepresented.cost;
                }

				totalMonetCostText.text = totalMonetCost.ToString();
				RefreshTotalProductCost();

                break;
        }
    }

    /// <summary>
    /// reune os dados contidos nesse painel e cria um novo produto, que entra na fase de concepcao
    /// </summary>
    public void CreateProduct()
    {
		ToggleDisplay(false);
		//new Product ou algo assim, recebendo as porcentagens de foco, nome fornecido e etc
		Product createdProduct = new Product();
		createdProduct.name = prodNameInputField.text;

		createdProduct.optionIDs = new List<string>();

		createdProduct.conceptSteps = GameManager.baseNumberOfConceptSteps;
		createdProduct.devSteps = GameManager.baseNumberOfDevSteps;
		createdProduct.saleSteps = GameManager.baseNumberOfSaleSteps;

		for(int i = 0; i < pickedConcepts.Count; i++) {
			createdProduct.optionIDs.Add(pickedConcepts[i].id);
			createdProduct.conceptSteps += Mathf.Max(pickedConcepts[i].multiplier - 1, 0);
        }

		for (int i = 0; i < pickedDevOptions.Count; i++) {
			createdProduct.optionIDs.Add(pickedDevOptions[i].id);
			createdProduct.devSteps += Mathf.Max(pickedDevOptions[i].multiplier - 1, 0);
		}

		for (int i = 0; i < pickedMonetOptions.Count; i++) {
			createdProduct.optionIDs.Add(pickedMonetOptions[i].id);
			createdProduct.saleSteps += Mathf.Max(pickedMonetOptions[i].multiplier - 1, 0);
		}

		GameManager.instance.AddNewPlayerProduct(createdProduct);
		
		

	}
	
}
