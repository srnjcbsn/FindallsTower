using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGraph
{
	public class Vertex
	{
		private Point position;
		private HashSet<Edge> edges;
		private Tile content;

		public Tile Content 
		{ 
			get { return content; } 
			set { content = value; }
		}

		public Point Position { get { return position; } }
		public IEnumerable<Edge> Edges { get { return edges; } }

		public IEnumerable<Vertex> Adjacent {
			get {
				foreach (Edge edge in edges)
					yield return edge.OtherVertex (this);
			}
		}

		public Vertex (Point position, Tile content)
		{
			this.position = position;
			this.content = content;
			edges = new HashSet<Edge> ();
		}

		public Vertex (Vertex vertex) : this (vertex.Position, vertex.Content)
		{
		}

		public void AddEdge (Edge edge)
		{
			edges.Add (edge);
		}

		public override string ToString ()
		{
			return string.Format ("{0} @ {1}]", Content, Position, Edges, Adjacent);
		}

		public Edge RandomEdge (Random rng)
		{
			return Edges.ElementAt (rng.Next (Edges.Count ()));
		}
	}

	public class VertexComparer : IEqualityComparer<Vertex>
	{
		#region IEqualityComparer implementation

		public bool Equals (Vertex v1, Vertex v2)
		{
			return (v1.Position == v2.Position);
		}

		public int GetHashCode (Vertex obj)
		{
			return obj.Position.GetHashCode ();
		}

		#endregion
	}
}

