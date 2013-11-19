using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{

    public float force;
    public int ReduceForceXFrame = 10;
    public int moveSpeed = 2;
    public int rotationSpeed = 3; //speed of turning
    public float MaxVelocity = 1.7f;

    private Transform myTransform;
    private int counter = 0;

    void Awake()
    {
        myTransform = transform; //cache transform data for easy access/preformance
    }

    void Start()
    {
        rigidbody.maxAngularVelocity = MaxVelocity;
    }
	
	void Update () 
    {
        Vector3 infront = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z + 1);

        myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(infront - myTransform.position), rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            //rigidbody.AddForce(0f, 0f, force, ForceMode.Force);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //rigidbody.AddForce(0f, 0f, -force, ForceMode.Force);
            myTransform.position += -myTransform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //rigidbody.AddForce(force, 0f, 0f, ForceMode.Force);
            myTransform.position += myTransform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //rigidbody.AddForce(-force, 0f, 0f, ForceMode.Force);
            myTransform.position += -myTransform.right * moveSpeed * Time.deltaTime;
        }

        if (rigidbody.velocity.magnitude > MaxVelocity)
            rigidbody.velocity = rigidbody.velocity.normalized * MaxVelocity;
        //Debug.Log("vel: " + myTransform.rigidbody.velocity.magnitude);
	}
}
