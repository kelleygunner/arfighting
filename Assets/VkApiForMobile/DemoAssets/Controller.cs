using UnityEngine;
using System.Collections;
using com.playGenesis.VkUnityPlugin;
//using JsonFx.Json;
using System.Collections.Generic;
using System;
using Facebook.MiniJSON;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	public VkApi vkapi;
	public Downloader d;
	public List<VKUser> friends=new List<VKUser>();
	public VkSettings sets;
	// Use this for initialization

	void Start () {
		vkapi=VkApi.VkApiInstance;
		vkapi.LoggedIn+=()=>
		{
			startWorkingWithVk();

		};
	}

	public void Login()
	{
		vkapi.Login ();
	}
	public void LogOut()
	{
		vkapi.Logout();
		sets.forceOAtuh = true;
		sets.revoke = true;

	}

	public void startWorkingWithVk()
	{
		if (vkapi.TokenValidFor () < 120)
			Login();
		Get3FriendsDataFromVk(0);
	}

	public void Get3FriendsDataFromVk(int attempt)
	{

		var r="friends.get?user_id="+VkApi.currentToken.user_id +"&count=3&fields=photo_200";
		vkapi.Call(r,OnGet5FriendsCompleted,new object[]{attempt});
		//vkapi.call принимает 3 параметра строку запроса, функцию обработчика запроса,
		//и массив обеъктов(можно передать любые объекты, их пожно использовать в обработчике
		//например, можно рализовать повторные запросы при неудаче как здесь.
	}



	void OnGet5FriendsCompleted (VkResponseRaw arg1, object[] arg2)
	{
		//проверяем на ошибки
		if (arg1.ei != null) {
			//если ошибка, помним что мы передали номер запроса, можно повторить запрос некоторое количество раз
			var attempt=(int)arg2[0];
			if(attempt<5)
			{
				Get3FriendsDataFromVk(attempt+1);
			}
			return;
		}

		var dict=Json.Deserialize(arg1.text) as Dictionary<string,object>;
		var resp= (Dictionary<string,object>)dict["response"];
		var items=(List<object>)resp["items"];

		foreach(var item in items)
		{
			friends.Add(VKUser.Deserialize(item));
		}
		for(var i=0;i<3;i++)
		{
			var friendsOnScene=GameObject.FindObjectsOfType<FriendManager>();

			Action<WWW, object[]> doOnFinish =(www,objarray)=>
			{
				var friendCard=(FriendManager)objarray[0];
				friendCard.setUpImage(www.bytes);
			};
			friendsOnScene[i].t.text=friends[i].first_name;
			d.download(friends[i].photo_200,doOnFinish,new object[]{friendsOnScene[i]});
		}
	}
}
