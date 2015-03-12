using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Spintervals
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool _isPlaying = false;
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        /// <summary>
        /// Draws random lines to test canvases
        /// </summary>
        private void DrawRandomLines()
        {
            Random rand = new Random();
            for (int i = 1; i < (int)IntervalCanvas.ActualWidth; )
            {
                int h = rand.Next(1, (int)IntervalCanvas.ActualHeight);
                for (int j = 0; j < 4; j++)
                {
                    DrawRect(i, 2, h);
                    i += 3;
                }
            }
        }

        /// <summary>
        /// Draws a rectangle on the interval canvas
        /// </summary>
        /// <param name="x">horizontal position of rectangle</param>
        /// <param name="y">vertical position of rectanlge</param>
        /// <param name="w">width of rectangle</param>
        /// <param name="h">height of rectangle</param>
        private void DrawRect(int x, int w, int h)
        {
            Rectangle rect = new Rectangle();
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, IntervalCanvas.ActualHeight - h);
            rect.Width = w;
            rect.Height = h;
            rect.Fill = new SolidColorBrush(Colors.Green);

            IntervalCanvas.Children.Add(rect);
        }

        private void IntervalCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawRandomLines();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(!_isPlaying)
            {

                Image i = new Image();
                i.Source = new BitmapImage(new Uri("ms-appx:/Images/pause.png"));
                PlayButton.Content = i;

                _isPlaying = true;
                songMediaElement.Play();
            }
            else
            {
                Image i = new Image();
                i.Source = new BitmapImage(new Uri("ms-appx:/Images/play.png"));
                PlayButton.Content = i;
                _isPlaying = false;
                songMediaElement.Pause();
            }
            
        }
    }
}
