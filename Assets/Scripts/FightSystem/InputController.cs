using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour 
{
	
	#region Textures
	public Texture leftField;
	public Texture leftHandler;
	public Texture texKickBtn;
	public Texture texPunchBtn;
	public Texture texBlockBtn;
	#endregion
	
	#region rects and vectors
	Rect rectLeftField;
	Rect rectLeftHandler;
	
	Vector2 lCenter;//координата Y отсчитывается сверху
	Vector2 lCenterInvert;//координата Y отсчитывается снизу
	
	Rect leftTouchRect;
	Vector2 PosLeftTouch;
	
	Rect rectKickBtn;
	Rect rectKickBtnInvert;
	Rect rectPunchBtn;
	Rect rectPunchBtnInvert;
	Rect rectBlockBtn;
	Rect rectBlockBtnInvert;
	#endregion
	
	#region BOOLS
	bool isLeftControlled = false;
	bool leftSideTouch = false;	
	
	public static bool IS_KICK=false;
	public static bool IS_PUNCH=false;
	public static bool IS_BLOCK=false;
	#endregion 
	
	#region Colors
	Color color;
	Color colorKick;
	Color colorPunch;
	Color colorBlock;
	#endregion
	
	#region Settings
	[Range(0.3f,0.6f)]
	public float color_alpha_min=0.5f;
	[Range(0.7f,0.9f)]
	public float color_alpha_max=0.8f;
	[Range(0.5f,1.5f)]
	public float color_fade_speed=0.8f;
	#endregion 
	
	//Хранит вектор-направление джойстика, длинна которого 1/10 высоты экрана.
	public static Vector2 TargetLeft;
	
	void Start () 
	{
		//Init all rects, vectors and colors
		InitRects ();
	}
	

	void Update () 
	{
		if (leftSideTouch)leftSideTouch = false;
		if (IS_KICK)IS_KICK = false;
		if (IS_PUNCH)IS_PUNCH = false;
		if (IS_BLOCK)IS_BLOCK = false;
		
		if (Input.touches.Length > 0) {

			PosLeftTouch = Vector2.one * Screen.width;
			
			for (int i=0; i<Input.touches.Length; i++) 
			{
				if (!isLeftControlled) {
					if (leftTouchRect.Contains (Input.touches [0].position))
						isLeftControlled = true;
				}
				if (Input.touches [i].position.x < Screen.width * 0.5f) 
				{
					leftSideTouch = true;
					if (Input.touches [i].position.x < PosLeftTouch.x) {
						PosLeftTouch = Input.touches [i].position;
					}
				}
				
				if (rectKickBtnInvert.Contains(Input.touches[i].position))IS_KICK=true;
				if (rectPunchBtnInvert.Contains(Input.touches[i].position))IS_PUNCH=true;
				if (rectBlockBtnInvert.Contains(Input.touches[i].position))IS_BLOCK=true;
				
			}
		}

		if (isLeftControlled)
		{
			if (leftSideTouch)
			{
				//PosLeftTouch.y = Screen.height - PosLeftTouch.y;
				TargetLeft = (PosLeftTouch - lCenterInvert).normalized*Screen.height*0.05f;
			}
			else
			{
				TargetLeft = Vector2.zero;
				rectLeftHandler = new Rect (lCenter.x - Screen.height * 0.075f, lCenter.y - Screen.height * 0.075f, Screen.height * 0.15f, Screen.height * 0.15f);
				isLeftControlled=false;
			}
		}

	}

	void OnGUI()
	{
		#region LeftHandler Drawing
		if (isLeftControlled)
		{
			rectLeftHandler = new Rect (lCenter.x + TargetLeft.x - Screen.height * 0.075f,lCenter.y - TargetLeft.y - Screen.height * 0.075f, Screen.height * 0.15f, Screen.height * 0.15f);
			if(color.a<color_alpha_max)color.a+=Time.deltaTime*color_fade_speed;
		}
		else
		{
			if(color.a>color_alpha_min)color.a-=Time.deltaTime*color_fade_speed;
		}

		GUI.color = color;
		GUI.DrawTexture (rectLeftField,leftField);
		GUI.DrawTexture (rectLeftHandler,leftHandler);
		#endregion
		
		#region KickBtn Drawing
		if (IS_KICK){if(colorKick.a<color_alpha_max)colorKick.a+=Time.deltaTime*color_fade_speed;}
		else {if(colorKick.a>color_alpha_min)colorKick.a-=Time.deltaTime*color_fade_speed;}
		
		GUI.color = colorKick;
		GUI.DrawTexture(rectKickBtn,texKickBtn);
		#endregion
		
		#region PunchBtn Drawing
		if (IS_PUNCH){if(colorPunch.a<color_alpha_max)colorPunch.a+=Time.deltaTime*color_fade_speed;}
		else {if(colorPunch.a>color_alpha_min)colorPunch.a-=Time.deltaTime*color_fade_speed;}
		
		GUI.color = colorPunch;
		GUI.DrawTexture(rectPunchBtn,texPunchBtn);
		#endregion
		
		#region BlockBtn Drawing
		if (IS_BLOCK){if(colorBlock.a<color_alpha_max)colorBlock.a+=Time.deltaTime*color_fade_speed;}
		else {if(colorBlock.a>color_alpha_min)colorBlock.a-=Time.deltaTime*color_fade_speed;}
		
		GUI.color = colorBlock;
		GUI.DrawTexture(rectBlockBtn,texBlockBtn);
		#endregion
		
	}

	void InitRects()
	{
		#region Left Side
		lCenter = new Vector2 (Screen.width * 0.1f + Screen.height * 0.05f,Screen.height * 0.95f - Screen.width * 0.1f);
		lCenterInvert = new Vector2 (lCenter.x, Screen.height - lCenter.y);

		leftTouchRect = new Rect (lCenter.x - Screen.height * 0.1f, Screen.height - lCenter.y - Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);

		rectLeftField = new Rect (lCenter.x - Screen.height * 0.1f, lCenter.y - Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);
		rectLeftHandler = new Rect (lCenter.x - Screen.height * 0.075f, lCenter.y - Screen.height * 0.075f, Screen.height * 0.15f, Screen.height * 0.15f);
		#endregion 
		
		rectKickBtn = new Rect(Screen.width-Screen.height*0.2f,Screen.height*0.5f,Screen.height*0.15f,Screen.height*0.15f);
		rectPunchBtn = new Rect(Screen.width-Screen.height*0.325f,Screen.height*0.675f,Screen.height*0.15f,Screen.height*0.15f);
		rectBlockBtn = new Rect(Screen.width-Screen.height*0.5f,Screen.height*0.8f,Screen.height*0.15f,Screen.height*0.15f);
		
		rectKickBtnInvert = rectKickBtn;
		rectKickBtnInvert.y = Screen.height-rectKickBtn.y-rectKickBtn.height;
		rectPunchBtnInvert = rectPunchBtn;
		rectPunchBtnInvert.y = Screen.height-rectPunchBtn.y-rectPunchBtn.height;
		rectBlockBtnInvert = rectBlockBtn;
		rectBlockBtnInvert.y = Screen.height-rectBlockBtn.y-rectBlockBtn.height;
		
		color = new Color (1.0f, 1.0f, 1.0f, color_alpha_max);
		colorKick = new Color (1.0f, 1.0f, 1.0f, color_alpha_max);
		colorPunch = new Color (1.0f, 1.0f, 1.0f, color_alpha_max);
		colorBlock = new Color (1.0f, 1.0f, 1.0f, color_alpha_max);
		
	}
}
