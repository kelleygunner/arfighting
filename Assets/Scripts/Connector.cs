using UnityEngine;
using System.Collections;

public class Connector : MonoBehaviour 
{
	
	private static Connector _instance;
	public static Connector Instance
	{
		get {return _instance;}
	}
	
	public static bool IS_SCENE_LOADED=false;
	public static bool IS_SCENE_INITIALIZED = false;

	//public GameObject PLAYER;

	HostData[] DATA=null;

	ConnectionType conType = ConnectionType.none;

	Rect rectMainButton;
	Rect rectBackButton;
	Rect rectMessage;

	string connection_history="Start...\n";

	float hostTestingTimer=0;
	float hostSearchingTime=2;

	int trying=0;

	NetworkView nw;

	public GUIStyle txtStyle;
	
	public Personage PersManager;
	
	int host=0;
	
	void Start () 
	{
		_instance = this;
		/*
		Global.USERNAME="Kelley";
		Global.PERSONAGE_SELECTED=1;*/
		
		DontDestroyOnLoad (this.gameObject);
		nw = GetComponent<NetworkView> ();

		connection_history += "trying to unregister Host...";
		try{MasterServer.UnregisterHost (); MasterServer.ClearHostList(); connection_history += " ok\n";}
		catch(System.Exception ex){connection_history+=" Error:"+ex+"\n";}

		InitRects ();
	}
	

	void Update () 
	{
		#region Тестирование на наличие Хостов
		if (conType == ConnectionType.host_testing && hostTestingTimer < hostSearchingTime) 
		{
			DATA = MasterServer.PollHostList ();
			hostTestingTimer+=Time.deltaTime;
			connection_history+=".";
		}
		if (conType == ConnectionType.host_testing && hostTestingTimer >= hostSearchingTime)
		{
			conType=ConnectionType.test_result;
			connection_history+="\n"+DATA.Length+" hosts found.\n";
		}
		#endregion

		#region Выбор режима на основании найденных хостов
		if (conType==ConnectionType.test_result)
		{
			host = FindEmptyHost();
			
			if (DATA.Length>0 && host!=-1)
			{
				if (trying>4)
				{
					MasterServer.ClearHostList();
					connection_history += "connecting failed\n";
					conType = ConnectionType.failed;
					try{MasterServer.UnregisterHost (); connection_history += " ok\n";}
					catch(System.Exception ex){connection_history+=" Error:"+ex+"\n";}
				}
				
				
				connection_history += "connecting to "+DATA[host].gameName+"...\n";
				conType = ConnectionType.client;
				try
				{
					Network.Connect(DATA[host].guid);
				}
				catch(System.Exception ex)
				{
					MasterServer.ClearHostList();
					connection_history += "Error: "+ex.Message+"\n";
					conType = ConnectionType.failed;
					Network.Disconnect();
				}
			}
			else
			{
				StartServer();
			}
		}
		#endregion 

		if (IS_SCENE_LOADED&&!IS_SCENE_INITIALIZED)
		{
			IS_SCENE_INITIALIZED=true;
			conType = ConnectionType.game;
			
			if (Network.isServer)
			{
				Network.Instantiate(PersManager.Prefabs[Global.PERSONAGE_SELECTED],Vector3.right*-2,Quaternion.identity,0);
			}
			else
			{
				Network.Instantiate(PersManager.Prefabs[Global.PERSONAGE_SELECTED],Vector3.right *2,Quaternion.identity,0);

			}
		}
	}

