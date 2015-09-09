using UnityEngine;
using System.Collections;

public class StoreMenu : MonoBehaviour {

	public MenuManager menuManager;
	
	public GameObject ui;


	// Use this for initialization
	void Start () {
		addListener ();
	}

	private void addListener(){

		EventManagerUI.onDialogStoreEvent += onConfirm;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void onConfirm( bool b ){
		Debug.Log ("onConfirm");
		Animator anim;

		if (b)
			//cuccess
			menuManager.showInfo ("Успех", "С обновочкой");
		else
			menuManager.showInfo ("Провал", "Покупка не удалась");
	}

	public void onBuy(){

		EventManagerUI.showDialogStore ("Подтверди", "Купить неведомую хуйню за 100500 ололошек?", DialogMenu.CallFrom.Store);
	}



	public void setVisible( bool b ){
		
		ui.SetActive (b);
	}
}
