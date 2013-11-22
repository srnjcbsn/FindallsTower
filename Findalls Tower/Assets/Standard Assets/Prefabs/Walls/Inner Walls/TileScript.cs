using UnityEngine;
using System.Collections;
using MazeGraph;

public class TileScript : MonoBehaviour 
{
	public Tile Model;
	private TiltPlane plane;
	
	// Use this for initialization
	void Start () 
	{
		plane = GameObject.Find ("Plane").GetComponent<TiltPlane> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public bool IsTileInWalkingRange (Tile other, int range)
	{
		return plane.AreTilesWithinRange(Model, other, range);
	}
	
    //void OnCollisionEnter (Collision collision)
    //{
    //    Transform collidingTransform = collision.transform;
		
    //    if (collidingTransform.name == "Player")
    //    {
    //        LightTilesInRange (collidingTransform.GetComponent<Player> ().visionRange);
    //    }
    //}
	
	private void LightTilesInRange (int range)
	{
		Debug.Log(Model);
		Debug.Log(plane);
		plane.LightTilesInRange(Model, range);
	}
}
