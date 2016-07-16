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
            Code.NotNull(field, "field");

            return GetEmbeddedFields
                (
                    field.SubFields,
                    DefaultCode
                );
        }

        /// <summary>
        /// Get embedded fields.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static RecordField[] GetEmbeddedFields
            (
                [NotNull] IEnumerable<SubField> subfields,
                char sign
            )
        {
            Code.NotNull(subfields, "subfields");

            List<RecordField> result = new List<RecordField>();

            RecordField found = null;

            foreach (SubField subField in subfields)
            {
                if (subField.Code == sign)
                {
                    if (found != null)
                    {
                        result.Add(found);
                    }
                    string value = subField.Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new FormatException();
                    }
                    string tag = value.Substring
                        (
                            0,
                            3
                        );
                    found = new RecordField(tag);
                    if (tag.StartsWith("00")
                        && (value.Length > 3)
                       )
                    {
                        found.Value = value.Substring(3);
                    }
                }
                else
                {
                    if (found != null)
                    {
                        found.AddSubField
                            (
                                subField.Code,
                                subField.Value
                            );
                    }
                }
            }

            if (found != null)
            {
                result.Add(found);
            }

            return result.ToArray();
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
            Code.NotNull(field, "field");
            Code.NotNullNorEmpty(tag, "tag");

            RecordField[] result = GetEmbeddedFields
                (
                    field.SubFields,
                    DefaultCode
                )
                .GetField(tag);

            return result;
        }

        #endregion
    }
}
