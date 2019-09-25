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
        public static Ellipse GetEllipse(Point center, Point bottom, Brush brush, string txt = "")
        {
            TextBlock text = new TextBlock
            {
                Text = txt,
                Background = brush,
                Width = 40,
                Height = 40,
                TextAlignment = System.Windows.TextAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch
            };

            BitmapCacheBrush br = new BitmapCacheBrush(text);

            Ellipse elps = new Ellipse
            {
                Width = 50,
                Height = 50,
                Opacity = 1,    // непрозрачность
                Stroke = Brushes.Black,
                Margin = new System.Windows.Thickness(center.X - 25, center.Y - 25, bottom.X, bottom.Y),
                Tag = txt       // надпись получим через tag       
            };

            elps.Fill = br;

            return elps;
        }

        public static void ChangeEllipse(Ellipse ellipse, Brush brush, string txt = "")
        {
            TextBlock text = new TextBlock
            {
                Text = txt,
                Background = brush,
                Width = ellipse.Width - 10,
                Height = ellipse.Height - 10,
                TextAlignment = System.Windows.TextAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            BitmapCacheBrush br = new BitmapCacheBrush(text);

            ellipse.Opacity = 1;
            ellipse.Tag = txt;
            ellipse.Fill = br;
        }
    }

}
