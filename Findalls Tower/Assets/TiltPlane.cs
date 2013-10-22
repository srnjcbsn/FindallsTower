using UnityEngine;
using System.Collections;

public class TiltPlane : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Fire1") && this.transform.rotation.eulerAngles.x < 45) 
		{
			this.transform.Rotate(new Vector3(1,0,0), 5);
		}
	}
}
