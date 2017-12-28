// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftGlobalManager.cs -- global variable manager
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftGlobalManager
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Dictionary holding all the global variables.
        /// </summary>
        [NotNull]
        public Dictionary<int, PftGlobal> Registry { get; private set; }

        /// <summary>
        /// Получение значения глобальной переменной по её индексу
        /// в строковом представлении. Если такой переменной нет,
        /// возвращается пустая строка.
        /// </summary>
        public string this[int index]
        {
            [NotNull]
            get
            {
                Code.Positive(index, "index");

                PftGlobal result;

                return Registry.TryGetValue(index, out result)
                    ? result.ToString()
                    : string.Empty;
            }
            set
            {
                Code.Positive(index, "index");

                if (ReferenceEquals(value, null))
                {
                    Registry.Remove(index);
                }
                else
                {
                    PftGlobal variable = new PftGlobal
                        (
                            index,
                            value
                        );
                    Registry[index] = variable;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobalManager()
        {
            Registry = new Dictionary<int, PftGlobal>();
        }

        #endregion

        #region Private members

        private PftGlobal _GetOrCreate
            (
                int index
            )
        {
            PftGlobal result;
            if (!Registry.TryGetValue(index, out result))
            {
                result = new PftGlobal(index);
                Registry.Add(index, result);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the variable.
        /// </summary>
        [NotNull]
        public PftGlobalManager Add
            (
                int index,
                [CanBeNull] string text
            )
        {
            Code.Positive(index, "index");

            this[index] = text;

            return this;
        }

        /// <summary>
        /// Append the variable.
        /// </summary>
        [NotNull]
        public PftGlobalManager Append
            (
                int index,
                [CanBeNull] string text
            )
        {
            Code.Positive(index, "index");

            if (!string.IsNullOrEmpty(text))
            {
                PftGlobal variable = _GetOrCreate(index);
                string[] lines = text.SplitLines()
                    .NonEmptyLines()
                    .ToArray();
                foreach (string line in lines)
                {
                    variable.ParseLine(line);
                }
            }

            return this;
        }

        /// <summary>
        /// Clear all the variables.
        /// </summary>
        [NotNull]
        public PftGlobalManager Clear()
        {
            Registry.Clear();

            return this;
        }

        /// <summary>
        /// Delete global variable with specified index.
        /// </summary>
        [NotNull]
        public PftGlobalManager Delete
            (
                int index
            )
        {
            Code.Positive(index, "index");

            Registry.Remove(index);

            return this;
        }

        /// <summary>
        /// Get fields for global variable with specified index.
        /// </summary>
        [NotNull]
        public RecordField[] Get
            (
                int index
            )
        {
            Code.Positive(index, "index");

            PftGlobal variable;
            if (Registry.TryGetValue(index, out variable))
            {
                return variable.Fields
                    .Select(f => f.Clone())
                    .ToArray();
            }

            return RecordFieldUtility.EmptyArray;
        }

        /// <summary>
        /// Get all variables.
        /// </summary>
        [NotNull]
        public PftGlobal[] GetAllVariables()
        {
            PftGlobal[] result = Registry.Values.ToArray();

            return result;
        }

        /// <summary>
        /// Have global variable with specified index?
        /// </summary>
        public bool HaveVariable
            (
                int index
            )
        {
            return Registry.ContainsKey(index);
        }

        /// <summary>
        /// Set the global variable.
        /// </summary>
        public void Set
            (
                int index,
                [CanBeNull] IEnumerable<RecordField> fields
            )
        {
            Code.Positive(index, "index");

            if (ReferenceEquals(fields, null))
            {
                Delete(index);

                return;
            }

            RecordField[] array = fields.ToArray();
            if (array.Length == 0)
            {
                Delete(index);

                return;
            }

            PftGlobal variable = _GetOrCreate(index);
            variable.Fields.Clear();
            variable.Fields.AddRange(array);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Clear();
            PftGlobal[] values = reader.ReadArray<PftGlobal>();
            foreach (PftGlobal value in values)
            {
                Registry.Add(value.Number, value);
            }
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            PftGlobal[] values = GetAllVariables();
            writer.WriteArray(values);
        }

        #endregion
    }
}

