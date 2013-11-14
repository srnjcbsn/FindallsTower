using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{

    public float force;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidbody.AddForce(0f, 0f, force, ForceMode.Force);
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigidbody.AddForce(0f, 0f, -force, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigidbody.AddForce(force, 0f, 0f, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody.AddForce(-force, 0f, 0f, ForceMode.Force);
        }
	}
}
