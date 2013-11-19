using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour 
{
    public int trapNr = 0;
    private string name = "StoneTrap";
    GameObject trap;
	// Use this for initialization
	void Start () 
    {
        //If more traps are placed they can be individually tied to a trigger
        //This was done because making traps children of triggers was not very practical
        if (trapNr != 0)
            name += trapNr;
        //Deactivate the trap that you control
        trap = GameObject.Find(name).gameObject;
        Debug.Log("Found: " + trap);
        trap.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnCollisionEnter(Collision col)
    {
        //Player can set off the trap, activating it and deactivating the trigger
        if (col.gameObject.name == "Player")
        {
            trap.SetActive(trap);
            gameObject.SetActive(false);
        }

    }
}
