using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    struct Edge
    {
        public string Name;
        public int X, Y;
    }

    struct Vertex
    {
        public int Weight;
    }

    class Graph
    {
        // словарь {Edge: <Edge, Vertex>, <...>}    // сразу лежит связь между вершинами и ребрами

    }
}
