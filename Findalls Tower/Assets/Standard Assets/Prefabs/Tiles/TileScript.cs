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
	public event EnemyEnteredEventHandler EnemyEntered;
	
	public void OnEnemyEntered (Transform transform)
	{
		if (EnemyEntered != null)
			EnemyEntered (this, transform);
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
        }
		
		if (collidingTransform.name == "Enemy")
		{
			OnEnemyEntered (collidingTransform);
		}
    }
}
