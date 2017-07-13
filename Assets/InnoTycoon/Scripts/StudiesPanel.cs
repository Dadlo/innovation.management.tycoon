using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyOptionsContainer
{
    public List<StudyOption> studyOptionsList;
}

public class StudiesPanel : ShowablePanel {

    public Transform studyListParent;

    public GameObject studyListEntryPrefab;

    /// <summary>
    /// cria um prefab de entrada da lista com os dados do targetProduct
    /// </summary>
    /// <param name="targetStudy"></param>
    public void AddStudyToList(StudyOption targetStudy)
    {
        GameObject newEntry = Instantiate(studyListEntryPrefab);
        newEntry.transform.SetParent(studyListParent, false);
        newEntry.GetComponent<StudiesListEntry>().SetMyContent(targetStudy);
    }

    /// <summary>
    /// destroi todas as entradas da lista.
    /// a ideia seria limpar a lista antes de popular novamente para que nao tenhamos produtos repetidos
    /// </summary>
    public void ClearProductList()
    {
        for (int i = 0; i < studyListParent.childCount; i++)
        {
            Destroy(studyListParent.GetChild(i).gameObject);
        }
    }

    void OnEnable()
    {
        ClearProductList();
        for (int i = 0; i < GameManager.instance.studies.studyOptionsList.Count; i++)
        {
            AddStudyToList(GameManager.instance.studies.studyOptionsList[i]);
        }
    }
}