	void OnGUI() 
	{
		//СТАРОВЫЙ ЭКРАН
		if (conType==ConnectionType.none)
		{
			if (GUI.Button(rectMainButton,"Подключиться")){StartCoroutine(RequestHosts()); trying=0;}
			if (GUI.Button(rectBackButton,"Назад")){Application.LoadLevel("menu");}
		}
		//ПОДКЛЮЧЕНИЕ
		if (conType!=ConnectionType.none&&
		    conType!=ConnectionType.failed&&
			conType!=ConnectionType.game)
		{
			if (GUI.Button(rectBackButton,"Прервать"))
			{
				connection_history = "Прерывание...";
				Network.Disconnect();
				try{
					MasterServer.UnregisterHost ();
					MasterServer.ClearHostList();
					connection_history += " ok\n";
				}
				catch(System.Exception ex){connection_history+=" Error:"+ex+"\n";}
				conType=ConnectionType.none;
				Application.LoadLevel(Application.loadedLevel);
				
				Reset();
				Destroy(Personage.Instance.gameObject);
				Destroy(this.gameObject);
			}
		}

		//СТАРОВЫЙ ЭКРАН
		if (conType==ConnectionType.failed)
		{
			if (GUI.Button(rectMainButton,"Попробовать еще раз"))
			{
				MasterServer.ClearHostList();
				StartCoroutine(RequestHosts()); trying=0;
			}
			if (GUI.Button(rectBackButton,"Назад")){Application.LoadLevel("menu");}
		}


		if(conType!=ConnectionType.game)GUI.Label (rectMessage,connection_history,txtStyle);

	}

	enum ConnectionType
	{
		none,
		host_testing,
		test_result,
		server,
		client,
		waiting,
		connecting,
		failed,
		game
	}

	IEnumerator RequestHosts()
	{
		MasterServer.RequestHostList (Global.GAME_TYPE_NAME);
		connection_history += "Request of Hosts...\n";
		DATA = MasterServer.PollHostList ();
		conType = ConnectionType.host_testing;
		yield return DATA;
	}

	void OnMasterServerEvent(MasterServerEvent msEvent) 
	{
		//if (msEvent == MasterServerEvent.HostListReceived) 
		//{}
		if (msEvent == MasterServerEvent.RegistrationSucceeded)
			connection_history += "Server Registered\n";
	}

	void InitRects()
	{
		rectMainButton = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.3f, Screen.height * 0.4f, Screen.height * 0.1f);
		rectBackButton = new Rect (Screen.width * 0.5f - Screen.height * 0.2f, Screen.height * 0.4f, Screen.height * 0.4f, Screen.height * 0.1f);

		rectMessage = new Rect (Screen.width * 0.1f, Screen.height * 0.5f, Screen.width * 0.8f, Screen.height * 0.5f);

		txtStyle.fontSize = (int)(Screen.height*0.02f);
	}

	void OnServerInitialized()
	{
		connection_history += "Server created. Waiting of client...\n";
	}
	void OnFailedToConnect(NetworkConnectionError error) 
	{
		connection_history += "Could not connect to server: " + error+"\n";
		trying++;
		/*
		if (DATA.Length-1>host)host++;
		else 
		{
			Network.Disconnect();
			MasterServer.UnregisterHost ();
			MasterServer.ClearHostList();
			StartServer();
		}
		*/
		//conType = ConnectionType.test_result;
		StartCoroutine( RequestHosts() );
	}
	void OnPlayerConnected(NetworkPlayer player) 
	{

	}

	void OnConnectedToServer()
	{
		nw.RPC ("LoadScene", RPCMode.All);
	}

	[RPC]
	void LoadScene()
	{
		Application.LoadLevel ("game");
	}
	
	void StartServer()
	{
		#region Запускаем Сервер
		connection_history += "Server Creating...\n";
		conType = ConnectionType.server;
		Network.InitializeServer(1, 25002, true);
		MasterServer.RegisterHost(Global.GAME_TYPE_NAME, Global.USERNAME, "let's fight");
		#endregion
	}
	
	int FindEmptyHost()
	{
		for(int i=0; i<DATA.Length; i++)
		{
			if (DATA[i].connectedPlayers<DATA[i].playerLimit){  Debug.Log(i.ToString() + " host is empty"); return i;}
		}
		Debug.Log("no empty hosts");
		return -1;
	}
	
	public static void Reset()
	{
		IS_SCENE_INITIALIZED = false;
		IS_SCENE_LOADED=false;
	}
	
}
