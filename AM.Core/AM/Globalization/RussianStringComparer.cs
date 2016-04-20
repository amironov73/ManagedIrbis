/* RussianStringComparer.cs -- 
 * Ars Magna project, https://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;

#endregion

namespace AM.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class RussianStringComparer : StringComparer
    {
        #region Properties

        private bool _considerYo;

        ///<summary>
        ///
        ///</summary>
        public bool ConsiderYo
        {
            [DebuggerStepThrough]
            get
            {
                return _considerYo;
            }
        }

        private bool _ignoreCase;

        ///<summary>
        ///
        ///</summary>
        public bool IgnoreCase
        {
            [DebuggerStepThrough]
            get
            {
                return _ignoreCase;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="RussianStringComparer"/>
        /// class.
        /// </summary>
        /// <param name="considerYo">if set to <c>true</c> 
        /// [consider yo].</param>
        /// <param name="ignoreCase">if set to <c>true</c>
        /// [ignore case].</param>
        public RussianStringComparer
            (
            bool considerYo,
            bool ignoreCase )
        {
            _considerYo = considerYo;
            _ignoreCase = ignoreCase;

            CultureInfo russianCulture = BuiltinCultures.Russian;
            _innerComparer = Create
                (
                 russianCulture,
                 ignoreCase );
        }

        #endregion

        #region Private members

        private StringComparer _innerComparer;

        private string _Replace ( string str )
        {
            if ( ConsiderYo )
            {
                return ( str == null )
                           ? str
                           : str.Replace
                                 (
                                  'ё',
                                  'е' )
                                .Replace
                                 (
                                  'Ё',
                                  'Е' );
            }
            return str;
        }

        #endregion

        ///<summary>
        /// When overridden in a derived class, compares two strings 
        /// and returns an indication of their relative sort order.
        ///</summary>
        ///<returns>
        /// ValueMeaningLess than zero x is less than y.
        /// -or- x is null. Zerox is equal to y. Greater than zerox is greater than y.
        /// -or- y is null.
        ///</returns>
        ///<param name="y">A string to compare to x.</param>
        ///<param name="x">A string to compare to y.</param>
        public override int Compare
            (
            string x,
            string y )
        {
            string xCopy = _Replace ( x );
            string yCopy = _Replace ( y );
            return _innerComparer.Compare
                (
                 xCopy,
                 yCopy );
        }

        ///<summary>
        /// When overridden in a derived class, indicates whether two strings 
        /// are equal.
        ///</summary>
        ///<returns>
        /// true if x and y refer to the same object, or x and y are equal; otherwise, 
        /// false.
        ///</returns>
        ///<param name="y">A string to compare to x.</param>
        ///<param name="x">A string to compare to y.</param>
        public override bool Equals
            (
            string x,
            string y )
        {
            string xCopy = _Replace ( x );
            string yCopy = _Replace ( y );
            return _innerComparer.Equals
                (
                 xCopy,
                 yCopy );
        }

        ///<summary>
        /// When overridden in a derived class, gets the hash code 
        /// for the specified string.
        ///</summary>
        ///<returns>
        /// A 32-bit signed hash code calculated from the value of the obj 
        /// parameter.
        ///</returns>
        ///<param name="obj">A string.</param>
        public override int GetHashCode ( string obj )
        {
            string objCopy = _Replace ( obj );
            return _innerComparer.GetHashCode ( objCopy );
        }
    }
}
