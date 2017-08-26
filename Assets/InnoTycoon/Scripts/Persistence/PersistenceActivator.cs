//PersistenceActivator

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PersistenceActivator : MonoBehaviour {

	public int sceneToStart = 1;

	// saved Data
	public SavedGame curGameData;

	// UI text fields
	public Text diaCalendarioUI;
	public Text capitalUI;
	public Text costUI;

	public Text costPrefixUI;
	public Text costSuffixUI;

	public Color profitCostColor, lossCostColor;

	public static PersistenceActivator instance;
	private StartOptions startoptions;
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class
	private Sprite icon = null;


	// Use this for initialization
	void Awake () {
		instance = this;
		startoptions = GetComponent<StartOptions> ();
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
	}

	public void SaveAllData() {
		// EndGame Conditional
		if (curGameData.capital <= curGameData.cost) {
			EndGame();
			Debug.Log("Game Over");
		} else {
			// Increase Day
			GameManager.instance.GoToNextDay();
			PersistenceHandler.SaveToFile(curGameData, "save01", false);
			//ModalPanel.MessageBox(icon, "Saving data...", "All data was saved.", NothingFunction, NothingFunction, NothingFunction, NothingFunction, false, "Ok");
			RenderAllChanges();
		}
	}
	public void EndGame() {
		ModalPanel.OkBox("Game Over", "You have gone bankrupted ! You've Lost!\n\nYou can see your status from this game,\nbut you'll need to start a new game from the\nmenu to play again.");
	}
	// Do nothing on ok
	public static void NothingFunction()
	{
	//
	}
	void StartGameFunction()
	{
		startoptions.StartGameInScene();
	}
	void StartNewGameFunction()
	{
		ResetAllValues();
		RenderAllChanges();
		startoptions.StartGameInScene();
	}

	void ResetAllValues()
	{
		// reset values
		curGameData.day = 1;
		curGameData.capital = 20000;
		curGameData.cost = 2000;
		curGameData.studiesList = new List<string>();
		curGameData.studyDoing = "";
		if(curGameData.productsList != null) {
			curGameData.productsList.Clear();
		}
		else {
			curGameData.productsList = new List<Product>();
		}
        curGameData.productsDoing = new List<Product>();

        curGameData.AiTycoons = new List<AITycoon>();
        curGameData.AiTycoons.Add(new AITycoon() { name = "Dumb Tycoon", intelligence = 0.2f});
        curGameData.AiTycoons.Add(new AITycoon() { name = "Average Tycoon", intelligence = 0.65f});
    }

	/// <summary>
	/// destroi todas as entradas da lista de saves e cria novas para cada save que for encontrado
	/// </summary>
	public void LoadAllData() {
		SavedGame loadedGame = PersistenceHandler.LoadFromFile<SavedGame>("save01");
		if(loadedGame != null) {
			// funcao de sucesso
			Debug.Log("Load ok");
			GetDataFromSave(loadedGame);
		}
		else {
			ModalPanel.OkBox("No load available", "No saved input to load \nA New Game will be started.");
			Debug.Log("There is no data to load");
		}
	}

	public void LoadGame() {
		StartGameFunction();
		LoadAllData();
	}

	public void NewGame() {
		SavedGame loadedGame = PersistenceHandler.LoadFromFile<SavedGame>("save01");
		if(loadedGame == null) { // no data saved - start a new game
			ResetAllValues();
			RenderAllChanges();
			StartNewGameFunction();
		}
		else { // there is a save - confirmation to button to remove data, if no: nothing, if yes: new game
			ModalPanel.YesNoBox("There is a saved file", "Do you want to start a New Game?\n\nAll data will be lost if you do!", StartNewGameFunction, NothingFunction);
		}
	}

	public void GetDataFromSave(SavedGame theSave) {
		// load all data from SavedGame
		curGameData = theSave;

		// render all ui
		RenderAllChanges();

        //setup productsDoing
        curGameData.productsDoing = new List<Product>();

        for(int i = 0; i < curGameData.productsList.Count; i++)
        {
            if(curGameData.productsList[i].MadeByPlayer && curGameData.productsList[i].currentPhase != Product.ProductPhase.done)
            {
                curGameData.productsDoing.Add(curGameData.productsList[i]);
            }
        }
		for(int i = 0; i < curGameData.productsDoing.Count; i++) {
			curGameData.productsDoing[i].UpdateLoadBar();
		}
	}
	public void RenderAllChanges() {
		// update UI
		diaCalendarioUI.text = curGameData.day.ToString(); // atualiza dia do calendario
		capitalUI.text = curGameData.capital.ToString(); // atualiza capital total
		// atualiza custo
		if (curGameData.cost > 0) {
			costUI.color = lossCostColor;
			costPrefixUI.color = lossCostColor;
			costSuffixUI.color = lossCostColor;
		}
		else {
			costUI.color = profitCostColor;
			costPrefixUI.color = profitCostColor;
			costSuffixUI.color = profitCostColor;
		}

		costUI.text = Mathf.Abs(curGameData.cost).ToString();

	}
}
