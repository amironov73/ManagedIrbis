// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ResponseInfo.cs -- 
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

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Информация об ответе сервера.
    /// </summary>
    class ResponseInfo
    {
        /// <summary>
        /// Код команды.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv1 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv2 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv3 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv4 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv5 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv6 { get; set; }

        /// <summary>
        /// Зарезервировано.
        /// </summary>
        public string Reserv7 { get; set; }

        /// <summary>
        /// Прочие данные, например, найденные записи.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Прочие данные в текстовом виде.
        /// </summary>
        public string[] Lines { get; set; }

        // ====================================================================

        /// <summary>
        /// Код возврата (не для всех ответов имеет смысл).
        /// </summary>
        public string ReturnCode
        {
            get
            {
                if ((Lines == null) || (Lines.Length == 0))
                {
                    return null;
                }
                string result = Lines[0];
                result = OnlyNumbersAllowed(result);
                return result;
            }
        }

        // ====================================================================

        private static string OnlyNumbersAllowed
            (
                string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int index = 0;
            if (text[0] == '-')
            {
                index++;
            }

            for (; index < text.Length; index++)
            {
                if (!char.IsDigit(text, index))
                {
                    return null;
                }
            }

            return text;
        }

        // ====================================================================

        /// <summary>
        /// Разбор ответа сервера по полям заголовка.
        /// </summary>
        public static ResponseInfo Parse
            (
                byte[] buffer,
                bool useUtf
            )
        {
            HeaderParser parser = new HeaderParser(buffer);
            ResponseInfo result = new ResponseInfo
            {
                Command = parser.NextString(),
                UserID = parser.NextString(),
                Index = parser.NextString(),
                Reserv1 = parser.NextString(),
                Reserv2 = parser.NextString(),
                Reserv3 = parser.NextString(),
                Reserv4 = parser.NextString(),
                Reserv5 = parser.NextString(),
                Reserv6 = parser.NextString(),
                Reserv7 = parser.NextString(),
                Data = parser.NextBytes()
            };

            result.Lines = parser.SplitLines
                (
                    result.Data,
                    useUtf
                );

            return result;
        }

        // ====================================================================

        /// <summary>
        /// Текстовое представление заголовка ответа.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Command: {0}", Command);
            result.AppendLine();
            result.AppendFormat("UserID: {0}", UserID);
            result.AppendLine();
            result.AppendFormat("Index: {0}", Index);
            //result.AppendLine();
            //result.AppendFormat("Reserv1: {0}", Reserv1);
            //result.AppendLine();
            //result.AppendFormat("Reserv2: {0}", Reserv2);
            //result.AppendLine();
            //result.AppendFormat("Reserv3: {0}", Reserv3);
            //result.AppendLine();
            //result.AppendFormat("Reserv4: {0}", Reserv4);
            //result.AppendLine();
            //result.AppendFormat("Reserv5: {0}", Reserv5);
            //result.AppendLine();
            //result.AppendFormat("Reserv6: {0}", Reserv6);
            //result.AppendLine();
            //result.AppendFormat("Reserv7: {0}", Reserv7);

            return result.ToString();
        }
    }
}
