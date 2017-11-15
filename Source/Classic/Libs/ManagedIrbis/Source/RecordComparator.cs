// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordComparator.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

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

            private int Tag { get; set; }

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
                Tag = field.Tag;
                Text = field.ToText();
            }

            #endregion

            #region IEquatable members

            public bool Equals
                (
                    OneField other
                )
            {
                other = other.ThrowIfNull();
                return Tag == other.Tag
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

        /// <summary>
        /// Find difference.
        /// </summary>
        [NotNull]
        public static FieldDifference[] FindDifference2
            (
                [NotNull] MarcRecord newRecord,
                [NotNull] MarcRecord oldRecord,
                [NotNull] int[] residuaryTags
            )
        {
            Code.NotNull(newRecord, "newRecord");
            Code.NotNull(oldRecord, "oldRecord");

            List<FieldDifference> result = new List<FieldDifference>();

            int[] tags = newRecord.Fields
                .Select(field => field.Tag)
                .Where(tag => !tag.OneOf(residuaryTags))
                .Distinct()
                .OrderBy(tag => tag)
                .ToArray();

            foreach (int tag in tags)
            {
                int count = newRecord.Fields.GetFieldCount(tag);
                for (int occ = 0; occ < count; occ++)
                {
                    RecordField firstField = newRecord.Fields.GetField(tag, occ)
                        .ThrowIfNull("firstField");
                    RecordField secondField = oldRecord.Fields.GetField(tag, occ);
                    FieldDifference diff = new FieldDifference
                    {
                        Tag = tag,
                        Repeat = occ + 1,
                        NewValue = firstField.ToText()
                    };
                    result.Add(diff);
                    if (ReferenceEquals(secondField, null))
                    {
                        diff.State = FieldState.Added;
                    }
                    else
                    {
                        secondField.UserData = true;
                        diff.OldValue = secondField.ToText();
                        diff.State = FieldState.Unchanged;
                        if (string.CompareOrdinal(diff.NewValue, diff.OldValue) != 0)
                        {
                            diff.State = FieldState.Edited;
                        }
                    }
                }
            }

            RecordField[] oldOnly = oldRecord.Fields
                .Where(field => !field.Tag.OneOf(residuaryTags))
                .Where(field => ReferenceEquals(field.UserData, null))
                .ToArray();
            foreach (RecordField field in oldOnly)
            {
                FieldDifference diff = new FieldDifference
                {
                    Tag = field.Tag,
                    Repeat = 0,
                    State = FieldState.Removed,
                    OldValue = field.ToText()
                };
                result.Add(diff);
            }

            return result.ToArray();
        }

        #endregion
    }
}
