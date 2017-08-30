using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

// nao usado
// serve de exemplo para retirar as funcoes dos botoes de alerta ate ter todos os padroes no codigo

public class UseModalWindow : MonoBehaviour
  {
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class
	private DisplayManager DisplayManager;   //reference to the DisplayManager Class

	public Sprite ErrorIcon;                 //Your error icon
	public Sprite InformationIcon;           //Your information icon
	public Sprite WarningIcon;               //Your warning icon
	public Sprite QuestionIcon;              //Your question icon

	void Awake()
	  {
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
		DisplayManager = DisplayManager.Instance(); //Instantiate the Display Manager
	  }
	//Test function:  Pop up the Modal Window with Yes, No, and Cancel buttons.
	public void TestYNC()
	  {
		Sprite icon = null;
		ModalPanel.MessageBox(icon, "Test Yes No Cancel", "Would you like a poke in the eye?\nHow about with a sharp stick?", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "YesNoCancel");
	  }
	//Test function:  Pop up the Modal Window with Yes and No buttons.
	public void TestYN()
	 {
		Sprite icon = null;
		ModalPanel.MessageBox(icon, "Test Yes No", "Answer 'Yes' or 'No':", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "YesNo");
	  }
	//Test function:  Pop up the Modal Window with an Ok button.
	public void TestOk()
	  {
		Sprite icon = null;
		ModalPanel.MessageBox(icon, "Saving...", "Game saved.", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "Ok");
	  }
	//Test function:  Pop up the Modal Window with an Ok button and an Icon.
	public void TestOkIcon()
	  {
		Sprite icon = null;
		ModalPanel.MessageBox(icon, "Test OK Icon", "Press Ok.", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, true, "Ok");
	  }
	//Test function:  Do something if the "Yes" button is clicked.
	void TestYesFunction()
	  {
		//
	  }
	//Test function:  Do something if the "No" button is clicked.
	void TestNoFunction()
	  {
		//
	  }
	//Test function:  Do something if the "Cancel" button is clicked.
	void TestCancelFunction()
	  {
		//
	  }
	//Test function:  Do something if the "Ok" button is clicked.
	void TestOkFunction()
	  {
		//
	  }
  }