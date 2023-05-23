using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SpaceShipGUI2
{
    public class MovingObject
    {
        public Image Img { get; set; }
        public Image Img2 { get; set; }

        public MovingObject(double x, double y)
        {
            Img = new Image();
            Img.Stretch = Stretch.Fill;
            Img2 = new Image();
            Img2.Stretch = Stretch.Fill;
            X = x;
            Y = y;
            Height = 50;
            Width = 50;
        }

        public double X
        {
            get
            {
                return Canvas.GetLeft(Img);
            }
            set
            {
                Canvas.SetLeft(Img, value);
            }
        }

        public double Y
        {
            get
            {
                return Canvas.GetTop(Img);
            }
            set
            {
                Canvas.SetTop(Img, value);
            }
        }

        public double Width
        {
            get
            {
                return Img.ActualWidth;
            }
            set
            {
                Img.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return Img.ActualHeight;
            }
            set
            {
                Img.Height = value;
            }
        }
    }
}
