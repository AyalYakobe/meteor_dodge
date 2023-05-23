using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SpaceShipGUI2
{
    public class Player : MovingObject
    {
        private static BitmapImage _aliveUri = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/arcadeSpaceship.png"));
        private static BitmapImage _deadUri = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/arcadeExplosion.jpg"));
        private static BitmapImage _zap = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/BlueLight.png"));
        Random r;     
        MediaPlayer mp3;        
        public int SpaceBarPressed = 0;
        int _stepSize = 8;
        bool isEnabled = true;

        public Player(double x, double y)
            :base(x,y)
        {
            r = new Random();
            Img.Source = _aliveUri;
            mp3 = new MediaPlayer();
        }

        public async void Move(VirtualKey direction, double canvasHeight, double canvasWidth)
        {
            switch (direction)
            {
                case VirtualKey.Up:
                    this.Y -= this._stepSize;
                    if(this.Y < 0)
                    {
                        this.Y = canvasHeight;
                    }
                    break;
                case VirtualKey.Down:
                    this.Y += this._stepSize;
                    if(this.Y > canvasHeight)
                    {
                        this.Y = 0;
                    }
                    break;
                case VirtualKey.Right:
                    this.X += this._stepSize;
                    if (this.X > canvasWidth)
                    {
                        this.X = 0;
                    }
                    break;
                case VirtualKey.Left:
                    this.X -= this._stepSize;
                    if (this.X < 0)
                    {
                        this.X = canvasWidth;
                    }
                    break;
                case VirtualKey.Space:
                    if(isEnabled)
                    {
                        this.X = this.r.Next(1, (int)Math.Floor(canvasWidth));
                        this.Y = this.r.Next(1, (int)Math.Floor(canvasHeight));
                        SpaceBarPressed++;
                        await TeleportSpaceship();                                           
                    }                   
                    break;
                default:
                    break;
            }
        }

        public void DestroyPlayer()
        {
            Img.Source = _deadUri;
        }

        public async Task SoundEffectZap()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("zap sound effect.mp3");
            mp3.AutoPlay = false;
            mp3.Source = MediaSource.CreateFromStorageFile(file);
            mp3.Play();
        }

        public async Task TeleportSpaceship()
        {
            Img.Source = _zap;
            await SoundEffectZap();
            await Task.Delay(120);
            Img.Source = _aliveUri;
            isEnabled = false;

        }


        
    }
}