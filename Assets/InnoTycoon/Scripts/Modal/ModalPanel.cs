using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class ModalPanel : MonoBehaviour
  {

	public class QueuedModalInfo {
		public Sprite iconPic;
		public string title, question;
		public UnityAction yesEvent, noEvent, cancelEvent, okEvent;
		public bool iconActive;
		public string messageType;

		public QueuedModalInfo(Sprite iconPic, string title, string question, UnityAction yesEvent, UnityAction noEvent, UnityAction cancelEvent,
			UnityAction okEvent, bool iconActive, string messageType) {
			this.iconPic = iconPic;
			this.title = title;
			this.question = question;
			this.yesEvent = yesEvent;
			this.noEvent = noEvent;
			this.cancelEvent = cancelEvent;
			this.okEvent = okEvent;
			this.iconActive = iconActive;
			this.messageType = messageType;
		}
	}

	public Text   Title;     //The Modal Window Title
	public Text   Question;  //The Modal Window Question (or statement)
	public Button Button1;   //The first button
	public Button Button2;   //The second button
	public Button Button3;   //The third button
	public Image  IconImage; //The Icon Image, if any

	public GameObject ModalPanelObject;       //Reference to the Panel Object
	private static ModalPanel MainModalPanel; //Reference to the Modal Panel, to make sure it's been included

	private List<QueuedModalInfo> queuedModals = new List<QueuedModalInfo>();

	public static ModalPanel Instance()
	  {
		if (!MainModalPanel)
		  {
			MainModalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
			if (!MainModalPanel)
			  {
				Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
			  }
		  }
		return MainModalPanel;
	  }

	public void MessageBox(Sprite IconPic, string Title, string Question, UnityAction YesEvent, UnityAction NoEvent, UnityAction CancelEvent, UnityAction OkEvent, bool IconActive, string MessageType)
	  {
		//if the panel is already open, we queue a new opening with the desired info.
		//the new panel will open as soon as the current one is closed
		if (ModalPanelObject.activeSelf) {
			queuedModals.Add(new QueuedModalInfo(IconPic, Title, Question, YesEvent, NoEvent, CancelEvent, OkEvent, IconActive, MessageType));
			return;
		}
		ModalPanelObject.SetActive(true);  //Activate the Panel; its default is "off" in the Inspector
		if (MessageType == "YesNoCancel")  //If the user has asked for the Message Box type "YesNoCancel"
		  {
			//Button1 is on the far left; Button2 is in the center and Button3 is on the right.  Each can be activated and labeled individually.
			Button1.onClick.RemoveAllListeners (); Button1.onClick.AddListener(YesEvent);    Button1.onClick.AddListener(ClosePanel); Button1.GetComponentInChildren<Text>().text = "Sim";
			Button2.onClick.RemoveAllListeners (); Button2.onClick.AddListener(NoEvent);     Button2.onClick.AddListener(ClosePanel); Button2.GetComponentInChildren<Text>().text = "Não";
			Button3.onClick.RemoveAllListeners (); Button3.onClick.AddListener(CancelEvent); Button3.onClick.AddListener(ClosePanel); Button3.GetComponentInChildren<Text>().text = "Cancelar";
			Button1.gameObject.SetActive(true); //We always turn on ONLY the buttons we need, and leave the rest off.
			Button2.gameObject.SetActive(true);
			Button3.gameObject.SetActive(true);
		  }
		if (MessageType == "YesNo")        //If the user has asked for the Message Box type "YesNo"
		  {
			Button1.onClick.RemoveAllListeners (); 
			Button2.onClick.RemoveAllListeners (); Button2.onClick.AddListener(YesEvent);     Button2.onClick.AddListener(ClosePanel); Button2.GetComponentInChildren<Text>().text = "Sim";
			Button3.onClick.RemoveAllListeners (); Button3.onClick.AddListener(NoEvent);      Button3.onClick.AddListener(ClosePanel); Button3.GetComponentInChildren<Text>().text = "Não";
			Button1.gameObject.SetActive(false);
			Button2.gameObject.SetActive(true);
			Button3.gameObject.SetActive(true);
		  }
		if (MessageType == "Ok")           //If the user has asked for the Message Box type "Ok"
		  {
			Button1.onClick.RemoveAllListeners ();
			Button2.onClick.RemoveAllListeners (); Button2.onClick.AddListener(OkEvent);     Button2.onClick.AddListener(ClosePanel); Button2.GetComponentInChildren<Text>().text = "Ok";
			Button3.onClick.RemoveAllListeners ();
			Button1.gameObject.SetActive(false);
			Button2.gameObject.SetActive(true);
			Button3.gameObject.SetActive(false);
		  }
		this.Title.text = Title;           //Fill in the Title part of the Message Box
		this.Question.text = Question;     //Fill in the Question/statement part of the Messsage Box
		if (IconActive)                    //If the Icon is active (true)...
		  {
			this.IconImage.gameObject.SetActive(true);  //Turn on the icon,
			this.IconImage.sprite = IconPic;            //and assign the picture.
		  }
		else
		  {
			//this.IconImage.gameObject.SetActive(false); //Turn off the icon.
		  }
  	  }

	public void MessageBox(QueuedModalInfo info) {
		MessageBox(info.iconPic, info.title, info.question, info.yesEvent, info.noEvent, info.cancelEvent, info.okEvent, info.iconActive, info.messageType);
	}

    /// <summary>
    /// janela com opcoes de sim e nao que podem fazer algo
    /// </summary>
    /// <param name="windowTitle"></param>
    /// <param name="windowText"></param>
    /// <param name="yesEvent"></param>
    /// <param name="noEvent"></param>
    public void YesNoBox(string windowTitle, string windowText, UnityAction yesEvent, UnityAction noEvent)
    {
        MessageBox(null, windowTitle, windowText, yesEvent, noEvent, PersistenceActivator.NothingFunction, PersistenceActivator.NothingFunction, false, "YesNo");
    }

    /// <summary>
    /// janela que aparece apenas para notificar o jogador sobre algo
    /// </summary>
    /// <param name="windowTitle"></param>
    /// <param name="windowText"></param>
    public void OkBox(string windowTitle, string windowText)
    {
        MessageBox(null, windowTitle, windowText, PersistenceActivator.NothingFunction,
            PersistenceActivator.NothingFunction, PersistenceActivator.NothingFunction, PersistenceActivator.NothingFunction, false, "Ok");
    }

	void ClosePanel()
	  {
		ModalPanelObject.SetActive(false); //Close the Modal Dialog
		if(queuedModals.Count > 0) {
			MessageBox(queuedModals[0]);
			queuedModals.RemoveAt(0);
		}
	  }
  }
