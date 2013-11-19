using System;
using System.Collections.Generic;

namespace MazeGraph
{
	public class Edge
	{
		public Vertex Vertex1;
		public Vertex Vertex2;

		public Edge (Vertex vertex1, Vertex vertex2)
		{
			this.Vertex1 = vertex1;
			this.Vertex2 = vertex2;
		}

		public Vertex OtherVertex(Vertex vertex)
		{
			if (vertex == Vertex1)
				return Vertex2;

			if (vertex == Vertex2)
				return Vertex1;

			return null;
		}

		public override string ToString ()
		{
			return string.Format ("{{{0} - {1}}}", Vertex1, Vertex2);
		}
	}

	public class EdgeComparer : IEqualityComparer<Edge>
	{
		#region IEqualityComparer implementation

		public bool Equals (Edge x, Edge y)
		{
			return ((x.Vertex1 == y.Vertex1 && x.Vertex2 == y.Vertex2) || (x.Vertex1 == y.Vertex2 && x.Vertex2 == y.Vertex1));
		}

		public int GetHashCode (Edge obj)
		{
			return (obj.Vertex1.Position + obj.Vertex2.Position).GetHashCode ();
		}

		#endregion
	}
}

