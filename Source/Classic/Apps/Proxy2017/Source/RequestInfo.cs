// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RequestInfo.cs -- 
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
    /// Запрос клиента в разобранном виде.
    /// </summary>
    class RequestInfo
    {
        /// <summary>
        /// Код команды.
        /// </summary>
        public string Command1 { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        public string Workstation { get; set; }

        /// <summary>
        /// Код команды (повтор).
        /// </summary>
        public string Command2 { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// Порядковый номер запроса.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Username { get; set; }

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
        /// Прочие данные, например, поисковый запрос.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Прочие данные в текстовом виде.
        /// </summary>
        public string[] Lines { get; set; }

        // ====================================================================

        /// <summary>
        /// Разбор клиентского запроса по полям заголовка.
        /// </summary>
        public static RequestInfo Parse
            (
                byte[] buffer
            )
        {
            HeaderParser parser = new HeaderParser(buffer);

            // Пропускаем заголовок, содержащий общую длину пакета
            parser.NextString();

            RequestInfo result = new RequestInfo
            {
                Command1 = parser.NextString(),
                Workstation = parser.NextString(),
                Command2 = parser.NextString(),
                UserID = parser.NextString(),
                Index = parser.NextString(),
                Password = parser.NextString(),
                Username = parser.NextString(),
                Reserv1 = parser.NextString(),
                Reserv2 = parser.NextString(),
                Reserv3 = parser.NextString(),
                Data = parser.NextBytes()
            };

            result.Lines = parser.SplitLines
                (
                    result.Data,
                    !new[] { "8", "A", "B" }
                        .Contains(result.Command1)
                );

            return result;
        }

        /// <summary>
        /// Разбор клиентского запроса по полям заголовка.
        /// </summary>
        public static RequestInfo Parse
            (
                byte[] buffer,
                bool useUtf
            )
        {
            HeaderParser parser = new HeaderParser(buffer);

            // Пропускаем заголовок, содержащий общую длину пакета
            parser.NextString();

            RequestInfo result = new RequestInfo
            {
                Command1 = parser.NextString(),
                Workstation = parser.NextString(),
                Command2 = parser.NextString(),
                UserID = parser.NextString(),
                Index = parser.NextString(),
                Password = parser.NextString(),
                Username = parser.NextString(),
                Reserv1 = parser.NextString(),
                Reserv2 = parser.NextString(),
                Reserv3 = parser.NextString(),
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
        /// Текстовое представление запроса.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Command1: {0}", Command1);
            result.AppendLine();
            result.AppendFormat("Workstation: {0}", Workstation);
            result.AppendLine();
            result.AppendFormat("Command2: {0}", Command2);
            result.AppendLine();
            result.AppendFormat("UserID: {0}", UserID);
            result.AppendLine();
            result.AppendFormat("Index: {0}", Index);
            result.AppendLine();
            result.AppendFormat("Password: {0}", Password);
            result.AppendLine();
            result.AppendFormat("Username: {0}", Username);
            //result.AppendLine();
            //result.AppendFormat("Reserv1: {0}", Reserv1);
            //result.AppendLine();
            //result.AppendFormat("Reserv2: {0}", Reserv2);
            //result.AppendLine();
            //result.AppendFormat("Reserv3: {0}", Reserv3);

            return result.ToString();
        }
    }
}
