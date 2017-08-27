using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsPanel : MonoBehaviour {

    public Transform statListParent;

    public GameObject statListEntryPrefab;

    /// <summary>
    /// cria um prefab de entrada da lista com os dados do aiTycoon
    /// </summary>
    /// <param name="targetTyco"></param>
    public void AddAiTycToList(AITycoon targetTyco)
    {
        GameObject newEntry = Instantiate(statListEntryPrefab);
        newEntry.transform.SetParent(statListParent, false);
        StatisticsListEntry entryScript = newEntry.GetComponent<StatisticsListEntry>();
        entryScript.nameTxt.text = targetTyco.name;
        entryScript.curMoneyTxt.text = GameManager.ConvertNumberToCoinString(targetTyco.curMoney);

        entryScript.SetIncomeValue(targetTyco.curIncome);
        if(targetTyco.weeksDownRemaining > 0)
        {
            entryScript.curMoneyTxt.color = Color.red;
            entryScript.curMoneyTxt.text = string.Concat(entryScript.curMoneyTxt.text, " (", GameManager.GetVariableText("bankrupted"), ")");
        }
        if(targetTyco.productDoing != null && targetTyco.productDoing.owner == targetTyco.name)
        {
            entryScript.numProdsTxt.text = targetTyco.productDoing.currentPhase == Product.ProductPhase.sales ?
                targetTyco.products.Count.ToString() : (targetTyco.products.Count - 1).ToString();
        }
        else
        {
            entryScript.numProdsTxt.text = targetTyco.products.Count.ToString();
        }
        
            
    }

    void AddPlayerEntry()
    {
        GameObject newEntry = Instantiate(statListEntryPrefab);
        newEntry.transform.SetParent(statListParent, false);
        StatisticsListEntry entryScript = newEntry.GetComponent<StatisticsListEntry>();
        entryScript.nameTxt.text = GameManager.GetVariableText("you");
        SavedGame persInstanceSave = PersistenceActivator.instance.curGameData;
        entryScript.curMoneyTxt.text = GameManager.ConvertNumberToCoinString(persInstanceSave.capital);
        entryScript.SetIncomeValue(-persInstanceSave.cost);
        entryScript.numProdsTxt.text = GameManager.instance.GetNumberOfPlayerCompletedProducts(true).ToString();
    }

    /// <summary>
    /// destroi todas as entradas da lista.
    /// a ideia seria limpar a lista antes de popular novamente para que nao tenhamos produtos repetidos
    /// </summary>
    public void ClearList()
    {
        for (int i = 0; i < statListParent.childCount; i++)
        {
            Destroy(statListParent.GetChild(i).gameObject);
        }
    }

    void OnEnable()
    {
        ClearList();
        for (int i = 0; i < PersistenceActivator.instance.curGameData.AiTycoons.Count; i++)
        {
            AddAiTycToList(PersistenceActivator.instance.curGameData.AiTycoons[i]);
        }

        AddPlayerEntry();
    }
}
