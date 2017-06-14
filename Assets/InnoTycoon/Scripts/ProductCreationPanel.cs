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
	}

	public void SetActivePanel(int panelIndex) {
		for(int i = 0; i < sectionContents.Length; i++) {
			if(i == panelIndex) {
				sectionContents[i].interactable = true;
				sectionContents[i].alpha = 1;
				if (!sectionToggles[i].isOn) {
					sectionToggles[i].isOn = true;
				}
			}
			else {
				sectionContents[i].interactable = false;
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
