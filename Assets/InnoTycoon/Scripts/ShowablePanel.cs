using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// um painel que possui uma funcao ToggleDisplay para ser mostrado e escondido
/// </summary>
public class ShowablePanel : MonoBehaviour {

	/// <summary>
	/// mostra ou esconde esse painel
	/// </summary>
	/// <param name="shouldDisplay"></param>
	public void ToggleDisplay(bool shouldDisplay) {
		gameObject.SetActive(shouldDisplay);
	}
}

