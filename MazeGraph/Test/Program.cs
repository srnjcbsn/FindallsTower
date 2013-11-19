using System;
using MazeGraph;
using System.Collections.Generic;

namespace Test
{
	public class Program
	{
		public static void Main ()
		{
			Maze maze = new Maze (255, 125);

			Vertex testVertex = new Vertex (new Point (0, 0), new Tile ());
			HashSet<Vertex> testSet = new HashSet<Vertex> (new VertexComparer ());
			testSet.Add (testVertex);
			testSet.Add (new Vertex (testVertex));

			Random rnd = new Random ();
			Console.WriteLine (rnd.Next (0, 1));
		}
	}
}

