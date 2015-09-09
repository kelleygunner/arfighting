using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class CheckTextFormat {

	public static bool checkNick( string s ){

		Debug.Log ("string " + s);
		string ex = @"[a-zA-Z0-9-_]{2,18}$";

		Match match = Regex.Match (s, ex, RegexOptions.Singleline);

		bool b = (match.Value == s);
		
		Debug.Log ("isMatch " + b.ToString());
		return b;
	}

	//*

	public static bool checkLogin( string s ){
		
		Debug.Log ("checkLogin " + s);
		string ex = @"[a-zA-Z0-9-_\.]{2,18}$";
		
		Match match = Regex.Match (s, ex, RegexOptions.Singleline);
		
		bool b = (match.Value == s);
		
		Debug.Log ("isMatch " + b.ToString());
		return b;
	}

	public static bool checkPassword( string s ){
		
		Debug.Log ("checkLogin " + s);
		string ex = @"[a-zA-Z0-9-_\.]{2,18}$";
		
		Match match = Regex.Match (s, ex, RegexOptions.Singleline);
		
		bool b = (match.Value == s);
		
		Debug.Log ("isMatch " + b.ToString());
		return b;
	}
	//*/

}
