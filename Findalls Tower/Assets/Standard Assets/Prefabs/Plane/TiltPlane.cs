using UnityEngine;
using System.Collections;
using MazeGraph;

public class TiltPlane : MonoBehaviour 
{
	public float maxAngle;
	public Transform WallPrefab;
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
			Tile tile = maze[pos];
			if (tile.GetType() == typeof(WallTile))
				GenerateOuterWall (MazeToPlaneCoords(pos), new Vector3(scale, scale, scale));
			//if (tile.GetType() == typeof(Tile))
				
		}		
		
		// generate outer walls
		GenerateOuterWall (new Vector3 (0, scale / 2, -5 - (scale / 2)), new Vector3 (10, scale, scale));   // South wall
		GenerateOuterWall (new Vector3 (0, scale / 2, 5 + (scale / 2)), new Vector3 (10, scale, scale));    // North wall
		GenerateOuterWall (new Vector3 (5 + (scale / 2), scale / 2, 0), new Vector3 (scale, scale, 10 + (scale * 2)));   // East Wall
		GenerateOuterWall (new Vector3 (-5 - (scale / 2), scale / 2, 0), new Vector3 (scale, scale, 10 + (scale * 2)));  // West Wall
	}
	
	private void GenerateOuterWall (Vector3 position, Vector3 scale)
	{
		Transform wall = (Transform)Instantiate (WallPrefab);
		wall.parent = this.transform;
		wall.localPosition = position;
		wall.localScale = scale;
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
		return new Point(0,0);
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
	}
}
