using System;
using System.Threading;
using System.Threading.Tasks;

namespace SensorKitSDK
{
    public delegate void TimerCallback(object state);

    public sealed class Timer : CancellationTokenSource, IDisposable
    {
        public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
        {
            Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
            {
                var tuple = (Tuple<TimerCallback, object>)s;

                while (true)
                {
                    if (IsCancellationRequested)
                        break;
                    Task.Run(() => tuple.Item1(tuple.Item2));
                    await Task.Delay(period);
                }

            }, Tuple.Create(callback, state), CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        public new void Dispose() { base.Cancel(); }
    }


    /// <summary>
    /// TODO: Lack of Timer in PCL .NET assembles 
    /// </summary>
    public class TimerHelper : IDisposable
    {
        private Timer _timer;
        private TimeSpan _interval;

        /// <summary>
        /// Interval between signals in milliseconds.
        /// </summary>
        public double Interval
        {
            get { return _interval.TotalMilliseconds; }
            set { _interval = TimeSpan.FromMilliseconds(value); }
        }

        /// <summary>
        /// True if PCLTimer is running, false if not.
        /// </summary>
        public bool Enabled
        {
            get { return null != _timer; }
            set { if (value) Start(); else Stop(); }
        }

        /// <summary>
        /// Occurs when the specified time has elapsed and the PCLTimer is enabled.
        /// </summary>
        public event EventHandler Elapsed;

        /// <summary>
        /// Starts the PCLTimer.
        /// </summary>
        public void Start()
        {
            if (0 == _interval.TotalMilliseconds)
                throw new InvalidOperationException("Set Elapsed property before calling PCLTimer.Start().");
            _timer = new Timer(OnElapsed, null, _interval, _interval);
        }

        /// <summary>
        /// Stops the PCLTimer.
        /// </summary>
        public void Stop()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        /// <summary>
        /// Releases all resources.
        /// </summary>
        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        /// <summary>
        /// Invokes Elapsed event.
        /// </summary>
        /// <param name="state"></param>
        private void OnElapsed(object state)
        {
            if (null != _timer && null != Elapsed)
                Elapsed(this, EventArgs.Empty);
        }

    }
}

