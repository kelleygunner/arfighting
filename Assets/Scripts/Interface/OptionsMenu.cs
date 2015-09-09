using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public MenuManager menuManager;

	public GameObject ui;

	public Slider sfxSlider;
	public Slider musicSlider;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onSFX(){


		Debug.Log ("onSFX" + sfxSlider.value.ToString());
	}

	public void onGraphics( int i = 0 ){

		
		Debug.Log ("onGraphics" + i.ToString());
	}
	public void onMusic(){

		Debug.Log ("onMusic" + musicSlider.value.ToString());
	}

	public void onBack(){

		menuManager.State = MenuManager.STATE.MainMenu;
		Debug.Log ("onBack");
	}

	public void setVisible( bool b ){
		
		ui.SetActive (b);
	}
}
