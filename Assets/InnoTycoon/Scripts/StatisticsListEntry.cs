using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsListEntry : MonoBehaviour {
    public Text nameTxt, curIncomeTxt, curMoneyTxt, numProdsTxt;

    /// <summary>
    /// mostra o valor recebido/perdido com a cor certa e com a formatacao adequada
    /// </summary>
    /// <param name="income"></param>
    public void SetIncomeValue(int income)
    {
        curIncomeTxt.text = GameManager.ConvertNumberToCoinString(Mathf.Abs(income));
        if (income > 0)
        {
            curIncomeTxt.color = PersistenceActivator.instance.profitCostColor;
        }
        else
        {
            curIncomeTxt.color = PersistenceActivator.instance.lossCostColor;
            curIncomeTxt.text = curIncomeTxt.text.Insert(0, "-");
        }
        

    }
}
