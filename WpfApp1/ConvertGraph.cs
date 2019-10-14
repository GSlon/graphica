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

namespace WpfApp1
{

    // преобразовывает входящие данные в Graph и наоборот
    class ConvertGraph
    {
        //private static string[] Modes = { "adj", "inc", "vert", "edg", "json" };

        // набор статических функций для преобразования

        // ребра пусть всегда получают названия по номерам (не пользовательские)
        public static Graph FromFile(string path)  // считаем, что все проверки на корректность path уже были сделаны до вызова функции
        {
            // можешь дать неориент ребру добавиться в граф 2 раза, но оно точно должно быть в коллекции from (можешь добавить, но в конце пройди и удали всё кроме первого вхождения такого ребра(orient = 0, ))
            // петли всегда не ориетир!!! 

            Graph graph = null;
            
            // проверь, правильная строка пришла ли
            StreamReader stream = new StreamReader(path);

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

                case "json":

                    break;

                default:
                    stream.Close();
                    throw new Exception("Wrong format");
            }

            stream.Close();
            return graph;         // возвращаем новый граф
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

        public static void ToFile(string path, Graph graph)   // mode: .inc; .adj
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

                case "json":

                    break;

                default:
                    stream.Close();
                    throw new Exception("Wrong format");
            }

            stream.Close();
        }

        public static Graph CanvasToGraph(Canvas canvas)
        {
            var edges = new List<Edge>();
            Dictionary<Point, Vertex> temp;     // temp - Dict, хранящий координаты и vertex; с ним будет легче собирать ребра
            

            // проверку на повтор имён!
            foreach (Shape item in canvas.Children)
            {
                // все эллипсы - в vertex; все path надо парсить на начальную и конечную координаты, сравнивая затем с серединами эллипсов 
                //if 
                
                
                
            }



            return new Graph();
        }

        private static Graph AdjToGraph(StreamReader stream)
        {



            return new Graph();
        }


        private static int[,] ToMatrIncid(Graph gr)    // mode - матрица смеж/инцед и др.
        {
            return new int[4,4];         // возвращаем двумерный массив
        }


    }
}
