using UnityEngine;
using System.Collections;

public class EventManagerUI : MonoBehaviour {
	//*
	public delegate void UIEvent ();
	public delegate void UIDialogEvent ( bool b );
	public delegate void UIShowDialogEvent ( string title, string msg, DialogMenu.CallFrom callFrom, string img );

	public static event UIDialogEvent onDialogStoreEvent;
	public static event UIShowDialogEvent onShowDialogStoreEvent;

	public static event UIEvent onWaitingCancelEvent;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void dialogStoreAnswer( bool b ){

		if( onDialogStoreEvent != null )
			onDialogStoreEvent( b );
	}

	public static void showDialogStore( string titile, string msg, DialogMenu.CallFrom callFrom, string img = "" ){

		if (onShowDialogStoreEvent != null)
			onShowDialogStoreEvent (titile, msg, callFrom, img);
	}

	public static void cancelWaitingConnect(){

		if (onWaitingCancelEvent != null)
			onWaitingCancelEvent ();
	}
	//*/
}
