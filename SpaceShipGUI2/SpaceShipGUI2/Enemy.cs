using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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
using Timer = System.Timers.Timer;

namespace SpaceShipGUI2
{
    public class Enemy : MovingObject
    {
        private static BitmapImage _meteor = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/aracadeAsteroid.jpg"));
        public static BitmapImage _destroyedMeteor = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/arcadeExplosion.jpg"));
        Random r;
        private int STEP = 5;    
        private bool goRight = true;
        private bool goDown = true;
       
        public Enemy(double x, double y)
            : base(x, y)
        {
            r = new Random();
            Img.Source = _meteor;
            Img2.Source = _destroyedMeteor;
            Height = Width = r.Next(20, 90);
            int step = r.Next(1, 10);
            STEP = step;         
        }

        public void Move()
        {
            if (goRight)
            {
                if (Canvas.GetLeft(Img) + Img.Width + STEP >= 1290)
                {
                    goRight = false;
                }
            }
            else
            {
                if (Canvas.GetLeft(Img) <= 0)
                {
                    goRight = true;
                }
            }

            if (goDown)
            {
                if (Canvas.GetTop(Img) + Img.Height + STEP >= 630)
                {
                    goDown = false;
                }                 
            }
            else
            {
                if (Canvas.GetTop(Img) <= 0)
                {
                    goDown = true;
                }                 
            }

            if (goDown)
            {
                Canvas.SetTop(Img, Canvas.GetTop(Img) + STEP); 
            }
            else
            {
                Canvas.SetTop(Img, Canvas.GetTop(Img) - STEP); 
            }

            if (goRight)
            {
                Canvas.SetLeft(Img, Canvas.GetLeft(Img) + STEP);  
            }
            else
            {
                Canvas.SetLeft(Img, Canvas.GetLeft(Img) - STEP); 
            }
        }
    }
}
 