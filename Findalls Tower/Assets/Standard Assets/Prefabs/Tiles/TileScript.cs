using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using MazeGraph;
using System;

public class TileScript : MonoBehaviour 
{
    private Vector3 playerPos = new Vector3();

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

        if (UnityEngine.Random.Range(1, 701) <= Game.DungeonLevel)
        {
            //Point pos = new Point((int)this.gameObject.transform.localPosition.x, (int)this.gameObject.transform.localPosition.z);
            Vector3 pos = playerPos;
            pos.y += 1;

            float yOffset = PlaneScript.Unit / 4f;
            Vector3 unitScale = new Vector3(PlaneScript.Unit / 2f, PlaneScript.Unit / 2f, PlaneScript.Unit / 2f);

            PlaneScript.GenerateEntity(PlaneScript.TrapPrefab, pos, unitScale);
        }
        
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
            playerPos = collidingTransform.localPosition;
			int range = PlaneScript.gameObject.GetComponent<PlayerStats> ().VisionRange;
			Debug.Log ("vision range: " + range);
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
