using UnityEngine;
using System.Collections;

public class Global
{
	public static string USERNAME
	{
		get{return PlayerPrefs.GetString("USERNAME");}
		set{PlayerPrefs.SetString("USERNAME",value);}
	}

	public static string GAME_TYPE_NAME="AR_FIGHTING_TOONIDEE";
	
	public static int PERSONAGE_SELECTED
	{
		get{return PlayerPrefs.GetInt("PERSONAGE_SELECTED");}
		set{PlayerPrefs.SetInt("PERSONAGE_SELECTED",value);}
	}
	
	//------------------------------------------------
/*
	public static int UserName
	{	
		get {return PlayerPrefs.GetString("USER_NAME");}
		set {PlayerPrefs.SetString("USER_NAME",value);}
	}
	public static int UID
	{	
		get {return PlayerPrefs.GetString("USER_LOGIN");}
		set {PlayerPrefs.SetString("USER_LOGIN",value);}
	}
	*/
	//------------------------------------------------
	public static int FirstStart
	{
		get {return PlayerPrefs.GetInt("FIRST_START");}
		set {PlayerPrefs.SetInt("FIRST_START",value);}
	}

	public static int Language
	{
		get {return PlayerPrefs.GetInt("LANGUAGE");}
		set {PlayerPrefs.SetInt("LANGUAGE",value);}
	}

	public static int IsAutorised
	{
		get {return PlayerPrefs.GetInt("AUTORISED");}
		set {PlayerPrefs.SetInt("AUTORISED",value);}
	}
	
	public static string SERVER
	{
		get{return "http://anigames.besaba.com/server/";}
	}
}