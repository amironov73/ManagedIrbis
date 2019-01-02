// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* EmbeddedField.cs -- работа со встроенными полями
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using UnsafeAM;
using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeIrbis
{
    /// <summary>
    /// Работа со встроенными полями.
    /// </summary>
    [PublicAPI]
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
            Code.NotNull(field, nameof(field));

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
            Code.NotNull(subfields, nameof(subfields));

            List<RecordField> result = new List<RecordField>();

            RecordField found = null;

            foreach (SubField subField in subfields)
            {
                if (subField.Code == sign)
                {
                    if (!ReferenceEquals(found, null))
                    {
                        result.Add(found);
                    }

                    string value = subField.Value;
                    if (string.IsNullOrEmpty(value))
                    {
                        Log.Error
                            (
                                "EmbeddedField::GetEmbeddedFields: "
                                + "bad format"
                            );

                        throw new FormatException();
                    }

                    int tag = FastNumber.ParseInt32(value.Substring(0, 3));
                    found = new RecordField(tag);
                    if (value.Length > 3)
                    {
                        found.Value = value.Substring(3);
                    }
                }
                else
                {
                    found?.AddSubField(subField.Code, subField.Value);
                }
            }

            if (!ReferenceEquals(found, null))
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
                int tag
            )
        {
            Code.NotNull(field, nameof(field));

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
