using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudiesListEntry : MonoBehaviour {

    public StudyOption myOption;

    public Text titleText, descriptionText, costText;

    public Button studyBtn;

    public Text studyBtnText;

   
    public void SetMyContent(StudyOption myOption)
    {
        this.myOption = myOption;
        titleText.text = myOption.title;
        costText.text = string.Concat(GameManager.ConvertNumberToCoinString(myOption.cost), "/dia");
        descriptionText.text = string.Concat(myOption.description, " (", myOption.type,")");

        studyBtn.interactable = false; //a menos que nao tenha um estudo sendo feito, nao podemos interagir com os botoes
		studyBtnText.color = Color.white;

        if (PersistenceActivator.instance.curGameData.studiesList.Contains(myOption.skillId))
        {
            studyBtnText.text = "Estudado";
        }
        else if(PersistenceActivator.instance.curGameData.studyDoing == myOption.skillId)
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
            studyBtnText.text = string.Concat("Estudar (", myOption.steps.ToString(), " dias)");
            studyBtn.interactable = true;
			studyBtnText.color = Color.black;
		}
    }

    public void OnMyButtonPressed()
    {
        ModalPanel.Instance().YesNoBox("Confirmar Estudo", string.Concat("Estudar '", myOption.title,"' por ",
            myOption.steps.ToString(), " dias, pagando ", GameManager.ConvertNumberToCoinString(myOption.cost)," por dia?"), StartThisStudy,
            PersistenceActivator.NothingFunction);
    }

    public void StartThisStudy()
    {
        GameManager.instance.showPanels.ToggleStudyPanelsDisplay(false);
        GameManager.instance.StartNewPlayerStudy(myOption);
    }
	
}
