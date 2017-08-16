// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* .cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FieldFilter
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Formatter.
        /// </summary>
        [NotNull]
        public PftFormatter Formatter { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldFilter
            (
                [NotNull] string format
            )
            : this
                (
                    new LocalProvider(),
                    format
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldFilter
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string format
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNullNorEmpty(format, "format");

            Provider = provider;

            Formatter = new PftFormatter();
            Formatter.SetProvider(provider);
            SetProgram(format);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Whether all fields satisfy the condition.
        /// </summary>
        public bool AllFields
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            bool result = false;

            foreach (RecordField field in fields)
            {
                result = CheckField(field);
                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Whether any field satisfy the condition.
        /// </summary>
        public bool AnyField
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            bool result = false;

            foreach (RecordField field in fields)
            {
                result = CheckField(field);
                if (result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Check the field.
        /// </summary>
        public bool CheckField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            MarcRecord record = new MarcRecord();
            RecordField copy = field.Clone();
            record.Fields.Add(copy);

            Formatter.Context.AlternativeRecord = field.Record;
            string text = Formatter.FormatRecord(record);
            bool result = text.SameString("1");

            return result;
        }

        /// <summary>
        /// Filter records.
        /// </summary>
        [NotNull]
        public RecordField[] FilterFields
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            List<RecordField> result = new List<RecordField>();

            foreach (RecordField field in fields)
            {
                if (CheckField(field))
                {
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Filter records by field specification.
        /// </summary>
        public IEnumerable<MarcRecord> FilterRecords
            (
                [NotNull] IEnumerable<MarcRecord> records
            )
        {
            Code.NotNull(records, "records");

            foreach (MarcRecord record in records)
            {
                if (AnyField(record.Fields))
                {
                    yield return record;
                }
            }
        }

            /// <summary>
        /// Find first satisfying field.
        /// </summary>
        [CanBeNull]
        public RecordField First
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            RecordField result = null;

            foreach (RecordField field in fields)
            {
                if (CheckField(field))
                {
                    result = field;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Find last satisfying field.
        /// </summary>
        [CanBeNull]
        public RecordField Last
            (
                [NotNull] IEnumerable<RecordField> fields
            )
        {
            Code.NotNull(fields, "fields");

            RecordField result = null;

            foreach (RecordField field in fields)
            {
                if (CheckField(field))
                {
                    result = field;
                }
            }

            return result;
        }

        /// <summary>
        /// Set filter program.
        /// </summary>
        public void SetProgram
            (
                [NotNull] string format
            )
        {
            Code.NotNullNorEmpty(format, "format");

            string text = string.Format
                (
                    "if {0} then '1' else '0' fi",
                    format
                );

            Formatter.ParseProgram(text);
        }

        #endregion

        #region Object members

        #endregion
    }
}

