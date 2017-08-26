// SavedGame.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[System.Serializable]
public class SavedGame {

	public int day = 1;
	public int capital=20000;
	public int cost=2000;
	public List<string> studiesList;
	public string studyDoing = "";
    public int curStudyStep = 0;
	public List<Product> productsList;

    public List<AITycoon> AiTycoons;

    /// <summary>
    /// produtos sendo feitos pelo jogador.
    /// nao e salva, pois pegamos os produtos que estao sendo feitos pelo jogador
    /// via checagem da variavel "madeByPlayer"
    /// </summary>
    [XmlIgnore]
	public List<Product> productsDoing;

	public SavedGame() {}
}
