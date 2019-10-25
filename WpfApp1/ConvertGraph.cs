using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;

namespace WpfApp1
{

    // преобразовывает входящие данные в Graph и наоборот
    class ConvertGraph
    {
        //private static string[] Modes = { "adj", "inc", "vert", "edg", "json" };
        // набор статических функций для преобразования

        // ребра пусть всегда получают названия по номерам (не пользовательские)
        
        // FROM
        public static void FromFile(string path, Graph graph)  // считаем, что все проверки на корректность path уже были сделаны до вызова функции
        {
            // петли всегда не ориетир!!! 

            graph.Clear();
            
            StreamReader stream = new StreamReader(path);

            switch (path.Split('.')[1])
            {
                case "adj":
                    FileAdjToGraph(stream, graph);
                    break;

                case "inc":
                    FileIncToGraph(stream, graph);
                    break;

                case "edg":
                    FileEdgeToGraph(stream, graph);
                    break;

                case "vert":
                    ListVertex(graph, stream);
                    break;

                //case "json":
                //    JsonToGraph(stream, graph);
                //    break;

                default:
                    stream.Close();
                    throw new Exception("Wrong format");
            }

            stream.Close();
        }

        // adj
        private static void FileAdjToGraph(StreamReader stream, Graph graph)
        {
            NumAdjToGraph(graph, MatrixAdjacency(stream));            
        }

        private static List<List<int>> MatrixAdjacency(StreamReader stream)
        {
            List<List<int>> matrix = new List<List<int>>();
            
            string line = "";

            while ((line = stream.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    line = line.Split('%')[0];  // отсекли коммент
                    var row = line.Split(' ');
                    
                    List<int> iRow = new List<int>();
                    foreach (var cell in row)
                    {
                        if (cell.Trim() != "")
                            iRow.Add(int.Parse(cell.Trim()));  // избавились от лишних пробелов
                    }

                    matrix.Add(iRow);
                    
                    if (matrix.Count != 0)
                        if (iRow.Count != matrix[0].Count)
                        {
                            throw new Exception("Matrix is not squarty");
                        }
                }
                else
                    continue;
            }
        
            return matrix;
        }

