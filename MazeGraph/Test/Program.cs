using System;
using MazeGraph;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
	public class Program
	{
		public static void Main ()
		{
			Maze maze = new Maze (35, 35);
			Graph pathGraph = maze.PathGraph;

			Vertex vertex = pathGraph.Vertices.First ();
			Tile t1 = pathGraph [vertex.Position].Content;
			Tile t2 = pathGraph [vertex.Adjacent.First ().Position].Content;
			Console.WriteLine (maze.AreTilesWithinRange (t1, t2, 1));
		}
	}
}

