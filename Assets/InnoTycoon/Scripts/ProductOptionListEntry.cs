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
        activeToggle.isOn = theInfo.active;
        theOptionRepresented = theInfo;
        this.myOptionType = myOptionType;
        if (theInfo.active)
        {
            //ja adicionamos o custo dessa opcao entao
            onToggled(this);
        }
    }

    public void OnMyValueChanged(bool active)
    {
        //conferir se realmente podemos ser desativados (pode ser que a pessoa tenha que ter pelo menos uma opcao ativa)
        onToggled(this);
        
    }
}
