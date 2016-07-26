/* ServerResponse.cs -- пакет с ответом сервера.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO make stream non-closable
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network
{
    /// <summary>
    /// Пакет с ответом сервера.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ServerResponse
        : IVerifiable
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

        /// <summary>
        /// Raw server response.
        /// </summary>
        [NotNull]
        public byte[] RawAnswer { get; private set; }

        /// <summary>
        /// Raw client request.
        /// </summary>
        [NotNull]
        public byte[] RawRequest { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerResponse
            (
                [NotNull] IrbisConnection connection,
                [NotNull] byte[] rawAnswer,
                [NotNull] byte[] rawRequest
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(rawAnswer, "rawAnswer");
            Code.NotNull(rawRequest, "rawRequest");

            Connection = connection;

            RawAnswer = rawAnswer;
            RawRequest = rawRequest;
            _stream = new MemoryStream(rawAnswer);
            CommandCode = RequireAnsiString();
            ClientID = RequireInt32();
            CommandNumber = RequireInt32();
            AnswerSize = RequireInt32();

            // 6 пустых строк
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
        }

        #endregion

        #region Private members

        private Stream _stream;

        internal bool _returnCodeRetrieved;

        #endregion

        #region Public methods

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
        /// Get array of ANSI strings.
        /// </summary>
        /// <returns><c>null</c>if there is not enough lines in
        /// the server response.</returns>
        [CanBeNull]
        public string[] GetAnsiStrings
            (
                int count
            )
        {
            Code.Positive(count, "count");

            List<string> result = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string line = GetAnsiString();
                if (ReferenceEquals(line, null))
                {
                    return null;
                }
                result.Add(line);
            }

            return result.ToArray();
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
        /// Get <see cref="TextReader"/>.
        /// </summary>
        [NotNull]
        public TextReader GetReader
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

#if FW45

            StreamReader result = new StreamReader
                (
                    _stream,
                    encoding,
                    false,
                    1024,
                    true
                );

#else
            // TODO make stream non-closeable

            StreamReader result = new StreamReader
                (
                    _stream,
                    encoding,
                    false
                );

#endif

            return result;
        }

        /// <summary>
        /// Get <see cref="TextReader"/>.
        /// </summary>
        [NotNull]
        public TextReader GetReaderCopy
            (
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(encoding, "encoding");

            Stream stream = GetStreamCopy();

            StreamReader result = new StreamReader
                (
                    stream,
                    encoding
                );

            return result;
        }

        /// <summary>
        /// Get <see cref="MarcRecord"/>.
        /// </summary>
        [CanBeNull]
        public MarcRecord GetRecord
            (
            [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string line = GetUtfString();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            ProtocolText.ParseResponseForReadRecord
                (
                    this,
                    record
                );

            return record;
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
        /// Get copy of the answer packet span.
        /// </summary>
        [NotNull]
        public byte[] GetAnswerCopy
            (
                int offset,
                int length
            )
        {
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(length, "length");

            if (ReferenceEquals(RawAnswer, null))
            {
                throw new IrbisException("packet is null");
            }

            byte[] result = RawAnswer.GetSpan(offset, length);

            return result;
        }

        /// <summary>
        /// Get copy of the answer packet.
        /// </summary>
        [NotNull]
        public byte[] GetAnswerCopy ()
        {
            if (ReferenceEquals(RawAnswer, null))
            {
                throw new IrbisException("packet is null");
            }

            byte[] result = RawAnswer.GetSpan((int) _stream.Position);

            return result;
        }

        /// <summary>
        /// Get stream with current state.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public Stream GetStream ()
        {
            if (ReferenceEquals(RawAnswer, null))
            {
                throw new IrbisException("packet is null");
            }

            return _stream;
        }

        /// <summary>
        /// Get stream with current state.
        /// </summary>
        [NotNull]
        public Stream GetStreamCopy()
        {
            byte[] buffer = GetAnswerCopy();
            Stream result = new MemoryStream(buffer);

            return result;
        }

        /// <summary>
        /// Get stream from the specified point.
        /// </summary>
        [NotNull]
        public Stream GetStream
            (
                int offset,
                int length
            )
        {
            Code.Nonnegative(offset, "offset");
            Code.Nonnegative(length, "length");

            if (ReferenceEquals(RawAnswer, null))
            {
                throw new IrbisException("packet is null");
            }

            MemoryStream result = new MemoryStream
                (
                    RawAnswer,
                    offset,
                    length
                );

            return result;
        }

        /// <summary>
        /// Get UTF-8 string.
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
        /// Get array of UTF-8 strings.
        /// </summary>
        /// <returns><c>null</c>if there is not enough lines in
        /// the server response.</returns>
        [CanBeNull]
        public string[] GetUtfStrings
            (
                int count
            )
        {
            Code.Positive(count, "count");

            List<string> result = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                string line = GetUtfString();
                if (ReferenceEquals(line, null))
                {
                    return null;
                }
                result.Add(line);
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
        [NotNull]
        public string RemainingAnsiText()
        {
            TextReader reader = GetReader(IrbisEncoding.Ansi);
            string result = reader.ReadToEnd();

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
        /// 
        /// </summary>
        [NotNull]
        public string RemainingUtfText()
        {
            TextReader reader = GetReader(IrbisEncoding.Utf8);
            string result = reader.ReadToEnd();

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
        /// Require UTF8 string.
        /// </summary>
        [NotNull]
        public string RequireUtfString()
        {
            string result = GetUtfString();
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

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwException
            )
        {
            Verifier<ServerResponse> verifier
                = new Verifier<ServerResponse>(this, throwException);

            verifier
                .NotNull(RawAnswer, "RawAnswer")
                .NotNull(RawRequest, "RawRequest")
                .NotNullNorEmpty(CommandCode, "CommandCode")
                .Assert(CommandNumber != 0, "CommandNumber");

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
