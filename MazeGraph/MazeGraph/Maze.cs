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
			GenerateRandomEntryExitPoints ();

			Stopwatch stopwatch = new Stopwatch ();
			stopwatch.Start ();
			Graph simpleGraph = new Graph ((dimension / 2) + 1, (_ => new Tile ()));
			stopwatch.Stop ();

			Console.WriteLine ("simplegraph generation took {0}", stopwatch.Elapsed);

			stopwatch.Reset ();
			stopwatch.Start ();
			HashSet<Edge> pathEdges = new HashSet<Edge> (simpleGraph.RandomDFS (simpleGraph [new Point (0, 0)], new HashSet<Vertex> (), rng));
			stopwatch.Stop ();
			Console.WriteLine ("DFS took {0}", stopwatch.Elapsed);

			int edgesLeftToAdd = additionalPaths;

			stopwatch.Reset ();
			stopwatch.Start ();
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

			stopwatch.Stop ();
			Console.WriteLine ("Removing edges took {0}", stopwatch.Elapsed);
			
			mazeGraph = new Graph (dimension, (pos => ConstructMaze(simpleGraph, pathEdges, pos)));
		}

		private Tile ConstructMaze (Graph simpleGraph, HashSet<Edge> pathEdges, Point pos)
		{
			if (pos.X % 2 == 0 && pos.Y % 2 == 0)
				return new Tile ();

			if (pos.X % 2 == 1 && pos.Y % 2 == 1)
				return new WallTile ();

			Vertex vert1 = simpleGraph [(int)Math.Floor (pos.X / 2.0), (int)Math.Floor (pos.Y / 2.0)];
			Vertex vert2 = simpleGraph [(int)Math.Ceiling (pos.X / 2.0), (int)Math.Ceiling (pos.Y / 2.0)];

			if (pathEdges.Contains (new Edge (vert1, vert2)))
				return new Tile ();
			else
				return new WallTile ();

		}

		private void GenerateRandomEntryExitPoints ()
		{
			Random rnd = new Random ();

			entryPoint = RandomPointUpperLeftQuarter (rnd);
			exitPoint = RandomPointUpperLeftQuarter (rnd);

			if (rnd.Next (0, 1) == 1)
				entryPoint = new Point (size - entryPoint.X - 1, entryPoint.Y);
			else
				exitPoint = new Point (size - exitPoint.X - 1, exitPoint.Y);

			if (rnd.Next (0, 1) == 1)
				entryPoint = new Point (entryPoint.X, size - entryPoint.Y - 1);
			else
				exitPoint = new Point (exitPoint.X, size - exitPoint.Y - 1);
		}

		private Tile RandomTile(Random rnd, Point position)
		{
			if (position == entryPoint || position == exitPoint)
				return new Tile ();

			if (RandomBool (rnd))
				return new Tile ();
			else
				return new WallTile ();
		}

		private bool RandomBool (Random rnd)
		{
			return (rnd.Next (0, 2) == 1);
		}

		private Point RandomPointUpperLeftQuarter (Random rnd)
		{
			int x = rnd.Next (0, size / 4);
			int y = rnd.Next (0, size / 4);
			return new Point (x, y);
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

