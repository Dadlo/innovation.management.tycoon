//PersistenceActivator

using UnityEngine;
using UnityEngine.UI;

public class PersistenceActivator : MonoBehaviour {

	public int sceneToStart = 1;
	public int day;
	public static PersistenceActivator instance;
	public Text diaCalendario;
	private StartOptions startoptions;
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class
	private ShowPanels showPanels;
	private Sprite icon = null;

	// Use this for initialization
	void Awake () {
		instance = this;
		startoptions = GetComponent<StartOptions> ();
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
		showPanels = GetComponent<ShowPanels> ();
	}

	public void SaveAllData() {
		SavedGame newSave = new SavedGame();
		day++;
		newSave.day = day;
		PersistenceHandler.SaveToFile(newSave, "save01", false);
		ModalPanel.MessageBox(icon, "Saving data...", "All data was saved.", NothingFunction, NothingFunction, NothingFunction, NothingFunction, false, "Ok");
		LoadAllData();
	}
	// Do nothing on ok
	void NothingFunction()
	{
	//
	}
	void StartGameFunction()
	{
		startoptions.StartGameInScene();
		//showPanels.ShowPausePanel();
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
		StartGameFunction();
	}

	public void NewGame() {
		SavedGame loadedGame = PersistenceHandler.LoadFromFile<SavedGame>("save01");
		if(loadedGame == null) { // no data saved - start a new game
			StartGameFunction();
		}
		else { // there is a save - confirmation to button to remove data, if no: nothing, if yes: new game
			ModalPanel.MessageBox(icon, "There is a saved file", "Do you want to start a New Game?\n\nAll data will be lost if you do!", StartGameFunction, NothingFunction, NothingFunction, NothingFunction, false, "YesNo");
		}
	}

	public void GetDataFromSave(SavedGame theSave) {
		day = theSave.day;
		diaCalendario.text = day.ToString(); // atualiza dia do calendario
	}
}
