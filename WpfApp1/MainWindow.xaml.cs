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
        
        public MainWindow()
        {
            InitializeComponent();

       
        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((cursor.IsChecked == true))
            {
                List<Vertex> vertices = new List<Vertex> { new Vertex("школа", 200, 100), new Vertex("дом", 450, 300), new Vertex("магаз", 100, 450) };
                List<Edge> edges = new List<Edge> { new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0],
                    vertices[1]),new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[1], vertices[2]),
                    new Edge("d", vertices[1], vertices[2]), new Edge("d", vertices[1], vertices[2]), new Edge("d", vertices[1], vertices[2]),new Edge("d", vertices[2], vertices[2]),new Edge("d", vertices[2], vertices[1]) };

                //List<Edge> edges = new List<Edge> { new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]) };

                DrawGraph drgr = new DrawGraph(field, new Graph(vertices, edges));
                drgr.Draw();

                //Vertex vertex = new Vertex("ee", 350, 150);
                //Vertex vertex2 = new Vertex("lol", 550, 250);
                //List<Vertex> vertices2 = new List<Vertex> { vertex, vertex2 };
                //List<Edge> edges2 = new List<Edge> { new Edge("d", vertex, vertex), new Edge("d", vertex2, vertex2), new Edge("t", vertex, vertex2) };

                //DrawGraph dr = new DrawGraph(field, new Graph(vertices2, edges2));
                //dr.Draw();

            }
            else if (vertex.IsChecked == true)
            {
                Ellipse el = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X, e.GetPosition(field).Y), new Point(0, 0), Brushes.Red, "uu");
                
                el.MouseDown += new MouseButtonEventHandler(Ellipse_MouseDown);
                
                // el.MouseMove += new MouseEventHandler(mouse_Move);
                // el.MouseUp += new MouseButtonEventHandler(mouse_Up);

                field.Children.Add(el);
                var path = BezPathFab.GetPath(new Point(e.GetPosition(field).X, e.GetPosition(field).Y), new Point((e.GetPosition(field).X + 50)/2 + 20,
                                        (e.GetPosition(field).Y + 100)/2 - 50), new Point(e.GetPosition(field).X - 50, e.GetPosition(field).Y - 75), Brushes.Red,"", true);

                path.MouseDown += new MouseButtonEventHandler(Path_MouseDown); 
                field.Children.Add(path);

                

                //Path.
                //field.Children.
                //new Grid().Children.Add();
            }
            
            else if (hand.IsChecked == true )
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
                
            }
            else if (delete.IsChecked == true)
            {
                var el = (Ellipse)sender;
                field.Children.Remove(el);
            }
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (cursor.IsChecked == true)
            {
                MessageBox.Show("d");
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
