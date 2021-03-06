using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject HUDPanel;								//Store a reference to the Game Object HUD 
	public ProductsPanel productsPanel;
    public StudiesPanel studiesPanel;
	public ProductCreationPanel pCreationPanel;


	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		optionsTint.SetActive(true);
		optionsPanel.SetActive(true);
	}

	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideOptionsPanel()
	{
		optionsPanel.SetActive(false);
		optionsTint.SetActive(false);
	}

	//Call this function to activate and display the main menu panel during the main menu
	public void ShowMenu()
	{
		menuPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the main menu panel during the main menu
	public void HideMenu()
	{
		menuPanel.SetActive (false);
	}

	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		optionsTint.SetActive(true);
		pausePanel.SetActive (true);
	}

	//Call this function to activate and display the HUD during game play
	public void ShowHUD()
	{
		HUDPanel.SetActive (true);
	}

	//Call this function to activate and display the HUD during game play
	public void HideHUD()
	{
		HUDPanel.SetActive (false);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		pausePanel.SetActive (false);
		optionsTint.SetActive(false);

	}

    public void ShowObj(GameObject targetObj)
    {
        targetObj.SetActive(true);
    }

    public void HideObj(GameObject targetObj)
    {
        targetObj.SetActive(false);
    }

    public void ToggleProductPanelsDisplay(bool shouldDisplay) {
		productsPanel.ToggleDisplay(shouldDisplay);
	}

	public void ToggleCreationPanelsDisplay(bool shouldDisplay) {
		pCreationPanel.ToggleDisplay(shouldDisplay);
	}

    public void ToggleStudyPanelsDisplay(bool shouldDisplay)
    {
        studiesPanel.ToggleDisplay(shouldDisplay);
    }
}
