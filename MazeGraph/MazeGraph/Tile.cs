using System;
using System.Collections.Generic;

namespace MazeGraph
{
	public class Tile
	{
		public Point Position { get; set; }

		protected List<Artifact> artifacts;
		public IEnumerable<Artifact> Artifacts { 
			get { return artifacts; } 
		}

		public Tile (Point position)
		{
			Position = position;
			artifacts = new List<Artifact> ();
		}

		public void AddArtifact (Artifact artifact)
		{
			artifacts.Add (artifact);
		}
	}
}

