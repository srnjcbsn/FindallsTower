using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace MazeGraph
{
	public class Maze
	{
		private Point entryPoint;
		private Point exitPoint;
		private Graph mazeGraph;
		private Graph pathGraph;
		private readonly Random rng;
		private readonly int size;

		public int Dimension 
		{
			get { return size; }
		}

		public Tile this[Point point]
		{
			get { return mazeGraph [point].Content; }
		}

		public Graph MazeGraph 
		{
			get { return mazeGraph; }
		}

		public Graph PathGraph
		{
			get { return pathGraph; }
		}

		public Maze (int dimension, int additionalPaths)
		{
			this.size = dimension;
			rng = new Random ();
			entryPoint = RandomPointInPath ();
			exitPoint = RandomPointInPath ();

			while (entryPoint == exitPoint)
				exitPoint = RandomPointInPath ();

			Graph simpleGraph = new Graph ((dimension / 2) + 1, (pos => new Tile (pos)));

			HashSet<Edge> pathEdges = new HashSet<Edge> (simpleGraph.RandomDFS (simpleGraph [new Point (0, 0)], new HashSet<Vertex> (), rng), new EdgeComparer ());

			Console.WriteLine (pathEdges.Count);

			int edgesLeftToAdd = additionalPaths;
			while (edgesLeftToAdd > 0)
			{
				Vertex randomVertex = simpleGraph [rng.Next ((dimension / 2) - 1), rng.Next ((dimension / 2) - 1)];
				Edge randomEdge = randomVertex.Edges.ElementAt (rng.Next (randomVertex.Edges.Count ()));
				if (!pathEdges.Contains (randomEdge))
				{
					pathEdges.Add (randomEdge);
					edgesLeftToAdd--;
				}
			}
			
			mazeGraph = new Graph (dimension, (pos => ConstructMaze(simpleGraph, pathEdges, pos)));
			pathGraph = MazeGraph.SubGraph (mazeGraph[entryPoint], (vertex => vertex.Content.GetType () == typeof(WallTile)));
		}

		public Tile RandomAdjacentPath (Tile tile, Random rng)
		{
			Vertex vertex = pathGraph [tile.Position];
			return vertex.RandomEdge (rng).OtherVertex (vertex).Content;
		}

		public bool AreTilesWithinRange (Tile t1, Tile t2, int range)
		{
			List<Edge> path = pathGraph.DFS (pathGraph [t1.Position], new HashSet<Vertex> (), pathGraph [t2.Position], range);

			if (path.Any ())
				return true;
			else
				return false;
		}

		private Tile ConstructMaze (Graph simpleGraph, HashSet<Edge> pathEdges, Point pos)
		{
			if (pos.X % 2 == 0 && pos.Y % 2 == 0)
			{
				if (pos == entryPoint)
					return new EntryTile (pos);
				if (pos == exitPoint)
					return new ExitTile (pos);

				return new Tile (pos);
			}

			if (pos.X % 2 == 1 && pos.Y % 2 == 1)
				return new WallTile (pos);

			Vertex vert1 = simpleGraph [(int)Math.Floor (pos.X / 2.0), (int)Math.Floor (pos.Y / 2.0)];
			Vertex vert2 = simpleGraph [(int)Math.Ceiling (pos.X / 2.0), (int)Math.Ceiling (pos.Y / 2.0)];

			if (pathEdges.Contains (new Edge (vert1, vert2)))
				return new Tile (pos);
			else
				return new WallTile (pos);
		}

		public Point RandomPointInPath ()
		{
			return new Point (rng.Next (0, size / 2) * 2, rng.Next (0, size / 2) * 2);
		}

		private bool RandomBool (Random rnd)
		{
			return (rnd.Next (0, 2) == 1);
		}

		public IEnumerable<Point> AllPoints ()
		{
			foreach (Point p in AllPointsInSquare(size))
				yield return p;
		}

		private IEnumerable<Point> AllPointsInSquare (int dimensions)
		{
			foreach (Point p in AllPointsInRange (new Point (0, 0), new Point (dimensions, dimensions)))
				yield return p;
		}

		private IEnumerable<Point> AllPointsInRange (Point start, Point end)
		{
			int startX = Math.Min (start.X, end.X);
			int startY = Math.Min (start.Y, end.Y);
			int endX = Math.Max (start.X, end.X);
			int endY = Math.Max (start.Y, end.Y);

			for (int x = startX; x < endX; x++)
			{
				for (int y = startY; y < endY; y++)
				{
					yield return new Point (x, y);
				}
			}
		}
	}
}

