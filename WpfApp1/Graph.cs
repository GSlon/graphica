using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
//using Newtonsoft.Json;
using System.ComponentModel;

namespace WpfApp1
{
    class Vertex: System.Object
    {
        public string Name { get; set; }
        public double X { get; set; } 
        public double Y { get; set; }

        public Brush brush { get; set; }

        public Vertex(): base()
        {
            Name = "";
            X = 0;
            Y = 0;
            brush = Brushes.Black;
        }

        public Vertex(string name, double x = 0, double y = 0) : base()
        {
            Name = name;
            X = x;
            Y = y;
            brush = Brushes.Red;
        }

        public Vertex(string name, Brush br ,double x = 0, double y = 0) : base()
        {
            Name = name;
            X = x;
            Y = y;
            brush = br;
        }
    }

    class Edge: Object
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public bool Orient { get; set; }
        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public Brush brush { get; set; }

        public Edge(): base()
        {
            Name = "";
            Weight = 1;
            Orient = false;
            From = null;
            To = null;
            brush = Brushes.Black;
        }

        public Edge(string name, Vertex from, Vertex to, bool orient = false, int weight = 1) : base()     // false - не направлено; вес по умолчанию = 1
        {
            Name = name;
            Orient = orient;
            Weight = weight;
            From = from;
            To = to;
            brush = Brushes.Black;
        }

