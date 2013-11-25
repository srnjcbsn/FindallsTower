using UnityEngine;
using System.Collections.Generic;
using MazeGraph;
using System.Linq;

public class TiltPlane : MonoBehaviour
{
	public float maxAngle;
	public Transform WallPrefab;
	public Transform PathPrefab;
	public int mazeSize;
	public int additionalPaths;
    public bool disableTilting = false;
	
	private Maze maze;
	private float scale;
	private Dictionary<GameObject, HashSet<Tile>> tilesRevealedBy;
	
	public Maze Maze {
		get { return maze; } 
	}
    
	public Dictionary<Tile, Transform> tileDict;
	
	void Start () 
	{
		tileDict = new Dictionary<Tile, Transform> ();
		tilesRevealedBy = new Dictionary<GameObject, HashSet<Tile>> ();
		
		maze = new Maze (mazeSize, additionalPaths);
        scale = 10f / mazeSize;

		foreach (Point pos in maze.AllPoints())
		{
			
       		Transform tileTransform = null;
			Tile tile = maze[pos];
			
			if (tile.GetType() == typeof(WallTile))
			{
				tileTransform = GenerateTile (WallPrefab, MazeToPlaneCoords(pos), new Vector3(scale, scale, scale));
				tileTransform.gameObject.layer = LayerMask.NameToLayer ("Hidden");
			}
			
			if (tile.GetType() == typeof(Tile))
			{
				tileTransform = GenerateTile (PathPrefab, MazeToPlaneCoords(pos), new Vector3(scale, scale / 20f, scale));
				tileTransform.gameObject.layer = LayerMask.NameToLayer ("Hidden");
				
				
				TileScript tscript = tileTransform.GetComponent<TileScript> ();
				tscript.Model = tile;
				tscript.plane = this;
			}
			
			tileDict.Add(tile, tileTransform);
		}		
		
		Transform tileTransform2;
		// generate outer walls
        tileTransform2 = GenerateTile(WallPrefab, new Vector3(0, scale / 2, -5 - (scale / 2)), new Vector3(10, scale, scale));   // South wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(WallPrefab, new Vector3(0, scale / 2, 5 + (scale / 2)), new Vector3(10, scale, scale));    // North wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(WallPrefab, new Vector3(5 + (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));   // East Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
        tileTransform2 = GenerateTile(WallPrefab, new Vector3(-5 - (scale / 2), scale / 2, 0), new Vector3(scale, scale, 10 + (scale * 2)));  // West Wall
        tileTransform2.gameObject.layer = LayerMask.NameToLayer("Visible");
    }

	
	private Transform GenerateTile (Transform TileType, Vector3 position, Vector3 scaleVector)
	{
		Transform tile = (Transform)Instantiate (TileType);
		tile.parent = this.transform;
		tile.localPosition = position;
		tile.localScale = scaleVector;
		
		return tile;
	}
	
	public Vector3 MazeToPlaneCoords (Point mazeCoords)
	{
		int offset = 5;
		
		float scalefactor = (float)mazeSize / 10f;
		
		float planeX = (mazeCoords.X / scalefactor) - offset + scale / 2;
		float planeY = (mazeCoords.Y / scalefactor) - offset + scale / 2;
		
		return new Vector3(planeX, scale / 2, planeY);
	}
	
	public Point PlaneToMazeCoords (Vector3 planeCoords)
	{
		int offset = 5;
		float scalefactor = (float)mazeSize / 10f;
		
		int mazeX = Mathf.FloorToInt( (planeCoords.x + offset) * scalefactor);
		int mazeY = Mathf.FloorToInt( (planeCoords.z + offset) * scalefactor);
		
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
	
	private IEnumerable<Tile> TilesInPathWithWalls (Tile fromTile, int range)
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
	
	void Update () 
	{
        if (disableTilting)
            return;
			
        Vector3 currentRotation = transform.localEulerAngles;
        float minAngle = 360 - maxAngle;

        float deltaXRotation = Input.GetAxis("Mouse Y");
        float deltaZRotation = -1 * Input.GetAxis("Mouse X");

        // Ensure that the angles are in the range [0; 360] by using the modulo operation,
        // since transform.eulerAngles are kept in this range.
        float newXRotation = (currentRotation.x + deltaXRotation) % 360;
        float newZRotation = (currentRotation.z + deltaZRotation) % 360;

        // If the new rotation angle is out of range, set it to the bound we are closest to:
        // (This prevents the rotation plane from "snapping" from one extreme to the other)
        if (newXRotation > maxAngle && newXRotation < minAngle)
        {
            if (Mathf.Abs(currentRotation.x - maxAngle) < Mathf.Abs(currentRotation.x - minAngle))
                newXRotation = maxAngle;

            else
                newXRotation = minAngle;
        }

        if (newZRotation > maxAngle && newZRotation < minAngle)
        {
            if (Mathf.Abs(currentRotation.z - maxAngle) < Mathf.Abs(currentRotation.z - minAngle))
                newZRotation = maxAngle;

            else
                newZRotation = minAngle;
        }

        Vector3 newRotation = new Vector3(newXRotation, 0, newZRotation);
        transform.localEulerAngles = newRotation;
	}
}