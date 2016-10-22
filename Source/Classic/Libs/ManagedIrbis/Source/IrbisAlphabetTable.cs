/* IrbisAlphabetTable.cs -- ISISAC.TAB
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    // Таблица алфавитных символов используется системой ИРБИС
    // при разбиении текста на слова и представляет собой список
    // кодов символов, которые считаются алфавитными.
    // Таблица реализована в виде текстового файла.
    // Местонахождение и имя файла по умолчанию:
    // <IRBIS_SERVER_ROOT>\ISISACW.TAB.
    // Местонахождение и имя файла определяется значением
    // параметра ACTABPATH в конфигурационном файле
    // ИРБИС и может быть изменено.
    //
    // Стандартное содержимое
    // 038 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 097 098 099 100
    // 101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 128 129 130 131 132 133 134 135 136 137
    // 138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169
    // 170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201
    // 202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233
    // 234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255


    /// <summary>
    /// ISISAC.TAB
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisAlphabetTable
    {
        #region Constants

        /// <summary>
        /// Имя файла таблицы по умолчанию.
        /// </summary>
        public const string FileName = "ISISACW.TAB";

        #endregion

        #region Properties

        /// <summary>
        /// Собственно таблица.
        /// </summary>
        public char[] Characters { get { return _characters; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                [NotNull] Encoding encoding, 
                [NotNull] byte[] table
            )
        {
            Code.NotNull(encoding, "encoding");
            Code.NotNull(table, "table");

            _encoding = encoding;
            _table = table;
            _characters = encoding.GetChars(table);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                [NotNull] IrbisConnection client,
                [NotNull] string fileName
            )
        {
            Code.NotNull(client, "client");
            Code.NotNullNorEmpty(fileName, "fileName");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisAlphabetTable
            (
                IrbisConnection client
            )
            : this ( client, FileName )
        {
        }

        #endregion

        #region Private members

        private Encoding _encoding;

        private byte[] _table;

        private char[] _characters;

        private void _CharToSourceCode
            (
                TextWriter writer,
                char c
            )
        {
            if (c < ' ')
            {
                writer.Write
                    (
                        @"'\x{0:X2}'",
                        (int)c
                    );
            }
            else
            {
                writer.Write
                    (
                        "'{0}'",
                        c
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the specified character is alpha
        /// according to table.
        /// </summary>
        public bool IsAlpha
            (
                char c
            )
        {
            return Array.IndexOf(_characters, c) >= 0;
        }

#if !WIN81

        /// <summary>
        /// Парсим локальный файл.
        /// </summary>
        [NotNull]
        public static IrbisAlphabetTable ParseLocalFile
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
                return ParseText(reader);
            }
        }

#endif

        /// <summary>
        /// Парсим таблицу из текстового представления.
        /// </summary>
        [NotNull]
        public static IrbisAlphabetTable ParseText
            (
                [NotNull] TextReader reader
            )
        {
            List<byte> table = new List<byte>(182);

            string text = reader.ReadToEnd();
            MatchCollection matches = Regex.Matches(text, @"\d+");
            foreach (Match match in matches)
            {
                byte b = byte.Parse(match.Value);
                table.Add(b);
            }

            IrbisAlphabetTable result = new IrbisAlphabetTable
                (
                    IrbisEncoding.Ansi,
                    table.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Разбиваем текст на слова.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] SplitWords
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string[0];
            }

            List<string> result = new List<string>();
            StringBuilder accumulator = new StringBuilder();

            foreach (char c in text)
            {
                if (IsAlpha(c))
                {
                    accumulator.Append(c);
                }
                else
                {
                    if (accumulator.Length != 0)
                    {
                        result.Add(accumulator.ToString());
                        accumulator.Length = 0;
                    }
                }
            }

            if (accumulator.Length != 0)
            {
                result.Add(accumulator.ToString());
            }

            return result.ToArray();
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

            writer.WriteLine("new char[] {");
            foreach (char c in Characters)
            {
                if (count == 0)
                {
                    writer.Write("   ");
                }

                writer.Write(" ");
                _CharToSourceCode(writer, c);
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

