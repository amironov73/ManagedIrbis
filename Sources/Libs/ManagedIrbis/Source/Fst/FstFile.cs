/* FstFile.cs -- FST file handling
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    [MoonSharpUserData]
    [DebuggerDisplay("FileName = {FileName}")]
    public sealed class FstFile
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// File name (for identification only).
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Lines of the file.
        /// </summary>
        [NotNull]
        [ItemNotNull]
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

        #region Private members

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
            using (TextReader reader = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                ))
            {
                FstFile result = ParseStream(reader);
                result.FileName = Path.GetFileName(fileName);
                return result;
            }
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            reader.ReadCollection(Lines);
        }

        /// <summary>
        /// Save object state to specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.WriteCollection(Lines);
        }

        #endregion

        #region Object members

        #endregion
    }
}
