// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* XRange.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

using T = System.Int32;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// foreach ( int i in new XRange ( 10, 50 ) )
    /// {
    ///  Console.WriteLine ( "Number: {0}", i );
    /// }
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class XRange
        : IEnumerable<T>
    {
        #region Properties

        private T _length;

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <value>The length.</value>
        public T Length
        {
            [DebuggerStepThrough]
            get
            {
                return _length;
            }
        }

        private T _start;

        /// <summary>
        /// Gets the start.
        /// </summary>
        /// <value>The start.</value>
        public T Start
        {
            [DebuggerStepThrough]
            get
            {
                return _start;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public XRange
            (
                T length
            )
            : this(0, length)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public XRange
            (
                T start,
                T length
            )
        {
            _start = start;
            _length = length;
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            for (T i = 0; i < Length; i++)
            {
                yield return Start + i;
            }
        }

        #endregion

        #region IEnumerable Members

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion
    }
}
