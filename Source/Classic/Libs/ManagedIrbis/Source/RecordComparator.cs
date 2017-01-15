// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordComparator.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Compares two record in field-by-field manner.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class RecordComparator
    {
        #region Nested classes

        class OneField //-V3126
            : IEquatable<OneField>
        {
            #region Properties

            [NotNull]
            private string Tag { get; set; }

            [NotNull]
            private string Text { get; set; }

            [NotNull]
            public RecordField Field { get; private set; }

            public bool Marked { get; set; }

            #endregion

            #region Construction

            public OneField
                (
                    [NotNull] RecordField field
                )
            {
                Field = field;
                Tag = FieldTag.Normalize(field.Tag);
                Text = field.ToText();
            }

            #endregion

            #region IEquatable members

            public bool Equals
                (
                    OneField other
                )
            {
                if (ReferenceEquals(other, null))
                {
                    return false;
                }

                return string.CompareOrdinal(Tag, other.Tag) == 0
                       && string.CompareOrdinal(Text, other.Text) == 0;
            }

            #endregion
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Find difference between two records.
        /// </summary>
        [NotNull]
        public static RecordDifferenceResult FindDifference
            (
                [NotNull] MarcRecord firstRecord,
                [NotNull] MarcRecord secondRecord
            )
        {
            Code.NotNull(firstRecord, "firstRecord");
            Code.NotNull(secondRecord, "secondRecord");

            OneField[] firstList = firstRecord.Fields.Select
                (
                    field => new OneField(field)
                )
                .ToArray();
            OneField[] secondList = secondRecord.Fields.Select
                (
                    field => new OneField(field)
                )
                .ToArray();

            RecordDifferenceResult result
                = new RecordDifferenceResult();

            foreach (OneField first in firstList)
            {
                foreach (OneField second in secondList)
                {
                    if (first.Equals(second)
                        && !second.Marked)
                    {
                        first.Marked = true;
                        second.Marked = true;
                        result.Both.Add(second.Field);
                        break;
                    }
                }
            }

            result.FirstOnly.AddRange
                (
                    firstList
                        .Where(item => !item.Marked)
                        .Select(item => item.Field)
                        .ToArray()
                );

            result.SecondOnly.AddRange
                (
                    secondList
                        .Where(item => !item.Marked)
                        .Select(item => item.Field)
                        .ToArray()
                );

            return result;
        }

        #endregion
    }
}
