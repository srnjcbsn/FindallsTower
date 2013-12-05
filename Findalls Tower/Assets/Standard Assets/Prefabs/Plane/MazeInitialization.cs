using UnityEngine;
using System.Collections.Generic;
using MazeGraph;

public class MazeInitialization
{
	private PlaneScript planeScript;
	
	public Transform wallPrefab;
	public Transform pathPrefab;
	public Transform playerPrefab;
	
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
		, int mazeSize 
		, int additionalPaths
		)
	{
		this.planeScript = planeScript;
		this.wallPrefab = wallPrefab;
		this.pathPrefab = pathPrefab;
		this.playerPrefab = playerPrefab;
		this.mazeSize = mazeSize;
		this.additionalPaths = additionalPaths;
	}
	
	public Maze ConstructMaze ()
	{	
		maze = new Maze (mazeSize, additionalPaths);
        
		float scale = planeScript.scale;

		foreach (Point pos in maze.AllPoints())
		{	
       		Transform tileTransform = null;
			Tile tile = maze[pos];
			
			if (tile.GetType() == typeof(WallTile))
			{
				tileTransform = GenerateTile (wallPrefab, planeScript.MazeToPlaneCoords(pos), new Vector3 (scale, scale, scale));
			}
			
			if (tile.GetType() == typeof(Tile))
			{
				tileTransform = GenerateTile (pathPrefab, planeScript.MazeToPlaneCoords(pos), new Vector3(scale, scale / 20f, scale));
				
				TileScript tscript = tileTransform.GetComponent<TileScript> ();
				tscript.Model = tile;
				tscript.planeScript = planeScript;
			}
			
			if (tile.GetType() == typeof(EntryTile))
				entryPosition = tile.Position;
			
			if (tile.GetType() == typeof(ExitTile))
				exitPosition = tile.Position;
			
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
		Transform player = (Transform) MonoBehaviour.Instantiate (playerPrefab);
		player.parent = planeScript.transform;
		player.position = planeScript.MazeToPlaneCoords (entryPosition);
		player.localScale = new Vector3 (.5f,.5f,.5f);
	}
}
