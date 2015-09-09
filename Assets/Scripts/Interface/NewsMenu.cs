using UnityEngine;
using System.Collections;

public class NewsMenu : MonoBehaviour {

	public MenuManager menuManager;

	public GameObject ui;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onBack(){
		
		menuManager.State = MenuManager.STATE.MainMenu;
	}


	public void setVisible( bool b ){
		
		ui.SetActive (b);
	}
}
