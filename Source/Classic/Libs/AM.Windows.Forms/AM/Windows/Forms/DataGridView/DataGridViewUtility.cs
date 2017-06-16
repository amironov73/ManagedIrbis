// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataGridViewUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using AM.Data;
using AM.Xml;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class DataGridViewUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply specified columns to the <see cref="DataGridView"/>.
        /// </summary>
        public static void ApplyColumns
            (
                [NotNull] DataGridView grid,
                [NotNull][ItemNotNull] IEnumerable<DataColumnInfo> columns
            )
        {
            Code.NotNull(grid, "grid");
            Code.NotNull(columns, "columns");

            try
            {
                grid.SuspendLayout();

                grid.AutoGenerateColumns = false;
                grid.Columns.Clear();

                foreach (DataColumnInfo info in columns)
                {
                    DataGridViewColumn column = info.ToGridColumn();
                    grid.Columns.Add(column);
                }
            }
            finally
            {
                grid.ResumeLayout();
            }
        }

        /// <summary>
        /// Converts the column description into
        /// <see cref="DataGridViewColumn"/>
        /// </summary>
        [NotNull]
        public static DataGridViewColumn ToGridColumn
            (
                [NotNull] this DataColumnInfo column
            )
        {
            Code.NotNull(column, "column");

            string columnTypeName = column.Type;
            Type columnType = null;
            if (!string.IsNullOrEmpty(columnTypeName))
            {
                columnType = Type.GetType(columnTypeName, true);
            }

            DataGridViewColumn result = ReferenceEquals(columnType, null)
                ? new DataGridViewTextBoxColumn()
                : (DataGridViewColumn)Activator.CreateInstance(columnType);
            result.DataPropertyName = column.Name;
            result.HeaderText = column.Title;
            result.DataPropertyName = column.Name;
            result.ReadOnly = column.ReadOnly;
            result.Frozen = column.Frozen;
            result.Visible = !column.Invisible;
            if (column.Width != 0)
            {
                if (column.Width > 0)
                {
                    result.AutoSizeMode
                        = DataGridViewAutoSizeColumnMode.None;
                    result.Width = column.Width;
                }
                else
                {
                    result.AutoSizeMode
                        = DataGridViewAutoSizeColumnMode.Fill;
                    result.FillWeight = Math.Abs(column.Width);
                }
            }
            result.HeaderCell.Style.Alignment
                        = DataGridViewContentAlignment.MiddleCenter;

            return result;
        }


        /// <summary>
        /// Gets the columns.
        /// </summary>
        [NotNull]
        public static List<DataGridViewColumn> GetColumns
            (
                [NotNull] string xmlText
            )
        {
            Code.NotNullNorEmpty(xmlText, "xmlText");

            DataSetInfo info
                = XmlUtility.DeserializeString<DataSetInfo>(xmlText);

            List<DataGridViewColumn> result = new List<DataGridViewColumn>();
            foreach (DataColumnInfo column in info.Tables[0].Columns)
            {
                result.Add(column.ToGridColumn());
            }

            return result;
        }

        ///// <summary>
        ///// Generates the table HTML.
        ///// </summary>
        ///// <param name="grid">The grid.</param>
        ///// <param name="title">The title.</param>
        ///// <returns></returns>
        //public static string GenerateTableHtml
        //    (
        //        DataGridView grid,
        //        string title
        //    )
        //{
        //    ArgumentUtility.NotNull(grid, "grid");
        //    ArgumentUtility.NotNull(title, "title");

        //    StringWriter writer = new StringWriter();
        //    Html32TextWriter html = new Html32TextWriter(writer);

        //    html.RenderBeginTag(HtmlTextWriterTag.Html);
        //    html.RenderBeginTag(HtmlTextWriterTag.Head);
        //    html.RenderBeginTag(HtmlTextWriterTag.Title);
        //    html.WriteEncodedText(title);
        //    html.RenderEndTag(); // title
        //    html.RenderEndTag(); // head
        //    html.RenderBeginTag(HtmlTextWriterTag.Body);
        //    html.AddAttribute(HtmlTextWriterAttribute.Width, "90%");
        //    html.AddAttribute(HtmlTextWriterAttribute.Border, "1");
        //    html.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
        //    html.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "3");
        //    html.AddAttribute(HtmlTextWriterAttribute.Bordercolor, "black");
        //    html.AddAttribute(HtmlTextWriterAttribute.Align, "center");
        //    html.RenderBeginTag(HtmlTextWriterTag.Table);

        //    html.RenderBeginTag(HtmlTextWriterTag.Tr);
        //    foreach (DataGridViewColumn column in grid.Columns)
        //    {
        //        if (column.Visible)
        //        {
        //            html.RenderBeginTag(HtmlTextWriterTag.Td);
        //            html.RenderBeginTag(HtmlTextWriterTag.B);
        //            html.WriteEncodedText(column.HeaderText);
        //            html.RenderEndTag();
        //            html.RenderEndTag(); // td
        //        }
        //    }
        //    html.RenderEndTag(); // tr

        //    foreach (DataGridViewRow row in grid.Rows)
        //    {
        //        html.RenderBeginTag(HtmlTextWriterTag.Tr);
        //        foreach (DataGridViewCell cell in row.Cells)
        //        {
        //            if (cell.OwningColumn.Visible)
        //            {
        //                html.RenderBeginTag(HtmlTextWriterTag.Td);
        //                object formattedValue = cell.FormattedValue;
        //                bool written = false;
        //                if (formattedValue != null)
        //                {
        //                    string text = formattedValue.ToString();
        //                    if (!string.IsNullOrEmpty(text))
        //                    {
        //                        html.WriteEncodedText(formattedValue.ToString());
        //                        written = true;
        //                    }
        //                }
        //                if (!written)
        //                {
        //                    html.Write("&nbsp;");
        //                }
        //                html.RenderEndTag(); // td
        //            }
        //        }
        //        html.RenderEndTag(); // tr
        //    }

        //    html.RenderEndTag(); // table

        //    html.RenderEndTag(); // body
        //    html.RenderEndTag(); // html

        //    html.Flush();

        //    return writer.ToString();
        //}

        #endregion
    }
}