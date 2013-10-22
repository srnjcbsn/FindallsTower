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
		if (Input.GetAxis("Vertical")>0 && (this.transform.rotation.eulerAngles.x >= 329 || transform.rotation.eulerAngles.x <= 25)) 
		{
			this.transform.RotateAround(new Vector3(0,0,0),new Vector3(1,0,0), 5);
		}
		else if (Input.GetAxis("Vertical")<0 && (this.transform.rotation.eulerAngles.x >= 335 || transform.rotation.eulerAngles.x <= 31)) 
		{
			this.transform.RotateAround(new Vector3(0,0,0),new Vector3(1,0,0), -5);
		}
		
		if (Input.GetAxis("Horizontal")>0 && (this.transform.rotation.eulerAngles.z >= 335 || transform.rotation.eulerAngles.z <= 31)) 
		{
			this.transform.RotateAround(new Vector3(0,0,0),new Vector3(0,0,1), -5);
		}
		else if (Input.GetAxis("Horizontal")<0 && (this.transform.rotation.eulerAngles.z >= 329 || transform.rotation.eulerAngles.z <= 25)) 
		{
			this.transform.RotateAround(new Vector3(0,0,0),new Vector3(0,0,1), 5);
		}
	}
	
//	void Update () 
//	{
//		if (Input.GetAxis("Mouse Y")>0 && (this.transform.rotation.eulerAngles.x >= 329 || transform.rotation.eulerAngles.x <= 25)) 
//		{
//			this.transform.Rotate(new Vector3(1,0,0), 5);
//		}
//		else if (Input.GetAxis("Mouse Y")<0 && (this.transform.rotation.eulerAngles.x >= 335 || transform.rotation.eulerAngles.x <= 31)) 
//		{
//			this.transform.Rotate(new Vector3(1,0,0), -5);
//		}
//		
//		if (Input.GetAxis("Mouse X")>0 && (this.transform.rotation.eulerAngles.z >= 329 || transform.rotation.eulerAngles.z <= 25)) 
//		{
//			this.transform.Rotate(new Vector3(0,0,1), 5);
//		}
//		else if (Input.GetAxis("Mouse X")<0 && (this.transform.rotation.eulerAngles.z >= 335 || transform.rotation.eulerAngles.z <= 31)) 
//		{
//			this.transform.Rotate(new Vector3(0,0,1), -5);
//		}
//	}
}
