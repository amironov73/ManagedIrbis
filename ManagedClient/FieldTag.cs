/* FieldTag.cs -- тег поля
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
    /// Тег поля.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldTag
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
        public static string Normalize
            (
                [NotNull] string tag
            )
        {
            Code.NotNullNorEmpty(tag, "tag");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Валидация с бросанием исключений.
        /// </summary>
        public static bool Validate
            (
                [NotNull] string tag,
                bool throwException
            )
        {
            Code.NotNullNorEmpty(tag, "tag");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        public static bool Validate
            (
                [NotNull] string tag
            )
        {
            return Validate
                (
                    tag,
                    ThrowOnValidate
                );
        }

        #endregion
    }
}
