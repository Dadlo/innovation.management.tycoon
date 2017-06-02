using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestLoadedSaveBtn : MonoBehaviour {

	public SavedGame savedGame;

	public Text savedGameNameText;


	public void SetBtnContents(SavedGame theSavedGame, string theSavedGameName) {
		savedGameNameText.text = theSavedGameName;
		savedGame = theSavedGame;
	}

	public void LoadThisGame() {
		PersistenceTestUI.instancia.GetDataFromSave(savedGame);
	}
}
