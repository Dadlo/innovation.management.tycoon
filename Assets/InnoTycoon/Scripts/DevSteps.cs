using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevSteps : MonoBehaviour {
	
	public Transform LoadingBarCon1;
	public Transform LoadingBarCon2;
	public Transform LoadingBarCon3;
	public Transform LoadingBarCon4;
	public Transform LoadingBarCon5;
	public Transform LoadingBarDev1;
	public Transform LoadingBarDev2;
	public Transform LoadingBarDev3;
	public Transform LoadingBarDev4;
	public Transform LoadingBarDev5;
	public Transform LoadingBarMon1;
	public Transform LoadingBarMon2;
	public Transform LoadingBarMon3;
	public Transform LoadingBarMon4;
	public Transform LoadingBarMon5;
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
				LoadingBarCon1.GetComponent<Image>().fillAmount = currentAmountCon / 100;
			} else {
				currentAmountCon = 0;
			}
			if(DevActive){
				if(currentAmountDev < nextAmountDev) {
					currentAmountDev += speed * Time.deltaTime;
				}
				LoadingBarDev1.GetComponent<Image>().fillAmount = currentAmountDev / 100;
			} else {
				currentAmountDev = 0;
			}
			if(MonActive){
				if(currentAmountMon < nextAmountMon) {
					currentAmountMon += speed * Time.deltaTime;
				}
				// Enquanto ativo precisa adicionar dinheiro do produto ativo
				// Ao ativar o MonActive a primeira vez precisa retornar msg de aviso sobre os dados de retorno financeiro e nota do produto
				LoadingBarCon1.GetComponent<Image>().fillAmount = currentAmountCon / 100;
				LoadingBarDev1.GetComponent<Image>().fillAmount = currentAmountDev / 100;
				LoadingBarMon1.GetComponent<Image>().fillAmount = (100 - currentAmountMon) / 100;
			}
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