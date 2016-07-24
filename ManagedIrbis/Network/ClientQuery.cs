/* IrbisClientQuery.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Клиентский пакет с запросом к серверу.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{CommandCode} {Workstation}"
        + " {ClientID} {CommandNumber}")]
    public sealed class ClientQuery
        : IVerifiable
    {
        #region Constants

        /// <summary>
        /// Разделитель строк в заголовке пакета.
        /// </summary>
        public const char Delimiter = '\x0A';

        #endregion

        #region Properties

        /// <summary>
        /// Код команды.
        /// </summary>
        [CanBeNull]
        public string CommandCode { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public int CommandNumber { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [CanBeNull]
        public string UserLogin { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [CanBeNull]
        public string UserPassword { get; set; }

        /// <summary>
        /// Аргументы команды.
        /// </summary>
        [NotNull]
        [ItemCanBeNull]
        public List<object> Arguments { get { return _arguments; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ClientQuery()
        {
            _arguments = new List<object>();
        }

        #endregion

        #region Private members

        private readonly List<object> _arguments;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление аргумента в список.
        /// </summary>
        [NotNull]
        public ClientQuery Add
            (
                [CanBeNull] object argument
            )
        {
            Arguments.Add(argument);

            return this;
        }

        /// <summary>
        /// Очистка списка аргументов.
        /// </summary>
        public ClientQuery Clear()
        {
            Arguments.Clear();

            return this;
        }

        /// <summary>
        /// Дамп запроса.
        /// </summary>
        public void Dump
            (
                [NotNull] TextWriter writer
            )
        {
            writer.WriteLine("Command code: '{0}'", CommandCode);
            writer.WriteLine
                (
                    "Workstation: '{0}'",
                    (char)Workstation
                );
            writer.WriteLine("Client ID: {0}", ClientID);
            writer.WriteLine("Command number: {0}", CommandNumber);
            writer.WriteLine
                (
                    "Login: '{0}'",
                    UserLogin.ToVisibleString()
                );
            writer.WriteLine
                (
                    "Password: '{0}'",
                    UserPassword.ToVisibleString()
                );

            writer.WriteLine("Arguments:");
            foreach (object argument in Arguments)
            {
                if (ReferenceEquals(argument, null))
                {
                    writer.WriteLine("(null)");
                }
                else
                {
                    Type type = argument.GetType();
                    writer.WriteLine
                    (
                        "{0}: {1}",
                        type,
                        argument.NullableToVisibleString()
                    );
                }
            }

            writer.WriteLine("------------------");
        }

        /// <summary>
        /// Кодирование пакета.
        /// </summary>
        [NotNull]
        public byte[] EncodePacket ()
        {
            MemoryStream result = new MemoryStream();

            result
                .EncodeString(CommandCode)      .EncodeDelimiter()
                .EncodeWorkstation(Workstation) .EncodeDelimiter()
                .EncodeString(CommandCode)      .EncodeDelimiter()
                .EncodeInt32(ClientID)          .EncodeDelimiter()
                .EncodeInt32(CommandNumber)     .EncodeDelimiter()
                .EncodeString(UserPassword)     .EncodeDelimiter()
                .EncodeString(UserLogin)        .EncodeDelimiter()

                // Три пустые перевода строки
                .EncodeDelimiter()
                .EncodeDelimiter()
                .EncodeDelimiter()

                // Всего десять строк
                ;

            foreach (object argument in Arguments)
            {
                result.EncodeAny(argument);
                result.EncodeDelimiter();
            }

            byte[] preResult =  result.ToArray();
            result = new MemoryStream();
            int length = preResult.Length;
            result
                .EncodeInt32(length)
                .EncodeDelimiter()
                .Write(preResult, 0, preResult.Length);

            return result.ToArray();
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Проверка, правильно ли заполнены поля запроса.
        /// </summary>
        public bool Verify
            (
                bool throwException
            )
        {
            bool result = !string.IsNullOrEmpty(CommandCode)
                && (Workstation != IrbisWorkstation.None)
                && (ClientID != 0)
                && (CommandNumber != 0)
                //&& !string.IsNullOrEmpty(UserLogin)
                //&& !string.IsNullOrEmpty(UserPassword)
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
