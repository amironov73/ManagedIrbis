/* PftGlobalManager.cs -- global variable manager
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
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
                PftGlobal variable;
                if (!Registry.TryGetValue(index, out variable))
                {
                    variable = new PftGlobal(index);
                    Registry.Add(index, variable);
                }
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

            return new RecordField[0];
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

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");


        }

        /// <inheritdoc/>
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");


        }

        #endregion

        #region Object members

        #endregion
    }
}
