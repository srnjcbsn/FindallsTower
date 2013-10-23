using UnityEngine;
using System.Collections;

public class TiltPlane : MonoBehaviour 
{
	public float maxAngle;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
<<<<<<< HEAD
	{	
		Vector3 currentRotation = transform.localEulerAngles;
		float minAngle = 360 - maxAngle;
		
		float deltaXRotation = Input.GetAxis("Mouse Y");
		float deltaZRotation = -1 * Input.GetAxis("Mouse X");
		 
		// Ensure that the angles are in the range [0; 360] by using the modulo operation,
		// since transform.eulerAngles are kept in this range.
		float newXRotation = (currentRotation.x + deltaXRotation) % 360;
		float newZRotation = (currentRotation.z + deltaZRotation) % 360;
		
		// If the new rotation angle is out of range, set it to the bound we are closest to:
		// (This prevents the rotation plane from "snapping" from one extreme to the other)
		if (newXRotation > maxAngle && newXRotation < minAngle)
		{
			if (Mathf.Abs (currentRotation.x - maxAngle) < Mathf.Abs (currentRotation.x - minAngle))
				newXRotation = maxAngle;
			
			else
				newXRotation = minAngle;
		}
		
		if (newZRotation > maxAngle && newZRotation < minAngle)
		{
			if (Mathf.Abs (currentRotation.z - maxAngle) < Mathf.Abs (currentRotation.z - minAngle))
				newZRotation = maxAngle;
			
			else
				newZRotation = minAngle;
=======
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
>>>>>>> d7dcfa2d5e65c0cc3a3f8074e08b3397b87aca0f
		}
		
		Vector3 newRotation = new Vector3 (newXRotation, 0, newZRotation);
		transform.localEulerAngles = newRotation;
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
