using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour 
{

	public STATE State = STATE.Prestart;
	private QueryType queryType = QueryType.NormalLogin;
	private ErrorType errorType = ErrorType.NormalLoginError;

	public GUISkin guiSkin1;

	public VkController vkController;

	#region Rects
	Rect rectBtn0;
	Rect rectBtn1;
	Rect rectBtn2;
	Rect rectBtn3;
	Rect rectBtn4;
	Rect rectBtn5;
	Rect rectFillLine1;
	Rect rectFillLine2;
	Rect rectFillLine3;
	Rect rectTextLine1;
	Rect rectTextLine2;
	Rect rectTextLine3;
	#endregion

	string login="";
	string password="";
	string username="";
	
	WWW www = null;
	
	void Start () 
	{
		//Настройки при первом запуске
		if (Global.FirstStart==0){FirstStartOptions();}
		if (Global.IsAutorised == 1)State = STATE.MainMenu;

		InitRects ();


	}
	

	void Update () 
	{
		if (State==STATE.Autorisation)
		{
			if(vkController.IS_LOGED)
			{

			}
		}
		
		#region Ожидание авторизации ВК, отправка запроса на Сервер и переход к стейту WAITING 
		if (State==STATE.VkAutorize)
		{
			if(vkController.IS_LOGED)
			{
				if(queryType==QueryType.VKReg)
					SendQuery("registration/vk.php?username="+username+"&vkuid="+vkController.uid);
				if(queryType==QueryType.VKLogin)
					SendQuery("autorization/vk.php?vkuid="+vkController.uid);
				State = STATE.Waiting;
			}
		}
		#endregion
		
		#region Ожидание ответа от Сервера и переход к стейту RESULT
		if (State==STATE.Waiting)
		{
			if(www.isDone){State = STATE.Result;}
		}
		#endregion
		
		#region Обработка ответа от сервера и переход к стейтам: RegSucces,RegFail,LogSucces,LogFail
		if (State==STATE.Result)
		{
			if (queryType==QueryType.NormalReg||queryType==QueryType.VKReg)
			{
				int res;			
				if (int.TryParse(www.text,out res))
				{
					if(res==1)State=STATE.RegSuccess;
					else{ State=STATE.Fail; 
						if (queryType==QueryType.VKReg)errorType=ErrorType.VkRegError;
						if (queryType==QueryType.NormalReg)errorType=ErrorType.NormalRegError;
					}
				}				
				else 
				{
					State=STATE.Fail; 
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
						State=STATE.MainMenu;
					}
					else
					{
						
						State=STATE.Fail; 
						if (queryType==QueryType.VKLogin)errorType=ErrorType.VkLoginError;
						if (queryType==QueryType.NormalLogin)errorType=ErrorType.NormalLoginError;
						
					}
				}
				catch
				{
					State=STATE.Fail; 
					if (queryType==QueryType.VKLogin)errorType=ErrorType.VkLoginError;
					if (queryType==QueryType.NormalLogin)errorType=ErrorType.NormalLoginError;
				}
			}
		}
		#endregion
		
		
	}

	void OnGUI()
	{
		GUI.skin = guiSkin1;

		#region PRESTART
		if (State==STATE.Prestart)
		{
			if(GUI.Button (rectBtn0,"ВХОД"))State = STATE.Autorisation;
			if(GUI.Button (rectBtn1,"РЕГИСТРАЦИЯ"))State=STATE.Registration;
			if(GUI.Button (rectBtn2,"ВЫХОД"))Application.Quit();
		}
		#endregion
		
		#region AUTORIZATION
		if (State==STATE.Autorisation)
		{
			GUI.Label(rectTextLine1,"Вход через Вконтакте");
			if(GUI.Button (rectBtn0,"ВХОД VK")){State=STATE.VkAutorize; queryType=QueryType.VKLogin; vkController.Login();}
			GUI.Label(rectTextLine2,"или введите логин и пароль:");
			login = GUI.TextField(rectFillLine1,login,20);
			password = GUI.PasswordField(rectFillLine2,password,'*',20);
			if(GUI.Button (rectBtn3,"ВХОД")){}
			if(GUI.Button (rectBtn4,"НАЗАД")){State=STATE.Prestart;}
		}
		#endregion
		
		#region REGISTRATION
		if (State==STATE.Registration)
		{
			GUI.Label(rectTextLine1,"Введите имя пользователя:");
			username = GUI.TextField(rectFillLine3,username,18);
			
			GUI.Label(rectTextLine2,"Введите логин и пароль ИЛИ авторизуйтесь ВК");
			login = GUI.TextField(rectFillLine1,login,18);
			password = GUI.PasswordField(rectFillLine2,password,'*',18);
			if(GUI.Button (rectBtn3,"Регистрация (Логин)")){}
			//Регистрация ВК
			if(GUI.Button (rectBtn4,"Регистрация ВК"))
			{
				if(username.Length>2)
				{
					State = STATE.VkAutorize;
					queryType = QueryType.VKReg;
					vkController.Login();
				}
			}
			if(GUI.Button (rectBtn5,"НАЗАД")){State=STATE.Prestart;}
		}
		#endregion
		
		if (State==STATE.Waiting)
		{
			GUI.Label(rectTextLine2,"Соединение с сервером...");
		}
		
		if (State==STATE.RegSuccess)
		{
			GUI.Label(rectTextLine1,"Регистрация прошла успешно!");
			if(GUI.Button (rectBtn3,"Продолжить")){Application.LoadLevel("connection");}
		}
		if (State==STATE.Fail)
		{
			if (errorType==ErrorType.VkRegError)
				GUI.Label(rectTextLine1,"Ошибка! Либо ваш ВК-аккаунт уже зарегистрирован, либо имя пользователя уже используется...");
			if (errorType==ErrorType.NormalRegError)
				GUI.Label(rectTextLine1,"Ошибка! Имя пользователя или Логин уже используются...");
			if (errorType==ErrorType.VkLoginError)
				GUI.Label(rectTextLine1,"Ошибка! Ваш ВК-аккаунт не найден в базе...");
			if (errorType==ErrorType.NormalLoginError)
				GUI.Label(rectTextLine1,"Ошибка! Логин или Пароль не действительны...");
			if(GUI.Button (rectBtn3,"Назад")){State=STATE.Registration;}
		}
		
		if (State == STATE.MainMenu)
		{
			GUI.Label(rectTextLine1,"Добро пожаловать, "+ Global.USERNAME);
		}
		
	}

	void InitRects()
	{
		rectBtn0 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.1f); 
		rectBtn1 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.3f, Screen.height * 0.4f, Screen.height * 0.1f); 
		rectBtn2 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.4f, Screen.height * 0.1f); 
		rectBtn3 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.5f, Screen.height * 0.4f, Screen.height * 0.1f);
		rectBtn4 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.6f, Screen.height * 0.4f, Screen.height * 0.1f);
		rectBtn5 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.7f, Screen.height * 0.4f, Screen.height * 0.1f);
		
		rectFillLine1 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.4f, Screen.height * 0.05f);
		rectFillLine2 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.45f, Screen.height * 0.4f, Screen.height * 0.05f);
		rectFillLine3 = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.05f);
		
		rectTextLine1 = new Rect (Screen.width * 0.5f-Screen.height*0.3f, Screen.height * 0.1f, Screen.height*0.6f, Screen.height*0.1f);
		rectTextLine2 = new Rect (Screen.width * 0.5f-Screen.height*0.3f, Screen.height * 0.3f, Screen.height*0.6f, Screen.height*0.1f);
		rectTextLine3 = new Rect (Screen.width * 0.5f-Screen.height*0.3f, Screen.height * 0.8f, Screen.height*0.6f, Screen.height*0.1f);

		guiSkin1.button.fontSize = (int)(Screen.height*0.04f);
		guiSkin1.label.fontSize = (int)(Screen.height*0.04f);
		guiSkin1.textField.fontSize = (int)(Screen.height*0.03f);
		guiSkin1.textField.fontSize = (int)(Screen.height*0.03f);
	}

	void FirstStartOptions()
	{
		switch(Application.systemLanguage)
		{
			case SystemLanguage.Russian : Global.Language = 1; break;
			default : Global.Language = 0; break;
		}

		Global.FirstStart = 1;
	}

	public enum STATE
	{
		Prestart,
		Autorisation,
		Registration,
		MainMenu,
		Settings,
		PersonageSelect,
		VkAutorize,
		Waiting,
		Result,
		Fail,
		RegSuccess
	}

	void SendQuery(string query)
	{
		www = new WWW(Global.SERVER+query);
		State=STATE.Waiting;
	}

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
}
