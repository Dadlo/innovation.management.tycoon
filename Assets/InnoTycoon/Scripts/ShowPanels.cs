using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject optionsPanel;							//Store a reference to the Game Object OptionsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
	public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 
	public GameObject HUDPanel;								//Store a reference to the Game Object HUD 
	public GameObject ScrollerPanel;                        //Store a reference to the Game Object Scroller 
    public GameObject gestaoPanel;
	public ProductsPanel productsPanel;
    public StudiesPanel studiesPanel;
	public ProductCreationPanel pCreationPanel;



	//Call this function to activate and display the Options panel during the main menu
	public void ShowOptionsPanel()
	{
		optionsPanel.SetActive(true);
		optionsTint.SetActive(true);
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
	
	//Call this function to activate and display the scroll menu panel during the main menu
	public void ShowScroll()
	{
		ScrollerPanel.SetActive (true);
	}

	//Call this function to deactivate and hide the scroll menu panel during the main menu
	public void HideScroll()
	{
		ScrollerPanel.SetActive (false);
	}

	//Call this function to activate and display the Pause panel during game play
	public void ShowPausePanel()
	{
		pausePanel.SetActive (true);
		optionsTint.SetActive(true);
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

    public void ToggleGestaoPanelsDisplay(bool shouldDisplay)
    {
        gestaoPanel.SetActive(shouldDisplay);
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
