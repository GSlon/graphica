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

            if (orient)
            {
                path.StrokeEndLineCap = PenLineCap.Round;
                
            }

            return path;
        }

        public static void ChangePath(Path path, Point from, Point middle, Point to, Brush brush, string name = "")
        {
            QuadraticBezierSegment bezierSegment = new QuadraticBezierSegment(middle, to, true);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = from;
            pathFigure.Segments.Add(bezierSegment);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            path.Stroke = brush;
            path.Data = pathGeometry;
            path.Tag = name;
        }

    }
}
