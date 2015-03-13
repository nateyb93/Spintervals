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

        private DispatcherTimer _timer;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

        }

        private void _initTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
        }

        private void _timer_Tick(object sender, object e)
        {
            timelineSlider.Value = songMediaElement.Position.TotalSeconds;
            PositionTextBlock.Text =
                songMediaElement.Position.Minutes.ToString("0") + ":" + songMediaElement.Position.Seconds.ToString("00");
        }

        /// <summary>
        /// Initializes the slider control with the 
        /// </summary>
        private void _initSlider()
        {
            timelineSlider.Maximum = songMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            timelineSlider.SmallChange = 1;
            timelineSlider.LargeChange = Math.Min(10, songMediaElement.NaturalDuration.TimeSpan.Seconds / 10);
            DurationTextBlock.Text = songMediaElement.NaturalDuration.TimeSpan.Minutes.ToString("0") + ":" + songMediaElement.NaturalDuration.TimeSpan.Seconds.ToString("00");
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
                for (int k = 0; k < 5; k++)
                {
                    Color c;

                    switch (k)
                    {
                        case 0:
                            c = Colors.White;
                            break;
                        case 1:
                            c = Colors.Red;
                            break;
                        case 2:
                            c = Colors.Lime;
                            break;
                        case 3:
                            c = Colors.Blue;
                            break;
                        case 4:
                            c = Colors.Yellow;
                            break;
                        default:
                            c = Colors.White;
                            break;
                    }

                    int h = rand.Next(1, (int)IntervalCanvas.ActualHeight);
                    for (int j = 0; j < (int)IntervalCanvas.ActualWidth / 5; j += 3)
                    {
                        if (i >= (int)IntervalCanvas.ActualWidth)
                            break;
                        
                        DrawRect(i, 2, h, c);
                        i += 3;
                    }
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
        private void DrawRect(int x, int w, int h, Color c)
        {
            Rectangle rect = new Rectangle();
            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, IntervalCanvas.ActualHeight - h);
            rect.Width = w;
            rect.Height = h;
            rect.Fill = new SolidColorBrush(c);

            IntervalCanvas.Children.Add(rect);
        }

        /// <summary>
        /// Called when the interval canvas has been initialized. We cannot draw any 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntervalCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        /// <summary>
        /// Called when the play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(!_isPlaying)
            {
                Image i = new Image();
                i.Source = new BitmapImage(new Uri("ms-appx:/Images/pause.png"));
                i.Stretch = Stretch.UniformToFill;
                i.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                PlayButton.Content = i;
                _isPlaying = true;
                songMediaElement.Play();
                _timer.Start();
            }
            else
            {
                Image i = new Image();
                i.Source = new BitmapImage(new Uri("ms-appx:/Images/play.png"));
                i.Stretch = Stretch.UniformToFill;
                i.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                PlayButton.Content = i;
                _isPlaying = false;
                songMediaElement.Pause();
                _timer.Stop();
            }
            
        }

        /// <summary>
        /// Called when the timelineSlider is dragged to a new position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timelineSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            songMediaElement.Position = TimeSpan.FromSeconds(timelineSlider.Value);
            PositionTextBlock.Text =
                songMediaElement.Position.Minutes.ToString("0") + ":" + songMediaElement.Position.Seconds.ToString("00");
        }

        /// <summary>
        /// Called when a song is opened by the media player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            _initSlider();
            _initTimer();
            DrawRandomLines();
        }
    }
}
