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
		}
		
		Vector3 newRotation = new Vector3 (newXRotation, 0, newZRotation);
		transform.localEulerAngles = newRotation;
	}
}
