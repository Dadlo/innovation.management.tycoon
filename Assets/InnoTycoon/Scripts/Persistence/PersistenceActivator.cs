//PersistenceActivator

using UnityEngine;
using UnityEngine.UI;

public class PersistenceActivator : MonoBehaviour {

	public int sceneToStart = 1;

	// saved Data
	public int day;
	public int capital;
	public int cost;
	public string studiesList;
	public string studyDoing;
	public string productsList;
	public string productDoing;
	public int conceptStep;
	public int conceptStepTotal;
	public int devStep;
	public int devStepTotal;
	public int monetStep;
	public int monetStepTotal;

	// UI text fields
	public Text diaCalendarioUI;
	public Text capitalUI;
	public Text costUI;

	public static PersistenceActivator instance;
	private StartOptions startoptions;
	private DevSteps DevSteps;
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class
	private ShowPanels showPanels;
	private Sprite icon = null;


	public float nextAmountCon;
	public float nextAmountDev;
	public float nextAmountMon;
	public bool ConActive;
	public bool DevActive;
	public bool MonActive;

	// Use this for initialization
	void Awake () {
		instance = this;
		DevSteps = DevSteps.Instance();
		startoptions = GetComponent<StartOptions> ();
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
		showPanels = GetComponent<ShowPanels> ();
	}

	public void SaveAllData() {
		SavedGame newSave = new SavedGame();
		// Increase Day
		day++;
		// Take cost from capital
		// EndGame Conditional
		if(capital <= cost) {
			EndGame();
			Debug.Log("Game Over");
		} else {
			capital=capital-cost;

			// Atualiza passos
			UpdateSteps(true);

			// Save Data
			newSave.day = day;
			newSave.capital = capital;
			newSave.cost = cost;
			newSave.studiesList = studiesList;
			newSave.studyDoing = studyDoing;
			newSave.productsList = productsList;
			newSave.productDoing = productDoing;
			newSave.conceptStep = conceptStep;
			newSave.conceptStepTotal = conceptStepTotal;
			newSave.devStep = devStep;
			newSave.devStepTotal = devStepTotal;
			newSave.monetStep = monetStep;
			newSave.monetStepTotal = monetStepTotal;

			PersistenceHandler.SaveToFile(newSave, "save01", false);
			ModalPanel.MessageBox(icon, "Saving data...", "All data was saved.", NothingFunction, NothingFunction, NothingFunction, NothingFunction, false, "Ok");
			LoadAllData();
		}
	}
	public void EndGame() {
		ModalPanel.MessageBox(icon, "Game Over", "You have gone bankrupted ! You've Lost!\n\nYou can see your status from this game,\nbut you'll need to start a new game from the\nmenu to play again.", NothingFunction, NothingFunction, NothingFunction, NothingFunction, false, "Ok");
	}
	public void UpdateSteps(bool increase) {
		if (conceptStepTotal > 0) {
			ConActive = true;
			if(increase){conceptStep++;}
			nextAmountCon = (conceptStep*100/conceptStepTotal);
			if(nextAmountCon >= 100) {
				conceptStep = 0;
				conceptStepTotal = 0;
			}
		} else {
			ConActive = false;
			nextAmountCon = 0;
		}
		if (devStepTotal > 0 && conceptStep == conceptStepTotal && !ConActive) {
			DevActive = true;
			if(increase){devStep++;}
			nextAmountDev = (devStep*100/devStepTotal);
			if(nextAmountDev >= 100) {
				devStep = 0;
				devStepTotal = 0;
			}
		} else {
			DevActive = false;
			nextAmountDev = 0;
		}
		if (monetStepTotal > 0 && devStep == devStepTotal && !DevActive) {
			MonActive = true;
			if(increase){monetStep++;}
			nextAmountMon = (monetStep*100/monetStepTotal);
			if(nextAmountMon >= 100) {
				monetStep = 0;
				monetStepTotal = 0;
			}
		} else {
			MonActive = false;
			nextAmountMon = 0;
		}
		DevSteps.SetData(ConActive,DevActive,MonActive,nextAmountCon,nextAmountDev,nextAmountMon);
	}

	// Do nothing on ok
	void NothingFunction()
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
		day = 1;
		capital = 20000;
		cost = 2000;
		studiesList = "";
		studyDoing = "";
		productsList = "";
		productDoing = "";
		conceptStep = 0;
		conceptStepTotal = 0;
		devStep = 0;
		devStepTotal = 0;
		monetStep = 0;
		monetStepTotal = 0;
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
			ModalPanel.MessageBox(icon, "No load available", "No saved input to load \nA New Game will be started.", NothingFunction, NothingFunction, NothingFunction, NothingFunction, false, "Ok");
			Debug.Log("There is no data to load");
		}
	}

	public void LoadGame() {
		LoadAllData();
		UpdateSteps(false);
		StartGameFunction();
	}

	public void NewGame() {
		SavedGame loadedGame = PersistenceHandler.LoadFromFile<SavedGame>("save01");
		if(loadedGame == null) { // no data saved - start a new game
			ResetAllValues();
			RenderAllChanges();
			StartNewGameFunction();
		}
		else { // there is a save - confirmation to button to remove data, if no: nothing, if yes: new game
			ModalPanel.MessageBox(icon, "There is a saved file", "Do you want to start a New Game?\n\nAll data will be lost if you do!", StartNewGameFunction, NothingFunction, NothingFunction, NothingFunction, false, "YesNo");
		}
	}

	public void GetDataFromSave(SavedGame theSave) {
		// load all data from SavedGame
		day = theSave.day;
		capital = theSave.capital;
		cost = theSave.cost;
		studiesList = theSave.studiesList;
		studyDoing = theSave.studyDoing;
		productsList = theSave.productsList;
		productDoing = theSave.productDoing;
		conceptStep = theSave.conceptStep;
		conceptStepTotal = theSave.conceptStepTotal;
		devStep = theSave.devStep;
		devStepTotal = theSave.devStepTotal;
		monetStep = theSave.monetStep;
		monetStepTotal = theSave.monetStepTotal;

		// render all ui
		RenderAllChanges();
	}
	public void RenderAllChanges() {
		// update UI
		diaCalendarioUI.text = day.ToString(); // atualiza dia do calendario
		capitalUI.text = capital.ToString(); // atualiza capital total
		costUI.text = cost.ToString(); // atualiza custo	
	}
}