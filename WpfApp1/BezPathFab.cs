using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace WpfApp1
{
    class BezPathFab
    {
        // возвращает сдвиг от середины 
        private static Point CalcMiddle(Point from, Point to, double shift)
        {
            Point middle = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);     // будем двигать середину отрезка from->to

            if (shift.CompareTo(0) == 0)    // сдвиг не нужен
            {
                return middle;
            }
            else   
            {
                // 4 случая: задана точка; задан горизонт, вертикал и общего вида отрезок   (по разному будем двигать середину from->to)
                if ((from.X.CompareTo(to.X) == 0) && (from.Y.CompareTo(to.Y) == 0))   // точка 
                {
                    middle.X = from.X + shift;
                    middle.Y = from.Y + shift;
                }

                else if (from.Y.CompareTo(to.Y) == 0)   // горизонтальный отрезок (delta y = 0)
                {
                    middle.Y += shift;
                }

                else if (from.X.CompareTo(to.X) == 0)   // вертикальный отрезок (delta x = 0)
                {
                    middle.X += shift;
                }

                else                                   
                {                                       
                    double d = Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));
                    double Xvec = to.Y - from.Y;    // векторы нормали
                    double Yvec = from.X - to.X;

                    middle.X += (Xvec / d) * shift; // умножаем на shift единичный вектор нормали
                    middle.Y += (Yvec / d) * shift;
                }
            }
            return middle;
        }

        // middle - середина дуги
        private static Point[] CalcArrow(Point from, Point middle, Point to)   // для расчета координат стрелочки
        {
            // считаем shift, чтобы получить две промежуточных точки 
            Point leastMiddle = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            double d = Math.Sqrt(Math.Pow(middle.X - leastMiddle.X, 2) + Math.Pow(middle.Y - leastMiddle.Y, 2));  // длина отрезка, конец которого - угол квадрата в QuadBezier

            Point SupportP1 = new Point();
            Point SupportP2 = new Point();

            if (d == 0)
            {
                SupportP1 = CalcMiddle(from, to, 5);
                SupportP2 = CalcMiddle(from, to, -5);
            }
            else
            {
                double Xvec = middle.X - leastMiddle.X;
                double Yvec = middle.Y - leastMiddle.Y;

                SupportP1.X = middle.X + (Xvec / d) * 5;
                SupportP1.Y = middle.Y + (Yvec / d) * 5;
                SupportP2.X = middle.X - (Xvec / d) * 5;
                SupportP2.Y = middle.Y - (Yvec / d) * 5;
            }

            // осталось выбрать правильное направление стрелки
            Point SupportP3;
            int shift = -8;     // отвечает за направление
            if ((to.X.CompareTo(from.X) < 0) || ((to.X.CompareTo(from.X) == 0) && (to.Y.CompareTo(from.Y) < 0)))
            {
                shift *= -1;
            }

            if (SupportP2.Y.CompareTo(SupportP1.Y) < 0)         // инверсия, когда вспомогательные точки поменялись местами
            {
                SupportP3 = CalcMiddle(SupportP1, SupportP2, shift);    // нужный эффект - движение "снизу вверх"
            }
            else
            {
                SupportP3 = CalcMiddle(SupportP2, SupportP1, shift);
            }

            return new Point[3] { SupportP1, SupportP3, SupportP2 };
        }

        // считает значение уравнения кривой Безье (middle - середина дуги, а не опорная точка! ее мы сейчас и ищем)
        private static Point BezValues(Point from, Point middle, Point to, double t)    // t - параметр
        {
            double x = (middle.X - Math.Pow(1 - t, 2) * from.X - t * t * to.X) / (2 * (1 - t) * t);
            double y = (middle.Y - Math.Pow(1 - t, 2) * from.Y - t * t * to.Y) / (2 * (1 - t) * t);

            return new Point(x, y);
        }

        // middle - середина дуги (надо преобразовать в угол квадрата)
        public static Path GetPath(Point from, Point middle, Point to, Brush brush, string name = "",  bool orient = false)     // false - не направ
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure bezfigure = new PathFigure
            {
                StartPoint = from
            };    // фигура, состоящая из сегмента (Безье)

            bezfigure.Segments.Add(new QuadraticBezierSegment(BezValues(from, middle, to, 0.5), to, true));   // преобразовали в значение для угла
            pathGeometry.Figures.Add(bezfigure);

            if (orient)                    
            {
                Point[] info = CalcArrow(from, middle, to);

                LineSegment line1 = new LineSegment(info[1], true);
                LineSegment line2 = new LineSegment(info[2], true);

                PathFigure linesfig = new PathFigure
                {
                    StartPoint = info[0]
                };

                linesfig.Segments.Add(line1);
                linesfig.Segments.Add(line2);

                pathGeometry.Figures.Add(linesfig);
                //pathGeometry.Transform = new SkewTransform(15,15);
            }

            Path path = new Path
            {
                Tag = name,
                Data = pathGeometry,
                Stroke = brush,
                StrokeThickness = 2
            };

            return path;
        }


        // shift - сдвиг от середины между from и to 
        public static Path GetPath(Point from, Point to, Brush brush, double shift, string name = "", bool orient = false)     // false - не направ
        {
            return GetPath(from, CalcMiddle(from, to, shift), to, brush, name, orient); 
        }

        public static void ChangePath(Path path, Point from, Point middle, Point to, Brush brush, string name = "", bool orient = false)
        {
            path = GetPath(from, middle, to, brush, name, orient);
        }

        // shift - сдвиг от середины между from и to
        public static void ChangePath(Path path, Point from, Point to, double shift, Brush brush, string name = "", bool orient = false)
        {
            ChangePath(path, from, CalcMiddle(from, to, shift), to, brush, name, orient);
        }
    }
}
