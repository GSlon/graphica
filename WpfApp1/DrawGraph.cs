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
        Panel panel;
        Graph graph;
        Brush brush;

        public DrawGraph(Panel panl, Graph gr)
        {
            panel = panl;
            graph = gr;
            brush = Brushes.Red;
        }

        public DrawGraph(Panel panl, Graph gr, Brush br)
        {
            panel = panl;
            graph = gr;
            brush = br;
        }

        public void Clear()
        {
            panel.Children.Clear();
        }
        
        // рисуй все ключи (вершины), а затем только рисуй ребра, проходя по значениям
        public void Draw()
        {
            panel.Children.Clear(); 

            // идем по графу и рисуем
            var data = graph.GetGraph();
            Vertex[] dataKeys = data.Keys.ToArray();

            // рисуем вершины
            foreach (var vertex in dataKeys)
            {
                panel.Children.Add(EllipseFab.GetEllipse(new Point(vertex.X, vertex.Y), new Point(0, 0), brush, vertex.Name));
            }

            // рисуем ребра
            List<Edge> temp = new List<Edge>();
            for (int j = 0; j < dataKeys.Length - 1; j++)
            {
                for (int i = j + 1; i < dataKeys.Length; i++)      // к последнему нет смысла обращаться
                {
                    foreach (var edge in data[dataKeys[j]])
                    {   
                        if (edge.To == dataKeys[i])     //ReferenceEqual    // сравниваем попарно (точно знаем, что from это ключ)
                        {
                            temp.Add(edge);
                        }
                    }

                    // в обратную сторону
                    foreach (var edge in data[dataKeys[i]])
                    {
                        if (edge.To == dataKeys[j])    
                        {
                            temp.Add(edge);
                        }
                    }

                    Point middle = new Point();
                    for (int k = 0; k < temp.Count; k++)
                    {
                        // формируем middle точку
                        if (k == 0)
                        {
                            middle.X = (temp[k].To.X + temp[k].From.X)/2;
                            middle.Y = (temp[k].To.Y + temp[k].From.Y)/2;
                        }
                        else if (k % 2 != 0)    // нечетные
                        {
                            middle.X = (temp[k].To.X + temp[k].From.X)/2 - k * 15 - 10;
                            middle.Y = (temp[k].To.Y + temp[k].From.Y)/2;
                        }
                        else
                        {
                            middle.X = (temp[k].To.X + temp[k].From.X)/2 + (k - 1) * 15 + 10;
                            middle.Y = (temp[k].To.Y + temp[k].From.Y)/2;
                        }

                        panel.Children.Add(BezPathFab.GetPath(new Point(temp[k].From.X, temp[k].From.Y), middle,
                                            new Point(temp[k].To.X, temp[k].To.Y), brush, temp[k].Name, temp[k].Orient));
                    }

                    temp.Clear();
                }
            }

            //собираем петли                        // можно оптимизировать
            foreach (var key in data.Keys)
            {
                foreach (var edge in data[key])
                {
                    if (edge.From == edge.To)
                    {
                        panel.Children.Add(BezPathFab.GetPath(new Point(edge.From.X-15, edge.From.Y-15), new Point(edge.To.X - 70,
                                       edge.To.Y + 70), new Point(edge.To.X+15, edge.To.Y+15), brush, edge.Name, false));

                        break;                      //петля для данной вершины всегда одна
                    }
                }
            }
        }
    }
}
