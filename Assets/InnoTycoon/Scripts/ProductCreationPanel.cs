using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductOptionsContainer
{
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

	public ProductCreationSlidersGroup creationFocusSlidersGroup;

    public int totalConceptsCost, totalDevCost, totalMonetCost;

	public int totalConceptDays, totalDevDays, totalMonetDays;

    public Text totalConceptsCostText, totalDevCostText, totalMonetCostText;

	public Text totalConceptDaysText, totalDevDaysText, totalMonetDaysText;

    public RectTransform conceptsListContainer, devListContainer, monetListContainer;

    public List<ProductOption> pickedConcepts = new List<ProductOption>(), 
        pickedDevOptions = new List<ProductOption>(), 
        pickedMonetOptions = new List<ProductOption>();

	public List<ProductOptionListEntry> conceptOptionEntries = new List<ProductOptionListEntry>(),
		devOptionEntries = new List<ProductOptionListEntry>(),
		monetOptionEntries = new List<ProductOptionListEntry>();

	public int totalProductCost;

	public Text totalProductCostText, totalProductPaidDaysText, totalProductProfitDaysText, pCreationWarningsText;

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

		totalConceptDays = GameManager.baseNumberOfConceptSteps;
		totalConceptDaysText.text = totalConceptDays.ToString();
		totalDevDays = GameManager.baseNumberOfDevSteps;
		totalDevDaysText.text = totalDevDays.ToString();
		totalMonetDays = GameManager.baseNumberOfSaleSteps;
		totalMonetDaysText.text = totalMonetDays.ToString();

		//nos registramos nos eventos de cada toggle para ficar sabendo quando um foi clicado e fazer o que precisarmos fazer de acordo
		for(int i = 0; i < sectionToggles.Length; i++) {
			sectionToggles[i].GetComponent<ProductCreationSectionToggle>().onToggled += OnToldToChangeSections;
		}

        //preenchemos as listas de opcoes...
        FillOptionsList(GameManager.instance.pConcepts.productOptionsList, conceptOptionEntries, ProductOptionListEntry.OptionType.concept, conceptsListContainer);
        FillOptionsList(GameManager.instance.pDevOptions.productOptionsList, devOptionEntries, ProductOptionListEntry.OptionType.dev, devListContainer);
        FillOptionsList(GameManager.instance.pMonetOptions.productOptionsList, monetOptionEntries, ProductOptionListEntry.OptionType.monet, monetListContainer);

		ToggleProductFinalizationOption(pickedConcepts.Count > 0);
    }

    void FillOptionsList(List<ProductOption> theOptionsList, List<ProductOptionListEntry> optionEntryListToFill, ProductOptionListEntry.OptionType optionType, RectTransform theOptionsContainer)
    {
        for (int i = 0; i < theOptionsList.Count; i++)
        {
            GameObject newEntry = Instantiate(prodOptionEntryPrefab);
            newEntry.transform.SetParent(theOptionsContainer, false);
            ProductOptionListEntry entryScript = newEntry.GetComponent<ProductOptionListEntry>();
            entryScript.onToggled += OnPickedProdOption;
            entryScript.SetContent(theOptionsList[i], optionType);
			optionEntryListToFill.Add(entryScript);
        }
    }

	public void RefreshOptionEntriesList(ProductOptionListEntry.OptionType targetTypeToRefresh) {
		switch (targetTypeToRefresh) {
			case ProductOptionListEntry.OptionType.concept:
				RefreshAllEntriesInList(conceptOptionEntries);
				break;
			case ProductOptionListEntry.OptionType.dev:
				RefreshAllEntriesInList(devOptionEntries);
				break;
			case ProductOptionListEntry.OptionType.monet:
				RefreshAllEntriesInList(monetOptionEntries);
				break;
		}

	}

	public void RefreshAllEntriesInList(List<ProductOptionListEntry> entryOptions) {
		for(int i = 0; i < entryOptions.Count; i++) {
			entryOptions[i].CheckIfCanBeSelected();
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
	void RefreshTotalProductInfo() {
		totalProductPaidDaysText.text = (totalDevDays + totalConceptDays).ToString();
		totalProductProfitDaysText.text = totalMonetDays.ToString();
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
					totalConceptDays += Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
                }
                else
                {
                    pickedConcepts.Remove(pickedOption.theOptionRepresented);
					totalConceptsCost -= pickedOption.theOptionRepresented.cost;
					totalConceptDays -= Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
				}

				totalConceptsCostText.text = totalConceptsCost.ToString();
				totalConceptDaysText.text = totalConceptDays.ToString();
				ToggleProductFinalizationOption(pickedConcepts.Count > 0);
				RefreshTotalProductInfo();

				break;
            case ProductOptionListEntry.OptionType.dev:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedDevOptions.Add(pickedOption.theOptionRepresented);
                    totalDevCost += pickedOption.theOptionRepresented.cost;
					totalDevDays += Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
				}
                else
                {
                    pickedDevOptions.Remove(pickedOption.theOptionRepresented);
                    totalDevCost -= pickedOption.theOptionRepresented.cost;
					totalDevDays -= Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
				}

				totalDevDaysText.text = totalDevDays.ToString();
				totalDevCostText.text = totalDevCost.ToString();
				RefreshTotalProductInfo();

				break;
            case ProductOptionListEntry.OptionType.monet:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedMonetOptions.Add(pickedOption.theOptionRepresented);
                    totalMonetCost += pickedOption.theOptionRepresented.cost;
					totalMonetDays += Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
				}
                else
                {
                    pickedMonetOptions.Remove(pickedOption.theOptionRepresented);
                    totalMonetCost -= pickedOption.theOptionRepresented.cost;
					totalMonetDays -= Mathf.Max(pickedOption.theOptionRepresented.multiplier - 1, 0);
				}

				totalMonetDaysText.text = totalMonetDays.ToString();
				totalMonetCostText.text = totalMonetCost.ToString();
				RefreshTotalProductInfo();

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

		//se tivermos nome repetido, lancar o 2, ou 3...
		if(GameManager.instance.GetProductByName(createdProduct.name) != null) {
			int sequelNumber = 2;
			while(GameManager.instance.GetProductByName(string.Concat(createdProduct.name, " ", sequelNumber.ToString())) != null) {
				sequelNumber++;
			}
			createdProduct.name = string.Concat(createdProduct.name, " ", sequelNumber.ToString());
        }

		createdProduct.pickedOptionIDs = new List<string>();

		createdProduct.conceptSteps = totalConceptDays;
		createdProduct.devSteps = totalDevDays;
		createdProduct.saleSteps = totalMonetDays;

		for(int i = 0; i < pickedConcepts.Count; i++) {
			createdProduct.pickedOptionIDs.Add(pickedConcepts[i].id);
        }

		for (int i = 0; i < pickedDevOptions.Count; i++) {
			createdProduct.pickedOptionIDs.Add(pickedDevOptions[i].id);
		}

		for (int i = 0; i < pickedMonetOptions.Count; i++) {
			createdProduct.pickedOptionIDs.Add(pickedMonetOptions[i].id);
		}

		createdProduct.conceptFocusPercentage = creationFocusSlidersGroup.sliders[0].value / 100;
		createdProduct.devFocusPercentage = creationFocusSlidersGroup.sliders[1].value / 100;
		createdProduct.saleFocusPercentage = creationFocusSlidersGroup.sliders[2].value / 100;

		GameManager.instance.AddNewPlayerProduct(createdProduct);
	}
	
}
