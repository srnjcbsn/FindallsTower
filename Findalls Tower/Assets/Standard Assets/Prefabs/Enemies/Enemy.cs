using UnityEngine;
using System.Collections.Generic;
using MazeGraph;

public class Enemy : MonoBehaviour
{
	public int VisionRange;
	int health;
	int defense;
	int attack;
	private PlaneScript planeScript;
	private EnemyAI AIScript;
	private Transform currentTile;

	public HashSet<Tile> tilesInVision;

	public Transform CurrentTile { get { return currentTile; } }

	static int level;
	static int offset;

	void Awake ()
	{
		planeScript = GameObject.FindWithTag ("Plane").GetComponent<PlaneScript> ();
		AIScript = GetComponent<EnemyAI> ();
		tilesInVision = new HashSet<Tile> ();

		level = Game.DungeonLevel;
		offset = level;

		health = Random.Range (level, level + offset + 1);
		defense = 0;
		attack = 1;
	}

	void Start ()
	{
//		planeScript = GameObject.FindWithTag ("Plane").GetComponent<PlaneScript> ();
//		AIScript = GetComponent<EnemyAI> ();
	}

	private void TileVisibilityChanged (object sender, Transform tileTransform)
	{
		if (tileTransform.GetComponent<TileVisibility> ().IsVisible)
			MakeOpaque ();
		else
			MakeTransparent ();
	}

	void OnCollisionEnter (Collision col)
	{	
		if (col.gameObject.CompareTag ("Path"))
		{
			TileVisibilityChanged (null, col.transform);

			col.gameObject.GetComponent<TileVisibility> ().VisibilityChangedEvent += TileVisibilityChanged;
			
			if (currentTile != null)
				currentTile.GetComponent<TileVisibility> ().VisibilityChangedEvent -= TileVisibilityChanged;
			
			currentTile = col.transform;

			foreach (Tile tile in tilesInVision)
				planeScript.tileDict [tile].GetComponent<TileScript> ().PlayerEnteredEvent -= AIScript.PlayerSpotted;

			tilesInVision = new HashSet<Tile> ();

			Debug.Log (col.transform.GetComponent<TileScript>().Model);

			foreach (Tile tile in planeScript.TilesInPath (col.transform.GetComponent<TileScript> ().Model, VisionRange))
			{
				if (planeScript.PlaneToMazeCoords (AIScript.Player.localPosition) == tile.Position)
					AIScript.PlayerSpotted (null);
				planeScript.tileDict [tile].GetComponent<TileScript> ().PlayerEnteredEvent += AIScript.PlayerSpotted;
				tilesInVision.Add (tile);
			}
		}

		if (col.gameObject.CompareTag ("Enemy"))
		{
			Debug.Log ("enemies collided");
			AIScript.Wander ();
		}
		
		//When colliding with player damage eachother.
		if (col.gameObject.tag == "Player")
		{
			Debug.Log ("Enemy collides with player");
			int playerAttack = PlayerStats.FightEnemy (attack);
			//Check to see if the enemy dies
			if (FightPlayer (playerAttack))
			{
				Game.EnemyKilled ();
				Dead ();
			}
		}
	}

	private void MakeTransparent ()
	{
//		Color thisColor = renderer.material.color;
//		renderer.material.color = new Color (thisColor.r, thisColor.g, thisColor.b, 0f);
	}

	private void MakeOpaque ()
	{
		Color thisColor = renderer.material.color;
		renderer.material.color = new Color (thisColor.r, thisColor.g, thisColor.b, 1f);
	}

	bool FightPlayer (int playerAttack)
	{
		health -= (playerAttack - defense);

		return health <= 0;
	}

	private void Dead ()
	{
		gameObject.SetActive (false);
	}
}
