// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PacketInterpreter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Infrastructure
{
    //
    // Код Значение
    // A   Строка ANSI
    // U   Строка UTF8
    // I   Целое число
    // R   Запись с разделителями 1F-1E
    // M   Запись с разделителями 0A-0D
    // T   Текст до конца пакета (ANSI)
    // L   Строки UTF8 до конца

    /// <summary>
    /// Network packet interpreter.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PacketInterpreter //-V3072
    {
        #region Properties

        /// <summary>
        /// Stream.
        /// </summary>
        [NotNull]
        public Stream Stream { get { return _stream; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PacketInterpreter
            (
                [NotNull] byte [] packet
            )
        {
            Code.NotNull(packet, "packet");

            _packet = packet;
            _stream = new MemoryStream(packet);
        }

        #endregion

        #region Private members

        private readonly byte[] _packet;
        private readonly MemoryStream _stream;

        #endregion

        #region Public methods

        /// <summary>
        /// Get ANSI string.
        /// </summary>
        [CanBeNull]
        public string GetAnsiString()
        {
            using (MemoryStream memory = new MemoryStream())
            {
                while (true)
                {
                    int code = Stream.ReadByte();
                    if (code < 0)
                    {
                        return null;
                    }

                    if (code == 0x0D)
                    {
                        code = Stream.ReadByte();
                        if (code == 0x0A)
                        {
                            break;
                        }

                        memory.WriteByte(0x0D);
                    }

                    if (code == 0x0A)
                    {
                        break;
                    }

                    memory.WriteByte((byte) code);
                }

                byte[] bytes = memory.ToArray();

                string result = EncodingUtility.GetString
                    (
                        IrbisEncoding.Ansi,
                        bytes
                    );

                return result;
            }
        }

        /// <summary>
        /// GetObject by code.
        /// </summary>
        [CanBeNull]
        public object GetObject
            (
                char code
            )
        {
            switch (code)
            {
                case 'A': // Строка ANSI
                    return GetAnsiString();

                case 'U': // Строка UTF8
                    return GetUtfString();

                case 'I': // Целое число
                    return GetInt32();

                case 'R': // Запись с разделителями 1F-1E
                    break;

                case 'M': // Запись с разделителями 0A-0D
                    break;

                case 'T': // Текст до конца пакета (ANSI)
                    return RemainingAnsiText();

                case 'L': // Строки UTF8 до конца
                    return RemainingUtfText();
            }

            throw new IrbisNetworkException();
        }

        /// <summary>
        /// Get Utf string.
        /// </summary>
        [CanBeNull]
        public string GetUtfString()
        {
            using (MemoryStream memory = new MemoryStream())
            {
                while (true)
                {
                    int code = _stream.ReadByte();
                    if (code < 0)
                    {
                        return null;
                    }

                    if (code == 0x0D)
                    {
                        code = Stream.ReadByte();
                        if (code == 0x0A)
                        {
                            break;
                        }

                        memory.WriteByte(0x0D);
                    }

                    if (code == 0x0A)
                    {
                        break;
                    }

                    memory.WriteByte((byte) code);
                }

                byte[] buffer = memory.ToArray();
                string result = IrbisEncoding.Utf8
                    .GetString(buffer, 0, buffer.Length);

                return result;
            }
        }

        /// <summary>
        /// Get 32-bit integer value.
        /// </summary>
        public int GetInt32 ()
        {
            string line = GetAnsiString();

            int result;

            if (!NumericUtility.TryParseInt32(line, out result))
            {
                Log.Error
                    (
                        "PacketInterpreter::GetInt32: "
                        + "bad format="
                        + line.ToVisibleString()
                    );

                throw new IrbisNetworkException();
            }

            return result;
        }

        /// <summary>
        /// Interpret the packet.
        /// </summary>
        [NotNull]
        public static string[] Interpret
            (
                [NotNull] byte[] packet,
                [NotNull] string specification
            )
        {
            Code.NotNull(packet, "packet");
            Code.NotNullNorEmpty(specification, "specification");

            List<string> result = new List<string>();
            PacketInterpreter interpreter = new PacketInterpreter(packet);
            foreach (char code in specification)
            {
                object o = interpreter.GetObject(code);
                string s = o.ToVisibleString();
                result.Add(s);
            }

            return result.ToArray();
        }

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
        public string RemainingAnsiText()
        {
            using (MemoryStream memory = new MemoryStream())
            {

                while (true)
                {
                    int code = Stream.ReadByte();
                    if (code < 0)
                    {
                        break;
                    }

                    memory.WriteByte((byte) code);
                }

                byte[] buffer = memory.ToArray();
                string result = IrbisEncoding.Ansi
                    .GetString(buffer, 0, buffer.Length);

                return result;
            }
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
        ///
        /// </summary>
        public string RemainingUtfText()
        {
            using (MemoryStream memory = new MemoryStream())
            {
                while (true)
                {
                    int code = Stream.ReadByte();
                    if (code < 0)
                    {
                        break;
                    }

                    memory.WriteByte((byte) code);
                }

                byte[] buffer = memory.ToArray();
                string result = IrbisEncoding.Utf8
                    .GetString(buffer, 0, buffer.Length);

                return result;
            }
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

            if (!NumericUtility.TryParseInt32(line, out result))
            {
                Log.Error
                    (
                        "PacketInterpreter::RequireInt32: "
                        + "bad format="
                        + line.ToVisibleString()
                    );

                throw new IrbisNetworkException();
            }

            return result;
        }

        #endregion
    }
}