        private static void NumAdjToGraph(Graph graph, List<List<int>> matrix)
        {
            Dictionary<string, Vertex> vers = new Dictionary<string, Vertex>();

            Point temp = new Point(50, 50);
            Point copyTemp;
            int mul = 1;
            for (int i = 0; i < matrix.Count; i++)
            {
                copyTemp = temp;

                if (i % 3 != 0)
                {
                    if (i % 2 == 0)
                    {
                        copyTemp.X += 80 * mul;
                    }
                    else
                        copyTemp.Y += 80 * mul;
                }
                else
                {
                    copyTemp.X += 80 * mul;
                    copyTemp.Y += 80 * mul;

                    ++mul;
                }

                Vertex vertex = new Vertex(i.ToString(), copyTemp.X, copyTemp.Y);
                graph.AddVertex(vertex);
                vers.Add(i.ToString(), vertex);
            }

            int name = 0;
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = i; j < matrix.Count; j++)
                {
                    if (((matrix[i][j] < 0) || (matrix[j][i] < 0)))
                        throw new Exception("wrong matrix format");

                    if (matrix[i][j] != matrix[j][i])
                    {
                        if ((matrix[i][j] != 0) && (matrix[j][i] != 0))
                            throw new Exception("wrong matrix format");
                        else
                        {
                            if (matrix[i][j] > matrix[j][i])
                                graph.AddEdge(new Edge(name.ToString(), vers[i.ToString()], vers[j.ToString()], true, matrix[i][j]));
                            
                            else if (matrix[i][j] < matrix[j][i])
                                graph.AddEdge(new Edge(name.ToString(), vers[j.ToString()], vers[i.ToString()], true, matrix[j][i]));
                        }
                    }
                    else
                        if (matrix[i][j] != 0)
                            graph.AddEdge(new Edge(name.ToString(), vers[i.ToString()], vers[j.ToString()], false, matrix[i][j]));

                    ++name;
                }
            }
        }

        // inc
        private static void FileIncToGraph(StreamReader stream, Graph graph)
        {
            NumIncToGraph(graph, MatrixIncidence(stream));
        }

        private static List<List<int>> MatrixIncidence(StreamReader stream)
        {
            List<List<int>> matrix = new List<List<int>>();

            string line = "";

            while ((line = stream.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    line = line.Split('%')[0];  // отсекли коммент
                    var row = line.Split(' ');

                    List<int> iRow = new List<int>();
                    foreach (var cell in row)
                    {
                        if (cell.Trim() != "")
                            iRow.Add(int.Parse(cell.Trim()));  // избавились от лишних пробелов
                    }

                    matrix.Add(iRow);
                }
                else
                    continue;
            }

            return matrix;
        }

        private static void NumIncToGraph(Graph graph, List<List<int>> matrix)
        {
            Dictionary<string, Vertex> vers = new Dictionary<string, Vertex>();

            Point temp = new Point(50, 50);
            Point copyTemp;
            int mul = 1;
            for (int i = 0; i < matrix.Count; i++)
            {
                copyTemp = temp;

                if (i % 3 != 0)
                {
                    if (i % 2 == 0)
                    {
                        copyTemp.X += 80 * mul;
                    }
                    else
                        copyTemp.Y += 80 * mul;
                }
                else
                {
                    copyTemp.X += 80 * mul;
                    copyTemp.Y += 80 * mul;

                    ++mul;
                }

                Vertex vertex = new Vertex(i.ToString(), copyTemp.X, copyTemp.Y);
                graph.AddVertex(vertex);
                vers.Add(i.ToString(), vertex);
            }

            int name = 0;
            List<int> vs = new List<int>();
            List<int> vert = new List<int>();
            for (int i = 0; i < matrix[0].Count; i++)
            {
                for (int j = 0; j < matrix.Count; j++)
                {
                    if (matrix[j][i] != 0)
                    {
                        vs.Add(matrix[j][i]);
                        vert.Add(j);
                    }
                }

                if (vs.Count == 1)  // loop
                {
                    if (vs[0] < 0)
                        throw new Exception("wrong format");

                    graph.AddEdge(new Edge(name.ToString(), vers[vert[0].ToString()], vers[vert[0].ToString()], false, vs[0]));
                }
                else if (vs.Count == 2)
                {
                    if ((Math.Abs(vs[0]) != Math.Abs(vs[1])) || ((vs[0] < 0) && (vs[1] < 0)))
                        throw new Exception("wrong format");

                    if (vs[0] == vs[1])
                        graph.AddEdge(new Edge(name.ToString(), vers[vert[0].ToString()], vers[vert[1].ToString()], false, vs[0]));
                    else if (vs[0] > vs[1])
                        graph.AddEdge(new Edge(name.ToString(), vers[vert[0].ToString()], vers[vert[1].ToString()], true, vs[0]));
                    else
                        graph.AddEdge(new Edge(name.ToString(), vers[vert[1].ToString()], vers[vert[0].ToString()], true, vs[1]));
                }
                else
                    throw new Exception("wrong format");

                ++name;

                vs.Clear();
                vert.Clear();
            }
        }

        // edges
        private static void FileEdgeToGraph(StreamReader stream, Graph graph)
        {
            DictEdgeToGraph(graph, DictEdge(stream));
        }

        private static Dictionary<int, List<string>> DictEdge(StreamReader stream)
        {
            string list;
            Dictionary<int, List<string>> list_ed = new Dictionary<int, List<string>>();

            list = stream.ReadToEnd(); // считали в одну строку

            Regex regex_edge = new Regex(@"Edges{[0-9a-zA-Z\s,()]+\}", RegexOptions.Multiline);  // проверка на Edge{......}
            MatchCollection matchCollection = regex_edge.Matches(list); // получение всех списков рёбер

            Regex regex_value = new Regex(@"(\d*)\( (\d*), (\w*), (\w*), (\d)\)"); // проверка на правильное содержимое {.....}

            foreach (Match match in matchCollection)
            {
                MatchCollection mat = regex_value.Matches(match.Value); // получение содержимого для данного списка рёбер

                foreach (Match i in mat)
                    if (!list_ed.ContainsKey(int.Parse(i.Groups[1].Value)))
                        list_ed.Add(int.Parse(i.Groups[1].Value), new List<string> { i.Groups[2].Value, i.Groups[3].Value, i.Groups[4].Value, i.Groups[5].Value });
            }

            return list_ed;
        }

        private static void DictEdgeToGraph(Graph graph, Dictionary<int, List<string>> info)
        {
            if (info.Count == 0)
                throw new Exception("Bad input");

            // собрали имена vertex
            HashSet<string> UniqueNames = new HashSet<string>();
            foreach (var data in info.Values)
            {
                if (!UniqueNames.Contains(data[1]))
                    UniqueNames.Add(data[1]);

                if (!UniqueNames.Contains(data[2]))
                    UniqueNames.Add(data[2]);
            }

            Point temp = new Point(50, 50);
            Point copyTemp;
            int mul = 1;
            Dictionary<string, Vertex> vers = new Dictionary<string, Vertex>();
            int i = 0;
            foreach (var name in UniqueNames)
            {
                copyTemp = temp;

                if (i % 3 != 0)
                {
                    if (i % 2 == 0)
                    {
                        copyTemp.X += 80 * mul;
                    }
                    else
                        copyTemp.Y += 80 * mul;
                }
                else
                {
                    copyTemp.X += 80 * mul;
                    copyTemp.Y += 80 * mul;

                    ++mul;
                }

                Vertex vertex = new Vertex(name, copyTemp.X, copyTemp.Y);
                graph.AddVertex(vertex);
                vers.Add(name, vertex);
                ++i;
            }


            foreach (int key in info.Keys)
            {
                // {[0] weight, [1] name_1, [2] name_2, [3] orient
                if (int.Parse(info[key][3]) == 0)
                    graph.AddEdge(new Edge(key.ToString(), vers[info[key][1]], vers[info[key][2]] , false, int.Parse(info[key][0])));
                else
                    if (int.Parse(info[key][3]) >= 1)
                        graph.AddEdge(new Edge(key.ToString(), vers[info[key][1]], vers[info[key][2]], true, int.Parse(info[key][0])));

            }
        }

        // verts
        private static void ListVertex(Graph graph, StreamReader stream)
        {
            string list = "";

            list = stream.ReadToEnd(); // считали в одну строку

            Regex regex_edge = new Regex(@"^Vertex{[0-9a-zA-Z\s,()]+\}", RegexOptions.Multiline);
            MatchCollection matchCollection = regex_edge.Matches(list);

            Regex regex_value = new Regex(@"(\w*)\( (\d*), (\d*)\)");

            foreach (Match match in matchCollection)
            {
                MatchCollection mat = regex_value.Matches(match.Value);

                foreach (Match i in mat)
                    graph.AddVertex(new Vertex(i.Groups[1].ToString(), double.Parse(i.Groups[2].ToString()), double.Parse(i.Groups[3].ToString())));
            }
        }


        // TO
        public static void ToFile(string path, Graph graph)   
        {
            StreamWriter stream = new StreamWriter(path);

            switch (path.Split('.')[1])
            {
                case "adj":
                    GraphToAdjFile(graph, stream);
                    break;

                case "inc":
                    GraphToIncFile(graph, stream);
                    break;

                case "vert":
                    GraphToVertFile(graph, stream);
                    break;

                case "edg":
                    GraphToEdgeFile(graph, stream);
                    break;

                default:
                    stream.Close();
                    throw new Exception("Wrong format");
            }

            stream.Close();
        }

        private static void GraphToVertFile(Graph graph, StreamWriter stream)
        {
            string line = "Vertex{";

            foreach (var vertex in graph.GetVertices())
            {
                line += vertex.Name + "( " + vertex.X + ", " + vertex.Y + "), ";
            }

            line = line.Remove(line.Length - 2) + "}";

            stream.WriteLine(line);
        }

        private static void GraphToEdgeFile(Graph graph, StreamWriter stream)
        {
            string line = "Edges{";

            foreach (var edge in graph.GetEdges())
            {
                line += edge.Name + "( " + edge.Weight.ToString() + ", " + edge.From.Name + ", " + edge.To.Name + ", " + Convert.ToInt32(edge.Orient).ToString() + "), ";
            }

            line = line.Remove(line.Length - 2) + "}";

            stream.WriteLine(line);
        }

        private static void GraphToAdjFile(Graph graph, StreamWriter stream)
        {
            var verts = graph.GetVertices();
            var edges = graph.GetEdges();

            Dictionary<Vertex, int> numVert = new Dictionary<Vertex, int>();    // пронумеруем все вершины
            int i = 0;
            foreach (var vertex in verts)
            {
                numVert.Add(vertex, i);
                ++i;
            }

            int[,] matrix = new int[verts.Count, verts.Count];
            foreach (var edge in edges)
            {
                if (edge.Orient)
                {
                    matrix[numVert[edge.From], numVert[edge.To]] = edge.Weight;
                    matrix[numVert[edge.To], numVert[edge.From]] = 0;
                }
                else
                {
                    matrix[numVert[edge.From], numVert[edge.To]] = edge.Weight;
                    matrix[numVert[edge.To], numVert[edge.From]] = edge.Weight;
                }
            }

            for (i = 0; i < verts.Count; i++)
            {
                for (int j = 0; j < verts.Count; j++)
                {
                    stream.Write(matrix[i, j].ToString() + " ");    
                }
                stream.Write("\n");
            }
        }

        private static void GraphToIncFile(Graph graph, StreamWriter stream)
        {
            var verts = graph.GetVertices();
            var edges = graph.GetEdges();

            Dictionary<Vertex, int> numVert = new Dictionary<Vertex, int>();    // пронумеруем все вершины
            int i = 0;
            foreach (var vertex in verts)
            {
                numVert.Add(vertex, i);
                ++i;
            }


            int[,] matrix = new int[verts.Count, edges.Count];
            for (i = 0; i < edges.Count; i++)
            {
                if (edges[i].Orient)
                {
                    matrix[numVert[edges[i].From], i] = edges[i].Weight;
                    matrix[numVert[edges[i].To], i] = -edges[i].Weight;
                }
                else
                {
                    matrix[numVert[edges[i].From], i] = edges[i].Weight;
                    matrix[numVert[edges[i].To], i] = edges[i].Weight;
                }
            }

            for (i = 0; i < verts.Count; i++)
            {
                for (int j = 0; j < edges.Count; j++)
                {
                    stream.Write(matrix[i, j].ToString() + " ");
                }
                stream.Write("\n");
            }
        }
        //


        public static void CanvasToGraph(Canvas canvas, Graph graph)
        {
            graph.Clear();

            var temp = new Dictionary<Point, Vertex>();     // temp - Dict, хранящий координаты и vertex; с ним будет легче собирать ребра
            var tempPathes = new List<System.Windows.Shapes.Path>();              // для сбора Path

            // эллипсы - в вершины, ребра пока собираем
            foreach (Shape item in canvas.Children)
            {
                if (item is Ellipse elps)
                {
                    var coord = EllipseFab.GetCenter(elps);
                    Vertex vertex = new Vertex(elps.Tag.ToString(), elps.Stroke, coord.X, coord.Y);

                    graph.AddVertex(vertex);    // граф сам сделает контроль имён
                    temp.Add(coord, vertex);
                }
                else if (item is System.Windows.Shapes.Path path)
                {
                    tempPathes.Add(path);   
                }
            }

            // по собранным вершинам находим ребра
            foreach (var path in tempPathes)
            {
                var data = BezPathFab.GetPathData(path);
                var coords = BezPathFab.GetPathCoord(path);

                Edge edge = new Edge((string)data[0], temp[coords[0]], temp[coords[1]], path.Stroke, (bool)data[1], (int)data[2]);
                graph.AddEdge(edge);
            }
        }

        public static void CanvasToPng(string path, Canvas canvas)      // path - путь до файла
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(canvas);
            double dpi = 96d;

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            pngEncoder.Save(ms);
                ms.Close();

            System.IO.File.WriteAllBytes(path, ms.ToArray());
        }

    }
}
