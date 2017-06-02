using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistenceTestUI : MonoBehaviour {

	public static PersistenceTestUI instancia;

	private SavedGame curLoadedSavedGame;

	public Text inputStringText, inputIntText, loadedStringText, loadedIntText;

	public GameObject loadedGameBtnPrefab;

	public RectTransform loadedGameBtnsContainer;

	int lastUsedSaveIndex = 0;

	// Use this for initialization
	void Awake () {
		instancia = this;
	}
	
	public void SaveProvidedData() {
		SavedGame newSave = new SavedGame();
		newSave.savedInteger = int.Parse(inputIntText.text);
		newSave.savedString = inputStringText.text;
		PersistenceHandler.SaveToFile(newSave, "save" + lastUsedSaveIndex.ToString(), true);
		lastUsedSaveIndex++;
		LoadAllData();
	}


	/// <summary>
	/// destroi todas as entradas da lista de saves e cria novas para cada save que for encontrado
	/// </summary>
	public void LoadAllData() {
		foreach(Transform childEntry in loadedGameBtnsContainer) {
			Destroy(childEntry.gameObject);
		}

		int lastLoadedSaveIndex = 0;
		bool couldLoadAGame = true;

		while (couldLoadAGame) {
			SavedGame loadedGame = PersistenceHandler.LoadFromFile<SavedGame>("save" + lastLoadedSaveIndex.ToString());
			if(loadedGame != null) {
				GameObject newSaveBtn = Instantiate(loadedGameBtnPrefab, loadedGameBtnsContainer);
				newSaveBtn.GetComponent<TestLoadedSaveBtn>().SetBtnContents(loadedGame, "save" + lastLoadedSaveIndex.ToString());
				lastLoadedSaveIndex++;
			}
			else {
				couldLoadAGame = false;
			}
			
		}
	}

	public void GetDataFromSave(SavedGame theSave) {
		loadedStringText.text = theSave.savedString;
		loadedIntText.text = theSave.savedInteger.ToString();
	}
}
