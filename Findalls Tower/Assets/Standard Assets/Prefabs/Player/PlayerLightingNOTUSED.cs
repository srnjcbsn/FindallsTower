using UnityEngine;
using System.Collections;

public class PlayerLighting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Make a game object
        GameObject lightGameObject = new GameObject("The Light");

        // Add the light component
        Component q = lightGameObject.AddComponent(typeof(Light));
        q.name = "qq";

        //Set properties
        lightGameObject.light.color = Color.white;
        lightGameObject.light.type = LightType.Spot;
        lightGameObject.light.spotAngle = 91;
        lightGameObject.light.intensity = 2;
        lightGameObject.light.range = 20;

        // Set the position and rotation
        this.gameObject.AddComponent("qq");
        lightGameObject.transform.position = new Vector3(0, 8, 0);
        lightGameObject.transform.eulerAngles = new Vector3(90, 0, 0);

        //Light light = new Light();
        //light.name = "The Light";
        ////Set properties
        //light.color = Color.white;
        //light.type = LightType.Spot;
        //light.spotAngle = 91;
        //light.intensity = 2;
        //light.range = 20;

        //// Set the position and rotation
        //this.gameObject.AddComponent("The Light");
        //light.transform.position = new Vector3(0, 8, 0);
        //light.transform.eulerAngles = new Vector3(90, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
