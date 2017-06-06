//PersistenceActivator

using UnityEngine;
using UnityEngine.UI;

public class PersistenceActivator : MonoBehaviour {

	public int day;
	public static PersistenceActivator instance;
	public Text diaCalendario;
	private StartOptions startoptions;
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class

	// Use this for initialization
	void Awake () {
		instance = this;
		startoptions = GetComponent<StartOptions> ();
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
	}

	public void SaveAllData() {
		SavedGame newSave = new SavedGame();
		day++;
		newSave.day = day;
		PersistenceHandler.SaveToFile(newSave, "save01", false);
		Sprite icon = null;
		ModalPanel.MessageBox(icon, "Saving data...", "All data was saved.", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "Ok");
		LoadAllData();
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
			Debug.Log("There is no data to load");
		}
	}


	public void LoadGame() {
		LoadAllData();
		startoptions.StartGameInScene();
	}
	

	public void GetDataFromSave(SavedGame theSave) {
		day = theSave.day;
		diaCalendario.text = day.ToString(); // atualiza dia do calendario
	}
}
