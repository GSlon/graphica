using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Vertex: System.Object
    {
        public string Name { get; set; }
        public double X { get; set; } 
        public double Y { get; set; }

        public Vertex(string name, double x = 0, double y = 0) : base()
        {
            Name = name;
            X = x;
            Y = y;
        }
    }

    class Edge: Object
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public bool Orient { get; set; }
        public Vertex From { get; set; }
        public Vertex To { get; set; }

        public Edge(string name, Vertex from, Vertex to, int weight = 1, bool orient = false) : base()     // false - не направлено; вес по умолчанию = 1
        {
            Name = name;
            Weight = weight;
            Orient = orient;
            From = from;
            To = to;
        }
    }

    class Graph
    {
        private Dictionary<Vertex, LinkedList<Edge>> data;      // значение по ключу - только те ребра, началом которых является данный Vertex-ключ

        public Graph(Vertex vertex)
        {
            data = new Dictionary<Vertex, LinkedList<Edge>>();
            data.Add(vertex, new LinkedList<Edge>());
        }

        public Graph(Vertex vertex, Edge edge)
        {
            if (edge.From != edge.To)
            {
                throw new FormatException("Edge need 2 Vertex");
            }

            data = new Dictionary<Vertex, LinkedList<Edge>>();
            LinkedList<Edge> list = new LinkedList<Edge>();
            list.AddLast(edge);
            data.Add(vertex, list);
        }

        public Graph(IEnumerable<Vertex> verts)
        {
            data = new Dictionary<Vertex, LinkedList<Edge>>();
            foreach (var item in verts)
            {
                data.Add(item, new LinkedList<Edge>());
            }
        }

        public Graph(IEnumerable<Vertex> verts, IEnumerable<Edge> edges)
        {
            data = new Dictionary<Vertex, LinkedList<Edge>>();
            
            foreach (var vertex in verts)
            {
                data.Add(vertex, new LinkedList<Edge>());
                foreach (var edge in edges)
                {
                    if (vertex == edge.From)
                    {
                        data[vertex].AddLast(edge);
                    }
                }
            }
        }
        
        //Add Vertex
        //Add Edge

        public Dictionary<Vertex, LinkedList<Edge>> GetGraph()
        {
            return data;
        }

    }
}
