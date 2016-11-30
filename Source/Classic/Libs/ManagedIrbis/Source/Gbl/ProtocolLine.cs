// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProtocolLine.cs -- GBL execution result for one record.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// GBL execution result for one record.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ProtocolLine
    {
        #region Properties

        /// <summary>
        /// Общий признак успеха.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Имя базы данных
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Результат Autoin.gbl
        /// </summary>
        [CanBeNull]
        public string Autoin { get; set; }

        /// <summary>
        /// UPDATE=
        /// </summary>
        [CanBeNull]
        public string Update { get; set; }

        /// <summary>
        /// STATUS=
        /// </summary>
        [CanBeNull]
        public string Status { get; set; }

        /// <summary>
        /// Код ошибки, если есть
        /// </summary>
        [CanBeNull]
        public string Error { get; set; }

        /// <summary>
        /// UPDUF=
        /// </summary>
        [CanBeNull]
        public string UpdUf { get; set; }

        /// <summary>
        /// Исходный текст (до парсинга)
        /// </summary>
        [CanBeNull]
        public string Text { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        // Типичная строка с положительным результатом:
        // DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#

        // Типичная строка с отрицательным результатом
        // DBN=IBIS#MFN=4#GBL_ERROR=-605

        /// <summary>
        /// Parse one text line.
        /// </summary>
        [NotNull]
        public static ProtocolLine Parse
            (
                [NotNull] string line
            )
        {
            Code.NotNullNorEmpty(line, line);

            ProtocolLine result = new ProtocolLine
            {
                Text = line,
                Success = true
            };
            string[] parts = line.Split('#');
            foreach (string part in parts)
            {
                string[] p = part.Split('=');
                if (p.Length > 0)
                {
                    string name = p[0].ToUpper();
                    string value = string.Empty;
                    if (p.Length > 1)
                    {
                        value = p[1];
                    }
                    switch (name)
                    {
                        case "DBN":
                            result.Database = value;
                            break;
                        case "MFN":
                            result.Mfn = value.SafeToInt32();
                            break;
                        case "AUTOIN":
                            result.Autoin = value;
                            break;
                        case "UPDATE":
                            result.Update = value;
                            break;
                        case "STATUS":
                            result.Status = value;
                            break;
                        case "UPDUF":
                            result.UpdUf = value;
                            break;
                        case "GBL_ERROR":
                            result.Error = value;
                            result.Success = false;
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parse server response.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ProtocolLine[] Parse
            (
                [NotNull] ServerResponse response
            )
        {
            Code.NotNull(response, "response");

            List<ProtocolLine> result = new List<ProtocolLine>();

            while (true)
            {
                string line = response.GetAnsiString();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                ProtocolLine item = Parse(line);
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region Object members


        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return Text.ToVisibleString();
        }

        #endregion
    }
}
