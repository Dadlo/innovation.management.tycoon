using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicFeedbacksManager : MonoBehaviour {

    public List<List<GameObject>> graphicsList = new List<List<GameObject>>();

    public static GraphicFeedbacksManager instance;

    public void Awake()
    {
        //sao 5 niveis de produtos possiveis, entao vamos criar 5 listas
        for(int i = 0; i < 5; i++)
        {
            graphicsList.Add(new List<GameObject>());
        }

        instance = this;
    }

    public void RegisterGraphic(GameObject theGraphic, int level)
    {
        graphicsList[level].Add(theGraphic);
    }

    /// <summary>
    /// exibe um novo grafico de feedback do nivel fornecido e retorna o nome do grafico escolhido, ou string.empty se nenhum foi escolhido
    /// (se todos ja estavam sendo usados, por exemplo)
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public string ShowANewFeedback(int level)
    {
        if (graphicsList[level].Count > 0)
        {
            int pickedIndex = Random.Range(0, graphicsList[level].Count);
            if (graphicsList[level][pickedIndex].activeSelf)
            {
                return string.Empty;
            }
            else
            {
                graphicsList[level][pickedIndex].SetActive(true);
                return graphicsList[level][pickedIndex].name;
            }
        }
        else return string.Empty;
        
    }

    public void ShowSpecificGraphic(int level, string name)
    {
        for(int i = 0; i < graphicsList[level].Count; i++)
        {
            if (graphicsList[level][i].name == name)
            {
                graphicsList[level][i].SetActive(true);
                return;
            }
        }
    }

    public void ClearAllGraphics()
    {
        for(int i = 0; i < graphicsList.Count; i++)
        {
            for(int j = 0; j < graphicsList[i].Count; j++)
            {
                graphicsList[i][j].SetActive(false);
            }
        }
    }
}
