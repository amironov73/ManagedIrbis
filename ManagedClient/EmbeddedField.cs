/* EmbeddedField.cs -- работа со встроенными полями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Работа со встроенными полями.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class EmbeddedField
    {
        #region Constants

        /// <summary>
        /// Код по умолчанию, используемый для встраивания полей.
        /// </summary>
        public const char DefaultCode = '1';

        #endregion

        #region Public methods

        /// <summary>
        /// Получение встроенных полей.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetEmbeddedFields
            (
                [NotNull] this RecordField field
            )
        {
            Code.NotNull(() => field);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение встроенных полей с указанным тегом.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetEmbeddedField
            (
                [NotNull] this RecordField field,
                [NotNull] string tag
            )
        {
            Code.NotNull(() => field);
            Code.NotNullNorEmpty(() => tag);

            throw new NotImplementedException();
        }

        #endregion
    }
}
