using UnityEngine;
using System.Collections;

public class PlayerLighting : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Make a game object
        GameObject lightGameObject = new GameObject("The Light");

        // Add the light component
        lightGameObject.AddComponent(typeof(Light));

        // Set color and position
        lightGameObject.light.color = Color.white;
        lightGameObject.light.type = LightType.Spot;

        // Set the position (or any transform property) after
        // adding the light component.
        lightGameObject.transform.position = new Vector3(0, 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
