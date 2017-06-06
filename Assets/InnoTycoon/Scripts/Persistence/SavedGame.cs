// SavedGame.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedGame {

	public int day = 1;
	public int capital=20000;
	public int cost=2000;
	public string studiesList = "";
	public string studyDoing = "";
	public string productsList = "";
	public string productDoing = "";
	public int conceptStep = 0;
	public int conceptStepTotal = 0;
	public int devStep = 0;
	public int devStepTotal = 0;
	public int monetStep = 0;
	public int monetStepTotal = 0;

	public SavedGame() {}
}
