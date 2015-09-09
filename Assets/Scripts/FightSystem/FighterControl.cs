using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterControl : MonoBehaviour 
{
	public Transform PLAYER;
	public Animator animator;

	
	STATE _state=STATE.IDLE;
	STATE State
	{
		get{return _state;}
		set
		{
			animator.ResetTrigger(TRIGGERS[STATE.BACK]);
			animator.ResetTrigger(TRIGGERS[STATE.BLOCK]);
			animator.ResetTrigger(TRIGGERS[STATE.DAMAGE]);
			animator.ResetTrigger(TRIGGERS[STATE.DEATH]);
			animator.ResetTrigger(TRIGGERS[STATE.FORWARD]);
			animator.ResetTrigger(TRIGGERS[STATE.IDLE]);
			animator.ResetTrigger(TRIGGERS[STATE.KICK]);
			animator.ResetTrigger(TRIGGERS[STATE.PUNCH]);
			animator.ResetTrigger(TRIGGERS[STATE.VICTORY]);
			animator.SetTrigger(TRIGGERS[value]);
			_state = value;
			//animator.SetTrigger
		}
	}
	Dictionary<STATE,string> TRIGGERS;
	
	STATE oldState=STATE.IDLE;	

	NetworkView netView;
	
	Vector3 newPosition;
	Vector3 oldPosition;
	float ActionTimer=0;
	
	float kickDamageTimer=0;
	float punchDamageTimer=0;
	//int Damage = 0;
	
	#region Public Settings
	public int PersonageID;
	[HideInInspector]
	public int SIDE=1;
	[HideInInspector]
	public int HP=100;
	[HideInInspector]
	public int StartHP=100;
	private int oldHP=100;
	[HideInInspector]
	public float SPEED=1;
	[HideInInspector]
	public float STRENGTH=1;
	[HideInInspector]
	public bool IS_MINE;
	[HideInInspector]
	public int WinStatus=0;
	#endregion
	
	FighterControl Enemy=null;
	bool isNear=false;
	
	FightManager fManager;
	
	void Start () 
	{
		InitTriggers();
		
		netView = PLAYER.gameObject.GetComponent<NetworkView>();
		fManager = FightManager.Instance;
		
		#region Public Settings Init
		oldHP = HP = StartHP = Personage.GetHP(PersonageID);
		SPEED = Personage.GetSPEED(PersonageID);
		STRENGTH = Personage.GetSTRENGTH(PersonageID);
		IS_MINE = netView.isMine;
		//Найти правого игрока
		if ((Network.isClient&&netView.isMine)||(Network.isServer&&!netView.isMine))
		{
			SIDE = -1;
			fManager.Client = this;
		}else {fManager.Server = this;}
		PLAYER.localScale = new Vector3((float)SIDE,1,1);//повернуть игрока в нужную сторону
		#endregion 		
		
		newPosition = PLAYER.transform.position;
	}
	

	void Update () 
	{
		
		if (WinStatus==0)
		{
		
			if (netView.isMine)
				UpdateMine ();
			else
				UpdateNetPlayer();
		
			if (oldHP!=HP)
			{
				oldHP=HP;
				State=STATE.DAMAGE;
				ActionTimer = 0.4f;
			}
			
		}
		
		if (HP<=0&&State!=STATE.DEATH)
		{
			State=STATE.DEATH;
		}
		
		if (Enemy!=null&&State!=STATE.VICTORY&&Enemy.HP<=0)
		{
			State=STATE.VICTORY;
		}
		
	}

	void UpdateMine()
	{
		if (Mathf.Abs(newPosition.x - PLAYER.position.x)>0.05f)netView.RPC("SetPosition",RPCMode.All,PLAYER.position.x);
		
		if (oldState!=State)
		{
			netView.RPC("SetState",RPCMode.All,GetIdByState(State));
			oldState=State;
		}

		
		if (ActionTimer == 0) 
		{
			if (!InputController.IS_BLOCK)
			{
				if (InputController.TargetLeft.x*SIDE>0 && State!=STATE.FORWARD )
				{
					State = STATE.FORWARD;
				}
				if (InputController.TargetLeft.x*SIDE<0 && State!=STATE.BACK )
				{
					State = STATE.BACK;
				}
			
				if (InputController.TargetLeft.x==0 && State!=STATE.IDLE )
				{
					State = STATE.IDLE;
					ResetTransformChanges();
				}
			}
			else
			{
				if(State!=STATE.BLOCK)State = STATE.BLOCK;
			}
			
			//------------------------------------------
			if (InputController.IS_KICK)
			{
				ResetTransformChanges();ActionTimer=0.75f;State=STATE.KICK;
				kickDamageTimer=0.02f;
			}
			if (InputController.IS_PUNCH)
			{
				ResetTransformChanges();
				ActionTimer=0.5f;
				State=STATE.PUNCH;
				punchDamageTimer=0.2f;
			}
			
			//if (InputController.IS_BLOCK){ ResetTransformChanges();ActionTimer=1.0f;State=STATE.BLOCK;}
		}
		if (ActionTimer > 0)
			ActionTimer -= Time.deltaTime;
		if (ActionTimer<0)
		{
			ActionTimer=0;
			ResetTransformChanges();
			State=STATE.IDLE;
		}
		
		if (State==STATE.FORWARD&&ActionTimer==0)
		{
			if(PLAYER.position.x<5 && SIDE==1){
				if(Mathf.Abs(PLAYER.position.x - fManager.Client.PLAYER.position.x)>1)
					PLAYER.position += Vector3.right*Time.deltaTime*SIDE*SPEED;}
			
			if(PLAYER.position.x>-5 && SIDE==-1){
				if(Mathf.Abs(PLAYER.position.x - fManager.Server.PLAYER.position.x)>1)
					PLAYER.position += Vector3.right*Time.deltaTime*SIDE*SPEED;}
		}
		if (State==STATE.BACK&&ActionTimer==0)
		{
			if((PLAYER.position.x>-5 && SIDE==1)||(PLAYER.position.x<5 && SIDE==-1))
			{
				PLAYER.position -= Vector3.right*Time.deltaTime*SIDE*SPEED;
			}
				
		}
		
		if (kickDamageTimer>0)kickDamageTimer+=Time.deltaTime;
		if (kickDamageTimer>0.5f){ if(isNear)netView.RPC("SendDamage",RPCMode.All,
				(int)(Random.Range(7.0f,15.0f)*STRENGTH)); kickDamageTimer=0;}
		
		if (punchDamageTimer>0)punchDamageTimer+=Time.deltaTime;
		if (punchDamageTimer>0.5f){ if(isNear)netView.RPC("SendDamage",RPCMode.All,
				(int)(Random.Range(5.0f,10.0f)*STRENGTH)); punchDamageTimer=0;}
		
	}
	
	void UpdateNetPlayer()
	{
		
		if (transform.position != newPosition)
			{
				PLAYER.transform.position = new Vector3(Mathf.Lerp(transform.position.x,newPosition.x,5*Time.deltaTime),0,0);
			}
			if (oldState!=State)
			{
				ResetTransformChanges();
				oldState=State;
			/*
				if (State==STATE.FORWARD)animator.SetTrigger(TRIGGERS[STATE.FORWARD]);
				if (State==STATE.BACK)animator.SetTrigger("WALK_BACK");
				if (State==STATE.BLOCK)animator.SetTrigger("BODY_BLOCK");
				if (State==STATE.DAMAGE)animator.SetTrigger("RECEIVE_PUNCH");
				if (State==STATE.IDLE)animator.SetTrigger("IDLE");
				if (State==STATE.KICK)animator.SetTrigger("KICK");
				if (State==STATE.PUNCH)animator.SetTrigger("PUNCH");*/
			}
	}
	
	void ResetTransformChanges()
	{
		animator.gameObject.transform.localPosition = Vector3.zero;
		animator.gameObject.transform.localRotation = Quaternion.identity;
	}

	public enum STATE
	{
		IDLE,
		BACK,
		FORWARD,
		KICK,
		PUNCH,
		BLOCK,
		DAMAGE,
		DEATH,
		VICTORY
	}

	int GetIdByState(STATE sta)
	{
		switch(sta)
		{
			case STATE.BACK : return 0;
			case STATE.BLOCK : return 1;
			case STATE.DAMAGE : return 2;
			case STATE.FORWARD : return 3;
			case STATE.IDLE : return 4;
			case STATE.KICK : return 5;
			case STATE.PUNCH : return 6;
			case STATE.DEATH : return 7;
			case STATE.VICTORY : return 8;
		}
		return 4;
	}

	STATE GetStateByID(int id)
	{
		switch (id)
		{
			case 0 : return STATE.BACK;
			case 1 : return STATE.BLOCK;
			case 2 : return STATE.DAMAGE;
			case 3 : return STATE.FORWARD;
			case 4 : return STATE.IDLE;
			case 5 : return STATE.KICK;
			case 6 : return STATE.PUNCH;
			case 7 : return STATE.DEATH;
			case 8 : return STATE.VICTORY;
		}
		return STATE.IDLE;
	}
	
	
	[RPC]
	void SetPosition(float posX){newPosition.x = posX;	}
	[RPC]
	void SetState(int stat)	{State = GetStateByID(stat);}	
	[RPC]
	void SendDamage(int damaga)
	{
		if(Enemy.State!=STATE.BLOCK){Enemy.HP-=damaga; if (Enemy.HP<0)Enemy.HP=0;}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (Enemy ==null && col.tag=="Player")
		{
			Enemy = col.gameObject.GetComponent<FighterControl>();
		}
		
		if (col.tag=="Player")
		{
			isNear=true;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if (col.tag=="Player")
		{
			isNear=false;
		}
	}
	
	void InitTriggers()
	{
		TRIGGERS = new Dictionary<STATE, string>();
		TRIGGERS.Add(STATE.BACK,"BACK");
		TRIGGERS.Add(STATE.BLOCK,"BLOCK");
		TRIGGERS.Add(STATE.DAMAGE,"DAMAGE");
		TRIGGERS.Add(STATE.DEATH,"DEATH");
		TRIGGERS.Add(STATE.FORWARD,"WALK");
		TRIGGERS.Add(STATE.IDLE,"IDLE");
		TRIGGERS.Add(STATE.KICK,"KICK");
		TRIGGERS.Add(STATE.PUNCH,"PUNCH");
		TRIGGERS.Add(STATE.VICTORY,"VICTORY");
	}
	
}
