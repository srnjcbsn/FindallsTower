using UnityEngine;
using System.Collections;
using MazeGraph;

public class TileScript : MonoBehaviour 
{
	public Tile Model;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public bool IsTileInWalkingRange (Tile other, int range)
	{
		GameObject plane = GameObject.Find ("Plane");
		return plane.GetComponent<TiltPlane>().AreTilesWithinRange(Model, other, range);
	}
	
	void OnCollisionEnter ()
	{
		//Debug.Log("COLLISION");	
	}
}
