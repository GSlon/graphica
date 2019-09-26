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

                else                                    // общий вид делится на 2 типа: отрезок образует угол с осью X больше 90 градусов или меньше
                {                                       // в зависимости от этого будет одинаковое изменение по обеим коорд (less 90) или противоположное по x и по y (more 90)
                    if ( ((from.Y.CompareTo(to.Y) > 0) && (from.X.CompareTo(to.X) < 0)) ||      
                         ((from.Y.CompareTo(to.Y) < 0) && (from.X.CompareTo(to.X) > 0)) )     // less 90 -> одинаковое приращение
                    {
                        middle.X += shift;
                        middle.Y += shift;
                    }
                    else   // more 90 -> ассиметричное приращ
                    {
                        middle.X += shift;
                        middle.Y -= shift;
                    }
                }

            }

            return middle;
        }


        public static Path GetPath(Point from, Point middle, Point to, Brush brush, string name = "",  bool orient = false)     // false - не направ
        {
            QuadraticBezierSegment bezierSegment = new QuadraticBezierSegment(middle, to, true);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = from;
            pathFigure.Segments.Add(bezierSegment);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path();
            path.Tag = name;
            path.Data = pathGeometry;
            path.Stroke = brush;
            path.StrokeThickness = 1.3;

            if (orient)                     // рисование стрелки посередине линии!!!!!!!!!!
            {
                path.StrokeEndLineCap = PenLineCap.Round;
                
            }

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
        public static void ChangePath(Path path, Point from, Point to, double shift, Brush brush, string name = "")
        {
            ChangePath(path, from, CalcMiddle(from, to, shift), to, brush, name);
        }
    }
}
