// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MarcRecordUtility.cs -- extensions for MarcRecord
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if CLASSIC || NETCORE

using YamlDotNet;
using YamlDotNet.Serialization;

#endif

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Extension methods for <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class MarcRecordUtility
    {
        #region Constants

        #endregion

        #region Properties
        
        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Begin update the record.
        /// </summary>
        [NotNull]
        public static MarcRecord BeginUpdate
            (
                [NotNull] this MarcRecord record
            )
        {
            record.Fields.BeginUpdate();

            return record;
        }

        /// <summary>
        /// Begin update the record.
        /// </summary>
        [NotNull]
        public static MarcRecord BeginUpdate
            (
                [NotNull] this MarcRecord record,
                int delta
            )
        {
            record.Fields.BeginUpdate();
            record.Fields.AddCapacity(delta);
            record.Modified = false;

            return record;
        }

        /// <summary>
        /// End of the record update.
        /// </summary>
        [NotNull]
        public static MarcRecord EndUpdate
            (
                [NotNull] this MarcRecord record
            )
        {
            record.Fields.EndUpdate();

            return record;
        }

        /// <summary>
        /// Есть хотя бы одно поле с указанными тегами?
        /// </summary>
        public static bool HaveField
            (
                this MarcRecord record,
                params string[] tags
            )
        {
            return (record.Fields.GetField(tags).Length != 0);
        }

        /// <summary>
        /// Нет ни одного поля с указанными тегами?
        /// </summary>
        public static bool HaveNotField
            (
                this MarcRecord record,
                params string[] tags
            )
        {
            return (record.Fields.GetField(tags).Length == 0);
        }

        /// <summary>
        /// Parse ALL-formatted records in server response.
        /// </summary>
        [NotNull]
        public static MarcRecord[] ParseAllFormat
            (
                [NotNull] string database,
                [NotNull] ServerResponse response
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(response, "response");

            List<MarcRecord> result = new List<MarcRecord>();

            while (true)
            {
                MarcRecord record = new MarcRecord
                {
                    HostName = response.Connection.Host,
                    Database = database
                };
                record = ProtocolText.ParseResponseForAllFormat
                    (
                        response,
                        record
                    );
                if (ReferenceEquals(record, null))
                {
                    break;
                }
                result.Add(record);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse ALL-formatted records in server response.
        /// </summary>
        [NotNull]
        public static MarcRecord[] ParseAllFormat
            (
                [NotNull] string database,
                [NotNull] IrbisConnection connection,
                [NotNull] string[] lines
            )
        {
            Code.NotNullNorEmpty(database, "database");
            Code.NotNull(connection, "connection");
            Code.NotNull(lines, "lines");

            List<MarcRecord> result = new List<MarcRecord>();

            foreach (string line in lines)
            {
                MarcRecord record = new MarcRecord
                {
                    HostName = connection.Host,
                    Database = database
                };
                record = ProtocolText.ParseResponseForAllFormat
                    (
                        line,
                        record
                    );

                // coverity[NULL_RETURNS]
                result.Add(record.ThrowIfNull("record"));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Remove all the fields with specified tag.
        /// </summary>
        [NotNull]
        public static MarcRecord RemoveField
            (
                [NotNull] this MarcRecord record,
                [NotNull] string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");

            RecordField[] found = record.Fields.GetField(tag);
            foreach (RecordField field in found)
            {
                record.Fields.Remove(field);
            }

            return record;
        }

        /// <summary>
        /// Replace fields with specified tag.
        /// </summary>
        [NotNull]
        public static MarcRecord ReplaceField
            (
                [NotNull] this MarcRecord record,
                [NotNull] string tag,
                [NotNull] IEnumerable<RecordField> newFields
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");
            Code.NotNull(newFields, "newFields");

            record.RemoveField(tag);
            record.Fields.AddRange(newFields);

            return record;
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Устанавливает значение только для
        /// первого повторения поля (если в записи их несколько)!
        /// </remarks>
        [NotNull]
        public static MarcRecord SetField
            (
                [NotNull] this MarcRecord record,
                string tag,
                string text
            )
        {
            Code.NotNull(record, "record");

            RecordField field = record.Fields
                .GetField(tag)
                .FirstOrDefault();

            if (field == null)
            {
                field = new RecordField(tag);
                record.Fields.Add(field);
            }

            field.Value = text;

            return record;
        }

        /// <summary>
        /// Установка поля.
        /// </summary>
        [NotNull]
        public static MarcRecord SetField
            (
                [NotNull] this MarcRecord record,
                string tag,
                int occurrence,
                string newText
            )
        {
            Code.NotNull(record, "record");

            RecordField field = record.Fields
                .GetField(tag)
                .GetOccurrence(occurrence);

            if (!ReferenceEquals(field, null))
            {
                field.Value = newText;
            }

            return record;
        }

        /// <summary>
        /// Установка подполя.
        /// </summary>
        [NotNull]
        public static MarcRecord SetSubField
            (
                [NotNull] this MarcRecord record,
                string tag,
                char code,
                string text
            )
        {
            Code.NotNull(record, "record");

            RecordField field = record.Fields
                .GetField(tag)
                .FirstOrDefault();

            if (field == null)
            {
                field = new RecordField(tag);
                record.Fields.Add(field);
            }

            field.SetSubField(code, text);

            return record;
        }

        /// <summary>
        /// Установка подполя.
        /// </summary>
        [NotNull]
        public static MarcRecord SetSubField
            (
                [NotNull] this MarcRecord record,
                string tag,
                int fieldOccurrence,
                char code,
                int subFieldOccurrence,
                string newText
            )
        {
            Code.NotNull(record, "record");

            RecordField field = record.Fields
                .GetField(tag)
                .GetOccurrence(fieldOccurrence);

            if (!ReferenceEquals(field, null))
            {
                SubField subField = field.GetSubField
                    (
                        code,
                        subFieldOccurrence
                    );
                if (!ReferenceEquals(subField, null))
                {
                    subField.Value = newText;
                }
            }

            return record;
        }

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Convert the <see cref="MarcRecord"/> to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string result = JObject.FromObject(record)
                .ToString(Formatting.None);

            return result;
        }

#endif

#if CLASSIC || NETCORE

        /// <summary>
        /// Convert the <see cref="MarcRecord"/> to YAML.
        /// </summary>
        [NotNull]
        public static string ToYaml
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            Serializer serializer = new Serializer();
            string result = serializer.Serialize(record);

            return result;
        }

#endif

        #endregion
    }
}
