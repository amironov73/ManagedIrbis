// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftGlobal.cs -- global variable
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
    /// Global variable.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !PocketPC
    [DebuggerDisplay("{Number} {ToString()}")]
#endif
    public sealed class PftGlobal
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Number of the variable.
        /// </summary>
        public int Number
        {
            get { return _number; }
            set
            {
                Code.Positive(value, "value");

                _number = value;
            }
        }

        /// <summary>
        /// Fields.
        /// </summary>
        [NotNull]
        public NonNullCollection<RecordField> Fields { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal()
        {
            Fields = new NonNullCollection<RecordField>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal
            (
                int number
            )
            : this()
        {
            Number = number;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGlobal
            (
                int number,
                string text
            )
            : this()
        {
            Number = number;
            Parse(text);
        }

        #endregion

        #region Private members

        private int _number;

        private static string _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            StringBuilder result = new StringBuilder();

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                char c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse (possibly multiline) text.
        /// </summary>
        [NotNull]
        public PftGlobal Parse
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {

                string[] lines = text.SplitLines();
                foreach (string line in lines)
                {
                    ParseLine(line);
                }

            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        [NotNull]
        public PftGlobal ParseLine
            (
                [NotNull] string line
            )
        {
            Code.NotNull(line, "line");

            StringReader reader = new StringReader(line);
            RecordField field = new RecordField(Number.ToInvariantString());
            Fields.Add(field);
            field.Value = _ReadTo(reader, '^');

            while (true)
            {
                int next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                char code = char.ToLower((char)next);
                string text = _ReadTo(reader, '^');
                SubField subField = new SubField
                {
                    Code = code,
                    Value = text
                };
                field.SubFields.Add(subField);
            }

            return this;
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

            Number = reader.ReadPackedInt32();
            reader.ReadCollection(Fields);
        }

        /// <inheritdoc/>
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WritePackedInt32(Number);
            writer.WriteCollection(Fields);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            bool first = true;

            foreach (RecordField field in Fields)
            {
                if (!first)
                {
                    result.AppendLine();
                }
                first = false;
                result.Append(field);
            }

            return result.ToString();
        }

        #endregion
    }
}
