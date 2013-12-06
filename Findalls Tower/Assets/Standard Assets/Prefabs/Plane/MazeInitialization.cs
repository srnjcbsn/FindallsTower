using UnityEngine;
using System.Collections.Generic;
using MazeGraph;

public class MazeInitialization
{
	private PlaneScript planeScript;
	
	public Transform wallPrefab;
	public Transform pathPrefab;
	public Transform playerPrefab;
	public Transform entryPrefab;
	public Transform exitPrefab;
	
	public int mazeSize;
	public int additionalPaths;
	
	private Point entryPosition;
	private Point exitPosition;
	
	private Maze maze;
	
	public MazeInitialization 
		( PlaneScript planeScript
		, Transform wallPrefab 
		, Transform pathPrefab 
		, Transform playerPrefab
		, Transform entryPrefab
		, Transform exitPrefab
		, int mazeSize 
		, int additionalPaths
		)
	{
		this.planeScript = planeScript;
		this.wallPrefab = wallPrefab;
		this.pathPrefab = pathPrefab;
		this.playerPrefab = playerPrefab;
		this.entryPrefab = entryPrefab;
		this.exitPrefab = exitPrefab;
		this.mazeSize = mazeSize;
		this.additionalPaths = additionalPaths;
	}
	
	public Maze ConstructMaze ()
	{	
		maze = new Maze (mazeSize, additionalPaths);
        
		float scale = planeScript.unit;

		foreach (Point pos in maze.AllPoints())
		{	
       		Transform tileTransform = null;
			Tile tile = maze[pos];
			
			if (tile.GetType() == typeof(WallTile))
			{
				tileTransform = GenerateTile (wallPrefab, planeScript.MazeToPlaneCoords(pos), new Vector3 (scale, scale * 2f, scale));
			}
			
			else if (tile is Tile)
			{
				Transform prefab = pathPrefab;
				
				if (tile.GetType() == typeof(EntryTile))
				{
					Debug.Log ("Entry position");
					entryPosition = tile.Position;
					prefab = entryPrefab;
				}
			
				if (tile.GetType() == typeof(ExitTile))
				{
					exitPosition = tile.Position;
					prefab = exitPrefab;
				}
				
				tileTransform = GenerateTile (prefab, planeScript.MazeToPlaneCoords(pos, -1f * scale /2f), new Vector3(scale, scale, scale));
				
				TileScript tscript = tileTransform.GetComponent<TileScript> ();
				Debug.Log(tscript);
				tscript.Model = tile;
				tscript.PlaneScript = planeScript;
			}
			
			planeScript.tileDict.Add(tile, tileTransform);
			tileTransform.gameObject.layer = LayerMask.NameToLayer ("Hidden");
		}		
		
		Transform tileTransform2;
		// generate outer walls
        tileTransform2 = GenerateTile(wallPrefab, new Vector3(0, scale / 2, -5 - (scale / 2)), new Vector3(10, scale, scale));   // South wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(wallPrefab, new Vector3(0, scale / 2, 5 + (scale / 2)), new Vector3(10, scale, scale));    // North wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(wallPrefab, new Vector3(5 + (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));   // East Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(wallPrefab, new Vector3(-5 - (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));  // West Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
		
		return maze;
	}
	
	private Transform GenerateTile (Transform TileType, Vector3 position, Vector3 scaleVector)
	{
		Transform tile = (Transform) MonoBehaviour.Instantiate (TileType);
		tile.parent = planeScript.transform;
		tile.localPosition = position;
		tile.localScale = scaleVector;
		
		return tile;
	}
	
	public void PopulateMaze ()
	{
		Debug.Log ("entry pos: " + entryPosition);
		Transform player = (Transform) MonoBehaviour.Instantiate (playerPrefab);
		player.parent = planeScript.transform;
		player.localPosition = planeScript.MazeToPlaneCoords (entryPosition);
		player.position = new Vector3 (player.position.x, planeScript.unit / 3f, player.position.z);
		Debug.Log ("player pos: " + player.position);
		player.localScale = new Vector3 (planeScript.unit / 2f, planeScript.unit / 2f, planeScript.unit / 2f);
	}
}
