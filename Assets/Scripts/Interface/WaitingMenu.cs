using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingMenu : MonoBehaviour {

	public MenuManager menuManager;
	
	public GameObject ui;

	public RectTransform rectTransform;
	public Vector3 speed;
	private bool isVisible = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		rotateIcon ();
	}

	public void onCancel(){

		EventManagerUI.cancelWaitingConnect ();
		menuManager.State = MenuManager.STATE.AutorizationRegistration;
	}

	private void rotateIcon(){

		rectTransform.Rotate (speed);
	}

	public void setVisible( bool b ){
		
		ui.SetActive (b);
		isVisible = b;
	}
}
