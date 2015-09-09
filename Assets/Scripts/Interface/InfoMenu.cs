using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour {

	public MenuManager menuManager;

	public GameObject ui;

	public Text titleText;
	public Text msgText;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setText( string title, string msg ){

		titleText.text = title;
		msgText.text = msg;
	}

	public void onBack(){

		ui.SetActive (false);
		//menuManager.State = MenuManager.STATE.MainMenu;
	}

	public void setVisible( bool b ){
		
		ui.SetActive (b);
	}
}
