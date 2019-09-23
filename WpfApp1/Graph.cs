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

        public Edge(string name, int x = 0, int y = 0)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }

    struct Vertex
    {
        public int Num, Weight, Orient;     

        public Vertex(int num, int weight = 1, int orient = 0)      // 0 - не направленно; вес по умолчанию = 1
        {
            Num = num;
            Weight = weight;
            Orient = orient;
        }
    }

    class Graph
    {

        // словарь {Edge: <Edge, Vertex>, <...>}    // сразу лежит связь между вершинами и ребрами


    }
}
