using System;
using MazeGraph;
using System.Collections.Generic;

namespace Test
{
	public class Program
	{
		public static void Main ()
		{
			Maze maze = new Maze (35, 35);

			Random rnd = new Random ();
			Console.WriteLine (rnd.Next (0, 1));
		}
	}
}

