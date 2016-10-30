/* FstProcessor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fst
{
    /// <summary>
    /// FST processor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FstProcessor
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FstProcessor
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Read FST file from server.
        /// </summary>
        [CanBeNull]
        public FstFile ReadFile
            (
                [NotNull] FileSpecification fileSpecification
            )
        {
            Code.NotNull(fileSpecification, "fileSpecification");

            string content = Connection.ReadTextFile(fileSpecification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            StringReader reader = new StringReader(content);
            FstFile result = FstFile.ParseStream(reader);
            if (result.Lines.Count == 0)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Transform record.
        /// </summary>
        [NotNull]
        public MarcRecord TransformRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] string format
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(format, "format");

            string transformed = Connection.FormatRecord
                (
                    format,
                    record
                )
                .ThrowIfNull("Connection.FormatRecord");

            MarcRecord result = new MarcRecord
            {
                Database = record.Database ?? Connection.Database
            };
            string[] lines = transformed.Split((char) 0x07);
            string[] separators = {"\r\n", "\r", "\n"};
            foreach (string line in lines)
            {
#if !WINMOBILE && !PocketPC

                string[] parts = line.Split
                    (
                        separators,
                        StringSplitOptions.RemoveEmptyEntries
                    );

#else

                string[] parts = line
                    .Replace("\r", string.Empty)
                    .Split('\n');

#endif

                if (parts.Length == 0)
                {
                    continue;
                }
                string tag = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string body = parts[i];
                    if (string.IsNullOrEmpty(body))
                    {
                        continue;
                    }
                    RecordField field
                        = RecordFieldUtility.Parse(tag, body);

                    SubField[] badSubFields
                        = field.SubFields
                        .Where(sf=>string.IsNullOrEmpty(sf.Value))
                        .ToArray();
                    foreach (SubField subField in badSubFields)
                    {
                        field.SubFields.Remove(subField);
                    }

                    if (!string.IsNullOrEmpty(field.Value)
                        || field.SubFields.Count != 0)
                    {
                        result.Fields.Add(field);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Transform the record.
        /// </summary>
        [NotNull]
        public MarcRecord TransformRecord
            (
                [NotNull] MarcRecord record,
                [NotNull] FstFile fstFile
            )
        {
            Code.NotNull(record, "record");
            Code.NotNull(fstFile, "fstFile");

            string format = fstFile.ConcatenateFormat();
            MarcRecord result = TransformRecord
                (
                    record,
                    format
                );

            return result;
        }

        #endregion
    }
}
