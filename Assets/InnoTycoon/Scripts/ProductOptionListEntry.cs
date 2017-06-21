using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductOptionListEntry : MonoBehaviour {

    public Text nameText, costText;

    public Toggle activeToggle;

    public bool beingInteractedWith = false;

    public delegate void OnToggled(ProductOptionListEntry theOption);

    public OnToggled onToggled;

    public ProductOption theOptionRepresented;

    public enum OptionType
    {
        concept,
        dev,
        monet
    }

    public OptionType myOptionType = OptionType.concept;

	public void SetContent(ProductOption theInfo, OptionType myOptionType)
    {
        nameText.text = theInfo.title;
        costText.text = string.Concat("+", theInfo.cost.ToString(), "/dia");
        theOptionRepresented = theInfo;
        this.myOptionType = myOptionType;

		//o onMyValueChanged ja roda caso essa opcao ja venha ativa
		activeToggle.isOn = theInfo.active;
    }

	/// <summary>
	/// a entrada como um todo pode ser clicada para a pessoa nao ter que ficar clicando so na caixinha do toggle.
	/// esse metodo cuida dessa parte de fazer a ligacao botao gigante-toggle
	/// </summary>
	public void BigButtonPress() {
		activeToggle.isOn = !activeToggle.isOn;
	}

    public void OnMyValueChanged(bool active)
    {
        //conferir se realmente podemos ser desativados (pode ser que a pessoa tenha que ter pelo menos uma opcao ativa)
        onToggled(this);
        
    }
}
