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
        bool capturedElps = false;
        bool capturedPath = false;
        Point Start;

        public MainWindow()
        {
            InitializeComponent();

        }

        // mode: 1 - выходим из; 2 - входим; 3 - выходим и входим
        private List<Path> FindPathes(Point point, int mode)
        {
            List<Path> templist = new List<Path>();
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Path path))
                    continue;

                var data = (PathGeometry)path.Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];

                switch (mode)
                {
                    case 1:
                        if (data.Figures[0].StartPoint == point)
                        {
                            templist.Add(path);
                        }
                        break;

                    case 2:
                        if (bezier.Point2 == point)
                        {
                            templist.Add(path);
                        }
                        break;

                    case 3:
                        if ((data.Figures[0].StartPoint == point) || (bezier.Point2 == point))
                        {
                            templist.Add(path);
                        }
                        break;

                    default:
                        break;
                }
            }

            return templist;
        }

        // mode: 1 - из/в; 2 - в/из; 3 - неважно
        private List<Path> FindPathes(Point start, Point end, int mode)
        {
            List<Path> templist = new List<Path>();
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Path path))
                    continue;

                var data = (PathGeometry)path.Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];

                switch (mode)
                {
                    case 1:
                        if ((data.Figures[0].StartPoint == start) && (bezier.Point2 == end))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 2:
                        if ((data.Figures[0].StartPoint == end) && (bezier.Point2 == start))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 3:
                        if (((data.Figures[0].StartPoint == start) && (bezier.Point2 == end)) ||
                                ((data.Figures[0].StartPoint == end) && (bezier.Point2 == start)))
                        {
                            templist.Add(path);
                        }
                        break;

                    default:
                        break;
                }
            }

            return templist;
        }

        private void AddPath(Point start, Point end)
        {
            if (start == end)   // узел  
            {
                end = new Point(end.X + 15, end.Y + 15);    // новый end для узла

                if (FindPathes(start, end, 3).Count == 0)
                    field.Children.Add(BezPathFab.GetPath(start, new Point((start.X + end.X) / 2 - 50,
                                       (start.Y + end.Y) / 2 + 50), end, Brushes.Black,
                                        PathCount().ToString() + " 1 false", false));
                else
                    return;

            }
            else
            {
                int k = 1;
                int count = FindPathes(start, end, 3).Count;
                ++count;

                // изменение направления вектора меняет знак приращения
                if (start.X.CompareTo(end.X) > 0)
                    k = -1;

                field.Children.Add(BezPathFab.GetPath(start, end, Brushes.Black, Math.Pow(-1, count) * (count / 2) * 20 * k, PathCount().ToString() +
                                                    " " + "1 " + "true", true));
            }
        }

        private void RepaintVertPair(Point start, Point end)
        {
            var edges = FindPathes(start, end, 3);

            int k = 1;  // направление вектора определяет направление shift
            for (int i = 0; i < edges.Count; i++)
            {
                field.Children.Remove(edges[i]);

                var data = (PathGeometry)edges[i].Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];

                if (data.Figures[0].StartPoint.X.CompareTo(bezier.Point2.X) > 0)
                    k = -1;
                else
                    k = 1;

                field.Children.Add(BezPathFab.GetPath(data.Figures[0].StartPoint, bezier.Point2, Brushes.Black, Math.Pow(-1, i) * ((i+1) / 2) * 20 * k, edges[i].Tag.ToString(),
                                            bool.Parse(edges[i].Tag.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2])));
            }
        }

        private void ChangeVertCoord(Point start, Point change)
        {
            var edgesFrom = FindPathes(start, 1);
            var edgesTo = FindPathes(start, 2);

            for (int i = 0; i < edgesFrom.Count; i++)
            {
                field.Children.Remove(edgesFrom[i]);

                var data = (PathGeometry)edgesFrom[i].Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];

                field.Children.Add(BezPathFab.GetPath(change, bezier.Point2, Brushes.Black, Math.Pow(-1, i) * (i / 2) * 20, edgesFrom[i].Tag.ToString(),
                                            bool.Parse(edgesFrom[i].Tag.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2])));
            }

            int j = edgesFrom.Count;
            for (int i = 0; i < edgesTo.Count; i++)
            {
                field.Children.Remove(edgesTo[i]);

                var data = (PathGeometry)edgesTo[i].Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];

                field.Children.Add(BezPathFab.GetPath(data.Figures[0].StartPoint, change, Brushes.Black, Math.Pow(-1, j + 1) * ((j + 1) / 2) * 20, edgesTo[i].Tag.ToString(),
                                            bool.Parse(edgesTo[i].Tag.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2])));
                ++j;
            }
        }

        private void DeleteEdges(Point center)
        {
            var edges = FindPathes(center, 3);

            for (int i = 0; i < edges.Count; i++)
                field.Children.Remove(edges[i]);
        }

        private int PathCount()
        {
            int count = 0;
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Path path))
                    continue;

                ++count;
            }

            return count;
        }

        private int EllpsCount()
        {
            int count = 0;
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Ellipse elps))
                    continue;

                ++count;
            }

            return count;
        }

        private void Field_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (edge.IsChecked == true)
                capturedPath = false;
            
            else if (vertex.IsChecked == true)
            {
                Ellipse el = EllipseFab.GetEllipse(new Point(e.GetPosition(field).X, e.GetPosition(field).Y), Brushes.Red, EllpsCount().ToString());

                field.Children.Add(el);
                Update_Canvas();
            }
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;   // отключили передачу события к восходящим элементам

            if (!(sender is Ellipse elps))
                return;

            Point center = new Point(elps.Margin.Left + 25, elps.Margin.Top + 25);
            if (edge.IsChecked == true)
            {
                if (capturedPath)
                {
                    AddPath(Start, center);
                    Update_Canvas();
                    capturedPath = false;
                }
                else
                {
                    Start = center;
                    capturedPath = true;
                }
            }
            else if (hand.IsChecked == true)
            {
                capturedElps = true;
            }
            else if (delete.IsChecked == true)
            {
                DeleteEdges(center);
                field.Children.Remove(elps);
            }
        }

        //
        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (capturedElps)
            {
                if (!(sender is Ellipse elps))
                    return;

                Point newCoord = new Point(e.GetPosition(field).X , e.GetPosition(field).Y );
                Point oldCoord = new Point(elps.Margin.Left + 25, elps.Margin.Top + 25);

                ChangeVertCoord(oldCoord, newCoord);
                EllipseFab.ChangeElpsCoord((Ellipse)sender, new Point(e.GetPosition(field).X, e.GetPosition(field).Y));

                Update_Canvas();
            }
        }
        //

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            capturedElps = false;
        }
       
        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            capturedElps = false;
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (!(sender is Path path))
                return;

            if (hand.IsChecked == true)
            { 

            }
            else if (delete.IsChecked == true)
            {
                field.Children.Remove(path);
                var data = (PathGeometry)path.Data;
                var bezier = (QuadraticBezierSegment)data.Figures[0].Segments[0];
                RepaintVertPair(data.Figures[0].StartPoint, bezier.Point2);
                Update_Canvas();
            }
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Path_MouseMove(object sender, MouseButtonEventArgs e)
        {

        }

        // новым фигурам заполняем делегаты (используй после Draw)
        private void Update_Canvas()
        {
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (field.Children[i].GetType().Name.ToString().Equals("Ellipse"))
                {
                    field.Children[i].MouseDown += new MouseButtonEventHandler(Ellipse_MouseDown);
                    field.Children[i].MouseMove += new MouseEventHandler(Ellipse_MouseMove);
                    field.Children[i].MouseUp += new MouseButtonEventHandler(Ellipse_MouseUp);
                    field.Children[i].MouseLeave += new MouseEventHandler(Ellipse_MouseLeave);
                }
                else if (field.Children[i].GetType().Name.ToString().Equals("Path"))
                {
                    field.Children[i].MouseDown += new MouseButtonEventHandler(Path_MouseDown);

                    //
                }
            }
        }

        private void Choose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            capturedPath = false;
            capturedElps = false;
        }
    }
}
