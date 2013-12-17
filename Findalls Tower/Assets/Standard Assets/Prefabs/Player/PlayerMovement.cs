using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed;
	public float MaxVelocity;

	void Start ()
	{
		rigidbody.maxAngularVelocity = MaxVelocity;
	}

	void Update ()
	{
		Vector3 direction = Vector3.zero;

		if (Input.GetKey (KeyCode.W))
			direction += Vector3.forward;
		if (Input.GetKey (KeyCode.S))
			direction -= Vector3.forward;
		if (Input.GetKey (KeyCode.D))
			direction += Vector3.right;
		if (Input.GetKey (KeyCode.A))
			direction -= Vector3.right;

		rigidbody.AddForce (direction * moveSpeed, ForceMode.VelocityChange);

		if (rigidbody.velocity.magnitude > MaxVelocity)
			rigidbody.velocity = rigidbody.velocity.normalized * MaxVelocity;
	}
}
