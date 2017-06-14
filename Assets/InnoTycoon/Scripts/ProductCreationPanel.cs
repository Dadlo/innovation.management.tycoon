using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductCreationPanel : ShowablePanel {

	public struct ProductOptionsContainer {
		public List<ProductOption> productOptionsList;
	}

	public ProductOptionsContainer pConcepts, pDevOptions, pMonetOptions;

	public TextAsset productConceptsAsset, productDevOptionsAsset, productMonetizationsAsset;

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


	public void DoneEditingProductName() {
		titleText.text = prodNameInputField.text;
	}

    /// <summary>
    /// reseta os conteudos desse painel, como se o usuario nunca tivesse mexido
    /// </summary>
	public void ResetPanels() {
		titleText.text = "Novo Produto";
		prodNameInputField.text = "Novo Produto";
		SetActivePanel(0);
        pickedConcepts.Clear();
        pickedDevOptions.Clear();
        pickedMonetOptions.Clear();
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

	void OnEnable() {
		ResetPanels();
	}

	// Use this for initialization
	void Start () {
		pConcepts = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productConceptsAsset);
		pDevOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productDevOptionsAsset);
		pMonetOptions = PersistenceHandler.LoadFromFile<ProductOptionsContainer>(productMonetizationsAsset);

		//nos registramos nos eventos de cada toggle para ficar sabendo quando um foi clicado e fazer o que precisarmos fazer de acordo
		for(int i = 0; i < sectionToggles.Length; i++) {
			sectionToggles[i].GetComponent<ProductCreationSectionToggle>().onToggled += OnToldToChangeSections;
		}


        //preenchemos as listas de opcoes...
        FillOptionsList(pConcepts.productOptionsList, ProductOptionListEntry.OptionType.concept, conceptsListContainer);
        FillOptionsList(pDevOptions.productOptionsList, ProductOptionListEntry.OptionType.dev, devListContainer);
        FillOptionsList(pMonetOptions.productOptionsList, ProductOptionListEntry.OptionType.monet, monetListContainer);
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

    public void OnPickedProdOption(ProductOptionListEntry pickedOption)
    {
        switch (pickedOption.myOptionType)
        {
            case ProductOptionListEntry.OptionType.concept:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedConcepts.Add(pickedOption.theOptionRepresented);
                    totalConceptsCost += pickedOption.theOptionRepresented.cost;
                    totalConceptsCostText.text = totalConceptsCost.ToString();
                }
                else
                {
                    pickedConcepts.Remove(pickedOption.theOptionRepresented);
                    totalConceptsCost -= pickedOption.theOptionRepresented.cost;
                    totalConceptsCostText.text = totalConceptsCost.ToString();
                }
                break;
            case ProductOptionListEntry.OptionType.dev:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedDevOptions.Add(pickedOption.theOptionRepresented);
                    totalDevCost += pickedOption.theOptionRepresented.cost;
                    totalDevCostText.text = totalDevCost.ToString();
                }
                else
                {
                    pickedDevOptions.Remove(pickedOption.theOptionRepresented);
                    totalDevCost -= pickedOption.theOptionRepresented.cost;
                    totalDevCostText.text = totalDevCost.ToString();
                }
                break;
            case ProductOptionListEntry.OptionType.monet:
                if (pickedOption.activeToggle.isOn)
                {
                    pickedMonetOptions.Add(pickedOption.theOptionRepresented);
                    totalMonetCost += pickedOption.theOptionRepresented.cost;
                    totalMonetCostText.text = totalMonetCost.ToString();
                }
                else
                {
                    pickedMonetOptions.Remove(pickedOption.theOptionRepresented);
                    totalMonetCost -= pickedOption.theOptionRepresented.cost;
                    totalMonetCostText.text = totalMonetCost.ToString();
                }
                break;
        }
    }

	public void OnToldToChangeSections(Transform newSectionToggle) {
		SetActivePanel(newSectionToggle.GetSiblingIndex());
	}

    /// <summary>
    /// reune os dados contidos nesse painel e cria um novo produto, que entra na fase de concepcao
    /// </summary>
    public void CreateProduct()
    {
        //new Product ou algo assim, recebendo as porcentagens de foco, nome fornecido e etc
    }
	
}
