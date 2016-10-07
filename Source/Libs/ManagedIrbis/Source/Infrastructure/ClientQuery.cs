/* IrbisClientQuery.cs -- client packet with query to the server
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Client network packet with query to the server.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{CommandCode} {Workstation}"
        + " {ClientID} {CommandNumber}")]
#endif
    public sealed class ClientQuery
        : IVerifiable
    {
        #region Constants

        /// <summary>
        /// Line delimiter in packet header.
        /// </summary>
        public const char Delimiter = '\x0A';

        #endregion

        #region Properties

        /// <summary>
        /// Command code.
        /// </summary>
        [CanBeNull]
        public string CommandCode { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Client identifier.
        /// </summary>
        public int ClientID { get; set; }

        /// <summary>
        /// Sequential command number.
        /// </summary>
        public int CommandNumber { get; set; }

        /// <summary>
        /// User login.
        /// </summary>
        [CanBeNull]
        public string UserLogin { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [CanBeNull]
        public string UserPassword { get; set; }

        /// <summary>
        /// Command arguments.
        /// </summary>
        /// <remarks>List can be empty.</remarks>
        [NotNull]
        [ItemCanBeNull]
        public List<object> Arguments { get { return _arguments; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientQuery()
        {
            _arguments = new List<object>();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Command arguments.
        /// </summary>
        private readonly List<object> _arguments;

        #endregion

        #region Public methods

        /// <summary>
        /// Add arbitrary argument.
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
        /// Add ANSI text line.
        /// </summary>
        public ClientQuery AddAnsi
            (
                [CanBeNull] string text
            )
        {
            Arguments.Add
                (
                    new TextWithEncoding
                    (
                        text,
                        IrbisEncoding.Ansi
                    )
                );

            return this;
        }

        /// <summary>
        /// Add UTF8 text line.
        /// </summary>
        public ClientQuery AddUtf8
            (
                [CanBeNull] string text
            )
        {
            Arguments.Add
                (
                    new TextWithEncoding
                    (
                        text,
                        IrbisEncoding.Utf8
                    )
                );

            return this;
        }

        /// <summary>
        /// Clear argument list.
        /// </summary>
        public ClientQuery Clear()
        {
            Arguments.Clear();

            return this;
        }

        /// <summary>
        /// Dump the query.
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
        /// Build the packet.
        /// </summary>
        [NotNull]
        public byte[] EncodePacket()
        {
            MemoryStream result = new MemoryStream();

            // Query header: 7 lines
            result
                .EncodeString(CommandCode)      .EncodeDelimiter()
                .EncodeWorkstation(Workstation) .EncodeDelimiter()
                .EncodeString(CommandCode)      .EncodeDelimiter()
                .EncodeInt32(ClientID)          .EncodeDelimiter()
                .EncodeInt32(CommandNumber)     .EncodeDelimiter()
                .EncodeString(UserPassword)     .EncodeDelimiter()
                .EncodeString(UserLogin)        .EncodeDelimiter()

                // Three empty lines
                .EncodeDelimiter()
                .EncodeDelimiter()
                .EncodeDelimiter()

                // Total: 10 lines
                ;

            if (Arguments.Count != 0)
            {
                int countMinus1 = Arguments.Count - 1;
                for (int i = 0; i < countMinus1; i++)
                {
                    result.EncodeAny(Arguments[i]);
                    result.EncodeDelimiter();
                }
                for (int i = countMinus1; i < Arguments.Count; i++)
                {
                    result.EncodeAny(Arguments[i]);
                    // DO NOT add delimiter to the last line!
                }
            }

            byte[] preResult = result.ToArray();
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
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ClientQuery> verifier = new Verifier<ClientQuery>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(CommandCode, "CommandCode")
                .Assert
                (
                    Workstation != IrbisWorkstation.None,
                    "Workstation"
                )
                .Assert
                (
                    ClientID != 0,
                    "ClientID"
                )
                .Assert
                (
                    CommandNumber != 0,
                    "CommandNumber"
                );

            return verifier.Result;
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
            return string.Format
                (
                    "CommandCode: {0}, Workstation: {1}, "
                    + "ClientID: {2}, CommandNumber: {3}, "
                    + "UserLogin: {4}, UserPassword: {5}, "
                    + "Arguments: {6}",
                    CommandCode,
                    Workstation,
                    ClientID,
                    CommandNumber,
                    UserLogin,
                    UserPassword,
                    Arguments.Count
                );
        }

        #endregion
    }
}
