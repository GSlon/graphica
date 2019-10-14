using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WpfApp1
{
    class DrawGraph
    {

        private static readonly double shift = 40;
        public static void Draw(Panel panel, Graph graph)
        {
            panel.Children.Clear();

            // идем по графу и рисуем
            var data = graph.GetLinks();
            var verts = graph.GetVertices();
            
            // рисуем вершины
            foreach (var vertex in verts)
            {
                panel.Children.Add(EllipseFab.GetEllipse(new Point(vertex.X, vertex.Y), vertex.brush, vertex.Name));
            }

            // рисуем ребра
            int i = 0;
            foreach (var key in data.Keys)
            {
                i = 0;
                foreach (var edge in data[key])
                {
                    panel.Children.Add(BezPathFab.GetPath(new Point(edge.From.X, edge.From.Y), new Point(edge.To.X, edge.To.Y), 
                                                edge.brush, Math.Pow(-1, i) * ((i + 1) / 2) * shift, edge.Name, edge.Orient, edge.Weight));
                    ++i;
                }
            }
        }
    }
}
