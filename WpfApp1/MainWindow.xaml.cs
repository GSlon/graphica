using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //DrawGraph drawGraph;
        //Graph graph;
        bool activ = false;

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((cursor.IsChecked == true))
            {
                //List<Vertex> vertices = new List<Vertex> { new Vertex("1", 200, 100), new Vertex("22", 500, 100), new Vertex("33", 100, 450), new Vertex("456744", 450, 350) };
                //List<Edge> edges = new List<Edge> { new Edge("d", vertices[0], vertices[1], true), new Edge("d", vertices[0], vertices[1], true), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0],
                //    vertices[1]),new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[1], vertices[2]),
                //    new Edge("d", vertices[1], vertices[2]), new Edge("d", vertices[1], vertices[2], true), new Edge("d", vertices[1], vertices[2], true),new Edge("d", vertices[2], vertices[2], true),new Edge("d", vertices[2], vertices[1]) };

                //List<Edge> edges = new List<Edge> { new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]) };

                //Graph graph = new Graph(vertices, edges);
                ////graph.DelVerbyName("школа");
                //DrawGraph drgr = new DrawGraph(field, graph);
                //drgr.Draw();

                Vertex vertex = new Vertex("ee", 100d, 100d);
                Vertex vertex2 = new Vertex("lol", 400d, 600d);
                List<Vertex> vertices2 = new List<Vertex> { vertex, vertex2 };
                List<Edge> edges2 = new List<Edge> { new Edge("d", vertex, vertex), new Edge("d", vertex2, vertex2), new Edge("t", vertex, vertex2), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), 
                    new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true) };
                //    new Edge("t", vertex, vertex2, true), new Edge("t", vertex2, vertex, true), new Edge("t", vertex2, vertex, true),  new Edge("t", vertex, vertex2, true),
                //    new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true)
                //, new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true),
                // new Edge("t", vertex, vertex2, true),  new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true),
                //new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true)};
                 //new Edge("t", vertex2, vertex, true), new Edge("t", vertex2, vertex, true), new Edge("t", vertex2, vertex, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex, vertex2, true), new Edge("t", vertex2, vertex, true),
                 //new Edge("t", vertex2, vertex, true), new Edge("t", vertex2, vertex, true), new Edge("t", vertex2, vertex, true)};

                DrawGraph dr = new DrawGraph(field, new Graph(vertices2, edges2));
                dr.Draw();
                this.Update_Canvas();

            }
            else if (vertex.IsChecked == true)
            {
                Ellipse el = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X, e.GetPosition(field).Y), Brushes.Red, "uu");

                el.MouseDown += new MouseButtonEventHandler(Ellipse_MouseDown);

                // el.MouseMove += new MouseEventHandler(mouse_Move);
                // el.MouseUp += new MouseButtonEventHandler(mouse_Up);

                field.Children.Add(el);
                var path = BezPathFab.GetPath(new Point(e.GetPosition(field).X, e.GetPosition(field).Y), new Point((e.GetPosition(field).X + 50) / 2 + 20,
                                        (e.GetPosition(field).Y + 100) / 2 - 50), new Point(e.GetPosition(field).X - 50, e.GetPosition(field).Y - 75), Brushes.Red, "", true);

                path.MouseDown += new MouseButtonEventHandler(Path_MouseDown);
                field.Children.Add(path);



                //Path.
                //field.Children.
                //new Grid().Children.Add();
            }

            else if (hand.IsChecked == true)
            {

            }

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cursor.IsChecked == true)
            {
                MessageBox.Show("Сделать петлю");

            }
            else if (vertex.IsChecked == true)
            {
                MessageBox.Show("здесь уже есть вершина");

            }
            else if (edge.IsChecked == true)
            {

            }
            else if (hand.IsChecked == true)
            {
                activ = true;
            }
            else if (delete.IsChecked == true)
            {
                var el = (Ellipse)sender;
                field.Children.Remove(el);
            }
        }

        private void Ellipse_MouseMove(object elps, MouseEventArgs e)
        {
            if (activ)
            {
                foreach (Ellipse child in field.Children)
                {
                    EllipseFab.ChangeEllipse(child, Brushes.Gainsboro, child.Name);
                    EllipseFab.ChangeElpsCoord(child, new Point(e.GetPosition(field).X, e.GetPosition(field).Y));
                    
                    break;
                }
            }
                
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cursor.IsChecked == true)
            {
                activ = false;
            }
        }

        // новым фигурам заполняем делегаты 
        private void Update_Canvas()
        {
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (field.Children[i].GetType().Name.ToString().Equals("Ellipse"))
                {
                    field.Children[i].MouseDown += Ellipse_MouseDown;
                    field.Children[i].MouseMove += Ellipse_MouseMove;
                    field.Children[i].MouseUp += Ellipse_MouseDown;
                }
                else if (field.Children[i].GetType().Name.ToString().Equals("Path"))
                {
                    field.Children[i].MouseDown += Path_MouseDown;
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }

        private void Field_Loaded(object sender, RoutedEventArgs e)
        {
           
        }


        /*
        protected void mouse_Move(object sender, MouseEventArgs e)
        {
            if (ui == null)
                return;

            ui.SetValue(Canvas.LeftProperty, e.GetPosition(this).X - p.Value.X - 70);
            ui.SetValue(Canvas.TopProperty, e.GetPosition(this).Y - p.Value.Y - 70);
        }

        protected void mouse_Up(object sender, MouseButtonEventArgs e)
        {
            ui = null;
        }
        */
        /*
           private void ell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
           {
               ui = sender as UIElement;
               p = e.GetPosition(ui);

           }

           private void ell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
           {
               ui = null;
           }

           private void ell_MouseMove(object sender, MouseEventArgs e)
           {
               if (ui == null)
                   return;

               ui.SetValue(Canvas.LeftProperty, e.GetPosition(this).X - p.Value.X);
               ui.SetValue(Canvas.TopProperty, e.GetPosition(this).Y - p.Value.Y);
           }
           */

    }
}
