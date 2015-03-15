using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spintervals
{
    public class SongData
    {
        private string _path;
        /// <summary>
        /// Path of the song object
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }


        private List<Interval> _intervalQueues;
        /// <summary>
        /// List of interval
        /// </summary>
        public List<Interval> IntervalQueues
        {
            get
            {
                return _intervalQueues;
            }
            set
            {
                _intervalQueues = value;
            }
        }

        public SongData()
        {
            _intervalQueues = new List<Interval>();
        }

        /// <summary>
        /// Adds an interval to the list of intervals
        /// </summary>
        /// <param name="interval"></param>
        public void AddInterval(Interval interval)
        {
            _intervalQueues.Add(interval);
        }

        /// <summary>
        /// Removes an interval from the list of intervals
        /// </summary>
        /// <param name="interval"></param>
        public void RemoveInterval(Interval interval)
        {
            _intervalQueues.Remove(interval);
        }

        /// <summary>
        /// Removes an interval from the list of intervals
        /// </summary>
        /// <param name="intervalName"></param>
        public void RemoveInterval(string intervalName)
        {
            Interval found = _intervalQueues.Find(x => x.IntervalName == intervalName);
            if (found != null)
                _intervalQueues.Remove(found);
        }
    }
}
