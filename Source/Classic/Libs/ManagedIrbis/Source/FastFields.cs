// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FastFields.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FastFields
    {
        #region Constants

        /// <summary>
        /// Capacity.
        /// </summary>
        public const int Capacity = 1024;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FastFields
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            _fields = new RecordField[Capacity][];
            List<RecordField>[] accumulator = new List<RecordField>[Capacity];
            foreach (RecordField field in fields)
            {
                int tag = field.Tag;
                int index = tag <= 0 || tag >= Capacity
                    ? 0
                    : tag;
                if (ReferenceEquals(accumulator[index], null))
                {
                    accumulator[index] = new List<RecordField>();
                }
                accumulator[index].Add(field);
            }

            for (int i = 0; i < Capacity; i++)
            {
                _fields[i] = ReferenceEquals(accumulator[i], null)
                    ? null
                    : accumulator[i].ToArray();
            }
        }

        #endregion

        #region Private members

        private readonly RecordField[][] _fields;

        #endregion

        #region Public methods

        /// <summary>
        /// Get fields.
        /// </summary>
        [CanBeNull]
        public RecordField[] GetFields
            (
                int tag
            )
        {
            if (tag <= 0)
            {
                return null;
            }

            if (tag >= Capacity)
            {
                RecordField[] array = _fields[0];
                if (ReferenceEquals(array, null))
                {
                    return null;
                }

                List<RecordField> result = null;
                foreach (RecordField field in array)
                {
                    if (field.Tag == tag)
                    {
                        if (ReferenceEquals(result, null))
                        {
                            result = new List<RecordField>();
                        }
                        result.Add(field);
                    }
                }
                return ReferenceEquals(result, null)
                    ? null
                    : result.ToArray();
            }

            return _fields[tag];
        }

        /// <summary>
        /// Get field.
        /// </summary>
        [CanBeNull]
        public RecordField GetField
            (
                int tag,
                int occurrence
            )
        {
            RecordField[] array = GetFields(tag);
            if (ReferenceEquals(array, null))
            {
                return null;
            }

            if (occurrence < 0 || occurrence >= array.Length)
            {
                return null;
            }

            return array[occurrence];
        }

        #endregion
    }
}
