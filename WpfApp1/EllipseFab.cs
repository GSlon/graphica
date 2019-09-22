using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApp1
{
    class EllipseFab
    {
        private Ellipse elps;
        private BitmapCacheBrush br;
        private TextBlock text;

        public EllipseFab(double x1, double y1, double x2, double y2, string txt = "")
        {
            text = new TextBlock
            {
                Text = txt,
                Background = Brushes.Red,
                Width = 40,
                Height = 40,
                TextAlignment = System.Windows.TextAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };

            br = new BitmapCacheBrush(text);

            elps = new Ellipse
            {
                Width = 50,
                Height = 50,
                Opacity = 1,
                Stroke = Brushes.Black,
                Margin = new System.Windows.Thickness(x1, y1, x2, y2)
            };

            elps.Fill = br;
        }

        //EllipseFab(string text, Int32 x, Int32 y, Brushes brush)  //на случай закраски другим цветом
        //{

        //}

        public Ellipse GetEllipse()
        {
            return elps;
        }
    }

}
