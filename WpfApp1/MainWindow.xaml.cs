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
        bool check; // можно убрать
        Ellipse ellipse; // универсальный эллипс

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
                MessageBox.Show("cursor");
            }
            else if (vertex.IsChecked == true)
            {
                Ellipse el = new EllipseFab(e.GetPosition(field).X - 25, e.GetPosition(field).Y - 25, 0, 0, i.ToString()).GetEllipse();

                el.MouseDown+= new MouseButtonEventHandler(El_MouseDown);          
                // el.MouseMove += new MouseEventHandler(mouse_Move);
                // el.MouseUp += new MouseButtonEventHandler(mouse_Up);

                i++;

                field.Children.Add(el);
                
                
            }
            
            else if (handle_move.IsChecked == true )
            {
               if ((check) & !(ellipse.IsMouseOver))
                {
                    ellipse.Margin = new Thickness(e.GetPosition(field).X - 25, e.GetPosition(field).Y - 25, 0, 0);
                    check = false;
                }
            }
           
        }

        protected void El_MouseDown(object sender, MouseButtonEventArgs e)
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
