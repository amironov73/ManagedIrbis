// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ResponseBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ResponseBuilder
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Estimated response size.
        /// </summary>
        public static int EstimatedSize = 3 * 1024;

        /// <summary>
        /// Memory.
        /// </summary>
        [NotNull]
        public MemoryStream Memory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseBuilder()
        {
            Memory = new MemoryStream(EstimatedSize);
        }

        #endregion

        #region Private members

        private static readonly byte[] _CRLF = { 0x0D, 0x0A };

        #endregion

        #region Public methods

        /// <summary>
        /// Append numeric value.
        /// </summary>
        [NotNull]
        public ResponseBuilder Append
            (
                int number
            )
        {
            string text = number.ToInvariantString();

            return AppendAnsi(text);
        }

        /// <summary>
        /// Append ANSI-encoded string.
        /// </summary>
        [NotNull]
        public ResponseBuilder AppendAnsi
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = IrbisEncoding.Ansi.GetBytes(text);
                Memory.Write(bytes, 0, bytes.Length);
            }

            return this;
        }

        /// <summary>
        /// Append OEM-encoded string.
        /// </summary>
        [NotNull]
        public ResponseBuilder AppendOem
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = IrbisEncoding.Oem.GetBytes(text);
                Memory.Write(bytes, 0, bytes.Length);
            }

            return this;
        }

        /// <summary>
        /// Append UTF-encoded string.
        /// </summary>
        [NotNull]
        public ResponseBuilder AppendUtf
            (
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = IrbisEncoding.Utf8.GetBytes(text);
                Memory.Write(bytes, 0, bytes.Length);
            }

            return this;
        }

        /// <summary>
        /// Append IRBIS line delimiter.
        /// </summary>
        [NotNull]
        public ResponseBuilder Delimiter()
        {
            Memory.WriteByte(0x1E);

            return this;
        }

        /// <summary>
        /// Encode the response.
        /// </summary>
        [NotNull]
        public byte[] Encode()
        {
            return Memory.ToArray();
        }

        /// <summary>
        /// Append new line.
        /// </summary>
        [NotNull]
        public ResponseBuilder NewLine()
        {
            Memory.Write(_CRLF, 0, 2);

            return this;
        }

        /// <summary>
        /// Standard header.
        /// </summary>
        [NotNull]
        public ResponseBuilder StandardHeader
            (
                [NotNull] string command,
                int clientId,
                int commandNumber
            )
        {
            Code.NotNull(command, "command");

            byte[] bytes = Memory.ToArray();
            Memory = new MemoryStream(Memory.Capacity + 100);

            AppendAnsi(command);
            NewLine();
            Append(clientId);
            NewLine();
            Append(commandNumber);
            NewLine();
            int responseSize = bytes.Length;
            Append(responseSize);
            NewLine();

            // 5 пустых переводов строки
            NewLine();
            NewLine();
            NewLine();
            NewLine();
            NewLine();

            // Предыдущее содержимое
            Memory.Write(bytes, 0, bytes.Length);

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Memory.Dispose();
        }

        #endregion

        #region Object members

        #endregion
    }
}
