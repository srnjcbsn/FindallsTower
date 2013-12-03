using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MazeGraph;
using System;

public class TileScript : MonoBehaviour 
{
	public Tile Model;
	public PlaneScript planeScript;
	public delegate void EnemyEnteredEventHandler (object sender, Transform enemyTransform);
	public event EnemyEnteredEventHandler EnemyEntered;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	public void OnEnemyEntered (Transform transform)
	{
		if (EnemyEntered != null)
			EnemyEntered (this, transform);
	}
	
	public bool IsTileInWalkingRange (Tile other, int range)
	{
		return planeScript.AreTilesWithinRange(Model, other, range);
	}
	
    void OnCollisionEnter (Collision collision)
    {
		
        Transform collidingTransform = collision.transform;
		
        if (collidingTransform.tag == "Player")
        {
			int range = planeScript.gameObject.GetComponent<PlayerStats> ().visionRange;
			planeScript.HideTilesRevealedBy (collidingTransform.gameObject);
			planeScript.RevealTilesInStraightPath (collidingTransform.gameObject, Model, range);
        }
		
		if (collidingTransform.name == "Enemy")
		{
			OnEnemyEntered (collidingTransform);
		}
    }
}
