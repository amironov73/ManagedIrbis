// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianSubFieldGrid.cs --
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

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI.Grid
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class SiberianSubFieldGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for field value.
        /// </summary>
        [NotNull]
        public SiberianSubFieldColumn SubFieldColumn { get; private set; }

        /// <summary>
        /// Column for code and title.
        /// </summary>
        [NotNull]
        public SiberianCodeColumn CodeColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianSubFieldGrid()
        {
            HeaderHeight = 26;

            CodeColumn = (SiberianCodeColumn)CreateColumn<SiberianCodeColumn>();
            CodeColumn.Title = "Subfield";
            CodeColumn.FillWidth = 100;

            SubFieldColumn = (SiberianSubFieldColumn)CreateColumn<SiberianSubFieldColumn>();
            SubFieldColumn.Title = "Value";
            SubFieldColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given WSS.
        /// </summary>
        public void Load
            (
                [NotNull] WssFile worksheet,
                [NotNull] RecordField field
            )
        {
            Code.NotNull(worksheet, "worksheet");
            Code.NotNull(field, "field");

            Rows.Clear();

            foreach (WorksheetItem item in worksheet.Items)
            {
                char code = item.Tag[0];

                SubField subField = field.SubFields
                    .GetFirstSubField(code);

                SiberianSubField line = SiberianSubField
                    .FromWorksheetItem
                    (
                        item
                    );

                if (!ReferenceEquals(subField, null))
                {
                    line.Value = subField.Value;
                }

                CreateRow(line);
            }

            Goto(1, 0);
        }

        #endregion

        #region SiberianGrid members

        /// <inheritdoc/>
        protected override SiberianRow CreateRow()
        {
            SiberianRow result = base.CreateRow();
            result.Height = 24;

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
