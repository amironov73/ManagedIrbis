// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Iso2709.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Текстовое представление записи, используемое в протоколе
    /// ИРБИС64-сервер.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ProtocolText
    {
        #region Constants

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members
        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append(IrbisText.IrbisDelimiter);
        }

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

        private static RecordField _ParseLine
            (
                string line
            )
        {
            StringReader reader = new StringReader(line);

            RecordField result = new RecordField
            {
                Tag = NumericUtility.ParseInt32(_ReadTo(reader, '#')),
                Value = _ReadTo(reader, '^')
            };

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
                result.SubFields.Add(subField);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode subfield.
        /// </summary>
        public static void EncodeSubField
            (
                [NotNull] StringBuilder builder,
                [NotNull] SubField subField
            )
        {
            builder.AppendFormat
                (
                    "{0}{1}{2}",
                    SubField.Delimiter,
                    subField.Code,
                    subField.Value
                );
        }

        /// <summary>
        /// Encode field.
        /// </summary>
        public static void EncodeField
            (
                [NotNull] StringBuilder builder,
                [NotNull] RecordField field
            )
        {
            builder.AppendFormat
                (
                    "{0}#",
                    field.Tag
                );

            builder.Append(field.Value);

            foreach (SubField subField in field.SubFields)
            {
                EncodeSubField
                    (
                        builder,
                        subField
                    );
            }

            builder.Append(IrbisText.IrbisDelimiter);
        }


        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        /// <param name="record">Запись для кодирования.</param>
        /// <returns>
        /// Закодированная запись.
        /// </returns>
        public static string EncodeRecord
            (
                MarcRecord record
            )
        {
            StringBuilder result = new StringBuilder();

            _AppendIrbisLine
                (
                    result,
                    "{0}#{1}",
                    record.Mfn,
                    (int)record.Status
                );
            _AppendIrbisLine
                (
                    result,
                    "0#{0}",
                    record.Version
                );

            foreach (RecordField field in record.Fields)
            {
                EncodeField
                    (
                        result,
                        field
                    );
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse MFN, status and version of the record
        /// </summary>
        [NotNull]
        public static MarcRecord ParseMfnStatusVersion
            (
                [NotNull] string line1,
                [NotNull] string line2,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNullNorEmpty(line1, "line1");
            Code.NotNullNorEmpty(line2, "line2");
            Code.NotNull(record, "record");

            Regex regex = new Regex(@"^(-?\d+)\#(\d*)?");
            Match match = regex.Match(line1);
            record.Mfn = Math.Abs(int.Parse(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus)int.Parse
                    (
                        match.Groups[2].Value
                    );
            }
            match = regex.Match(line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = int.Parse(match.Groups[2].Value);
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ReadRecordCommand.
        /// </summary>
        [NotNull]
        public static MarcRecord ParseResponseForReadRecord
            (
                [NotNull] ServerResponse response,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(response, "response");
            Code.NotNull(record, "record");

            try
            {
                record.Fields.BeginUpdate();

                ParseMfnStatusVersion
                    (
                        response.RequireUtfString(),
                        response.RequireUtfString(),
                        record
                    );

                string line;
                while (true)
                {
                    line = response.GetUtfString();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    if (line == "#")
                    {
                        break;
                    }
                    RecordField field = _ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
                if (line == "#")
                {
                    int returnCode = response.RequireInt32();
                    if (returnCode >= 0)
                    {
                        line = response.RequireUtfString();
                        line = IrbisText.IrbisToWindows(line);
                        record.Description = line;
                    }
                }

            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Parse server response for WriteRecordCommand.
        /// </summary>
        [NotNull]
        public static MarcRecord ParseResponseForWriteRecord
            (
                [NotNull] ServerResponse response,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(response, "response");
            Code.NotNull(record, "record");

            try
            {
                record.Fields.BeginUpdate();
                record.Fields.Clear();

                string first = response.RequireUtfString();
                string second = response.RequireUtfString();
                string[] split = second.Split('\x1E');

                ParseMfnStatusVersion
                    (
                        first,
                        split[0],
                        record
                    );

                for (int i = 1; i < split.Length; i++)
                {
                    string line = split[i];
                    RecordField field = _ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Parse server response for WriteRecordsCommand.
        /// </summary>
        [NotNull]
        public static MarcRecord ParseResponseForWriteRecords
            (
                [NotNull] ServerResponse response,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(response, "response");
            Code.NotNull(record, "record");

            try
            {
                record.Fields.BeginUpdate();
                record.Fields.Clear();

                string whole = response.RequireUtfString();
                string[] split = whole.Split('\x1E');

                ParseMfnStatusVersion
                    (
                        split[0],
                        split[1],
                        record
                    );

                for (int i = 2; i < split.Length; i++)
                {
                    string line = split[i];
                    RecordField field = _ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ALL-formatted record.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ParseResponseForAllFormat
            (
                [NotNull] ServerResponse response,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(response, "response");
            Code.NotNull(record, "record");

            try
            {
                record.Fields.BeginUpdate();
                record.Fields.Clear();

                string line = response.GetUtfString();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                string[] split = line.Split('\x1F');
                ParseMfnStatusVersion
                    (
                        split[1],
                        split[2],
                        record
                    );

                for (int i = 3; i < split.Length; i++)
                {
                    line = split[i];
                    RecordField field = _ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Parse server response for ALL-formatted record.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ParseResponseForAllFormat
            (
                [NotNull] string line,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            try
            {
                record.Fields.BeginUpdate();
                record.Fields.Clear();

                string[] split = line.Split('\x1F');
                ParseMfnStatusVersion
                    (
                        split[1],
                        split[2],
                        record
                    );

                for (int i = 3; i < split.Length; i++)
                {
                    line = split[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        RecordField field = _ParseLine(line);
                        if (field.Tag > 0)
                        {
                            record.Fields.Add(field);
                        }
                    }
                }
            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Parse response for global correction
        /// of virtual record.
        /// </summary>
        [CanBeNull]
        public static MarcRecord ParseResponseForGblFormat
            (
                [NotNull] string line,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            try
            {
                record.Fields.BeginUpdate();
                record.Fields.Clear();

                string[] split = line.Split('\x1E');
                for (int i = 1; i < split.Length; i++)
                {
                    line = split[i];
                    RecordField field = _ParseLine(line);
                    if (field.Tag > 0)
                    {
                        record.Fields.Add(field);
                    }
                }
            }
            finally
            {
                record.Fields.EndUpdate();
            }

            return record;
        }

        /// <summary>
        /// Convert the record to the protocol text.
        /// </summary>
        [CanBeNull]
        public static string ToProtocolText
            (
                [CanBeNull] this MarcRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            return EncodeRecord(record);
        }

        #endregion
    }
}
