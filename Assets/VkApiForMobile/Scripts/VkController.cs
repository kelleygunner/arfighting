using UnityEngine;
using System.Collections;
using com.playGenesis.VkUnityPlugin;
using Facebook.MiniJSON;
using System;
using System.Collections.Generic;

public class VkController : MonoBehaviour 
{

	public string test_string="";

	public VkApi vkApi;
	//public List <VKUser> friends = new List<VKUser>();
	//public VkSettings settings;
	public VKUser user = new VKUser();
	public int try_num=0;

	public bool IS_LOGED=false;
	public string uid="";

	void Start () 
	{
		vkApi = VkApi.VkApiInstance;
		vkApi.LoggedIn += ()=>
		{
			StartWorkingWithVK();
		};
	}



	void Update () {
	
	}

	void StartWorkingWithVK()
	{
		if (vkApi.TokenValidFor () < 120)
			Login ();
		GetUser ();
	}

	public void Login()
	{
		vkApi.Login ();
	}

	public void Logout()
	{
		vkApi.Logout ();
		IS_LOGED = false;
	}

	public void GetUser()
	{
		var r = "users.get?user_id=" + VkApi.currentToken.user_id +"&fields=bdate,city,country,sex";
		vkApi.Call (r, OnGetUser);
	}
		          
	void OnGetUser(VkResponseRaw resp, object[] obj)
	{
		if (resp.ei!=null&&try_num<5)
		{
			try_num++;
			GetUser();
			return;
		}

		user = VkParser.GetUserWithBdateCityCountry(resp.text);
		user.id = System.Convert.ToInt32( VkApi.currentToken.user_id );
		uid = ( VkApi.currentToken.user_id ).ToString();
		IS_LOGED = true;

	}


}
