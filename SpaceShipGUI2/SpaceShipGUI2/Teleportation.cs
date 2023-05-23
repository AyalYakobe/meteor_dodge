using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace SpaceShipGUI2
{
    class Teleportation : MovingObject
    {
        private const string blueLight = "ms-appx:///Assets/GamePics/BlueLight.png";

        public Teleportation(double x, double y)
            : base(x, y)
        {
            Img.Source = new BitmapImage(new Uri(blueLight));
            Height = 25;
            Width = 25;
        }
    }
}
