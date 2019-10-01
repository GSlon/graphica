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

        public Edge(string name, Vertex from, Vertex to, bool orient = false, int weight = 1) : base()     // false - не направлено; вес по умолчанию = 1
        {
            Name = name;
            Orient = orient;
            Weight = weight;
            From = from;
            To = to;
        }
    }

    class Graph
    {
        private Dictionary<Vertex, LinkedList<Edge>> data;      // значение по ключу - только те ребра, началом которых является данный Vertex-ключ

        public Graph()
        {
            data = new Dictionary<Vertex, LinkedList<Edge>>(); 
        }

        public Graph(Vertex vertex)
        {
            data = new Dictionary<Vertex, LinkedList<Edge>>();
            data.Add(vertex, new LinkedList<Edge>());
        }

        // петля
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
                    if ((vertex == edge.From) && (verts.Contains(edge.To)))    // ключ: вершина; значение: все ребра, из этой вершины стартующие
                    {
                        data[vertex].AddLast(edge);
                    }
                }
            }
        }

        public Graph(Graph graph)
        {
            data = graph.GetGraph();
        }

        public void AddVertex(Vertex ver)
        {
            data.Add(ver, new LinkedList<Edge>());
        }

        public void AddEdge(Edge edge)
        {
            if (data.ContainsKey(edge.From) && data.ContainsKey(edge.To))
            {
                data[edge.From].AddLast(edge);
            }
        }

        public void SetGraph(Graph graph)
        {
            data = graph.GetGraph();
        }

        public void DelVerbyName(string name)
        {
            var keys = data.Keys;
            List<Edge> temp = new List<Edge>();
            foreach (var key in data.Keys)
            {
                if (key.Name == name)
                {
                    foreach (var values in data.Values)        // удаляем те ребра, которые входят в нашу вершину
                    {
                        foreach (var edge in values)
                        {
                            if (edge.To == edge.From)          // циклы и так удалятся
                                continue;
                            
                            if (edge.To == key)
                            {
                                temp.Add(edge);
                            }
                        }
                    }

                    data.Remove(key);

                    for (int i = 0; i < temp.Count; i++)
                        data[temp[i].From].Remove(temp[i]);

                    break;      // будем думать, что ключи не дублируются
                }
            }
        }

        public void DelEdgebyName(string name)
        {
            List<Edge> temp = new List<Edge>(); 
            foreach (var values in data.Values)
            {
                foreach (var edge in values)
                {
                    if (edge.Name == name)
                    {
                        temp.Add(edge);
                    }
                }
            }

            for (int i = 0; i < temp.Count; i++)
            {
                data[temp[i].From].Remove(temp[i]);
            }
        }

        public Dictionary<Vertex, LinkedList<Edge>> GetGraph()
        {
            return data;
        }

    }
}
