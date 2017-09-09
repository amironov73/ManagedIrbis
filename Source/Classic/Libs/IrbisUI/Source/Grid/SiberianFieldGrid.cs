// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SiberianFieldGrid.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

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
    public class SiberianFieldGrid
        : SiberianGrid
    {
        #region Properties

        /// <summary>
        /// Column for field value.
        /// </summary>
        [NotNull]
        public SiberianFieldColumn FieldColumn { get; private set; }

        /// <summary>
        /// Column for repeat number.
        /// </summary>
        [NotNull]
        public SiberianRepeatColumn RepeatColumn { get; private set; }

        /// <summary>
        /// Column for tag and title.
        /// </summary>
        [NotNull]
        public SiberianTagColumn TagColumn { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianFieldGrid()
        {
            HeaderHeight = 26;

            TagColumn = (SiberianTagColumn) CreateColumn<SiberianTagColumn>();
            TagColumn.Title = "Field";
            TagColumn.FillWidth = 100;

            RepeatColumn = (SiberianRepeatColumn)CreateColumn<SiberianRepeatColumn>();

            FieldColumn = (SiberianFieldColumn) CreateColumn<SiberianFieldColumn>();
            FieldColumn.Title = "Value";
            FieldColumn.FillWidth = 100;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Use given page.
        /// </summary>
        public void Load
            (
                [NotNull] WorksheetPage page,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(page, "page");
            Code.NotNull(record, "record");

            Rows.Clear();

            foreach (WorksheetItem item in page.Items)
            {
                int repeat = 1;

                RecordField[] found = record.Fields.GetField
                    (
                        item.Tag.SafeToInt32()
                    );
                if (found.Length == 0)
                {
                    SiberianField line = SiberianField.FromWorksheetItem
                        (
                            item
                        );
                    line.Repeat = repeat;
                    CreateRow(line);
                }
                else
                {
                    foreach (RecordField field in found)
                    {
                        SiberianField line = SiberianField.FromWorksheetItem
                            (
                                item
                            );
                        line.Repeat = repeat;
                        line.Value = field.ToText();
                        if (repeat != 1)
                        {
                            line.Title = "--//--";
                        }
                        repeat++;
                        CreateRow(line);
                    }
                }
            }

            Goto(2, 0);
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
