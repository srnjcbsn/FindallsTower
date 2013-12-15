using UnityEngine;
using System.Collections;
using MazeGraph;

public class EnemyAI : MonoBehaviour
{
    private bool destroy = false;

	public float MoveSpeed;

	public bool WanderPackage = true;
	public bool FollowPackage = true;

	private Transform myTransform;
	private PlaneScript planeScript;

	private bool IsFollowingPlayer = false;

	private Vector3 targetPosition;
	private System.Random rng;

	private Transform player;
	public Transform Player { get { return player; } }
	private Enemy enemyScript;

	private Vector3 lastKnownPlayerLocation;
	private Vector3 lastPosition;

	void Awake ()
	{
		myTransform = transform; //cache transform data for easy access/preformance
        FindPlayer();
	}

    

	void Start ()
	{
        
		targetPosition = transform.localPosition;
		planeScript = GameObject.Find ("Plane").GetComponent<PlaneScript> ();
		enemyScript = GetComponent<Enemy> ();
		rng = new System.Random ();
	}

	void Update ()
	{
        if (PlaneScript.DestroySelf)
            Destroy(this);
		rigidbody.velocity = Vector3.zero;

		if (IsFollowingPlayer)
		{
			Follow ();
		}

		Vector3 newPos = Vector3.MoveTowards (transform.localPosition, targetPosition, MoveSpeed * Time.deltaTime);

		// If we have reached the target position, or if we're stuck, get a new wander target, and move towards that
		if (newPos == transform.localPosition || lastPosition == transform.localPosition)
		{
			Wander ();
			newPos = Vector3.MoveTowards (transform.localPosition, targetPosition, MoveSpeed * Time.deltaTime);
		}

		lastPosition = transform.localPosition;
		transform.localPosition = newPos;
	}

	public void Wander ()
	{
		Tile thisTile = planeScript.GetTileOfPoint (planeScript.PlaneToMazeCoords (myTransform.localPosition));
		Tile targetTile = planeScript.Maze.RandomAdjacentPath (thisTile, rng);
		targetPosition = planeScript.MazeToPlaneCoords (targetTile.Position, transform.localPosition.y);
	}

	void Follow ()
	{
		if (enemyScript.tilesInVision.Contains (planeScript.GetTileOfPoint (planeScript.PlaneToMazeCoords (player.localPosition))))
			lastKnownPlayerLocation = player.localPosition;
		else
			IsFollowingPlayer = false;

		targetPosition = lastKnownPlayerLocation;
	}

	public void PlayerSpotted (object sender)
	{
		IsFollowingPlayer = true;
	}

    public void FindPlayer()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    
}
