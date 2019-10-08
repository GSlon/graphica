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
    class EllipseFab
    {
        // ChangeText - через создание нового эллипса (эллипсы являются неизменяемыми)
        public static Ellipse GetEllipse(Point center, Brush brush, string txt)
        {
            if (txt.Trim().Length == 0)
                txt = "def";

            TextBlock text = new TextBlock
            {
                Text = txt,
                Background = brush,
                Width = 60,
                Height = 40,
                TextAlignment = System.Windows.TextAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
            };

            BitmapCacheBrush br = new BitmapCacheBrush(text);

            Ellipse elps = new Ellipse
            {
                Width = 50,
                Height = 50,
                Opacity = 1,    // непрозрачность
                Stroke = brush,
                //Margin = new System.Windows.Thickness(10, 10, 10, 80),    // -25 чтобы рисовать вокруг точки center
                Tag = txt,       // надпись получим через tag  
            };

            elps.Fill = br;
            
            Canvas.SetLeft(elps, center.X - 25);
            Canvas.SetTop(elps, center.Y - 25);
            Canvas.SetZIndex(elps, 2);      // 'доминация' над линиями
            return elps;
        }

        public static void ChangeEllipse(Ellipse ellipse, Brush brush, string txt = " ")
        {
            TextBlock text = new TextBlock
            {
                Text = txt,
                Background = brush,
                Width = 60,
                Height = 40,
                TextAlignment = System.Windows.TextAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            BitmapCacheBrush br = new BitmapCacheBrush(text);

            ellipse.Stroke = brush;
            ellipse.Opacity = 1;
            ellipse.Fill = br;

            if (txt.Trim().Length != 0)      // по умолчанию будет без изменений
                ellipse.Tag = txt;
        }

        public static void ChangeElpsCoord(Ellipse elps, Point center)
        {
            //ellipse.Margin = new Thickness(coord.X - 25, coord.Y - 25, 0, 0);
            Canvas.SetLeft(elps, center.X - 25);
            Canvas.SetTop(elps, center.Y - 25);
        }

        public static Point GetCenter(Ellipse elps)
        {
            return new Point(Canvas.GetLeft(elps)+25, Canvas.GetTop(elps)+25);
        }
    }

}
