﻿using UnityEngine;
using System.Collections;
using MazeGraph;

public class TiltPlane : MonoBehaviour 
{
	public float maxAngle;
	public Transform WallPrefab;
	public Transform PathPrefab;
	public int mazeSize;
	public int additionalPaths;
	
	private Maze maze;
	private float scale;
	
	public Maze Maze {
		get { return maze; } 
	}
    
	void Start () 
	{
		maze = new Maze (mazeSize, additionalPaths);
		scale = 10f / mazeSize;

		foreach (Point pos in maze.AllPoints())
		{
            //continue;
			Tile tile = maze[pos];
			if (tile.GetType() == typeof(WallTile))
			{
				Transform thistile = GenerateTile (WallPrefab, MazeToPlaneCoords(pos), new Vector3(scale, scale, scale));
				thistile.gameObject.layer = LayerMask.NameToLayer ("WallsHid");
                thistile.gameObject.tag = "Wall";
			}
			if (tile.GetType() == typeof(Tile))
			{                
                Transform thistile = GenerateTile(PathPrefab, MazeToPlaneCoords(pos), new Vector3(scale, scale / 20f, scale));
				thistile.gameObject.layer = LayerMask.NameToLayer ("FloorVis");

			}
		}		
		
		// generate outer walls
		GenerateTile (WallPrefab, new Vector3 (0, scale / 2, -5 - (scale / 2)), new Vector3 (10, scale, scale));   // South wall
		GenerateTile (WallPrefab, new Vector3 (0, scale / 2, 5 + (scale / 2)), new Vector3 (10, scale, scale));    // North wall
		GenerateTile (WallPrefab, new Vector3 (5 + (scale / 2), scale / 2, 0), new Vector3 (scale, scale, 10 + (scale * 2)));   // East Wall
		GenerateTile (WallPrefab, new Vector3 (-5 - (scale / 2), scale / 2, 0), new Vector3 (scale, scale, 10 + (scale * 2)));  // West Wall
	}
	
	private Transform GenerateTile (Transform TileType, Vector3 position, Vector3 scale)
	{
        Transform tile = (Transform)Instantiate(TileType);
		tile.parent = this.transform;
		tile.localPosition = position;
		tile.localScale = scale;
		
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
		
		int mazeX = Mathf.CeilToInt( (planeCoords.x + offset) * scalefactor);
		int mazeY = Mathf.CeilToInt( (planeCoords.z + offset) * scalefactor);
		
		return new Point(mazeX, mazeY);
	}

    public Tile GetTileOfPoint(Point pos)
    {
        return maze[pos];
    }
	
	void Update () 
	{
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
		
		//Debug.Log (PlaneToMazeCoords(new Vector3(-3,0,4)));
	}
}
