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
using Newtonsoft.Json;

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

                    break;

                case "vert":

                    break;

                case "edg":

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

        private static void FileAdjToGraph(StreamReader stream, Graph graph)
        {
            NumAdjToGraph(graph, MatrixAdjacency(stream));            
        }

        private static void JsonToGraph(StreamReader stream, Graph graph)
        {
            graph = JsonConvert.DeserializeObject<Graph>(stream.ReadToEnd());
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

            Point temp = new Point(0, 0);
            Point copyTemp;
            for (int i = 0; i < matrix.Count; i++)
            {
                copyTemp = temp;

                if (i % 3 != 0)
                {
                    if (i % 2 == 0)
                    {
                        copyTemp.X += 100;
                    }
                    else
                        copyTemp.Y += 100;
                }
                else
                {
                    temp.X += 100;
                    temp.Y += 100;

                    copyTemp.X += 100;
                    copyTemp.Y += 100;
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


        //static public List<List<int>> MatrixIncidence(string path)
        //{
        //    List<List<int>> matrix_inc = new List<List<int>>();

        //    using (StreamReader sr = new StreamReader(path))
        //    {
        //        string line;

        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            int i = 0;
        //            List<int> list = new List<int>();

        //            while ((line[i] != '\n') && (line[i] != '%'))
        //            {
        //                if (line[i] == ' ')
        //                {
        //                    i++;
        //                    continue;
        //                }
        //                else
        //                {
        //                    if ((line[i] == '-') || ((line[i] >= '0') && (line[i] <= '9')))
        //                    {
        //                        string str = "";
        //                        while ((line[i] != ' ') && (line[i] != '\n'))
        //                        {
        //                            str += line[i];
        //                            if ((line.Length - (++i)) == 0)
        //                                break;
        //                        }

        //                        list.Add(Int16.Parse(str));
        //                    }
        //                    else
        //                        throw new Exception("bad char");
        //                }

        //                if (i < line.Length - 1)
        //                    i++;
        //                else
        //                    break;
        //            }
        //            matrix_inc.Add(list);
        //        }
        //    }

        //    return matrix_inc;
        //}

        //static public Dictionary<int, List<string>> ListEdge(string path)
        //{
        //    string list;
        //    Dictionary<int, List<string>> list_ed = new Dictionary<int, List<string>>();

        //    using (StreamReader sr = new StreamReader(path))
        //        list = sr.ReadToEnd(); // считали в одну строку

        //    Regex regex_edge = new Regex(@"Edges{[0-9a-zA-Z\s,()]+\}", RegexOptions.Multiline);  // проверка на Edge{......}
        //    MatchCollection matchCollection = regex_edge.Matches(list); // получение всех списков рёбер

        //    Regex regex_value = new Regex(@"(\d*)\( (\d*), (\w*), (\w*), (\d)\)"); // проверка на правильное содержимое {.....}

        //    foreach (Match match in matchCollection)
        //    {
        //        MatchCollection mat = regex_value.Matches(match.Value); // получение содержимого для данного списка рёбер

        //        foreach (Match i in mat)
        //            if (!list_ed.ContainsKey(int.Parse(i.Groups[1].Value)))
        //                list_ed.Add(int.Parse(i.Groups[1].Value), new List<string> { i.Groups[2].Value, i.Groups[3].Value, i.Groups[4].Value, i.Groups[5].Value });
        //    }

        //    return list_ed;
        //}

        //static public void ListVertex(Graph graph, string path)
        //{
        //    string list;

        //    using (StreamReader sr = new StreamReader(path))
        //        list = sr.ReadToEnd(); // считали в одну строку

        //    Regex regex_edge = new Regex(@"^Vertex{[0-9a-zA-Z\s,()]+\}", RegexOptions.Multiline);
        //    MatchCollection matchCollection = regex_edge.Matches(list);

        //    Regex regex_value = new Regex(@"(\w*)\( (\d*), (\d*)\)");

        //    foreach (Match match in matchCollection)
        //    {
        //        MatchCollection mat = regex_value.Matches(match.Value);

        //        foreach (Match i in mat)
        //            graph.addVert(new Vertex(double.Parse(i.Groups[2].ToString()), double.Parse(i.Groups[3].ToString())), i.Groups[1].ToString());
        //    }
        //}

        

        //static public void FromIncToGraph(Graph graph, List<List<int>> matr)
        //{
        //    for (int i = 0; i < matr.Count; i++)
        //        graph.addVert(new Vertex(center.X + 50 * i * (1 - Math.Pow(-1, i)), center.Y + 50 * i * (1 - Math.Pow(-1, i + 1)))); //graph.addVert(new Vertex(center.X + 50 * i * Math.Pow(-1, i), center.Y + 50 * i * Math.Pow(-1, i)));

        //    for (int i = 0; i < matr[0].Count; i++)
        //    {
        //        List<int> vs = new List<int>();
        //        List<int> check_positon = new List<int>();

        //        for (int j = 0; j < matr.Count; j++)
        //        {
        //            if (matr[j][i] != 0)
        //            {
        //                vs.Add(matr[j][i]);
        //                check_positon.Add(j);
        //            }
        //        }

        //        if (vs.Count == 1)
        //            graph.addEdge(new List<string> { check_positon[0].ToString(), check_positon[0].ToString() }, new Edge(vs[0].ToString()));
        //        else
        //            if (vs.Count == 2)
        //            if (vs[0] == vs[1])
        //                graph.addEdge(new List<string> { check_positon[0].ToString(), check_positon[1].ToString() }, new Edge(vs[0].ToString()));
        //            else if (vs[0] > vs[1])
        //                graph.addEdge(new List<string> { check_positon[0].ToString(), check_positon[1].ToString() }, new Edge(vs[0].ToString(), true));
        //            else
        //                graph.addEdge(new List<string> { check_positon[0].ToString(), check_positon[1].ToString() }, new Edge((-vs[0]).ToString(), false, true));
        //        else
        //            throw new Exception("It is edge not exist");
        //    }
        //}

        //static public void FromListEdgeToGraph(Graph graph, Dictionary<int, List<string>> list_ed)
        //{
        //    if (list_ed.Count == 0)
        //        throw new Exception("Bad enter");

        //    int i = 0;
        //    foreach (int key in list_ed.Keys)
        //    {
        //        // {[0] weight, [1] name_1, [2] name_2, [3] orient}

        //        graph.addVert(new Vertex(center.X + 50 * i * (1 - Math.Pow(-1, i)), center.Y + 50 * i * (1 - Math.Pow(-1, i + 1))), list_ed[key][1]);
        //        i++;
        //        graph.addVert(new Vertex(center.X + 50 * i * (1 - Math.Pow(-1, i)), center.Y + 50 * i * (1 - Math.Pow(-1, i + 1))), list_ed[key][2]);

        //        if (int.Parse(list_ed[key][3]) == 0)
        //            graph.addEdge(new List<string> { list_ed[key][1], list_ed[key][2] }, new Edge(list_ed[key][0].ToString()));
        //        else
        //            if (int.Parse(list_ed[key][3]) >= 1)
        //            graph.addEdge(new List<string> { list_ed[key][1], list_ed[key][2] }, new Edge(list_ed[key][0].ToString(), true));

        //    }
        //}
        //



        // TO
        public static void ToFile(string path, Graph graph)   
        {
            StreamWriter stream = new StreamWriter(path);

            switch (path.Split('.')[1])
            {
                case "adj":

                    break;

                case "inc":

                    break;

                case "vert":

                    break;

                case "edg":

                    break;

                //case "json":
                //    GraphToJson(stream, graph);
                //    break;

                default:
                    stream.Close();
                    throw new Exception("Wrong format");
            }

            stream.Close();
        }

        private static int[,] GraphToInc(StreamWriter stream, Graph graph)   
        {

            return new int[4, 4];         // возвращаем двумерный массив
        }

        private static void GraphToJson(StreamWriter stream, Graph graph)
        {
            stream.Write(JsonConvert.SerializeObject(graph));
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
