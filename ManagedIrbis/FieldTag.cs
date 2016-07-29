/* FieldTag.cs -- тег поля
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
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
                [CanBeNull] string tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return tag;
            }

            string result = tag
                .ToUpper()
                .TrimStart('0');

            return result;
        }

        /// <summary>
        /// Валидация с бросанием исключений.
        /// </summary>
        public static bool Verify
            (
                [NotNull] string tag,
                bool throwException
            )
        {
            Code.NotNullNorEmpty(tag, "tag");

            return true;
        }

        /// <summary>
        /// Валидация.
        /// </summary>
        public static bool Verify
            (
                [NotNull] string tag
            )
        {
            return Verify
                (
                    tag,
                    ThrowOnValidate
                );
        }

        #endregion
    }
}
