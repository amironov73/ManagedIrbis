// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisUpperCaseTable.cs -- table for uppercase character conversion
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Table for uppercase character conversion.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisUpperCaseTable
    {
        #region Constants

        /// <summary>
        /// Имя файла таблицы по умолчанию.
        /// </summary>
        public const string FileName = "ISISUCW.TAB";

        #endregion

        #region Properties
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisUpperCaseTable
            (
                [NotNull] Encoding encoding,
                [NotNull] byte[] table
            )
        {
            Code.NotNull(encoding, "encoding");
            Code.NotNull(table, "table");

#if !SILVERLIGHT && !WIN81

            if (!encoding.IsSingleByte)
            {
                throw new IrbisException
                    (
                        "Must be single-byte encoding"
                    );
            }

#endif

            if (table.Length != 256)
            {
                throw new IrbisException
                    (
                        "Must be 256 bytes in table"
                    );
            }

            _encoding = encoding;
            _table = table;
        }

        #endregion

        #region Private members

        private readonly Encoding _encoding;

        private readonly byte[] _table;

        #endregion

        #region Public methods

        /// <summary>
        /// Load the table from specified server file.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable FromServer
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification specification
                = new FileSpecification
                    (
                        IrbisPath.System,
                        fileName
                    );

            string text = connection.ReadTextFile
                (
                    specification
                );

            if (string.IsNullOrEmpty(text))
            {
                throw new IrbisNetworkException
                    (
                        "No file " + fileName
                    );
            }

            IrbisUpperCaseTable result = ParseText
                (
                    IrbisEncoding.Ansi,
                    text
                );

            return result;
        }

#if !WIN81

        /// <summary>
        /// Парсим локальный файл.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable ParseLocalFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamReader reader
                = new StreamReader
                    (
                        File.OpenRead(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                string text = reader.ReadToEnd();

                return ParseText
                    (
                        IrbisEncoding.Ansi,
                        text
                    );
            }
        }

#endif

        /// <summary>
        /// Parse text and build upper-case table.
        /// </summary>
        [NotNull]
        public static IrbisUpperCaseTable ParseText
            (
                [NotNull] Encoding encoding,
                [NotNull] string text
            )
        {
            Code.NotNull(encoding, "encoding");
            Code.NotNullNorEmpty(text, "text");

            List<byte> table = new List<byte>(256);

            MatchCollection matches = Regex.Matches(text, @"\d+");
            foreach (Match match in matches)
            {
                byte b = byte.Parse(match.Value);
                table.Add(b);
            }

            IrbisUpperCaseTable result = new IrbisUpperCaseTable
                (
                    encoding,
                    table.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Converts specified character to uppercase.
        /// </summary>
        public char ToUpper
            (
                char c
            )
        {
            string text = new string(c, 1);

            return ToUpper(text)[0];
        }

        /// <summary>
        /// Формируем исходный код с определением таблицы.
        /// </summary>
        public void ToSourceCode
            (
                [NotNull] TextWriter writer
            )
        {
            int count = 0;

            writer.WriteLine("new byte [] {");
            foreach (byte c in _table)
            {
                if (count == 0)
                {
                    writer.Write("   ");
                }

                writer.Write(" ");
                writer.Write(c.ToString("000"));
                writer.Write(",");

                count++;
                if (count > 10)
                {
                    count = 0;
                    writer.WriteLine();
                }
            }
            writer.WriteLine();
            writer.WriteLine("};");
        }

        /// <summary>
        /// Converts specified string to uppercase.
        /// </summary>
        [NotNull]
        public string ToUpper
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            byte[] bytes = _encoding.GetBytes(text);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = _table[bytes[i]];
            }

            string result = _encoding.GetString(bytes);

            return result;
        }


#if !WIN81

        /// <summary>
        /// Записываемся в файл.
        /// </summary>
        public void WriteLocalFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            using (StreamWriter writer = new StreamWriter
                    (
                        File.Create(fileName),
                        IrbisEncoding.Ansi
                    ))
            {
                WriteTable(writer);
            }
        }

#endif

        /// <summary>
        /// Записываемся в поток.
        /// </summary>
        public void WriteTable
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            int count = 0;

            foreach (byte b in _table)
            {
                if (count != 0)
                {
                    writer.Write(" ");
                }
                writer.Write
                    (
                        "{0:000}",
                        b
                    );
                count++;
                if (count == 32)
                {
                    writer.WriteLine();
                    count = 0;
                }
            }
        }

        #endregion
    }
}
