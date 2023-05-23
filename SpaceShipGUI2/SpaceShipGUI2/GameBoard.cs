using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace SpaceShipGUI2
{
    public class GameBoard
    {
        Image imageTemp;
        MediaPlayer mp3;
        Teleportation teleportation;
        Enemy enemy;
        Health bar1, bar2, bar3;
        Random _rnd;
        private Canvas _board;
        private Player _player;
        public List<Enemy> _enemies;    
        private DispatcherTimer _timer;
        public int count = 0;
        int internalCount = 0;     
        public bool endGame;
        public bool _isGameRunning;            
        
        public GameBoard(Canvas cnvs)
        {
            imageTemp = new Image();
            _rnd = new Random();
            _board = cnvs;
            enemy = new Enemy(40, 40);
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            _enemies = new List<Enemy>();
            mp3 = new MediaPlayer();
            _player = new Player(0,0);
        }

        public void StartTimer()
        {
            _timer.Start();
            _isGameRunning = true;
        }

        private void _timer_Tick(object sender, object e)
        {
            MoveEnemies();
        }

        public void MovePlayer(VirtualKey key)
        {
            if (!_isGameRunning || _player == null)
            {
                return;
            }
            if (_player.SpaceBarPressed > 0)
            {
                _board.Children.Remove(teleportation.Img);
            }                
            _player.Move(key, _board.ActualHeight, _board.ActualWidth);
        }

        private async void MoveEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].Move();
                if (CheckCollision(_enemies[i], _player))
                {
                    count++;
                    if (count == 1)
                    {                      
                        _enemies[i].Img.Source = enemy.Img2.Source;
                        await ExplodeRemove1();
                        _board.Children.Remove(_enemies[i].Img);
                        _enemies.Remove(_enemies[i]);                      
                    }
                    if (count == 2)
                    {
                        _enemies[i].Img.Source = enemy.Img2.Source;
                        await ExplodeRemove2();
                        _board.Children.Remove(_enemies[i].Img);
                        _enemies.Remove(_enemies[i]);                     
                    }
                    if (count == 3)
                    {
                        _enemies[i].Img.Source = enemy.Img2.Source;
                        _board.Children.Remove(_enemies[i].Img);
                        _enemies.Remove(_enemies[i]);
                        bar1.Img.Visibility = Visibility.Collapsed;
                        await SoundEffectExplosion();
                        GameOver(true);
                    }
                }
                for (int j = _enemies.Count - 1; j > i; j--)
                {
                    if (CheckCollision(_enemies[j], _enemies[i]))
                    {
                        imageTemp = _enemies[j].Img;
                        _enemies[j].Img = _enemies[i].Img;
                        _enemies[i].Img = imageTemp;
                        break;
                    }
                }
            }
        }

        public void SetUpGame()
        {
            CleanUp();            
            count = 0;
            _player = new Player(_board.ActualWidth / 2, _board.ActualHeight / 2);
            _board.Children.Add(_player.Img);
            bar1 = new Health(125, -26);
            _board.Children.Add(bar1.Img);
            bar2 = new Health(160, -26);
            _board.Children.Add(bar2.Img);
            bar3 = new Health(195, -26);
            _board.Children.Add(bar3.Img);
            teleportation = new Teleportation(1000, -26);
            _board.Children.Add(teleportation.Img);
            for (int i = 0; i < 20; i++)
            {
                Enemy enemy = GetRandomEnemyLocation();
                _board.Children.Add(enemy.Img);
                _enemies.Add(enemy);
            }
        }

        private async Task ExplodeRemove1()
        {
            _timer.Stop();
            _isGameRunning = false;
            await SoundEffectExplosion();
            await Task.Delay(400);
            _isGameRunning = true;
            _timer.Start();
            bar3.Img.Visibility = Visibility.Collapsed;
        }

        private async Task ExplodeRemove2()
        {
            _timer.Stop();
            _isGameRunning = false;
            await SoundEffectExplosion();
            await Task.Delay(400);
            _isGameRunning = true;
            _timer.Start();
            bar2.Img.Visibility = Visibility.Collapsed;
        }

        private Enemy GetRandomEnemyLocation()
        {
            Enemy enemy;
            do
            {
                enemy = new Enemy(
                    _rnd.Next(0, (int)Math.Floor(_board.ActualWidth)),
                    _rnd.Next(0, (int)Math.Floor(_board.ActualHeight)));
            } while (!CheckMinDistFromAll(enemy));
            return enemy;
        }

        private bool CheckMinDistFromAll(MovingObject gamePiece)
        {
            if(!CheckMinDistFromSpecific(gamePiece, _player, 200))
            {
                return false;
            }
            foreach (Enemy enemy in _enemies)
            {
                if(!CheckMinDistFromSpecific (gamePiece,enemy,100))
                {
                    return false;
                }                
            }
            return true;
        }

        private bool CheckMinDistFromSpecific(MovingObject piece1, MovingObject piece2, double min)
        {
            double x = piece1.X - piece2.X;
            double y = piece1.Y - piece2.Y;
            double dist = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return dist > min;
        }

        private bool CheckCollision(MovingObject piece1, MovingObject piece2)
        {
            return !CheckMinDistFromSpecific(piece1, piece2, (((piece1.Width + piece2.Width) / 2) + ((piece1.Height + piece2.Height) / 2)) / 2);
        }

        private void CleanUp()
        {
            _player = null;
            _enemies.Clear();
            _board.Children.Clear();
            _timer.Stop();
            internalCount = 0;
        }

        public void GameOver (bool win)
        {
            _timer.Stop();
            _isGameRunning = false;
            endGame = true;
            _player.DestroyPlayer();
        }

        public void Pause()
        {
            internalCount++;
            _timer.Stop();
            _isGameRunning = false;
            if (internalCount % 2 == 0)
            {
                _isGameRunning = true;
                _timer.Start();
            }
        }

        public async Task SoundEffectExplosion()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Explosion sonido efecto.mp3");
            mp3.AutoPlay = false;
            mp3.Source = MediaSource.CreateFromStorageFile(file);
            mp3.Play();
        }
    }
}
