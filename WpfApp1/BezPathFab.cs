using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;

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
                    if (to.X.CompareTo(from.X) > 0)
                        shift *= -1;

                    double d = Math.Sqrt(Math.Pow(to.X - from.X, 2) + Math.Pow(to.Y - from.Y, 2));
                    double Xvec = to.Y - from.Y;    // векторы нормали
                    double Yvec = from.X - to.X;

                    middle.X += (Xvec / d) * shift; // умножаем на shift единичный вектор нормали
                    middle.Y += (Yvec / d) * shift;
                }
            }

            return middle;
        }

        // middle - значение угла
        private static Point[] CalcArrow(Point from, Point PAngle, Point to)   // для расчета координат стрелочки
        {
            Point middle = BezValuesMid(from, PAngle, to, 0.5);

            // считаем shift, чтобы получить две промежуточных точки 
            Point leastMiddle = new Point((from.X + to.X) / 2, (from.Y + to.Y) / 2);
            double d = Math.Sqrt(Math.Pow(middle.X - leastMiddle.X, 2) + Math.Pow(middle.Y - leastMiddle.Y, 2));  // длина отрезка, конец которого - угол квадрата в QuadBezier
            double check = Math.Round((middle.X - from.X) * (to.Y - from.Y) - (middle.Y - from.Y) * (to.X - from.X));
            double d1 = 0, d2 = 0;

            Point SupportP1 = new Point();
            Point SupportP2 = new Point();

            if (((d.CompareTo(-10) >= 0) && (d.CompareTo(10) <= 0)) || ((check.CompareTo(800) <= 0) && (check.CompareTo(-800) >= 0)))   // погрешность в пределах 5 px; либо middle лежит на прямой
            {
                SupportP1 = CalcMiddle(from, to, 5);
                SupportP2 = CalcMiddle(from, to, -5);
            }
            else
            {
                // высчитываем дальность middle от from и to (стрелочка будет отодвинута от центра на разницу дальностей)
                d1 = Math.Round(Math.Sqrt(Math.Pow(middle.X - from.X, 2) + Math.Pow(middle.Y - from.Y, 2)), 3);
                d2 = Math.Round(Math.Sqrt(Math.Pow(middle.X - to.X, 2) + Math.Pow(middle.Y - to.Y, 2)), 3);
                double d3 = d1 / d2;

                if (d3 > 2)
                    d3 = 2;

                try
                {
                    middle = BezValuesMid(from, PAngle, to, 0.5 + Math.Sign(d1 - d2) * Math.Pow(Math.Abs(d3 - 1), 1.0 / 3) / 6);     // power = 1/3
                    check = (middle.X - from.X) * (to.Y - from.Y) - (middle.Y - from.Y) * (to.X - from.X);
                }
                catch (Exception)
                {
                    middle = BezValuesMid(from, middle, to, 0.5);
                }

                double Xvec = middle.X - leastMiddle.X;
                double Yvec = middle.Y - leastMiddle.Y;

                SupportP1.X = middle.X + (Xvec / d) * 5;
                SupportP1.Y = middle.Y + (Yvec / d) * 5;
                SupportP2.X = middle.X - (Xvec / d) * 5;
                SupportP2.Y = middle.Y - (Yvec / d) * 5;
            }

            // осталось выбрать правильное направление стрелки         
            Point SupportP3;
            int shift = 8;     // отвечает за направление
            //if (check.CompareTo(0)== 0)
            //    MessageBox.Show("лежит на отрезке");
            //else if (check > 0)
            //    MessageBox.Show("выше");
            //else
            //    MessageBox.Show("ниже");
            //if ((to.Y.CompareTo(from.Y) < 0) || ((to.Y.CompareTo(from.Y) == 0) && (to.X.CompareTo(from.X) < 0)))
            //{
            //    shift *= -1;
            //}
            int situation = 0;
            if ((to.X.CompareTo(from.X) > 0) && (to.Y.CompareTo(from.Y) < 0))
            {
                situation = 1;
            }
            else if ((to.X.CompareTo(from.X) < 0) && (to.Y.CompareTo(from.Y) > 0))
            {
                situation = 2;
            }
            else if ((to.X.CompareTo(from.X) > 0) && (to.Y.CompareTo(from.Y) > 0))
            {
                situation = 3;
            }
            else if ((to.X.CompareTo(from.X) < 0) && (to.Y.CompareTo(from.Y) < 0))
            {
                situation = 4;
            }
            else if ((to.X.CompareTo(from.X) > 0) && (to.Y.CompareTo(from.Y) == 0))
            {
                situation = 5;
            }
            else if ((to.X.CompareTo(from.X) < 0) && (to.Y.CompareTo(from.Y) == 0))
            {
                situation = 6;
            }
            else if ((to.X.CompareTo(from.X) == 0) && (to.Y.CompareTo(from.Y) < 0))
            {
                situation = 7;
            }
            else if ((to.X.CompareTo(from.X) == 0) && (to.Y.CompareTo(from.Y) > 0))
            {
                situation = 8;
            }

            // 4 взаимных расположения Support
            if (((SupportP1.X.CompareTo(SupportP2.X) < 0) && (SupportP1.Y.CompareTo(SupportP2.Y) < 0)) || ((SupportP1.X.CompareTo(SupportP2.X) > 0) && (SupportP1.Y.CompareTo(SupportP2.Y) > 0)))
            {
                switch (situation)
                {
                    case 2:
                    case 6:
                            shift *= 1;
                        break;

                    case 3:
                        if ((d1 - d2) != 0)
                            shift *= Math.Sign(check) * Math.Sign(d1 - d2);
                        break;

                    case 4:
                        if ((d1 - d2) != 0)
                            shift *= -Math.Sign(check) * Math.Sign(d1 - d2);
                        break;

                    default:
                            shift *= -1;
                        break;
                };

            }
            else if (((SupportP1.X.CompareTo(SupportP2.X) > 0) && (SupportP1.Y.CompareTo(SupportP2.Y) < 0)) || ((SupportP1.X.CompareTo(SupportP2.X) < 0) && (SupportP1.Y.CompareTo(SupportP2.Y) > 0)))
            {
                switch (situation)
                {
                    case 1:
                        if ((d1 - d2) != 0)
                            shift *= Math.Sign(check) * Math.Sign(d1 - d2);
                        else
                            shift *= -1;
                        break;

                    case 2:
                        if ((d2 - d1) != 0)
                            shift *= Math.Sign(check) * Math.Sign(d2 - d1);
                        else
                            shift *= -1;
                        break;
                    case 4:
                            shift *= -1;
                        break;

                    case 3:
                            shift *= 1;
                        break;

                    default:
                            shift *= -1;
                        break;
                };
            }
            else if (SupportP1.X.CompareTo(SupportP2.X) == 0)
            {
                switch (situation)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 8:
                        shift *= 1;
                        break;

                    default:
                        shift *= -1;
                        break;
                };
            }
            else if (SupportP1.Y.CompareTo(SupportP2.Y) == 0)
            {
                switch (situation)
                {
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                        if ((d1 - d2) != 0)
                            shift *= Math.Sign(check) * Math.Sign(d1 - d2);
                        break;

                    case 8:
                        shift *= 1;
                        break;

                    default:
                        shift *= -1;
                        break;
                };
            }

            //if (SupportP2.Y.CompareTo(SupportP1.Y) < 0)         // инверсия, когда вспомогательные точки поменялись местами
            //{
            //    SupportP3 = CalcMiddle(SupportP1, SupportP2, shift);    // нужный эффект - движение "снизу вверх"
            //}
            //else
            //{
            //    SupportP3 = CalcMiddle(SupportP2, SupportP1, shift);
            //}
            SupportP3 = CalcMiddle(SupportP1, SupportP2, shift);

            return new Point[3] { SupportP1, SupportP3, SupportP2 };
        }

        // считает значение уравнения кривой Безье (middle - середина дуги, а не опорная точка! ее мы сейчас и ищем)
        private static Point BezValuesbyMid(Point from, Point middle, Point to, double t)    // t - параметр
        {
            double x = (middle.X - Math.Pow(1 - t, 2) * from.X - t * t * to.X) / (2 * (1 - t) * t);
            double y = (middle.Y - Math.Pow(1 - t, 2) * from.Y - t * t * to.Y) / (2 * (1 - t) * t);

            return new Point(x, y);
        }

        private static Point BezValuesMid(Point from, Point Pangle, Point to, double t)
        {
            double x = Math.Pow(1 - t, 2) * from.X  + 2 * (1 - t) * t * Pangle.X + t * t * to.X;
            double y = Math.Pow(1 - t, 2) * from.Y + 2 * (1 - t) * t * Pangle.Y + t * t * to.Y;

            return new Point(x, y);
        }

        // middle - середина дуги (надо преобразовать в угол квадрата)
        private static GeometryGroup GetCombPathGeometry(Point from, Point middle, Point to, bool orient, int weight)
        {
            GeometryGroup combined = new GeometryGroup {
                FillRule = FillRule.EvenOdd
            };
            
            // безье и стрелка
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure bezfigure = new PathFigure   // фигура, состоящая из сегмента (Безье)
            {
                StartPoint = from,
                //IsFilled = false,
                IsClosed = false
            };    

            if (from.Equals(to))     // узлы рисуются так
            {
                bezfigure.Segments.Add(new PolyBezierSegment(new List<Point> { new Point(from.X - 100, from.Y + 75), 
                                    new Point(from.X + 100, from.Y + 75), to}, true));   // преобразовали в значение для угла
                
                pathGeometry.Figures.Add(bezfigure);
            }
            else
            {
                Point Pangle = BezValuesbyMid(from, middle, to, 0.5);

                bezfigure.Segments.Add(new QuadraticBezierSegment(Pangle, to, true));   // преобразовали в значение для угла
                pathGeometry.Figures.Add(bezfigure);

                if (orient)
                {
                    Point[] info = CalcArrow(from, Pangle, to);        

                    LineSegment line1 = new LineSegment(info[1], true);
                    LineSegment line2 = new LineSegment(info[2], true);

                    PathFigure linesfig = new PathFigure
                    {
                        StartPoint = info[0],
                        IsClosed = false
                    };

                    linesfig.Segments.Add(line1);
                    linesfig.Segments.Add(line2);

                    pathGeometry.Figures.Add(linesfig);
                    //pathGeometry.Transform = new SkewTransform(15,15);
                }
            }

            // вес
            FormattedText formatText = new FormattedText(weight.ToString(), CultureInfo.CurrentCulture,
                                                     FlowDirection.LeftToRight,
                                                     new Typeface(new FontFamily("Colibri"), FontStyles.Oblique, FontWeights.UltraLight, FontStretches.UltraCondensed),
                                                     13, Brushes.Black);

            Point center = new Point();
            if (from.Equals(to))
            {
                center.X = from.X;
                center.Y = from.Y + 60;
            }
            else 
            {
                //center = CalcMiddle(middle, BezValuesMid);

                center.X = middle.X;        // не красиво
                center.Y = middle.Y + 8;
            }
            
            Geometry geometry = formatText.BuildGeometry(center);

            combined.Children.Add(pathGeometry);
            combined.Children.Add(geometry);
            return combined;
        }

        public static Path GetPath(Point from, Point middle, Point to, Brush brush, string name = " ",  bool orient = false, int weight = 1)     // false - не направ
        {
            if (name.Trim().Length == 0)
                name = "def";

            Path path = new Path
            {
                Tag = new object[] { name, orient, weight },
                Data = GetCombPathGeometry(from, middle, to, orient, weight), 
                Stroke = brush,
                StrokeThickness = 2
            };

            return path;
        }

        // shift - сдвиг от середины между from и to 
        public static Path GetPath(Point from, Point to, Brush brush, double shift, string name = " ", bool orient = false, int weight = 1)     // false - не направ
        {
            return GetPath(from, CalcMiddle(from, to, shift), to, brush, name, orient, weight); 
        }

        public static void ChangePathCoord(Path path, Point from, Point middle, Point to)
        {
            var data = (object[])path.Tag;
            path.Data = GetCombPathGeometry(from, middle, to, (bool)data[1], (int)data[2]);
        }

        // shift - сдвиг от середины между from и to
        public static void ChangePathCoord(Path path, Point from, Point to, double shift)
        {
            ChangePathCoord(path, from, CalcMiddle(from, to, shift), to);
        }

        // без перерисовки
        public static void ChangePathData(Path path, Brush brush, string name = " ", bool orient = false, int weight = 1) 
        {
            path.Stroke = brush;

            if (name.Trim().Length != 0)     // если 'пустой' name - ничего не меняем
                path.Tag = new object[] { name, orient, weight };
        }

        // [start, end, middle]
        public static Point[] GetPathCoord(Path path)
        {
            var combined = (GeometryGroup)path.Data;
            var data = (PathGeometry)combined.Children[0];

            if (data.Figures[0].Segments[0] is QuadraticBezierSegment bezier)
                return new Point[3] { data.Figures[0].StartPoint, bezier.Point2, BezValuesMid(data.Figures[0].StartPoint, bezier.Point1, bezier.Point2, 0.5) }; // вернем middle, а не Pangle

            else if (data.Figures[0].Segments[0] is PolyBezierSegment polybezier)
            {
                var points = polybezier.Points.ToArray();
                return new Point[4] { data.Figures[0].StartPoint, points[2], points[1], points[0] };
            }

            else
                throw new Exception("wrong type");

            //return new Point[] { };
        }

        public static object[] GetPathData(Path path)
        {
            var data = (object[])path.Tag;

            if (data.Length == 3)
                return data;
            else
                return null;
        }

    }
}
