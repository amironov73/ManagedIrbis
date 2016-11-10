/* Throttle .cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
        private readonly object _lock = new object();

        private readonly TimeSpan _interval;

        private DateTime _nextTime;

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

        public Task GetNext()
        {
            TimeSpan delay;
            return GetNext(out delay);
        }

        public Task GetNext(out TimeSpan delay)
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
    }
}
