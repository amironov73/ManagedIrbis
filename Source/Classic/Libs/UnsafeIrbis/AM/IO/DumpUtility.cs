// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DumpUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using UnsafeAM.Text;
using UnsafeAM.Text.Output;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    /// Utility methods for dumping binary data.
    /// </summary>
    [PublicAPI]
    public static class DumpUtility
    {
        #region Public methods

        /// <summary>
        /// Dump the array of data.
        /// </summary>
        public static void Dump<T>
            (
                [NotNull] TextWriter writer,
                [NotNull] T[] data
            )
            where T: struct
        {
            Code.NotNull(writer, nameof(writer));
            Code.NotNull(data, nameof(data));

            string format = " {0:X8}";

            if ((data is byte[])
                 || (data is sbyte[]))
            {
                format = " {0:X2}";
            }
            if ((data is long[])
                 || (data is ulong[]))
            {
                format = " {0:X16}";
            }

            bool begin = true;
            for (int offset = 0; offset < data.Length; offset++)
            {
                if ((offset % 16) == 0)
                {
                    if (!begin)
                    {
                        writer.WriteLine();
                    }
                    else
                    {
                        begin = false;
                    }
                    writer.Write("{0:X6}> ", offset);
                }

                if ((offset % 4) == 0)
                {
                    writer.Write(" ");
                }

                T item = data[offset];
                writer.Write(format, item);
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Dump the array of data.
        /// </summary>
        public static void Dump<T>
            (
                [NotNull] Stream stream,
                [NotNull] T[] data
            )
            where T: struct
        {
            Code.NotNull(stream, nameof(stream));
            Code.NotNull(data, nameof(data));

            StreamWriter writer = new StreamWriter
                (
                    stream,
                    EncodingUtility.DefaultEncoding
                );

            Dump(writer, data);
        }

        /// <summary>
        /// Dump the array of data to console output.
        /// </summary>
        public static void DumpToConsole<T>
            (
                [NotNull] T[] data
            )
            where T: struct
        {
            Code.NotNull(data, nameof(data));

            Dump(Console.Out, data);
        }

        /// <summary>
        /// Dump the array of data to <see cref="AbstractOutput"/>.
        /// </summary>
        public static void DumpToOutput<T>
            (
                [NotNull] AbstractOutput output,
                [NotNull] T[] data
            )
            where T : struct
        {
            Code.NotNull(output, nameof(output));
            Code.NotNull(data, nameof(data));

            string text = DumpToText(data);
            output.WriteLine(text);
        }

        /// <summary>
        /// Dump the array of data to string.
        /// </summary>
        [NotNull]
        public static string DumpToText<T>
            (
                [NotNull] T[] data
            )
            where T : struct
        {
            Code.NotNull(data, nameof(data));

            StringWriter writer = new StringWriter();
            Dump
                (
                    writer,
                    data
                );

            return writer.ToString();
        }

#endregion
    }
}
