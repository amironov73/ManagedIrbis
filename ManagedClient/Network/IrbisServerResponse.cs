/* IrbisServerResponse.cs -- пакет с ответом сервера.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Network
{
    /// <summary>
    /// Пакет с ответом сервера.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisServerResponse
    {
        #region Constants

        /// <summary>
        /// Разделитель.
        /// </summary>
        public const string Delimiter = "\x0D\x0A";

        #endregion

        #region Properties

        /// <summary>
        /// Команда клиента.
        /// </summary>
        [CanBeNull]
        public string CommandCode { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public int CommandNumber { get; set; }

        /// <summary>
        /// Размер ответа сервера в байтах.
        /// </summary>
        public int AnswerSize { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка, правильно ли заполнены поля ответа.
        /// </summary>
        public bool Verify
            (
                bool throwException
            )
        {
            bool result = !string.IsNullOrEmpty(CommandCode)
                && (ClientID != 0)
                && (CommandNumber != 0)
                ;

            if (throwException && !result)
            {
                throw new ApplicationException();
            }

            return result;
        }


        #endregion

        #region Object members

        #endregion
    }
}
