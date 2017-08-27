using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayableGraphicFeedback : MonoBehaviour {

    
    /// <summary>
    /// o nivel de produto feito, comecando em 0, que ira fazer esse objeto surgir
    /// </summary>
    [Range(0, 4)]
    public int feedbackLevel = 0;

	// Use this for initialization
	void Start () {
        GraphicFeedbacksManager.instance.RegisterGraphic(gameObject, feedbackLevel);
        gameObject.SetActive(false);
	}
	
}
