using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MazeGraph;
using System;

public class TileScript : MonoBehaviour 
{
	public Tile Model;
	public PlaneScript PlaneScript { get; set; }

	public delegate void EnemyEnteredEventHandler (object sender, Transform enemyTransform);
	public event EnemyEnteredEventHandler EnemyEnteredEvent;

	public delegate void PlayerEnteredEventHandler (object sender);
	public event PlayerEnteredEventHandler PlayerEnteredEvent;
	
	public void OnEnemyEntered (Transform transform)
	{
		if (EnemyEnteredEvent != null)
			EnemyEnteredEvent (this, transform);
	}

	public void OnPlayerEntered ()
	{
		if (PlayerEnteredEvent != null)
			PlayerEnteredEvent (this);
	}
	
	public bool IsTileInWalkingRange (Tile other, int range)
	{
		return PlaneScript.AreTilesWithinRange(Model, other, range);
	}
	
    void OnCollisionEnter (Collision collision)
    {
		
        Transform collidingTransform = collision.transform;
		
        if (collidingTransform.tag == "Player")
        {
			int range = PlaneScript.gameObject.GetComponent<PlayerStats> ().visionRange;
			PlaneScript.HideTilesRevealedBy (collidingTransform.gameObject);
			PlaneScript.RevealTilesInStraightPath (collidingTransform.gameObject, Model, range);
			OnPlayerEntered ();
        }
		
		if (collidingTransform.name == "Enemy")
		{
			OnEnemyEntered (collidingTransform);
		}
    }
}
