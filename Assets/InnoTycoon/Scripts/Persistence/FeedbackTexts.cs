using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// os qualquer texto pode ser guardado aqui.
/// na hora de pegar o texto, podemos trocar a lista da qual vamos buscar
/// e, assim, fazer a localizacao.
/// os textos fornecidos aqui precisam ter o texto de identificacao e o texto de conteudo em si separados por um |
/// </summary>
public class FeedbackTexts {

	private Dictionary<string, string> organizedTextsDict = new Dictionary<string, string>();

	public List<string> theTextList;

	public string GetText(string textID) {
		return organizedTextsDict[textID];
	}

	public void GetOrganizedTextsFromList() {
		for (int i = 0; i < theTextList.Count; i++) {
			string[] textAndId = theTextList[i].Split('|');
			organizedTextsDict.Add(textAndId[0], textAndId[1]);
			//Debug.Log(string.Concat("id: ", textAndId[0], " texto: ", textAndId[1]));
		}
		
	}
}
