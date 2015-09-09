using UnityEngine;
using System.Collections;

public class Personage : MonoBehaviour 
{
	
	static Personage _instance;
	public static Personage Instance
	{get{return _instance;}}
	
	public GameObject[] Prefabs;
	
	void Start () 
	{
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
	}
	
	public static int GetHP(int ID)
	{
		switch (ID)
		{
			case 0 : return 90;
			case 1 : return 110;
			default : return 100;
		}
	}
	public static float GetSPEED(int ID)
	{
		switch (ID)
		{
			case 0 : return 1.8f;
			case 1 : return 1.5f;
			default : return 1;
		}
	}
	public static float GetSTRENGTH(int ID)
	{
		switch (ID)
		{
			case 0 : return 1;
			case 1 : return 1.1f;
			default : return 1;
		}
	}
}
