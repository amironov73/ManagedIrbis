// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordState.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Client
{
    /// <summary>
    /// State of the <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("record")]
    [DebuggerDisplay("{Mfn} {Status} {Version}")]
    public struct RecordState
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        [JsonProperty("mfn")]
        [XmlAttribute("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Status.
        /// </summary>
        [JsonProperty("status")]
        [XmlAttribute("status")]
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Version.
        /// </summary>
        [JsonProperty("version")]
        [XmlAttribute("version")]
        public int Version { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static char[] _delimiters =
        {
            ' ', '\t', '\r', '\n', '#', '\x1F', '\x1E'
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server answer.
        /// </summary>
        public static RecordState ParseServerAnswer
            (
                [NotNull] string line
            )
        {
            Code.NotNullNorEmpty(line, "line");

            //
            // &uf('G0$',&uf('+0'))
            //
            // 0 MFN#STATUS 0#VERSION OTHER
            // 0 161608#0 0#1 101#
            //

            RecordState result = new RecordState();

            string[] parts = line.Split
                (
                    _delimiters,
                    StringSplitOptions.RemoveEmptyEntries
                );
            if (parts.Length < 5)
            {
                throw new IrbisException("bad line format");
            }

            result.Mfn = NumericUtility.ParseInt32(parts[1]);
            result.Status
                = (RecordStatus) NumericUtility.ParseInt32(parts[2]);
            result.Version = NumericUtility.ParseInt32(parts[4]);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Mfn = reader.ReadPackedInt32();
            Status = (RecordStatus) reader.ReadPackedInt32();
            Version = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Mfn)
                .WritePackedInt32((int) Status)
                .WritePackedInt32(Version);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return string.Format
                (
                    "{0} {1} {2}",
                    Mfn,
                    Status,
                    Version
                );
        }

        #endregion
    }
}
