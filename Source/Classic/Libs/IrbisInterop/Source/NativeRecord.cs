// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* NativeRecord.cs -- 
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NativeRecord
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Fields collection.
        /// </summary>
        [NotNull]
        public NonNullCollection<NativeField> Fields { get; private set; }

        /// <summary>
        /// Version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Flags.
        /// </summary>
        public int Flags { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NativeRecord()
        {
            Fields = new NonNullCollection<NativeField>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse memory representation of rectord.
        /// </summary>
        [NotNull]
        public static NativeRecord ParseMemory
            (
                [NotNull] byte[] memory
            )
        {
            Code.NotNull(memory, "memory");

            NativeRecord result = new NativeRecord
            {
                Mfn = BitConverter.ToInt32(memory, 0)
            };

            int totalLength = BitConverter.ToInt32(memory, 4);
            if (totalLength < memory.Length)
            {
                throw new IrbisException();
            }

            // Always zero. Padding?
            int unknown1 = BitConverter.ToInt32(memory, 8);
            int unknown2 = BitConverter.ToInt32(memory, 12);

            int dataOffset = BitConverter.ToInt32(memory, 0x10);
            int fieldCount = BitConverter.ToInt32(memory, 0x14);
            result.Version = BitConverter.ToInt32(memory, 0x14);
            result.Flags = BitConverter.ToInt32(memory, 0x1C);
            int offset = 0x20;
            Encoding encoding = new UTF8Encoding(false, true);
            int end = 0;

            for (int index = 0; index < fieldCount; index++)
            {
                int tag = BitConverter.ToInt32(memory, offset);
                int fieldOffset = BitConverter.ToInt32(memory, offset + 4);
                if (fieldOffset != end)
                {
                    throw new IrbisException();
                }
                int fieldLentgh = BitConverter.ToInt32(memory, offset + 8);
                end = fieldOffset + fieldLentgh;
                string text = encoding.GetString
                    (
                        memory,
                        dataOffset + fieldOffset,
                        fieldLentgh
                    );

                NativeField field = new NativeField
                {
                    Tag = tag,
                    Value = text
                };
                result.Fields.Add(field);

                offset += 12;
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat
                (
                    "MFN={0}, Version={1}, Flags={2}", 
                    Mfn,
                    Version,
                    Flags
                );
            result.AppendLine();

            foreach (NativeField field in Fields)
            {
                result.Append(field);
                result.AppendLine();
            }

            return result.ToString();
        }

        #endregion
    }
}
