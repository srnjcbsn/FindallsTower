using UnityEngine;
using System.Collections;

public class PlayerLighting : MonoBehaviour
{

    private int counter = 0;
    private Transform myTransform;
    private GameObject plane;
    private GameObject player;

	void Start () {
        myTransform = transform;
        plane = GameObject.Find("Plane");
        player = GameObject.FindGameObjectWithTag("Player");
        //myTransform.Rotate(90f, 355f, 150f);
        //myTransform.eulerAngles.Set(90f, 355f, 355f);
	}


	
	void Update () {

        counter++;
        if (counter % 2 != 0)
            return;


        //GameObject target = GameObject.FindGameObjectsWithTag("Player")[0];    


        myTransform.position = new Vector3(player.transform.position.x, myTransform.position.y, player.transform.position.z);


        myTransform.LookAt(player.transform);

        //Changes the light so it stays in the right angle to only illuminate the player LoS
        //myTransform.localEulerAngles = new Vector3(plane.transform.localEulerAngles.x + 90, 0, plane.transform.localEulerAngles.z);


        //myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
            //Quaternion.LookRotation(player.transform.position - myTransform.position), 0f * Time.deltaTime);
	}

}
