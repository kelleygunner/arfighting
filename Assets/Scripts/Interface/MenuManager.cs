using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public MainMenu mainMenu;
	public OptionsMenu optionsMenu;
	public NewsMenu newsMenu;
	public LogInRegistration logInRegistration;
	public InfoMenu infoMenu;
	public WaitingMenu waitingMenu;
	public StoreMenu storeMenu;

	public delegate void GUIEvent ( STATE s );
	public event GUIEvent changeState;

	//Test
	private bool isFirstStart = true;
	private bool isFirstStartToday = true;
	//

	private STATE _state = STATE.Prestart;
	public STATE State {
		get{ return _state; }
		set{
			hideAllMenu();
			_state = value;
			showMenu(_state);
		}

	}

	private void showMenu( STATE state ){

		switch( state ){
			
		case STATE.Settings:
			optionsMenu.setVisible(true);
			break;
		case STATE.MainMenu:
			mainMenu.setVisible(true);
			break;
		case STATE.News:
			newsMenu.setVisible(true);
			break;
		case STATE.AutorizationRegistration:
			logInRegistration.setVisible(true);
			break;
		case STATE.Waiting:
			waitingMenu.setVisible(true);
			break;
		case STATE.Store:
			storeMenu.setVisible(true);
			break;
		default:
			mainMenu.setVisible(true);
			break;
		}
	}
	
	private void hideAllMenu(){

		mainMenu.setVisible(false);
		optionsMenu.setVisible(false);
		newsMenu.setVisible(false);
		logInRegistration.setVisible(false);
		waitingMenu.setVisible (false);
		storeMenu.setVisible (false);
	}

	public enum STATE
	{
		Prestart,
		AutorizationRegistration,
		News,
		MainMenu,
		Store,
		Settings,
		PersonageSelect,
		Waiting,

	}

	void Start(){

		init ();
	}

	private void init(){

		hideAllMenu();

		if (Global.FirstStart == 0) {
			isFirstStart = true;
			Global.FirstStart = 1;
		}

		if ( isFirstStart ) {
			showMenu (STATE.AutorizationRegistration);
			
		} else if (isFirstStartToday && Global.IsAutorised == 1) {
			showMenu (STATE.News);
		} else
			showMenu (STATE.MainMenu);

	}

	public void showInfo( string title, string msg ){

		infoMenu.setText (title, msg);
		infoMenu.setVisible (true);
	}
}
