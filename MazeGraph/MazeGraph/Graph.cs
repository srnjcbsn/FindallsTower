using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeGraph
{
	public class Graph
	{
		Dictionary<Point, Vertex> vertices;

		public Vertex this [int x, int y] {
			get { return this [new Point (x, y)]; }
		}

		public Vertex this[Point pos] {
			get { return vertices [pos]; }
		}

		public IEnumerable<Vertex> Vertices { 
			get { return vertices.Values; } 
		}

		public Graph ()
		{
			vertices = new Dictionary<Point, Vertex> ();
		}

		public Graph (IEnumerable<Vertex> vertices) : this ()
		{
			foreach (Vertex vertex in vertices)
				this.vertices.Add (vertex.Position, new Vertex(vertex));

			BuildConnections ();
		}

		public Graph (int dimension, Func<Point, Tile> tileFun)
		{
			vertices = new Dictionary<Point, Vertex> ();


			for (int x = 0; x < dimension; x++) 
			{
				for (int y = 0; y < dimension; y++) 
				{
					Point thisPos = new Point (x, y);
					Vertex vertex = new Vertex (thisPos, tileFun (thisPos));
					vertices [thisPos] = vertex;
				}
			}

			BuildConnections ();
		}

		public void BuildConnections ()
		{
			foreach (KeyValuePair<Point, Vertex> kv in vertices) 
			{
				Point currPoint = kv.Key;
				Vertex currVertex = kv.Value;

				Point pointAbove = new Point (currPoint.X - 1, currPoint.Y);
				Point pointLeft = new Point (currPoint.X, currPoint.Y - 1);

				if (vertices.ContainsKey (pointAbove))
					AddConnection (currVertex, vertices [pointAbove]);

				if (vertices.ContainsKey (pointLeft))
					AddConnection (currVertex, vertices [pointLeft]);
			}
		}

		private void AddConnection (Vertex v1, Vertex v2)
		{
			Edge edge = new Edge (v1, v2);
			v1.AddEdge (edge);
			v2.AddEdge (edge);
		}

		public void AddVertex (Point position, Vertex vertex)
		{
			vertices [position] = vertex;
		}

//		public Graph SubGraph (Predicate<Vertex> ignoreVertex)
//		{
//			//TODO: Throw exception if more than one subgraph?
//			return SubGraphs (ignoreVertex).FirstOrDefault ();
//		}

//		public IEnumerable<Graph> SubGraphs (Predicate<Vertex> ignoreVertex)
//		{
//			HashSet<Vertex> explored = new HashSet<Vertex> (new VertexComparer ());
//
//			foreach (Vertex vertex in vertices.Values)
//			{ 
//				if (explored.Contains (vertex) || vertex.Content.GetType () == typeof(WallTile))
//					continue;
//
//				Graph subGraph = SubGraph (vertex, ignoreVertex);
//
//				explored.UnionWith (subGraph.Vertices);
//
//				yield return subGraph;
//			}
//		}

//		public Graph SubGraph (Vertex fromVertex, Predicate<Vertex> ignoreVertex)
//		{
//			HashSet<Vertex> verts = new HashSet<Vertex> ();
//
//			DFS (fromVertex, verts, ignoreVertex);
//
//			return new Graph (verts);
//		}

		public List<Edge> RandomDFS (Vertex vertex, HashSet<Vertex> explored, Random rng)
		{
			return RandomDFS (vertex, explored, (_ => false), rng);
		}

		public List<Edge> RandomDFS (Vertex vertex, HashSet<Vertex> explored, Predicate<Vertex> ignoreVertex, Random rng)
		{
			explored.Add (vertex);
//			HashSet<Edge> edgesUsed = new HashSet<Edge> (new EdgeComparer ());
			List<Edge> edgesUsed = new List<Edge> ();
			HashSet<Edge> edgesLeft = new HashSet<Edge> (vertex.Edges);

			while (edgesLeft.Any ())
			{
				Edge edge = vertex.RandomEdge (rng);
				edgesLeft.Remove (edge);

				Vertex adjVertex = edge.OtherVertex (vertex);

				if (!ignoreVertex (adjVertex) && !explored.Contains (adjVertex))
				{
					edgesUsed.Add (edge);
//					edgesUsed.UnionWith (DFS (adjVertex, explored, ignoreVertex, rng));
					edgesUsed.AddRange (RandomDFS (adjVertex, explored, ignoreVertex, rng));
				}
			}

			return edgesUsed;
		}
	}
}

