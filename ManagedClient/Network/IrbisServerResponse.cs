/* IrbisServerResponse.cs -- пакет с ответом сервера.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
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
        /// Connection used.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

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

        /// <summary>
        /// Код возврата.
        /// </summary>
        public int ReturnCode { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisServerResponse
            (
                [NotNull] IrbisConnection connection
            )
        {
            Code.NotNull(connection, "connection");

            Connection = connection;
        }

        #endregion

        #region Private members

        private Stream _stream;

        private bool _returnCodeRetrieved;

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public List<string> RemainingAnsiStrings()
        {
            List<string> result = new List<string>();

            string line;
            while ((line = GetAnsiString()) != null)
            {
                result.Add(line);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public List<string> RemainingUtfStrings()
        {
            List<string> result = new List<string>();

            string line;
            while ((line = GetUtfString()) != null)
            {
                result.Add(line);
            }

            return result;
        }

        /// <summary>
        /// Get ANSI string.
        /// </summary>
        [CanBeNull]
        public string GetAnsiString()
        {
            MemoryStream memory = new MemoryStream();

            while (true)
            {
                int code = _stream.ReadByte();
                if (code < 0)
                {
                    return null;
                }
                if (code == 0x0D)
                {
                    code = _stream.ReadByte();
                    if (code == 0x0A)
                    {
                        break;
                    }
                    memory.WriteByte(0x0D);
                }
                memory.WriteByte((byte)code);
            }

            string result = IrbisEncoding.Ansi.GetString
                (
                    memory.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Get Utf string.
        /// </summary>
        [CanBeNull]
        public string GetUtfString()
        {
            MemoryStream memory = new MemoryStream();

            while (true)
            {
                int code = _stream.ReadByte();
                if (code < 0)
                {
                    return null;
                }
                if (code == 0x0D)
                {
                    code = _stream.ReadByte();
                    if (code == 0x0A)
                    {
                        break;
                    }
                    memory.WriteByte(0x0D);
                }
                memory.WriteByte((byte)code);
            }

            string result = IrbisEncoding.Utf8.GetString
                (
                    memory.ToArray()
                );

            return result;
        }

        /// <summary>
        /// Get 32-bit integer value.
        /// </summary>
        public int GetInt32
            (
                int defaultValue
            )
        {
            string line = GetAnsiString();
            int result;
            if (!int.TryParse(line, out result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get return code.
        /// </summary>
        public int GetReturnCode()
        {
            if (!_returnCodeRetrieved)
            {
                ReturnCode = RequireInt32();
                _returnCodeRetrieved = true;
            }

            return ReturnCode;
        }

        /// <summary>
        /// Parse the buffer.
        /// </summary>
        public static IrbisServerResponse Parse
            (
                IrbisConnection connection,
                byte[] buffer
            )
        {
            IrbisServerResponse result = new IrbisServerResponse (connection)
            {
                _stream = new MemoryStream(buffer)
            };
            result.CommandCode = result.RequireAnsiString();
            result.ClientID = result.RequireInt32();
            result.CommandNumber = result.RequireInt32();
            result.AnswerSize = result.RequireInt32();

            // 6 пустых строк
            result.RequireAnsiString();
            result.RequireAnsiString();
            result.RequireAnsiString();
            result.RequireAnsiString();
            result.RequireAnsiString();
            result.RequireAnsiString();

            return result;
        }

        /// <summary>
        /// Require ANSI string.
        /// </summary>
        [NotNull]
        public string RequireAnsiString()
        {
            string result = GetAnsiString();
            if (ReferenceEquals(result, null))
            {
                throw new IrbisNetworkException();
            }

            return result;
        }

        /// <summary>
        /// Require 32-bit integer.
        /// </summary>
        public int RequireInt32()
        {
            string line = GetAnsiString();
            int result;
            if (!int.TryParse(line, out result))
            {
                throw new IrbisNetworkException();
            }

            return result;
        }

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
                throw new IrbisException();
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
