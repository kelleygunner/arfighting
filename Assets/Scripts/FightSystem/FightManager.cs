using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour 
{
	public static FightManager Instance
	{
		get {return _instance;}
	}
	
	private static FightManager _instance;
	
	#region Textures
	public Texture tex_hp_line;
	public Texture tex_hp_back;
	public Texture tex_hp_frame;
	public Texture tex_youwin;
	public Texture tex_youlose;
	#endregion
	
	#region Rects
	Rect rectHpBackLeft;
	Rect rectHpFrameLeft;
	Rect rectHpLineLeft;
	Rect rectHpBackRight;
	Rect rectHpFrameRight;
	Rect rectHpLineRight;
	
	Rect rectValueLeft;
	Rect rectValueRight;
	
	Rect rectYouWin;
	Rect rectYouLose;
	
	Rect rectExit;
	#endregion 
	
	int oldServerHP=0;
	int oldClientHP=0;
	
	[HideInInspector]
	public FighterControl Server = null;
	[HideInInspector]
	public FighterControl Client = null;
	
	public GUISkin skin;
	
	public bool GAME_OVER=false;
	
	void Start () 
	{
		_instance = this;
		Connector.IS_SCENE_LOADED = true;
		InitRects();
	}	

	void Update () 
	{
		if (Client!=null && Server!=null && (oldServerHP!=Server.HP||oldClientHP!=Client.HP))SetHPLines();
	}
	
	void OnGUI()
	{
		
		if (Server!=null && Client!=null)
		{
			
			GUI.skin = skin;
		
			GUI.DrawTexture(rectHpBackLeft,tex_hp_back);
			GUI.DrawTexture(rectHpBackRight,tex_hp_back);
			GUI.DrawTexture(rectHpLineLeft,tex_hp_line);
			GUI.DrawTexture(rectHpLineRight,tex_hp_line);
			GUI.DrawTexture(rectHpFrameLeft,tex_hp_frame);
			GUI.DrawTexture(rectHpFrameRight,tex_hp_frame);
		
		
			GUI.Label(rectValueLeft,Server.HP.ToString()+"/"+Server.StartHP);
			GUI.Label(rectValueRight,Client.HP.ToString()+"/"+Client.StartHP);	
			
			if (Client.HP<=0 || Server.HP<=0)
			{
				if (!GAME_OVER)
				{
					Client.WinStatus = Server.WinStatus = 1;
					GAME_OVER=true;
				}
				if (Network.isServer)
				{
					if(Server.HP<=0){GUI.DrawTexture(rectYouLose,tex_youlose); }
					else {GUI.DrawTexture(rectYouWin,tex_youwin); }
				}
				else
				{
					if(Server.HP<=0)GUI.DrawTexture(rectYouWin,tex_youwin);
						else GUI.DrawTexture(rectYouLose,tex_youlose);
				
				}
				
				if (GUI.Button(rectExit,"Выход"))
				{
					Connector.Reset();
					Destroy(Connector.Instance.gameObject);
					Destroy(Personage.Instance.gameObject);
					Application.LoadLevel("connection");
				}
			}
		}
		
	}
	
	void InitRects()
	{
		rectHpBackLeft = new Rect(Screen.width*0.5f-Screen.height*0.6f,
				Screen.height*0.1f,
				Screen.height*0.5f,
				Screen.height*0.04f);
		rectHpFrameLeft = new Rect(Screen.width*0.5f-Screen.height*0.6f,
				Screen.height*0.1f,
				Screen.height*0.5f,
				Screen.height*0.04f);
		rectHpBackRight = new Rect(Screen.width*0.5f+Screen.height*0.1f,
				Screen.height*0.1f,
				Screen.height*0.5f,
				Screen.height*0.04f);
		rectHpFrameRight = new Rect(Screen.width*0.5f+Screen.height*0.1f,
				Screen.height*0.1f,
				Screen.height*0.5f,
				Screen.height*0.04f);
		
		rectValueLeft = new Rect(Screen.width*0.5f-Screen.height*0.45f,Screen.height*0.1f,Screen.height*0.2f,Screen.height*0.04f);
		rectValueRight = new Rect(Screen.width*0.5f+Screen.height*0.25f,Screen.height*0.1f,Screen.height*0.2f,Screen.height*0.04f);
		
		rectYouWin = rectYouLose = new Rect(Screen.width*0.5f-Screen.height*0.3f,
			Screen.height*0.3f,Screen.height*0.6f,Screen.height*0.15f);
		
		rectExit = new Rect(Screen.width*0.5f-Screen.height*0.2f,Screen.height*0.6f,Screen.height*0.4f,Screen.height*0.1f);
		
		skin.label.fontSize = (int)(Screen.height*0.03f);
	}
	
	void SetHPLines()
	{
		rectHpLineLeft = new Rect(Screen.width*0.5f-Screen.height*0.6f,
				Screen.height*0.1f,
				Screen.height*0.5f*((float)Server.HP/(float)Server.StartHP),
				Screen.height*0.04f);
		rectHpLineRight = new Rect(Screen.width*0.5f+Screen.height*0.1f,
				Screen.height*0.1f,
				Screen.height*0.5f*((float)Client.HP/(float)Client.StartHP),
				Screen.height*0.04f);
		
		oldServerHP = Server.HP;
		oldClientHP = Client.HP;
	}
}
