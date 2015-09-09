using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public MenuManager menuManager;

	public GameObject ui;
	// Use this for initialization
	void Start () {
	

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void onBattle(){
		Debug.Log ("onBattle");
	}

	public void onPers(){
		Debug.Log ("onPers");
	}
	public void onOptions(){
		//MenuManager.State = MenuManager.STATE.Settings;
		menuManager.State = MenuManager.STATE.Settings;
		Debug.Log ("onOptions");
	}
	public void onLogOut(){
		
	}
	public void onExit(){
		Debug.Log ("onExit");
		Application.Quit ();
	}
	public void setVisible( bool b ){

		ui.SetActive (b);
	}
}
