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
                Vertex vert = new Vertex("rr", 100, 100);
                DrawGraph drgr = new DrawGraph(field, new Graph(vert, new Edge("d", vert, vert)));
                drgr.Draw();



            }
            else if (vertex.IsChecked == true)
            {
                Ellipse el = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X - 25, e.GetPosition(field).Y - 25), new Point(0, 0), Brushes.Red, i.ToString());
                Ellipse el2 = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X - 100, e.GetPosition(field).Y - 100), new Point(0, 0), Brushes.Red, i.ToString());

                el.MouseDown += new MouseButtonEventHandler(El_MouseDown);
                el2.MouseDown += new MouseButtonEventHandler(El_MouseDown);
                
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

        private void El_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Path_Mouse_Down(object sender, MouseButtonEventArgs e)
        {
            if (cursor.IsChecked == true)
            {
                MessageBox.Show("d");
            }
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
