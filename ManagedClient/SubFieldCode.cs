/* SubFieldCode.cs -- код подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Код подполя
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class SubFieldCode
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Бросать исключения при нормализации?
        /// </summary>
        public static bool ThrowOnNormalize { get; set; }

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        public static bool ThrowOnVerification { get; set; }

        /// <summary>
        /// List of valid codes.
        /// </summary>
        [NotNull]
        public static List<char> ValidCodes
        {
            get { return _validCodes; }
        }

        #endregion

        #region Construction

        static SubFieldCode()
        {
            _validCodes = new List<char>();
        }

        #endregion

        #region Private members

        private static readonly List<char> _validCodes;

        #endregion

        #region Public methods

        /// <summary>
        /// Нормализация.
        /// </summary>
        public static char Normalize
            (
                char code
            )
        {
            return Char.ToLowerInvariant(code);
        }

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                char code,
                bool throwException
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                char code
            )
        {
            return Verify
                (
                    code,
                    ThrowOnVerification
                );
        }

        #endregion
    }
}
