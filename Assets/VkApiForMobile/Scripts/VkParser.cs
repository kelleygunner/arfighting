using UnityEngine;
using System.Collections;

public class VkParser
{
	
	public static com.playGenesis.VkUnityPlugin.VKUser GetUserWithBdateCityCountry(string response)
	{
		com.playGenesis.VkUnityPlugin.VKUser user = new com.playGenesis.VkUnityPlugin.VKUser();
		
		//response=response.Remove(0,16);
		//user.id = System.Convert.ToInt32(response.Substring(0,response.IndexOf(',')));
		
		if (response.Contains("first_name"))
		{
			response = response.Remove(0,response.IndexOf("first_name")+13);
			user.first_name = response.Substring(0,response.IndexOf('\"'));			
		}
		if (response.Contains("last_name"))
		{
			response = response.Remove(0,response.IndexOf("last_name")+12);
			user.first_name = response.Substring(0,response.IndexOf('\"'));			
		}
		if (response.Contains("sex"))
		{
			response = response.Remove(0,response.IndexOf("sex")+5);
			user.sex = System.Convert.ToInt32(response.Substring(0,1));			
		}
		if (response.Contains("bdate"))
		{
			response = response.Remove(0,response.IndexOf("bdate")+8);
			user.bdate = response.Substring(0,response.IndexOf('\"'));			
		}
		if (response.Contains("city"))
		{
			response = response.Remove(0,response.IndexOf("title")+8);
			user.city = response.Substring(0,response.IndexOf('\"'));			
		}
		if (response.Contains("country"))
		{
			response = response.Remove(0,response.IndexOf("title")+8);
			user.country = response.Substring(0,response.IndexOf('\"'));
		}
		
		return user;
	}
	
}
