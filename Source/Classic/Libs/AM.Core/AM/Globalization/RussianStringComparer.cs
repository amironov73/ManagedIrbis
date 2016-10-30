/* RussianStringComparer.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class RussianStringComparer
        : StringComparer
    {
        #region Properties

        ///<summary>
        /// Consider YO letter?
        ///</summary>
        public bool ConsiderYo { get; private set; }

        ///<summary>
        /// Ignore case?
        ///</summary>
        public bool IgnoreCase { get; private set; }

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
                bool ignoreCase
            )
        {
            ConsiderYo = considerYo;
            IgnoreCase = ignoreCase;

            CultureInfo russianCulture = BuiltinCultures.Russian;

            //_innerComparer = Create
            //    (
            //        russianCulture,
            //        ignoreCase
            //     );

            CompareOptions options = ignoreCase
                ? CompareOptions.IgnoreCase
                : CompareOptions.None;

            _innerComparer = (left, right)
                => russianCulture.CompareInfo.Compare
                    (
                        left,
                        right,
                        options
                    );
        }

        #endregion

        #region Private members

        private Func<string, string, int> _innerComparer;

        private string _Replace
            (
                string str
            )
        {
            if (ReferenceEquals(str, null))
            {
                return null;
            }

            if (ConsiderYo)
            {
                str = str
                    .Replace ( 'ё', 'е' )
                    .Replace ( 'Ё', 'Е' );
            }

            return str;
        }

        #endregion

        ///<inheritdoc/>
        public override int Compare
            (
                string x,
                string y
            )
        {
            string xCopy = _Replace(x);
            string yCopy = _Replace(y);
            
            return _innerComparer
                (
                    xCopy,
                    yCopy
                 );
        }

        ///<inheritdoc/>
        public override bool Equals
            (
                string x,
                string y
            )
        {
            string xCopy = _Replace(x);
            string yCopy = _Replace(y);
            
            return _innerComparer
                (
                    xCopy,
                    yCopy
                 ) == 0;
        }

        ///<inheritdoc/>
        public override int GetHashCode
            (
                string obj
            )
        {
            string objCopy = _Replace(obj);

            if (IgnoreCase
                && !ReferenceEquals(objCopy, null))
            {
                objCopy = objCopy.ToUpper();
            }

            return ReferenceEquals(objCopy, null)
                ? 0
                : objCopy.GetHashCode();
        }
    }
}
