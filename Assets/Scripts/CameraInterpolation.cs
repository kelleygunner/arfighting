using UnityEngine;
using System.Collections;

public class CameraInterpolation : MonoBehaviour 
{
	
	Vector3 oldPosition;

	void Start () 
	{
		oldPosition = this.transform.position;
	}
	

	void LateUpdate () 
	{
		this.transform.position =
			new Vector3(
				Mathf.Lerp(oldPosition.x,this.transform.position.x,Time.deltaTime*5),
				Mathf.Lerp(oldPosition.y,this.transform.position.y,Time.deltaTime*5),
				Mathf.Lerp(oldPosition.z,this.transform.position.z,Time.deltaTime*5));
	}
}
