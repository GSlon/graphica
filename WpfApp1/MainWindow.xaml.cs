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
        //   private Graph graph;

       
        static Int16 i;
        bool check;         // можно убрать
        Ellipse ellipse;    // универсальный эллипс
        //DrawGraph drawGraph;
        //Graph graph;
        
        public MainWindow()
        {
            InitializeComponent();

            i = 0;
            check = false;
            ellipse = new Ellipse();
       
        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((cursor.IsChecked == true))
            {
                List<Vertex> vertices = new List<Vertex> { new Vertex("школа", 150, 100), new Vertex("дом", 100, 60) };
                //List<Edge> edges = new List<Edge> { new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], 
                //    vertices[1]),new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]),
                //    new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[0], vertices[1]), new Edge("d", vertices[1], vertices[2]),new Edge("d", vertices[2], vertices[2]),new Edge("d", vertices[2], vertices[1]) };

                List<Edge> edges = new List<Edge> { new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]), new Edge("f", vertices[0], vertices[1]) };

                DrawGraph drgr = new DrawGraph(field, new Graph(vertices, edges));
                drgr.Draw();

               
               

                //Vertex vertex = new Vertex("ee", 350, 150);
                //Vertex vertex2 = new Vertex("lol", 550, 250);
                //List<Vertex> vertices = new List<Vertex> { vertex, vertex2 };
                //List<Edge> edges = new List<Edge> { new Edge("d", vertex, vertex), new Edge("d", vertex2, vertex2), new Edge("t", vertex, vertex2) };

                //DrawGraph dr = new DrawGraph(field, new Graph(vertices, edges));
                //dr.Draw();

            }
            else if (vertex.IsChecked == true)
            {
                Ellipse el = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X - 25, e.GetPosition(field).Y - 25), new Point(0, 0), Brushes.Red, i.ToString());
                Ellipse el2 = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X - 100, e.GetPosition(field).Y - 100), new Point(0, 0), Brushes.Red, i.ToString());

                el.MouseDown += new MouseButtonEventHandler(Ellipse_MouseDown);
                el2.MouseDown += new MouseButtonEventHandler(Ellipse_MouseDown);
                
                // el.MouseMove += new MouseEventHandler(mouse_Move);
                // el.MouseUp += new MouseButtonEventHandler(mouse_Up);

                i++;

                field.Children.Add(el);
                field.Children.Add(el2);
                var path = BezPathFab.GetPath(new Point(e.GetPosition(field).X + 25, e.GetPosition(field).Y+25), new Point(e.GetPosition(field).X + 150,
                                        e.GetPosition(field).Y - 150), new Point(e.GetPosition(field).X - 50, e.GetPosition(field).Y - 75), Brushes.Red,"", true);

                path.MouseDown += new MouseButtonEventHandler(Path_Mouse_Down); 
                field.Children.Add(path);

                

                //Path.
                //field.Children.
                //new Grid().Children.Add();
            }
            
            else if (handle_move.IsChecked == true )
            {
               if ( !(ellipse.IsMouseOver))
                {
                    ellipse.Margin = new Thickness(e.GetPosition(field).X - 25, e.GetPosition(field).Y - 25, 0, 0);
                    check = false;
                }
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
            else if (handle_move.IsChecked == true)
            {   
                ellipse = (Ellipse)sender;
                check = true;
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
