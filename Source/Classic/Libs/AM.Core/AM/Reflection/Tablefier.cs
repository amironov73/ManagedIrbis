// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Tablefier.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Tablefier
    {
        #region Inner classes

        /// <summary>
        /// Column.
        /// </summary>
        public class Column
        {
            /// <summary>
            /// Title.
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Width.
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Align to right?
            /// </summary>
            public bool RightAlign { get; set; }

            /// <summary>
            /// Property info.
            /// </summary>
            public PropertyInfo Property { get; set; }
        }

        #endregion

        #region Private members

        private void _LeftAlign
            (
                [NotNull] TextWriter writer,
                [NotNull] string text,
                int width
            )
        {
            writer.Write(text);
            for (int i = text.Length; i < width; i++)
            {
                writer.Write(' ');
            }
        }

        private void _RightAlign
            (
                [NotNull] TextWriter writer,
                [NotNull] string text,
                int width
            )
        {
            for (int i = text.Length; i < width; i++)
            {
                writer.Write(' ');
            }
            writer.Write(text);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adjust columns.
        /// </summary>
        public void AdjustColumns
            (
                [NotNull] Column[] columns,
                [NotNull] string[][] cells
            )
        {
            Code.NotNull(columns, "columns");
            Code.NotNull(cells, "cells");

            int height = cells.Length;
            for (int i = 0; i < columns.Length; i++)
            {
                int width = columns[i].Title.Length;
                for (int j = 0; j < height; j++)
                {
                    width = Math.Max(width, cells[j][i].Length);
                }

                columns[i].Width = width;
            }
        }

        /// <summary>
        /// Get cells.
        /// </summary>
        [NotNull]
        public string[][] GetCells<T>
            (
                [NotNull] IEnumerable<T> items,
                [NotNull] Column[] columns
            )
        {
            Code.NotNull(items, "items");
            Code.NotNull(columns, "columns");

            List<string[]> result = new List<string[]>();
            int length = columns.Length;
            foreach (object item in items)
            {
                string[] line = new string[length];
                for (int i = 0; i < length; i++)
                {
                    object value = columns[i].Property.GetValue(item);
                    string text = value.ToVisibleString();
                    line[i] = text;
                }
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get columns.
        /// </summary>
        [NotNull]
        public Column[] GetColumns
            (
                [NotNull] Type type
            )
        {
            Code.NotNull(type, "type");

            List<Column> result = new List<Column>();
            PropertyInfo[] properties
                = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                TypeCode code = Type.GetTypeCode(property.PropertyType);
                bool rightAlign;
                switch (code)
                {
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        rightAlign = true;
                        break;

                    default:
                        rightAlign = false;
                        break;
                }
                Column column = new Column
                {
                    Title = property.Name,
                    Property = property,
                    RightAlign = rightAlign
                };
                result.Add(column);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Print the table.
        /// </summary>
        public void Print
            (
                [NotNull] TextWriter writer,
                [NotNull] Column[] columns,
                [NotNull] string[][] cells
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(columns, "columns");
            Code.NotNull(cells, "cells");

            for (int i = 0; i < columns.Length; i++)
            {
                if (i != 0)
                {
                    writer.Write(' ');
                }
                _LeftAlign(writer, columns[i].Title, columns[i].Width);
            }
            writer.WriteLine();

            for (int i = 0; i < columns.Length; i++)
            {
                if (i != 0)
                {
                    writer.Write(' ');
                }
                writer.Write(new string('-', columns[i].Width));
            }
            writer.WriteLine();

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < columns.Length; j++)
                {
                    if (j != 0)
                    {
                        writer.Write(' ');
                    }

                    if (columns[j].RightAlign)
                    {
                        _RightAlign(writer, cells[i][j], columns[j].Width);
                    }
                    else
                    {
                        _LeftAlign(writer, cells[i][j], columns[j].Width);
                    }
                }
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Print the table.
        /// </summary>
        public void Print<T>
            (
                [NotNull] TextWriter writer,
                [NotNull] IEnumerable<T> items
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(items, "items");

            Type type = typeof(T);
            Column[] columns = GetColumns(type);
            string[][] cells = GetCells(items, columns);
            AdjustColumns(columns, cells);
            Print(writer, columns, cells);
        }

        /// <summary>
        /// Print the table.
        /// </summary>
        public void Print<T>
            (
                [NotNull] TextWriter writer,
                [NotNull] IEnumerable<T> items,
                [NotNull] string[] properties
            )
        {
            Code.NotNull(writer, "writer");
            Code.NotNull(items, "items");
            Code.NotNull(properties, "properties");

            if (properties.Length == 0)
            {
                throw new ArgumentException();
            }

            Type type = typeof(T);
            Column[] columns = GetColumns(type);
            if (properties.Any(p => columns.All(c => c.Title != p)))
            {
                throw new ArgumentException();
            }

            columns = columns.Where(c => c.Title.OneOf(properties)).ToArray();
            string[][] cells = GetCells(items, columns);
            AdjustColumns(columns, cells);
            Print(writer, columns, cells);
        }

        /// <summary>
        /// Print.
        /// </summary>
        [NotNull]
        public string Print<T>
            (
                [NotNull] IEnumerable<T> items,
                params string[] properties
            )
        {
            Code.NotNull(items, "items");

            TextWriter result = new StringWriter();
            if (ReferenceEquals(properties, null) || properties.Length == 0)
            {
                Print(result, items);
            }
            else
            {
                Print(result, items, properties);
            }

            return result.ToString();
        }

        #endregion
    }
}
