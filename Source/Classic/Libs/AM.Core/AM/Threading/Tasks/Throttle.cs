// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Throttle .cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if FW45

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

namespace AM.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Borrowed from Tom DuPont:
    /// http://www.tomdupont.net/2015/02/await-interval-with-throttle-class-in.html
    /// </remarks>
    public class Throttle
        : IThrottle
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Throttle
            (
                TimeSpan interval
            )
        {
            _interval = interval;
            _nextTime = DateTime.Now.Subtract(interval);
        }

        #endregion

        #region Private members

        private readonly object _lock = new object();

        private readonly TimeSpan _interval;

        private DateTime _nextTime;

        #endregion

        #region Public methods

        /// <summary>
        /// Get next task.
        /// </summary>
        public Task GetNext()
        {
            TimeSpan delay;

            return GetNext(out delay);
        }

        /// <summary>
        /// Get next task.
        /// </summary>
        public Task GetNext
            (
                out TimeSpan delay
            )
        {
            lock (_lock)
            {
                var now = DateTime.Now;

                _nextTime = _nextTime.Add(_interval);

                if (_nextTime > now)
                {
                    delay = _nextTime - now;
                    return Task.Delay(delay);
                }

                _nextTime = now;

                delay = TimeSpan.Zero;
                return Task.FromResult(true);
            }
        }

        #endregion
    }
}

#endif
