using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInRegistration : MonoBehaviour {

	public MenuManager menuManager;
	
	public GameObject ui;
	public GameObject uiReg;

	public VkController vkController;

	public InputField loginField;
	public InputField passwordField;

	public InputField loginRegField;
	public InputField passwordRegField;
	public InputField nicknameRegField;
	public InputField nicknameVKRegField;


	private string login = "";
	private string password = "";

	private string loginReg = "";
	private string passwordReg = "";
	private string nicknameReg = "";
	private string nicknameVKReg = "";

	private bool isLogUI = true;

	private QueryType queryType = QueryType.NormalLogin;
	private ErrorType errorType = ErrorType.NormalLoginError;

	private WWW www = null;

	enum QueryType
	{
		VKReg,
		VKLogin,
		NormalReg,
		NormalLogin
	}
	
	enum ErrorType
	{
		VkRegError,
		VkLoginError,
		NormalRegError,
		NormalLoginError
	}

	private LOGIN_STATE LogInState = LOGIN_STATE.Prestart;
	enum LOGIN_STATE {
		Prestart,
		Autorisation,
		VkAutorize,
		WaitingServerAnswer,
		RegSuccess,
		Fail,
		Result
	}
	// Use this for initialization
	void Start () {

		addListeners ();
	}

	private void addListeners(){

		EventManagerUI.onWaitingCancelEvent += onCancelWaiting;
	}

	void OnDestroy(){
		removeListeners ();
	}

	private void removeListeners(){

		EventManagerUI.onWaitingCancelEvent -= onCancelWaiting;
	}

	private void onCancelWaiting(){

		LogInState = LOGIN_STATE.Prestart;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (LogInState == LOGIN_STATE.Fail) {

			LogInState = LOGIN_STATE.Prestart;
			regFail();
		}

		if (LogInState==LOGIN_STATE.Autorisation)
		{
			if(vkController.IS_LOGED)
			{

			}
		}
		
		#region Ожидание авторизации ВК, отправка запроса на Сервер и переход к стейту WAITING 
		if (LogInState==LOGIN_STATE.VkAutorize)
		{
			if(vkController.IS_LOGED)
			{
				if(queryType==QueryType.VKReg){
					string query = "registration/vk.php?username="+nicknameVKReg+"&vkuid="+vkController.user.id + "&sex="+ vkController.user.sex.ToString() +"&bday="+vkController.user.bdate+"&city="+vkController.user.city+"&country="+vkController.user.country;
					SendQuery(query);
			}
				if(queryType==QueryType.VKLogin)
					SendQuery("autorization/vk.php?vkuid="+vkController.uid);
				//LogInState = LOGIN_STATE.WaitingServerAnswer;
				//menuManager.State = MenuManager.STATE.Waiting;
			}
		}
		#endregion
		
		#region Ожидание ответа от Сервера и переход к стейту RESULT
		if (LogInState==LOGIN_STATE.WaitingServerAnswer)
		{
			if(www.isDone){LogInState = LOGIN_STATE.Result;}
		}
		#endregion
		
		#region Обработка ответа от сервера и переход к стейтам: RegSucces,RegFail,LogSucces,LogFail
		if (LogInState==LOGIN_STATE.Result)
		{
			if (queryType==QueryType.NormalReg||queryType==QueryType.VKReg)
			{
				int res;			
				if (int.TryParse(www.text,out res))
				{
					if(res==1){
						LogInState=LOGIN_STATE.RegSuccess;
						regSuccess();
					}
					else{ LogInState=LOGIN_STATE.Fail; 
						if (queryType==QueryType.VKReg)errorType=ErrorType.VkRegError;
						if (queryType==QueryType.NormalReg)errorType=ErrorType.NormalRegError;
					}
				}				
				else 
				{
					LogInState=LOGIN_STATE.Fail;
					if (queryType==QueryType.VKReg)errorType=ErrorType.VkRegError;
					if (queryType==QueryType.NormalReg)errorType=ErrorType.NormalRegError;
				}
			}
			else
			{
				string result="";
				try
				{
					result = www.text;
					if (result!="none")
					{
						Global.USERNAME=result;
						menuManager.State = MenuManager.STATE.MainMenu;
						//LogInState=LOGIN_STATE.MainMenu;
					}
					else
					{
						
						LogInState=LOGIN_STATE.Fail; 
						if (queryType==QueryType.VKLogin)errorType=ErrorType.VkLoginError;
						if (queryType==QueryType.NormalLogin)errorType=ErrorType.NormalLoginError;
						
					}
				}
				catch
				{
					LogInState=LOGIN_STATE.Fail; 
					if (queryType==QueryType.VKLogin)errorType=ErrorType.VkLoginError;
					if (queryType==QueryType.NormalLogin)errorType=ErrorType.NormalLoginError;
				}
			}
		}
		#endregion
		

	}

	private void regSuccess(){

		menuManager.State = MenuManager.STATE.MainMenu;

	}
	private void regFail(){
		
		//menuManager.State = MenuManager.STATE.MainMenu;
		menuManager.showInfo ("Error", "Connect to server fail, try later");
	}

	public void onLogIn(){

		login = loginField.text;
		password = passwordField.text;

		if (!CheckTextFormat.checkLogin(login))
			menuManager.showInfo ("Error", "Enter you login");
		else if (!CheckTextFormat.checkPassword(password))
			menuManager.showInfo ("Error", "Enter you password");
		else
			logIn (login, password);
	}

	//
	public void onRegistration(){

		loginReg = loginRegField.text;
		passwordReg = passwordRegField.text;
		nicknameReg = nicknameRegField.text;
		//emailReg = emailRegField.text;

		if (!CheckTextFormat.checkLogin(loginReg ))
			menuManager.showInfo ("Error", "Not correct login");
		else if (!CheckTextFormat.checkPassword(passwordReg))
			menuManager.showInfo ("Error", "Not correct password");
		else if (!CheckTextFormat.checkNick(nicknameReg))
			menuManager.showInfo ("Error", "Not correct nickname");
		else
			registration (nicknameReg, loginReg, passwordReg);
	}

	//Регистрируем новый акк
	private void registration( string nick, string log, string pass ){

		queryType = QueryType.NormalReg;
	}

	//Логинимся тут
	private void logIn(string log, string pass){

		queryType = QueryType.NormalLogin;
		//Log In 
	}

	//Входим через ВК
	private void VKSingin(string nick){

		queryType = QueryType.VKLogin;
		LogInState = LOGIN_STATE.VkAutorize;
		//menuManager.State = MenuManager.STATE.Waiting;
	}
	
	public void onVKSingIn(){


		//nicknameReg
		//VKSingin (nicknameReg);
	}

	public void onVKRegistration(){

		nicknameVKReg = nicknameVKRegField.text;

		if (CheckTextFormat.checkNick (nicknameVKReg))
			VKRegistration ();
		else
			menuManager.showInfo ("Error", "Nickname is not correct");
	}

	private void VKRegistration(){

		queryType = QueryType.VKReg;
		LogInState = LOGIN_STATE.VkAutorize;
		menuManager.State = MenuManager.STATE.Waiting;
		vkController.Login ();
	}
	

	public void setVisible( bool b ){

		if (!b) {
			ui.SetActive (false);
			uiReg.SetActive (false);
			isLogUI = true;
		} else {

			ui.SetActive (true);
			uiReg.SetActive (false);
		}
	}

	public void onChangeLogReg(){

		isLogUI = !isLogUI;
		ui.SetActive (isLogUI);
		uiReg.SetActive (!isLogUI);
	}

	void SendQuery(string query)
	{
		www = new WWW(Global.SERVER+query);
		LogInState=LOGIN_STATE.WaitingServerAnswer;
		menuManager.State = MenuManager.STATE.Waiting;
	}
}
