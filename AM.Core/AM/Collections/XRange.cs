/* XRange.cs -- 
 * Ars Magna project, http://arsmagna.ru
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
    [Serializable]
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
        /// Initializes a new instance of the <see cref="XRange"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        public XRange(T length)
            : this(0, length)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XRange"/> class.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        public XRange(T start, T length)
        {
            _start = start;
            _length = length;
        }

        #endregion

        #region IEnumerable<T> members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/>
        /// that can be used to iterate through the collection.
        ///</returns>
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            for (T i = 0; i < Length; i++)
            {
                yield return (Start + i);
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/>
        /// object that can be used to iterate through the collection.
        ///</returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion
    }
}