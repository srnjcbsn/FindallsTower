using UnityEngine;
using System.Collections.Generic;
using MazeGraph;
using System.Linq;
using System;

public class PlaneScript : MonoBehaviour 
{
	public Transform WallPrefab;
	public Transform PathPrefab;
	public Transform PlayerPrefab;
	public Transform EnemyPrefab;
	public Transform EntryPrefab;
	public Transform ExitPrefab;
    public Transform ItemPrefab;
    public Transform TrapPrefab;
    public Transform TrapTriggerPrefab;
	public int mazeSize;
	public int additionalPaths;

    private static bool destroy = false;
    public static bool DestroySelf { get { return destroy; } }
	
	private Maze maze;
	internal float unit;
    public float Unit { get { return unit;} }
	private Dictionary<GameObject, HashSet<Tile>> tilesRevealedBy;
	public Dictionary<Tile, Transform> tileDict;
	
	public Maze Maze {
		get { return maze; } 
	}
	
	void Start () 
	{
		transform.localScale = new Vector3 (mazeSize, mazeSize, mazeSize);
		unit = 10f / mazeSize;
		tileDict = new Dictionary<Tile, Transform> ();
		tilesRevealedBy = new Dictionary<GameObject, HashSet<Tile>> ();

        CreateLevel();
	}

    void Update()
    {
        if (ExitTileTrigger.exit)
            NewLevel();
    }

    private void DestroyLevel()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in this.transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(child => Destroy(child));
        maze = null;
        destroy = true;
    }

    private void CreateLevel()
    {
        destroy = false;
        MazeInitialization mazeInitializer = new MazeInitialization
            (
                this,
                WallPrefab,
                PathPrefab,
                PlayerPrefab,
                EntryPrefab,
                ExitPrefab,
                ItemPrefab,
                TrapPrefab,
                TrapTriggerPrefab,
                mazeSize,
                additionalPaths
            );

        maze = mazeInitializer.ConstructMaze();
        mazeInitializer.PopulateMaze();
        
    }

	public void Restart ()
	{
		Game.Restart ();
		DestroyLevel ();
		CreateLevel ();
		PlayerStats.InitializePlayer ();
	}

    public void NewLevel()
    {
        ExitTileTrigger.exit = false;
        Game.NewLevel();
        DestroyLevel();
        CreateLevel();
    }
	
	public Vector3 MazeToPlaneCoords (Point mazeCoords, float yOffset)
	{
		int offset = 5;
		
		float scalefactor = (float)mazeSize / 10f;
		
		float planeX = (mazeCoords.X / scalefactor) - offset + unit / 2;
		float planeY = (mazeCoords.Y / scalefactor) - offset + unit / 2;
		
		return new Vector3(planeX, yOffset, planeY);
	}
	
	public Vector3 MazeToPlaneCoords (Point mazeCoords)
	{
		return MazeToPlaneCoords (mazeCoords, 0f);
	}
	
	public Point PlaneToMazeCoords (Vector3 planeCoords)
	{
		int offset = 5;
		float scalefactor = (float)mazeSize / 10f;
		
		int mazeX = Mathf.Clamp (Mathf.FloorToInt( (planeCoords.x + offset) * scalefactor), 0, mazeSize - 1);
		int mazeY = Mathf.Clamp (Mathf.FloorToInt( (planeCoords.z + offset) * scalefactor), 0, mazeSize - 1);
		
		return new Point(mazeX, mazeY);
	}
	
	public Tile GetTileOfPoint(Point pos)
    {
		return maze[pos];
    }
	
	public bool AreTilesWithinRange(Tile t1, Tile t2, int range)
	{
		return maze.AreTilesWithinRange(t1, t2, range);
	}
	
	public void RevealTilesInStraightPath (GameObject revealer, Tile fromTile, int range)
	{
		HashSet<Tile> revealed = new HashSet<Tile> ();
		
		foreach (Tile tile in TilesInPathWithWalls (fromTile, range))
		{
			// if tile is made visible for the first time
			if (!((TileVisibility)tileDict[tile].GetComponent<TileVisibility>()).IsDiscovered)
				Game.TileUncovered ();

			tileDict[tile].GetComponent<TileVisibility> ().Reveal (revealer);
			revealed.Add (tile);
		}

		tilesRevealedBy.Add (revealer, revealed);
	}
	
	public void HideTilesRevealedBy (GameObject hider)
	{
		if (!tilesRevealedBy.ContainsKey (hider))
			return;
		
		foreach (Tile tile in tilesRevealedBy[hider])
			tileDict[tile].GetComponent<TileVisibility> ().Hide (hider);
		
		tilesRevealedBy.Remove(hider);
	}

	public IEnumerable<Tile> TilesInPath (Tile fromTile, int range)
	{
		HashSet<Vertex> vertices = new HashSet<Vertex> ();

		vertices.UnionWith (VerticesInPath (fromTile, new Point (1, 0), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (0, 1), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (-1, 0), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (0, -1), range));

		return vertices.Select (vert => vert.Content);
	}
	
	public IEnumerable<Tile> TilesInPathWithWalls (Tile fromTile, int range)
	{
		HashSet<Vertex> vertices = new HashSet<Vertex> ();
		
		vertices.UnionWith (VerticesInPath (fromTile, new Point (1, 0), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (0, 1), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (-1, 0), range));
		vertices.UnionWith (VerticesInPath (fromTile, new Point (0, -1), range));
		
		foreach (Vertex vertex in vertices)
		{
			yield return vertex.Content;
			foreach (Vertex adjWall in vertex.Adjacent.Where (vert => vert.Content.GetType () == typeof(WallTile)))
				yield return adjWall.Content;
		}
	}
		
	private IEnumerable<Vertex> VerticesInPath (Tile fromTile, Point direction, int range)
	{
		
		for (int i = 0; i < range; i++)
		{
			Point newPoint = fromTile.Position + new Point (direction.X * i, direction.Y * i);
			
			int max = maze.Dimension;
			if (newPoint.X >= max || newPoint.X < 0 || newPoint.Y >= max || newPoint.Y < 0)
				break; // handle this in maze[Point] property
			
			Vertex newVertex = maze.MazeGraph [newPoint];
			
			if (newVertex.Content.GetType() == typeof (WallTile))
				break;
			
			yield return newVertex;
		}
	}
	
	private bool TileInRange (Point fromPos, Point pos, int range)
	{	
		return (pos.X == fromPos.X && Mathf.Abs(Mathf.Abs(fromPos.Y) - Mathf.Abs(pos.Y)) < range)
				|| (pos.Y == fromPos.Y && Mathf.Abs(Mathf.Abs(fromPos.X) - Mathf.Abs(pos.X)) < range);
	}

    public Transform GenerateEntity(Transform prefab, Vector3 position, Vector3 scaleVector)
    {
        Transform entity = (Transform)MonoBehaviour.Instantiate(prefab);
        entity.parent = this.transform;
        entity.localPosition = position;
        entity.localScale = scaleVector;

        return entity;
    }
}
