using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogMenu : MonoBehaviour {

	public MenuManager menuManager;
	
	public GameObject ui;
	
	public Text titleText;
	public Text msgText;

	private CallFrom callFrom = CallFrom.Store;
	public enum CallFrom{

		Store,
		Options

	}
	// Use this for initialization
	void Start () {
	
		init ();
	}

	private void init(){

		EventManagerUI.onShowDialogStoreEvent += onStore;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onOk(){

		switch (callFrom) {

		case CallFrom.Store:
			EventManagerUI.dialogStoreAnswer( true );
			break;
		case CallFrom.Options:
			//EventManagerUI.dialogOptionsAnswer( true );
			break;
		default:
			break;
		}
		setVisible (false);
		Debug.Log ("onOk");
	}

	public void onCancel(){

		switch (callFrom) {
			
		case CallFrom.Store:
			EventManagerUI.dialogStoreAnswer( false );
			break;
		case CallFrom.Options:
			//EventManagerUI.dialogOptionsAnswer( false );
			break;
		default:
			break;
		}
		setVisible (false);
		Debug.Log ("onCancel");
	}

	public void onStore(string title, string msg, CallFrom callFrom, string img){

		setText (title, msg);
		if (img != "")
			setImg (img);
		setVisible (true);
	}

	private void setImg( string img ){

	}

	public void setText( string title, string msg ){
		
		titleText.text = title;
		msgText.text = msg;
	}

	public void setVisible( bool b ){
		
		ui.SetActive (b);
	}
}
