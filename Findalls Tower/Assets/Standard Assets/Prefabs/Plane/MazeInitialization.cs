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
    public Transform itemPrefab;
    public Transform trapPrefab;
    public Transform trapTriggerPrefab;
	public int baseNumberOfEnemies = 10;
    public int baseNumberOfItems = 10;
    public int baseNumberOfTraps = 3;
	
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
        , Transform itemPrefab
        , Transform trapPrefab
        , Transform trapTriggerPrefab
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
        this.itemPrefab = itemPrefab;
        this.trapPrefab = trapPrefab;
        this.trapTriggerPrefab = trapTriggerPrefab;
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
                tileTransform = planeScript.GenerateEntity(wallPrefab, planeScript.MazeToPlaneCoords(pos), new Vector3(scale, scale * 2f, scale));
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

                tileTransform = planeScript.GenerateEntity(prefab, planeScript.MazeToPlaneCoords(pos, -1f * scale / 2f), new Vector3(scale, scale, scale));
				
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
        tileTransform2 = planeScript.GenerateEntity(wallPrefab, new Vector3(0, scale / 2, -5 - (scale / 2)), new Vector3(10, scale, scale));   // South wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = planeScript.GenerateEntity(wallPrefab, new Vector3(0, scale / 2, 5 + (scale / 2)), new Vector3(10, scale, scale));    // North wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = planeScript.GenerateEntity(wallPrefab, new Vector3(5 + (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));   // East Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = planeScript.GenerateEntity(wallPrefab, new Vector3(-5 - (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));  // West Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
		
		return maze;
	}
	
	
	
	public void PopulateMaze ()
	{
		System.Random rng = new System.Random ();
		float yOffset = planeScript.unit / 4f;
		Vector3 unitScale = new Vector3 (planeScript.unit / 2f, planeScript.unit / 2f, planeScript.unit / 2f);
		
		HashSet<Point> usedTiles = new HashSet<Point> ();
		usedTiles.Add (entryPosition);
		
		Vector3 playerSpawnPosition = planeScript.MazeToPlaneCoords (entryPosition, yOffset);
        planeScript.GenerateEntity(playerPrefab, playerSpawnPosition, unitScale);
		
		int numberOfEnemies = rng.Next (baseNumberOfEnemies, baseNumberOfEnemies + Game.DungeonLevel);
		
		while (numberOfEnemies > 0)
		{
			Point randomPos = planeScript.Maze.RandomPointInPath ();
			if (usedTiles.Contains (randomPos))
				continue;
			
			usedTiles.Add (randomPos);
            planeScript.GenerateEntity(planeScript.EnemyPrefab, planeScript.MazeToPlaneCoords(randomPos, yOffset), unitScale);
			
			numberOfEnemies--;
		}


        int numberOfItems = rng.Next(baseNumberOfItems, baseNumberOfItems + Game.DungeonLevel);

        while (numberOfItems > 0)
        {
            Point randomPos = planeScript.Maze.RandomPointInPath();
            if (usedTiles.Contains(randomPos))
                continue;

            usedTiles.Add(randomPos);
            planeScript.GenerateEntity(planeScript.ItemPrefab, planeScript.MazeToPlaneCoords(randomPos, yOffset), unitScale);

            numberOfItems--;
        }


        //int numberOfTraps = rng.Next(baseNumberOfTraps, baseNumberOfTraps + Game.DungeonLevel);

        //while (numberOfTraps > 0)
        //{
        //    Point randomPos = planeScript.Maze.RandomPointInPath();
        //    if (usedTiles.Contains(randomPos))
        //        continue;

        //    usedTiles.Add(randomPos);
        //    Transform trigger = planeScript.GenerateEntity(planeScript.TrapTriggerPrefab, planeScript.MazeToPlaneCoords(randomPos, 0.0001f), unitScale);

        //    Transform trap = planeScript.GenerateEntity(planeScript.TrapPrefab, planeScript.MazeToPlaneCoords(randomPos, yOffset), unitScale);


        //    numberOfTraps--;
        //}
	}
	
	
}
