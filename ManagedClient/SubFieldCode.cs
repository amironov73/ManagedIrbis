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
        /// Бросать исключения при валидации?
        /// </summary>
        public static bool ThrowOnValidate { get; set; }

        #endregion

        #region Private members

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Валидация с бросанием исключений.
        /// </summary>
        public static bool Validate
            (
                char code,
                bool throwException
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        public static bool Validate
            (
                char code
            )
        {
            return Validate
                (
                    code,
                    ThrowOnValidate
                );
        }

        #endregion
    }
}
