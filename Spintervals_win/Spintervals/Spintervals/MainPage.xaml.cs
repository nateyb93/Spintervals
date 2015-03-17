using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Notifications;
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

        private bool _wasTapped = true;

        private SongData _currentSong;

        private DispatcherTimer _timer;

        private DispatcherTimer _mediaPositionChangedTimer;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            ///Handle page load events here
            this.Loaded += (s, e) =>
            {
                _currentSong = new SongData();
                _currentSong.Path = "ms-appx:/Assets/Muse - Psycho.mp3";

                IntervalListView.ItemsSource = _currentSong.IntervalQueues;
                songMediaElement.Source = new Uri(_currentSong.Path);
                setDialogCancel();
                setDialogSave();
            };
        }

        /// <summary>
        /// Initializes the timer object
        /// </summary>
        private void _initTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
        }

        /// <summary>
        /// Initializes the timer that checks the position 
        /// </summary>
        private void _initMediaPositionChangedTimer()
        {
            _mediaPositionChangedTimer = new DispatcherTimer();
            _mediaPositionChangedTimer.Interval = TimeSpan.FromMilliseconds(200);
            _mediaPositionChangedTimer.Tick += _mediaPositionChangedTimer_Tick;
        }


        /// <summary>
        /// Event handler for timer's tick event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Tick(object sender, object e)
        {

        }

        private void sendToast(int index)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(_currentSong.IntervalQueues[index].IntervalName));

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }


        /// <summary>
        /// Tick event for positionChanged timer check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _mediaPositionChangedTimer_Tick(object sender, object e)
        {
            timelineSlider.Value = songMediaElement.Position.TotalSeconds;
            PositionTextBlock.Text =
                songMediaElement.Position.Minutes.ToString("0") + ":" + songMediaElement.Position.Seconds.ToString("00");

            int pos = (int)songMediaElement.Position.TotalSeconds;
            List<Interval> its = _currentSong.IntervalQueues;
            for (int i = 0; i < its.Count; i++)
            {
                if (pos == its[i].SongPosition)
                {
                    _wasTapped = false;
                    IntervalListView.SelectedIndex = i;
                    _wasTapped = true;
                }
            }
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

        private void _redraw()
        {
            IntervalCanvas.Children.Clear();
            int totalDuration = (int)songMediaElement.Position.TotalSeconds;
            int pos = 0;
            for (int i = 0; i < _currentSong.IntervalQueues.Count; i++)
            {
                Interval iv = _currentSong.IntervalQueues[i];
                if(i == _currentSong.IntervalQueues.Count - 1)
                {
                    int intensity = iv.Intensity;
                    
                    double h = IntervalCanvas.ActualHeight * ((double)intensity / 20.0);
                    double w = (int)(IntervalCanvas.ActualWidth - pos);
                    DrawRect(pos, w, h, iv.Color);
                    pos += (int)w;
                }

                else
                {
                    double xLim =
                        (double)_currentSong.IntervalQueues[i + 1].SongPosition / songMediaElement.NaturalDuration.TimeSpan.TotalSeconds
                        * IntervalCanvas.ActualWidth;

                    int intensity = iv.Intensity;
                    double h = IntervalCanvas.ActualHeight * ((double)intensity / 20);
                    double w = xLim - pos;
                    DrawRect(pos, w, h, iv.Color);

                    pos += (int)w;
                    
                }
                    
            }
        }

        /// <summary>
        /// Draws random lines to test canvases
        /// </summary>
        private void DrawRandomLines()
        {
            IntervalCanvas.Children.Clear();
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
        private void DrawRect(int x, double w, double h, Color c)
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
                PlayMedia();
            }
            else
            {
                PauseMedia();
            }
            
        }

        /// <summary>
        /// Handles all the actions tied to pausing the media
        /// </summary>
        private void PauseMedia()
        {
            Image i = new Image();
            i.Source = new BitmapImage(new Uri("ms-appx:/Images/play.png"));
            i.Stretch = Stretch.UniformToFill;
            i.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            PlayButton.Content = i;
            _isPlaying = false;
            songMediaElement.Pause();
            _timer.Stop();
            _mediaPositionChangedTimer.Stop();
        }

        /// <summary>
        /// Handles all the actions tied to playing the media
        /// </summary>
        private void PlayMedia()
        {
            Image i = new Image();
            i.Source = new BitmapImage(new Uri("ms-appx:/Images/pause.png"));
            i.Stretch = Stretch.UniformToFill;
            i.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            PlayButton.Content = i;
            _isPlaying = true;
            songMediaElement.Play();
            _timer.Start();
            _mediaPositionChangedTimer.Start();
        }

        /// <summary>
        /// Sets the action for the dialog's cancel button
        /// </summary>
        private void setDialogCancel()
        {
            DialogNewInterval.SetCancelButtonClick((s, e) =>
            {
                FlyoutNewInterval.Hide();
            });
        }

        /// <summary>
        /// Sets the action for the dialog's save button
        /// </summary>
        private void setDialogSave()
        {
            DialogNewInterval.SetSaveButtonClick((s, e) =>
            {
                //add interval with the information from the dialog
                _currentSong.AddInterval(new Interval()
                {
                    SongPosition = (int)songMediaElement.Position.TotalSeconds,
                    IntervalName = DialogNewInterval.IntervalName,
                    Color = DialogNewInterval.Color,
                    Intensity = DialogNewInterval.Intensity
                });

                _currentSong.IntervalQueues.Sort((it1, it2) => it1.SongPosition.CompareTo(it2.SongPosition));
                int idx = IntervalListView.SelectedIndex;
                NotifyDataChanged();
                IntervalListView.SelectedIndex = idx;
                _redraw();

                DialogNewInterval.ClearData();
                FlyoutNewInterval.Hide();
            });
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
            _initMediaPositionChangedTimer();
            DrawRandomLines();
        }

        private void AddIntervalButton_Click(object sender, RoutedEventArgs e)
        {
            DialogNewInterval.SelectIntervalName();
            PauseMedia();
        }

        private void IntervalListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IntervalListView.SelectedIndex == -1)
            {
                return;
            }

            Interval i = _currentSong.IntervalQueues[IntervalListView.SelectedIndex];
            if (!_wasTapped)
            {
                sendToast(IntervalListView.SelectedIndex);
            } else if (_wasTapped)
            {
                songMediaElement.Position = TimeSpan.FromSeconds(i.SongPosition);
            }
        }

        private void IntervalListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == Windows.UI.Input.HoldingState.Started)
            {
                MenuFlyout mf = new MenuFlyout();
                MenuFlyoutItem mfi = new MenuFlyoutItem {
                    Text = "Delete"
                };

                mfi.Click += (s, ev) =>
                {
                    TextBlock o = e.OriginalSource as TextBlock;
                    
                    Interval item = (Interval)(sender as FrameworkElement).DataContext;
                    _currentSong.RemoveInterval(item);
                    NotifyDataChanged();
                };

                mf.Items.Add(mfi);

                mf.ShowAt(IntervalListView);


            }
        }

        private void NotifyDataChanged()
        {
            IntervalListView.ItemsSource = null;
            IntervalListView.ItemsSource = _currentSong.IntervalQueues;
            _redraw();
        }

        /// <summary>
        /// Fired when the song media ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            timelineSlider.Value = 0.0;
            PauseMedia();
        }

    }
}
