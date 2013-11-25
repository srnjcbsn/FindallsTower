using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MazeGraph;
using System;

public class TileScript : MonoBehaviour 
{
	public Tile Model;
	public TiltPlane plane;
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
		return plane.AreTilesWithinRange(Model, other, range);
	}
	
    void OnCollisionEnter (Collision collision)
    {
		
        Transform collidingTransform = collision.transform;
		
        if (collidingTransform.name == "Player")
        {
			int range = collidingTransform.GetComponent<PlayerStats> ().visionRange;
			plane.HideTilesRevealedBy (collidingTransform.gameObject);
			plane.RevealTilesInStraightPath (collidingTransform.gameObject, Model, range);
        }
		
		if (collidingTransform.name == "Enemy")
		{
			OnEnemyEntered (collidingTransform);
		}
    }
}
