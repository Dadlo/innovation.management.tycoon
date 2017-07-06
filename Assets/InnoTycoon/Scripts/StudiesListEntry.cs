using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudiesListEntry : MonoBehaviour {

    public StudyOption myOption;

    public Text titleText, descriptionText, stepsText, costText;

    public Button studyBtn;

    public Text studyBtnText;

   
    public void SetMyContent(StudyOption myOption)
    {
        this.myOption = myOption;
        titleText.text = myOption.title;
        stepsText.text = myOption.steps.ToString();
        costText.text = myOption.cost.ToString();
        descriptionText.text = string.Concat(myOption.description, " (", myOption.type,")");

        studyBtn.interactable = false; //a menos que nao tenha um estudo sendo feito, nao podemos interagir com os botoes

        if (PersistenceActivator.instance.curGameData.studiesList.Contains(myOption.title))
        {
            studyBtnText.text = "Estudado";
        }
        else if(PersistenceActivator.instance.curGameData.studyDoing == myOption.title)
        {
            studyBtnText.text = string.Concat("Estudando... dia ", PersistenceActivator.instance.curGameData.curStudyStep.ToString(), " de ",
                myOption.steps.ToString());
        }
        else if(PersistenceActivator.instance.curGameData.studyDoing != "")
        {
            studyBtnText.text = "Aguardando t\u00E9rmino de estudo";
        }
        else
        {
            studyBtnText.text = "Estudar";
            studyBtn.interactable = true;
        }
    }

    public void OnMyButtonPressed()
    {
        ModalPanel.Instance().MessageBox(null, "Confirmar Estudo", string.Concat("Estudar '", myOption.title,"' por ",
            myOption.steps.ToString(), " dias, pagando R$", myOption.cost.ToString(),",00 por dia?"), StartThisStudy, null, null, null, false, "YesNo");
    }

    public void StartThisStudy()
    {
        GameManager.instance.showPanels.ToggleStudyPanelsDisplay(false);
        GameManager.instance.StartNewPlayerStudy(myOption);
    }
	
}