        public Edge(string name, Vertex from, Vertex to, Brush br, bool orient = false, int weight = 1) : base()     // false - не направлено; вес по умолчанию = 1
        {
            Name = name;
            Orient = orient;
            Weight = weight;
            From = from;
            To = to;
            brush = br;
        }
    }
    
    internal class PairEquality : IEqualityComparer<Pair>
    {
        public bool Equals(Pair firstPair, Pair secondPair)
        {
            return ( ((firstPair.first == secondPair.first) && (firstPair.second == secondPair.second)) || ((firstPair.second == secondPair.first) && (firstPair.first == secondPair.second)) );
        }

        public int GetHashCode(Pair pair)
        {
            int hCode = pair.GetHashCode() ^ pair.GetHashCode();
            return hCode.GetHashCode();
        }
    }
   
    class Pair: System.Object
    {
        public Vertex first;
        public Vertex second;

        public Pair(Vertex frst, Vertex scnd): base()
        {
            first = frst;
            second = scnd;
        }

        public Pair(Vertex vert): base()
        {
            first = second = vert;
        }

        public bool Contains(Vertex vertex)
        {
            if ((first == vertex) || (second == vertex))
                return true;

            return false;
        }
    }


    class Graph : ICloneable
    {
        private Dictionary<string, Edge> edges;                 // для 'записи' (просто удобный поиск, пользователь класса получит только data/список list)
        private Dictionary<Pair, LinkedList<Edge>> links;      // значение по ключу — те ребра, которые соединяют два Vertexа в любую сторону (нет данных, если нет ребер)   // для 'чтения'
        private Dictionary<string, Vertex> verts;               // список vertex понадобится пользователю
        
        public Graph()
        {
            links = new Dictionary<Pair, LinkedList<Edge>>(new PairEquality());
            verts = new Dictionary<string, Vertex>();
            edges = new Dictionary<string, Edge>();
        }

        public Graph(Vertex vertex)
        {
            verts = new Dictionary<string, Vertex> 
            {
                { vertex.Name, vertex }
            };
            
            links = new Dictionary<Pair, LinkedList<Edge>>(new PairEquality());
            edges = new Dictionary<string, Edge>();
        }

        // петля (всего одна вершина указана)
        public Graph(Vertex vertex, Edge edge)
        {
            if ((edge.From != edge.To) && (edge.From != vertex))
            {
                throw new FormatException("Edge need 2 Vertex or loop");
            }

            LinkedList<Edge> list = new LinkedList<Edge>();
            list.AddLast(edge);

            links = new Dictionary<Pair, LinkedList<Edge>>(new PairEquality())
            {
                { new Pair(vertex), list }
            };

            verts = new Dictionary<string, Vertex>
            {
                { vertex.Name , vertex}
            };

            edges = new Dictionary<string, Edge>
            {
                { edge.Name , edge}
            };
        }

        // проверка на повтор имен (по ключам словарей)
        public Graph(IEnumerable<Vertex> vers)
        {
            verts = new Dictionary<string, Vertex>();
            
            foreach (var item in vers)
            {
               verts.Add(item.Name, item);
            }
            
            links = new Dictionary<Pair, LinkedList<Edge>>(new PairEquality());
            edges = new Dictionary<string, Edge>();
        }

        public Graph(IEnumerable<Vertex> vers, IEnumerable<Edge> edgs)
        {
            links = new Dictionary<Pair, LinkedList<Edge>>(new PairEquality());
            verts = new Dictionary<string, Vertex>();
            edges = new Dictionary<string, Edge>();

            foreach (var vertex in vers)
            {
                verts.Add(vertex.Name, vertex);
            }

            foreach (var item in edgs)
            {
                if ((vers.Contains(item.From)) && (vers.Contains(item.To)))
                {
                    edges.Add(item.Name, item);

                    var pair = new Pair(item.From, item.To);
                    if (links.ContainsKey(pair))
                    {
                        links[pair].AddLast(item);
                    }
                    else
                    {
                        var list = new LinkedList<Edge>();
                        list.AddLast(item);
                        links.Add(pair, list);
                    }
                }
                else
                    throw new FormatException("Edge need 2 Vertex or loop");
            }
        }

        public void AddVertex(Vertex vertex)
        {
            verts.Add(vertex.Name, vertex);
        }

        public void AddEdge(Edge edge)
        {
            if (verts.ContainsValue(edge.From) && verts.ContainsValue(edge.To))
            {
                if (edge.To == edge.From)
                    edge.Orient = false;

                edges.Add(edge.Name, edge);
                
                var pair = new Pair(edge.From, edge.To);
                if (links.ContainsKey(pair))
                    links[pair].AddLast(edge);
                else
                {
                    var list = new LinkedList<Edge>();
                    list.AddLast(edge);
                    links.Add(pair, list);
                }
            }
            else
            {
                throw new FormatException("Edge refer to vertex that doesnt exist");
            }
        }

        public void AddEdge(string name, string from, string to, Brush brush, bool orient, int weight)
        {
            if (edges.ContainsKey(name) || !verts.ContainsKey(from) || !verts.ContainsKey(to))
                throw new Exception("wrong data");

            if (from == to) // loop
                orient = false;

            Edge newEdge = new Edge(name, verts[from], verts[to], brush, orient, weight);
            edges.Add(name, newEdge);

            var pair = new Pair(verts[from], verts[to]);
            if (links.ContainsKey(pair))
                links[pair].AddLast(newEdge);
            else
            {
                var list = new LinkedList<Edge>();
                list.AddLast(newEdge);
                links.Add(pair, list);
            }
        }
        
        public void DeleteVertex(string name)
        {
            if (!verts.ContainsKey(name))
                throw new FormatException("Vertes '" + name + "' doesnt exist");

            Vertex delEdge = verts[name];
            List<Pair> temp = new List<Pair>();
            var keys = links.Keys;
            foreach (var key in keys)
            {
                if (key.Contains(delEdge))
                    temp.Add(key);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                foreach (var edge in links[temp[i]])    // удаляем вершины
                {
                    edges.Remove(edge.Name);
                }

                links.Remove(temp[i]);
            }

            verts.Remove(name);
        }

        public void DeleteEdge(string name)
        {
            if (!edges.ContainsKey(name))
                throw new Exception(name + " doesnt exists");

            Edge delEdge = edges[name];

            if ((!verts.ContainsValue(delEdge.From)) || (!verts.ContainsValue(delEdge.To)))
                throw new FormatException("Edge refer to vertex that doesnt exist");

            var pair = new Pair(delEdge.From, delEdge.To);
            
            links[pair].Remove(delEdge);
            edges.Remove(name);

            // удаляем пары с пустыми листами
            var temp = new List<Pair>();
            foreach (var key in links.Keys)
            {
                if (links[key].Count == 0)
                    temp.Add(key);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                links.Remove(temp[i]);
            }
        }

        public void ChangeVertex(string oldName, string newName, double X, double Y, Brush brush)
        {
            if (verts.ContainsKey(oldName))
            {
                var temp = verts[oldName];
                temp.Name = newName;
                temp.X = X;
                temp.Y = Y;
                temp.brush = brush;

                verts.Remove(oldName);
                verts.Add(newName, temp);
            }
            else
                throw new Exception("The name doesnt exist");
        }

        // имя не меняем у ребер
        public void ChangeEdge(string name, bool orient, int weight, Brush brush)
        {
            if (edges.ContainsKey(name))
            {
                var temp = edges[name];
                temp.Name = name;
                temp.Orient = orient;
                temp.Weight = weight;
                temp.brush = brush;
            }
            else
                throw new Exception("The name doesnt exist");

        }

        public Dictionary<Pair, LinkedList<Edge>> GetLinks()
        {
            return links;     
        }

        public List<Vertex> GetVertices()
        {
            return verts.Values.ToList<Vertex>();   
        }

        public List<Edge> GetEdges()
        {
            return edges.Values.ToList<Edge>();
        }

        public object Clone()
        {
            List<Vertex> tempVerts = new List<Vertex>();
            List<Edge> tempEdges = new List<Edge>();

            Dictionary<Vertex, Vertex> OldToNew = new Dictionary<Vertex, Vertex>();     // удобно перекинуть ссылки со старых к новым вершинам в ребрах

            foreach (var ver in GetVertices())
            {
                Vertex temp = new Vertex(ver.Name, ver.brush, ver.X, ver.Y);
                tempVerts.Add(temp);
                OldToNew.Add(ver, temp);
            }

            foreach (var edge in edges.Values)
            {
                Edge temp = new Edge(edge.Name, OldToNew[edge.From], OldToNew[edge.To], edge.brush, edge.Orient, edge.Weight);
                tempEdges.Add(temp);
            }

            return new Graph(tempVerts, tempEdges);
        }

        public void Clear()
        {
            edges.Clear();
            verts.Clear();
            links.Clear();
        }
    }
}
