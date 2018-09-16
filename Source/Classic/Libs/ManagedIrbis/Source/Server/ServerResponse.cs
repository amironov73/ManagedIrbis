// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerResponse.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Server response.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ServerResponse
    {
        #region Properties

        /// <summary>
        /// Memory.
        /// </summary>
        [NotNull]
        public MemoryStream Memory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor for mocking.
        /// </summary>
        public ServerResponse()
        {
            // To make Resharper happy
            Memory = new MemoryStream();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerResponse
            (
                [NotNull] ClientRequest request
            )
        {
            Code.NotNull(request, "request");

            Memory = new MemoryStream();

            WriteAnsiString(request.CommandCode1).NewLine();
            WriteAnsiString(request.ClientId).NewLine();
            WriteAnsiString(request.CommandNumber).NewLine();

            _prefix = Memory;
            Memory = new MemoryStream();

            // Тут должен идти размер ответа в байтах,
            // мы сформируем его в последнюю очередь

            // Для команды A может быть строка с версией сервера

            // Пять пустых переводов строки
            NewLine().NewLine().NewLine().NewLine().NewLine();

            // Дальше -- код возврата
        }

        #endregion

        #region Private members

        private MemoryStream _prefix;

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование ответа.
        /// </summary>
        [NotNull]
        public byte[][] Encode
            (
                [CanBeNull] string version
            )
        {
            Encoding ansi = IrbisEncoding.Ansi;
            byte[][] result = new byte[4][];
            result[0] = _prefix.ToArray();
            result[1] = ansi.GetBytes(FastNumber.Int64ToString(Memory.Length) + "\r\n");
            if (string.IsNullOrEmpty(version))
            {
                result[2] = new byte[] { 0x0D, 0x0A };
            }
            else
            {
                result[2] = ansi.GetBytes(version + "\r\n");
            }
            result[3] = Memory.ToArray();

            return result;
        }

        /// <summary>
        /// Write line break.
        /// </summary>
        [NotNull]
        public ServerResponse NewLine()
        {
            Memory.WriteByte(0x0D);
            Memory.WriteByte(0x0A);

            return this;
        }

        /// <summary>
        /// Write ANSI string.
        /// </summary>
        [NotNull]
        public ServerResponse WriteAnsiString
            (
                [CanBeNull] string line
            )
        {
            if (!string.IsNullOrEmpty(line))
            {
                byte[] bytes = IrbisEncoding.Ansi.GetBytes(line);
                Memory.Write(bytes, 0, bytes.Length);
            }

            return this;
        }

        /// <summary>
        /// Write integer.
        /// </summary>
        [NotNull]
        public ServerResponse WriteInt32
            (
                int value
            )
        {
            string line = FastNumber.Int32ToString(value);

            return WriteAnsiString(line);
        }

        /// <summary>
        /// Write UTF string.
        /// </summary>
        [NotNull]
        public ServerResponse WriteUtfString
            (
                [CanBeNull] string line
            )
        {
            if (!string.IsNullOrEmpty(line))
            {
                byte[] bytes = IrbisEncoding.Utf8.GetBytes(line);
                Memory.Write(bytes, 0, bytes.Length);
            }

            return this;
        }

        #endregion
    }
}
