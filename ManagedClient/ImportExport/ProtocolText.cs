/* Iso2709.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;
using ManagedClient.Network;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.ImportExport
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
                Tag = _ReadTo(reader, '#'),
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
        /// Parse MFN, status and version of the record
        /// </summary>
        [NotNull]
        public static IrbisRecord ParseMfnStatusVersion
            (
                [NotNull] string line1,
                [NotNull] string line2,
                [NotNull] IrbisRecord record
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
        /// Parse server response for single record.
        /// </summary>
        [NotNull]
        public static IrbisRecord ParseResponseForSingleRecord
            (
                [NotNull] IrbisServerResponse response,
                [NotNull] IrbisRecord record
            )
        {
            Code.NotNull(response, "response");
            Code.NotNull(record, "record");

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
                record.Fields.Add(field);
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

            return record;
        }

        #endregion
    }
}
