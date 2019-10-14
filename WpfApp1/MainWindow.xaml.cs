using Microsoft.Win32;
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
        //LinkedList<KeyValuePair<Graph , int[,]>> GraphCopies;     // матрица смежности и граф (не придется пересчитывать)
        //LinkedList<Graph> graphs;
        Edition edition;

        bool capturedElps = false;
        bool capturedPath = false;
        Point Start;
        private const double shift = 40;        // единое отклонение
        HashSet<string> names;
        bool WasSaved = false;                  // предлагать ли сохранение

        public MainWindow()
        {
            InitializeComponent();

            names = new HashSet<string>();

            edition = new Edition(new Graph());
            //graphs = new LinkedList<Graph>();
        }
        
        private void Choose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            capturedPath = false;
            capturedElps = false;
        }

        // mode: 0 - узлы (в 2/3 mode узлы не берем); 1 - выходим из; 2 - входим в; 3 - выходим и входим; 4 - всё, связанное с point
        private List<Path> FindPathes(Point point, int mode)
        {
            int oldMode = mode;

            List<Path> templist = new List<Path>();
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Path path))
                    continue;

                mode = oldMode;

                var data = BezPathFab.GetPathCoord(path);

                if ((data[0].Equals(point)) && (data[1].Equals(point)))      // узел
                {
                    if (mode == 0)
                    {
                        templist.Add(path);
                        break;              // пока один узел
                    }

                    if (mode == 4)
                    {
                        templist.Add(path);
                    }

                    continue;
                }
                else if (mode == 0)
                    continue;

                if (!(bool)BezPathFab.GetPathData(path)[1])   // не ориентир
                {
                    mode = 3;
                }

                switch (mode)
                {
                   case 1:
                        if (data[0].Equals(point))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 2:
                        if (data[1].Equals(point))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 4:
                    case 3:
                        if ((data[0].Equals(point)) || (data[1].Equals(point)))
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

        // mode: 1 - из/в; 2 - в/из; 3 - неважно    (всегда без петель)
        private List<Path> FindPathes(Point start, Point end, int mode)
        {
            int oldMode = mode;

            List<Path> templist = new List<Path>();
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Path path))
                    continue;

                if (start.Equals(end))
                    continue;

                mode = oldMode;
                var data = BezPathFab.GetPathCoord(path);

                if (!(bool)BezPathFab.GetPathData(path)[1])      //не ориентир
                {
                    mode = 3;
                }

                switch (mode)
                {
                    case 1:
                        if ((data[0].Equals(start)) && (data[1].Equals(end)))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 2:
                        if ((data[0].Equals( end)) && (data[1].Equals(start)))
                        {
                            templist.Add(path);
                        }
                        break;

                    case 3:
                        if (((data[0].Equals(start)) && (data[1].Equals(end))) ||
                                ((data[0].Equals(end)) && (data[1].Equals(start))))
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

        private Ellipse[] FindEllipses(Point start, Point end)
        {
            Ellipse[] pair = new Ellipse[2];
            foreach (var child in field.Children)
            {
                if (child is Ellipse elps)
                {
                    var coord = EllipseFab.GetCenter(elps);

                    if (coord == start)
                    {
                        pair[0] = elps;

                        if (start == end)   // loop
                        {
                            pair[1] = elps;
                            break;
                        }
                    }
                    else if (coord == end)
                        pair[1] = elps;
                }
            }

            return pair;
        }
        
        // лежит ли check в координатах чьего - то эллипса, кроме without
        private bool isEmptyCoord(Point check)
        {
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Ellipse elps))
                    continue;

                Point centerElps = EllipseFab.GetCenter(elps);

                if (centerElps == check)
                {
                    return false;
                }
            }

            return true;
        }

        private bool isEmptyCoord(Point check, Point without)
        {
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Ellipse elps))
                    continue;

                Point centerElps = EllipseFab.GetCenter(elps);

                if (centerElps == without)
                    return true;

                if (centerElps == check) 
                {
                    return false;
                }
            }

            return true;
        }

        private bool isEmptyCoord(Point check, List<Point> without)
        {
            for (int i = 0; i < field.Children.Count; i++)
            {
                if (!(field.Children[i] is Ellipse elps))
                    continue;

                for (int j = 0; j < without.Count; j++)
                {
                    if (!isEmptyCoord(check, without[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void AddEdge(Point start, Point end)
        {
            int count = 0;
            bool orient = true;

            if (start.Equals(end))  //петля
            {
                count = FindPathes(start, 0).Count;

                if (count != 0)    // уже есть петля
                    return;
                else
                {
                    orient = false;
                }
            }
            else
                count = FindPathes(start, end, 3).Count;

            Path path = new Path();

            path = BezPathFab.GetPath(start, end, Brushes.Black, Math.Pow(-1, count) * ((count+1) / 2) * shift, PathCount().ToString(), orient, 1);
            BindData(path);

            field.Children.Add(path);
            UpdateGraph(path, "add");
        }

        // перерисовка всех ребер, соединяющих две данные вершины
        private void RepaintVertPair(Point start, Point end)
        {
            var edges = FindPathes(start, end, 3);

            for (int i = 0; i < edges.Count; i++)
            {
                var data = BezPathFab.GetPathCoord(edges[i]);

                BezPathFab.ChangePathCoord(edges[i], data[0], data[1], Math.Pow(-1, i) * ((i+1) / 2) * shift);
            }
        }

        // start переходит в change
        private void ChangeVertCoord(Point start, Point change)
        {
            if (start.Equals(change))
            {
                return;
            }

            Point end = new Point();
            for (int k = 0; k < field.Children.Count; k++)
            {
                if (!(field.Children[k] is Ellipse elps))
                    continue;

                end = EllipseFab.GetCenter(elps);

                if (end.Equals(start))      // петля
                {
                    var node = FindPathes(start, 0);
                    if (node.Count != 0)
                        BezPathFab.ChangePathCoord(node[0], change, change, 5);

                    continue;
                }

                var edgesFrom = FindPathes(start, end, 3);

                for (int i = 0; i < edgesFrom.Count; i++)
                {
                    var data = BezPathFab.GetPathCoord(edgesFrom[i]);

                    if (data[0].Equals(start))
                        BezPathFab.ChangePathCoord(edgesFrom[i], change, data[1], Math.Pow(-1, i) * ((i + 1) / 2) * shift);
                    
                    else if (data[0].Equals(end))
                        BezPathFab.ChangePathCoord(edgesFrom[i], data[0], change, Math.Pow(-1, i) * ((i + 1) / 2) * shift);

                }
            }
        }

        private void AddVertex(Point center)
        {
            int NewName = 0;

            while (names.Contains(NewName.ToString()))
            {
                ++NewName;
            }
            names.Add(NewName.ToString());

            Ellipse el = EllipseFab.GetEllipse(center, Brushes.Red, NewName.ToString());
            BindData(el);

            field.Children.Add(el);
            UpdateGraph(el, "add");
        }

        // удаление всех ребер, связанных с вершиной
        private void DeleteEdges(Point center)
        {
            var edges = FindPathes(center, 4);

            for (int i = 0; i < edges.Count; i++)
            {
                DeleteEdge(edges[i]);
            }
        }

        private void DeleteEdge(Path path)
        {
            if (path != null)
            {
                DeleteBindData(path);
                field.Children.Remove(path);
                UpdateGraph(path, "delete");
            }
        }

        private void DeleteVertex(Ellipse elps)
        {
            if (elps != null)
            {
                DeleteBindData(elps);
                field.Children.Remove(elps);
                names.Remove(elps.Tag.ToString());
                UpdateGraph(elps, "delete");
            }
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
                AddVertex(new Point(e.GetPosition(field).X, e.GetPosition(field).Y));
            }
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;   // отключили передачу события к восходящим элементам

            if (!(sender is Ellipse elps))
                return;

            Point center = new Point(Canvas.GetLeft(elps) + 25, Canvas.GetTop(elps) + 25);
            if (edge.IsChecked == true)
            {
                if (capturedPath)
                {
                    AddEdge(Start, center);
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
                elps.MouseMove += new MouseEventHandler(Ellipse_MouseMove);
                elps.MouseUp += new MouseButtonEventHandler(Ellipse_MouseUp);
                elps.MouseLeave += new MouseEventHandler(Ellipse_MouseLeave);

                capturedElps = true;
            }
            else if (delete.IsChecked == true)
            {
                DeleteEdges(center);
                DeleteVertex(elps);
            }
        }

        private void Ellipse_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContextMenu context = FindResource("ContMenuElps") as ContextMenu;
            for (int i = 0; i < context.Items.Count; i++)
            {
                if (context.Items[i] is MenuItem item)
                    item.DataContext = sender;
            }

            context.IsOpen = true;
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;

            if (capturedElps)
            {
                if (!(sender is Ellipse elps))
                    return;

                Point newCoord = new Point(e.GetPosition(field).X, e.GetPosition(field).Y);
                Point oldCoord = new Point(Canvas.GetLeft(elps) + 25, Canvas.GetTop(elps) + 25);

                if (!isEmptyCoord(newCoord, oldCoord))
                {
                    capturedElps = false;
                    Point koef = new Point(newCoord.X - oldCoord.X, newCoord.Y - oldCoord.Y);
                    newCoord.X += koef.X * 10;
                    newCoord.Y += koef.Y * 10;
                }

                ChangeVertCoord(oldCoord, newCoord);
                EllipseFab.ChangeElpsCoord(elps, newCoord);
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Ellipse elps))
                return;

            elps.MouseMove -= new MouseEventHandler(Ellipse_MouseMove);
            elps.MouseUp -= new MouseButtonEventHandler(Ellipse_MouseUp);
            elps.MouseLeave -= new MouseEventHandler(Ellipse_MouseLeave);

            capturedElps = false;

            UpdateGraph(elps, "update");
        }
       
        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse_MouseUp(sender, new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, new MouseButton()));
        }


        private void Path_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (!(sender is Path path))
                return;

            var data = BezPathFab.GetPathCoord(path);
            if (hand.IsChecked == true)
            {
                if (data[0].Equals(data[1]))      // петли пока не двигаем
                    return;

                path.MouseMove += new MouseEventHandler(Path_MouseMove);
                path.MouseUp += new MouseButtonEventHandler(Path_MouseUp);
                path.MouseLeave += new MouseEventHandler(Path_MouseLeave);

                capturedPath = true;
                Mouse.Capture(path);
            }
            else if (delete.IsChecked == true)
            {
                DeleteEdge(path);

                if (!(data[0].Equals(data[1])))
                    RepaintVertPair(data[0], data[1]);
            }
        }

        private void Path_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Path path))
                return;

            ContextMenu context;
            var coords = BezPathFab.GetPathCoord(path);
            if (coords[0].Equals(coords[1]))    // loop
                context = FindResource("ContMenuLoop") as ContextMenu;
            else 
                context = FindResource("ContMenuPath") as ContextMenu;
            
            for (int i = 0; i < context.Items.Count; i++)
            {
                if (context.Items[i] is MenuItem item)
                    item.DataContext = sender;
            }

            context.IsOpen = true;
        }

        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is Path path))
                return;

            var data = BezPathFab.GetPathCoord(path);
            BezPathFab.ChangePathCoord(path, data[0], new Point(e.GetPosition(field).X, e.GetPosition(field).Y), data[1]);

            //BezPathFab.ChangePathCoord(path, data[0], data[1],  e.GetPosition(field).Y - e.GetPosition(field).X);
        }

        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Path path))
                return;

            path.MouseMove -= new MouseEventHandler(Path_MouseMove);
            path.MouseUp -= new MouseButtonEventHandler(Path_MouseUp);
            path.MouseLeave -= new MouseEventHandler(Path_MouseLeave);

            capturedPath = false;
            Mouse.Capture(null);
        }
        
        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Path path))
                return;

            path.MouseMove -= new MouseEventHandler(Path_MouseMove);
            path.MouseUp -= new MouseButtonEventHandler(Path_MouseUp);
            path.MouseLeave -= new MouseEventHandler(Path_MouseLeave);

            capturedPath = false;

            Mouse.Capture(null);
        }

        //
        // новым фигурам заполняем делегаты (используй только после Draw) (лишнее использоыание - лишний добавленный делегат!!!)
        private void ExtBindData()
        {
            foreach (UIElement child in field.Children)
            {
                if ((child.GetType().Name.ToString().Equals("Ellipse")) || (child.GetType().Name.ToString().Equals("Path")))
                {
                    BindData(child);

                    if (child is Ellipse elps)
                        names.Add(elps.Tag.ToString());
                }
            }
        }

        // только для полной перерисовки (не вызывает UpdateGraph)
        private void Clear_Canvas()
        {
            for (int i = 0; i < field.Children.Count; i++) 
            {
                if (field.Children[i] is Ellipse elps)
                {
                    DeleteBindData(elps);
                    field.Children.Remove(elps);
                }
                else if (field.Children[i] is Path path)
                {
                    DeleteBindData(path);
                    field.Children.Remove(path);
                }
            }
            names.Clear();
        }

        private void BindData(UIElement element)
        {
            if (!(element is Shape figure))
                return;

            if (figure.GetType().Name.ToString().Equals("Ellipse"))
            {
                figure.MouseLeftButtonDown += new MouseButtonEventHandler(Ellipse_MouseLeftButtonDown);
                figure.MouseRightButtonDown += new MouseButtonEventHandler(Ellipse_MouseRightButtonDown);
            }
            else if (figure.GetType().Name.ToString().Equals("Path"))
            {
                figure.MouseLeftButtonDown += new MouseButtonEventHandler(Path_MouseLeftButtonDown);
                figure.MouseRightButtonDown += new MouseButtonEventHandler(Path_MouseRightButtonDown);
            }
        }

        private void DeleteBindData(UIElement element)
        {
            if (!(element is Shape figure))
                return;

            if (figure.GetType().Name.ToString().Equals("Ellipse"))
            {
                figure.MouseLeftButtonDown -= Ellipse_MouseLeftButtonDown;
                figure.MouseRightButtonDown -= Ellipse_MouseRightButtonDown;
            }
            else if (figure.GetType().Name.ToString().Equals("Path"))
            {
                figure.MouseLeftButtonDown -= Path_MouseLeftButtonDown;
                figure.MouseRightButtonDown -= Path_MouseRightButtonDown;
            }
        }

        private void MakeLoop(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem data))
                return;

            Point center = EllipseFab.GetCenter((Ellipse)data.DataContext);
            AddEdge(center, center);
        }

        private void DeleteLoop(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem data))
                return;

            if (data.DataContext is Ellipse elps)
                DeleteEdge(FindPathes(EllipseFab.GetCenter(elps), 0).FirstOrDefault<Path>());
        }

        private void ChangeText(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem data))
                return;

            var elps = (Ellipse)data.DataContext;

            // окно ввода текста (txt)
            TextWindow text = new TextWindow();

            if (text.ShowDialog() == true)
            {
                string txt = text.GetText();
                if (!names.Contains(txt))
                {
                    string oldName = elps.Tag.ToString();
                    names.Remove(oldName);
                    EllipseFab.ChangeEllipse(elps, elps.Stroke, txt);
                    names.Add(txt);

                    UpdateGraph(elps, "update", oldName);
                }
                else
                    MessageBox.Show("This name is existing now");
            }
        }

        private void ChangeDirection(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem data))
                return;

            var path = (Path)data.DataContext;
            var pathData = BezPathFab.GetPathData(path);

            if (pathData == null)
                throw new Exception("wrong path data");

            var coords = BezPathFab.GetPathCoord(path);
            if (data.Name.Equals("noorient"))
            {
                if (pathData[1].ToString().Equals("True"))      // если уже false, то ничего делать не надо
                {
                    BezPathFab.ChangePathData(path, path.Stroke, (string)pathData[0], false, (int)pathData[2]);
                    BezPathFab.ChangePathCoord(path, coords[0], coords[2], coords[1]);      // перерисовка
                }
            }
            else if (data.Name.Equals("orient"))
            {
                if (pathData[1].ToString().Equals("False"))      // если еще не направ, то делаем это
                {
                    BezPathFab.ChangePathData(path, path.Stroke, (string)pathData[0], true, (int)pathData[2]);
                    BezPathFab.ChangePathCoord(path, coords[0], coords[2], coords[1]);      // перерисовка
                }
                else if (pathData[1].ToString().Equals("True")) // если направ, то меняем местами from и to
                {
                    BezPathFab.ChangePathCoord(path, coords[1], coords[2], coords[0]);
                }
            }

            UpdateGraph(path, "update");
        }

        private void ChangeWeight(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem data))
                return;

            var path = (Path)data.DataContext;
            var pathData = BezPathFab.GetPathData(path);
            var coords = BezPathFab.GetPathCoord(path);

            // окно ввода веса
            EditWindow edit = new EditWindow();

            if (edit.ShowDialog() == true)
            {
                BezPathFab.ChangePathData(path, path.Stroke, (string)pathData[0], (bool)pathData[1], edit.GetWeight());
                BezPathFab.ChangePathCoord(path, coords[0], coords[2], coords[1]);      // перерисовка
            }

            UpdateGraph(path, "update");
        }

        ///////// логика рисовалки кончилась
        private void DrawOnCnvs(Graph graph)
        {
            Clear_Canvas();

            DrawGraph.Draw(field, graph);
            ExtBindData();
        }
        private void SaveGraph(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Adjacency|*.adj|Incidence|*.inc|Vertex|*.vert|Edge|*.edg|Json|*.json"
            };

            if (dialog.ShowDialog().Value)
            {
                try
                {
                    //ConvertGraph.CanvasToPng(dialog.FileName, field);
                    ConvertGraph.FromFile(dialog.FileName);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }


                WasSaved = true;
            }
        }

        private void SaveCanvas(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Png Image(.png) | *.png"
            };

            if (dialog.ShowDialog().Value)
            {
                try
                {
                    ConvertGraph.CanvasToPng(dialog.FileName, field);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void OpenGraph(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Adjacency|*.adj|Incidence|*.inc|Vertex|*.vert|Edge|*.edg|Json|*.json"
            };

            Graph gr = null;
            if (dialog.ShowDialog().Value)
            {
                try
                {
                    //ConvertGraph.ToFile(dialog.FileName, gr);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
                return;

            //GraphCopies.Clear();    // очистили 'историю'
            DrawOnCnvs(gr);

            WasSaved = true;    // пока сохранять не надо
        }

        // эта функция будет вызываться только когда сделано что то с графом, требующее сохранение для undo/redo
        private void UpdateGraph(UIElement elem, string comm, string oldName = "")
        {
            Graph newGraph = (Graph)edition.CurrentGraph().Clone();

            // связь Shape и элемента в графе - через name
            if (elem is Ellipse elps)
            {
                Point coord = EllipseFab.GetCenter(elps);
                string name = elps.Tag.ToString();
                switch (comm)
                {
                    case "add":
                        newGraph.AddVertex(new Vertex(name, elps.Stroke, coord.X, coord.Y));
                        break;

                    case "delete":
                        newGraph.DeleteVertex(name);
                        break;

                    case "update":
                        if (oldName.Length == 0)     // не меняли имени
                            newGraph.ChangeVertex(name, name, coord.X, coord.Y, elps.Stroke);
                        else
                            newGraph.ChangeVertex(oldName, name, coord.X, coord.Y, elps.Stroke);

                        break;

                    default:
                        break;
                }
            }
            else if (elem is Path path)
            {
                var data = BezPathFab.GetPathData(path);

                switch (comm)
                {
                    case "add":
                        var coord = BezPathFab.GetPathCoord(path);
                        var elpses = FindEllipses(coord[0], coord[1]);
                        newGraph.AddEdge((string)data[0], elpses[0].Tag.ToString(), elpses[1].Tag.ToString(), path.Stroke, (bool)data[1], (int)data[2]);
                        break;

                    case "delete":
                        newGraph.DeleteEdge((string)data[0]);
                        break;

                    case "update":
                        newGraph.ChangeEdge((string)data[0], (bool)data[1], (int)data[2], path.Stroke);
                        break;

                    default:
                        break;
                }
            }

            edition.AddGraph(newGraph);

            WasSaved = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // проверить WasSaved


        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            DrawOnCnvs(edition.Undo());
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            DrawOnCnvs(edition.Redo());
        }
    }
}
