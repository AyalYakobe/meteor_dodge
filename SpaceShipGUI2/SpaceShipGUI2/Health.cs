using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace SpaceShipGUI2
{
    class Health : MovingObject
    {
        private const string bar = "ms-appx:///Assets/GamePics/arcadeLife.jpg";

        public Health(double x, double y)
            : base(x, y)
        {
            Img.Source = new BitmapImage(new Uri(bar));
            Height = 25;
            Width = 25;
        }
    }
}
