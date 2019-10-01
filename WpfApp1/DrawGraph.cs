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
                panel.Children.Add(EllipseFab.GetEllipse(new Point(vertex.X, vertex.Y), brush, vertex.Name));
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
                        if ((edge.Orient) && (edge.To == dataKeys[j]))    // неориет ребро уже добавлено во время прохода по коллекции data[dataKeys[j]]
                        {
                            temp.Add(edge);
                        }
                    }

                    int itemp = 0;          // на первом шаге без отступа
                    for (int k = 0; k < temp.Count; k++)
                    {
                        if (k % 2 != 0)     // на нечетных - инкремент отступа
                            itemp += 20;

                        // k отвечает за сдвиг
                        panel.Children.Add(BezPathFab.GetPath(new Point(temp[k].From.X, temp[k].From.Y),
                                            new Point(temp[k].To.X, temp[k].To.Y), brush, Math.Pow(-1, k) * itemp, 
                                            temp[k].Name + " " + temp[k].Weight.ToString() + " " + temp[k].Orient.ToString(), temp[k].Orient));      // вместо name передаем весь tag

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
                        panel.Children.Add(BezPathFab.GetPath(new Point(edge.From.X, edge.From.Y), new Point((edge.To.X + edge.From.X)/2 - 50,
                                       (edge.To.Y + edge.To.Y)/2 + 50), new Point(edge.To.X+15, edge.To.Y+15), brush, 
                                       edge.Name + " " + edge.Weight.ToString() + " " + edge.Orient.ToString(), false));

                        break;                      //петля для данной вершины всегда одна
                    }
                }
            }
        }
    }
}
