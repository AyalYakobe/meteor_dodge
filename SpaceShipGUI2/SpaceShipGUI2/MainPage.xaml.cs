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
using Windows.Media.Playback;
using Windows.Media.Core;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpaceShipGUI2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DispatcherTimer tmr; 
        public DateTime startTime;   
        MediaPlayer mp3;
        GameBoard game;
        int internalCount = 0;

        public MainPage()
        {          
            this.InitializeComponent();

            game = new GameBoard(canvas);

            mp3 = new MediaPlayer();
            InstaPlay();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            startTime = DateTime.Now;
            tmr = new DispatcherTimer();
            tmr.Interval = new TimeSpan(0, 0, 0, 0, 50);
            tmr.Tick += OnDurationHandler;           
            
            ImageBrush backGround = new ImageBrush();
            backGround.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/GamePics/arcadeSpace.jpg"));
            canvas.Background = backGround;
        }

        private async void InstaPlay()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("Passion Pit - Smile Upon Me (128 kbps).mp3");
            mp3.AutoPlay = true;
            mp3.Source = MediaSource.CreateFromStorageFile(file);
            mp3.Play();
        }

        private async void OnDurationHandler(object sender, object e)
        {
            TimeSpan Duration = DateTime.Now - startTime; 
            txtDuration.Text = Duration.TotalSeconds.ToString("N2");

            if (game._isGameRunning == false)
            {
                tmr.Stop();
                await Task.Delay(400);
                tmr.Start();
            }

            if(game._isGameRunning==false && game.count == 3)
            {
                tmr.Stop();
                PromptGameRestart();
            }
        }

        public async void PromptGameRestart()
        {
            MessageDialog dlg = new MessageDialog("Play again?", $"GAME OVER. YOU LASTED : {txtDuration.Text} SECONDS");
            UICommand yesCommand = new UICommand("Yes", RestartGameCommand);
            UICommand noCommand = new UICommand("No", CloseGameCommand);
            dlg.Commands.Add(yesCommand);
            dlg.Commands.Add(noCommand);
            await dlg.ShowAsync();
        }

        private void CloseGameCommand(IUICommand command)
        {
            Application.Current.Exit();
        }

        public void RestartGameCommand(IUICommand command)
        {
            TextBlock textBlock = txtDuration;
            game.SetUpGame();
            game.StartTimer();
            tmr.Start();
            startTime = DateTime.Now;
            canvas.Children.Add(textBlock);
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                case VirtualKey.Up:
                case VirtualKey.Right:
                case VirtualKey.Down:
                case VirtualKey.Space:
                    game.MovePlayer(args.VirtualKey);
                    break;
                default:
                    break;
            }
        }

        private async void PlayClicked(object sender, TappedRoutedEventArgs e)
        {
            TextBlock textBlock = txtDuration;
            game.SetUpGame();
            game.StartTimer();
            tmr.Start();
            startTime = DateTime.Now; 
            canvas.Children.Add(textBlock);

            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("passion-pit-little-secrets-8bit-cover-(youtube-mp3.site) (1).mp3");
            mp3.AutoPlay = true;
            mp3.Source = MediaSource.CreateFromStorageFile(file);
            mp3.Play();
        }

        private void PauseGame(object sender, TappedRoutedEventArgs e)
        {
            game.Pause();
            tmr.Stop();
            internalCount++;
            if (internalCount % 2 == 0)
            {
                game._isGameRunning = true;
                tmr.Start();
            }
        }
    }
}
        
           







   
      