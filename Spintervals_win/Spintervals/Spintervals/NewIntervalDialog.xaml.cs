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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spintervals
{
    public sealed partial class NewIntervalDialog : UserControl
    {
        private string _intervalName;
        public string IntervalName
        {
            get
            {
                _intervalName = IntervalNameTextBox.Text;
                return _intervalName;
            }
            set
            {
                _intervalName = value;
            }
        }

        private Color _color;
        public Color Color
        {
            get
            {
                _color = _getColor();
                return _color;
            }
            set
            {
                _color = Color;
            }
        }

        private int _intensity;
        public int Intensity
        {
            get
            {
                _intensity = (int)IntensitySlider.Value;
                return _intensity;
            }
            set
            {
                _intensity = value;
            }
        }

        public NewIntervalDialog()
        {
            this.InitializeComponent();
        }

        public void SetCancelButtonClick(RoutedEventHandler e)
        {
            CancelButton.Click += e;
        }

        public void SetSaveButtonClick(RoutedEventHandler e)
        {
            SaveButton.Click += e;
        }

        public void ClearData()
        {
            IntervalNameTextBox.Text = "";
        }

        private Color _getColor()
        {
            switch (ColorComboBox.SelectedIndex)
            {
                case 0:
                    return Colors.White;
                case 1:
                    return Colors.Red;
                case 2:
                    return Colors.Lime;
                case 3:
                    return Colors.Blue;
                case 4:
                    return Colors.Yellow;
                default:
                    return Colors.White;
            }
        }

        private void IntensitySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (IntensityTextBlock != null)
                IntensityTextBlock.Text = "Intensity: " + IntensitySlider.Value;
        }
    }
}
