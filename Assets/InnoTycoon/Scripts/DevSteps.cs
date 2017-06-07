using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSteps : MonoBehaviour {
	
	public Transform LoadingBarCon;
	public Transform LoadingBarDev;
	public Transform LoadingBarMon;
	[SerializeField] public float currentAmountCon = 0;
	[SerializeField] public float currentAmountDev = 0;
	[SerializeField] public float currentAmountMon = 0;
	[SerializeField] public float nextAmountCon = 0;
	[SerializeField] public float nextAmountDev = 0;
	[SerializeField] public float nextAmountMon = 0;
	[SerializeField] public float speed = 30;
	public bool CovActive = false;
	public bool DevActive = false;
	public bool MonActive = false;
	private static DevSteps MainDevSteps;
	
	public static DevSteps Instance()
	  {
		if (!MainDevSteps)
		  {
			MainDevSteps = FindObjectOfType(typeof(DevSteps)) as DevSteps;
			if (!MainDevSteps)
			  {
				Debug.LogError("There needs to be one active DevSteps script on a GameObject in your scene.");
			  }
		  }
		return MainDevSteps;
	  }

	// Update is called onde per frame
	void Update () {
		if(CovActive || DevActive || MonActive){
			if(CovActive){
				if(currentAmountCon < nextAmountCon) {
					currentAmountCon += speed * Time.deltaTime;
				}
			}
			if(DevActive){
				if(currentAmountDev < nextAmountDev) {
					currentAmountDev += speed * Time.deltaTime;
				}
			}
			if(MonActive){
				if(currentAmountMon < nextAmountMon) {
					currentAmountMon += speed * Time.deltaTime;
				}
			}
			LoadingBarCon.GetComponent<Image>().fillAmount = currentAmountCon / 100;
			LoadingBarDev.GetComponent<Image>().fillAmount = currentAmountDev / 100;
			LoadingBarMon.GetComponent<Image>().fillAmount = currentAmountMon / 100;
		}
	}
	public void SetData(bool CovActiveR, bool DevActiveR, bool MonActiveR, float nextAmountConR, float nextAmountDevR, float nextAmountMonR) {
		// update vars
		CovActive =	CovActiveR;
		DevActive =	DevActiveR;
		MonActive =	MonActiveR;
		nextAmountCon =	nextAmountConR;
		nextAmountDev =	nextAmountDevR;
		nextAmountMon =	nextAmountMonR;
	}
}