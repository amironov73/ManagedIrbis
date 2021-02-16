// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UseNameofExpression

/* FstFile.cs -- FST file handling
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// FST file handling
    /// </summary>
    [PublicAPI]
    [XmlRoot("fst")]
    [MoonSharpUserData]
    [DebuggerDisplay("FileName = {FileName}")]
    public sealed class FstFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// File name (for identification only).
        /// </summary>
        [XmlAttribute("fileName")]
        [JsonProperty("fileName", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }

        /// <summary>
        /// Lines of the file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlElement("line")]
        [JsonProperty("lines")]
        public NonNullCollection<FstLine> Lines { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FstFile()
        {
            Lines = new NonNullCollection<FstLine>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build concatenated format string.
        /// </summary>
        [NotNull]
        public string ConcatenateFormat()
        {
            StringBuilder result = new StringBuilder();

            // result.Append('!');

            foreach (FstLine line in Lines)
            {
                result.Append(line.ToFormat());
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse the stream.
        /// </summary>
        [NotNull]
        public static FstFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FstFile result = new FstFile();

            int lineNumber = 1;
            FstLine line;
            while ((line = FstLine.ParseStream(reader)) != null)
            {
                line.LineNumber = lineNumber;
                result.Lines.Add(line);
                lineNumber++;
            }

            return result;
        }


        /// <summary>
        /// Parse local file.
        /// </summary>
        [NotNull]
        public static FstFile ParseLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            using (TextReader reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    encoding
                ))
            {
                FstFile result = ParseStream(reader);
                result.FileName = Path.GetFileName(fileName);

                return result;
            }
        }

        /// <summary>
        /// Should serialize the <see cref="Lines"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeLines()
        {
            return Lines.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FileName = reader.ReadNullableString();
            reader.ReadCollection(Lines);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(FileName);
            writer.WriteCollection(Lines);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FstFile> verifier
                = new Verifier<FstFile>(this, throwOnError);

            verifier.Assert(Lines.Count != 0, "Lines.Count != 0");
            foreach (FstLine line in Lines)
            {
                verifier.VerifySubObject(line);
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return FileName.ToVisibleString();
        }

        #endregion
    }
}
