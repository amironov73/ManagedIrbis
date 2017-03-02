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
        /// Client.
        /// </summary>
        [NotNull]
        public AbstractClient Client { get; private set; }

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
                [NotNull] AbstractClient client,
                [NotNull] string format
            )
        {
            Code.NotNull(client, "client");
            Code.NotNullNorEmpty(format, "format");

            Client = client;

            Formatter = new PftFormatter();
            Formatter.SetEnvironment(client);
            SetProgram(format);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

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

            string text = Formatter.Format(record);
            bool result = text.SameString("1");

            return result;
        }

        /// <summary>
        /// Filter records.
        /// </summary>
        [NotNull]
        public RecordField[] FilterFields
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            List<RecordField> fields 
                = new List<RecordField>(record.Fields.Count);

            foreach (RecordField field in record.Fields)
            {
                if (CheckField(field))
                {
                    fields.Add(field);
                }
            }

            return fields.ToArray();
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
