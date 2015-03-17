using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Spintervals
{
    public class Interval
    {
        private Color _color;
        /// <summary>
        /// Color for the physical display
        /// </summary>
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }


        private int _songPosition;

        /// <summary>
        /// Song position in seconds
        /// </summary>
        public int SongPosition
        {
            get
            {
                return _songPosition;
            }
            set
            {
                _songPosition = value;
                
            }
        }

        private string _songPositionString;
        public string SongPositionString
        {
            get
            {
                int min = _songPosition / 60;
                int sec = _songPosition % 60;
                _songPositionString = "[" + min.ToString("0") + ":" + sec.ToString("00") + "]";
                return _songPositionString;
            }
        }   


        private string _intervalName;
        /// <summary>
        /// Name of the interval
        /// </summary>
        public string IntervalName
        {
            get
            {
                return _intervalName;
            }
            set
            {
                _intervalName = value;
            }
        }

        private int _intensity;
        /// <summary>
        /// Intensity/resistance of workout
        /// </summary>
        public int Intensity
        {
            get
            {
                return _intensity;
            }
            set
            {
                _intensity = value;
            }
        }
    }
}
