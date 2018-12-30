// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ServerResponse.cs -- server response network packet
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 * TODO make stream non-closeable
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnsafeAM;
using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

using UnsafeIrbis.ImportExport;

#endregion

namespace UnsafeIrbis.Infrastructure
{
    /// <summary>
    /// Server response network packet.
    /// </summary>
    [PublicAPI]
    public sealed class ServerResponse //-V3072
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
        public IIrbisConnection Connection { get; private set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        // ReSharper disable InconsistentNaming
        public int ClientID { get; set; }
        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public int CommandNumber { get; set; }

        /// <summary>
        /// Размер ответа сервера в байтах.
        /// </summary>
        public int AnswerSize { get; set; }

        /// <summary>
        /// Server version.
        /// </summary>
        [CanBeNull]
        public string ServerVersion { get; set; }

        /// <summary>
        /// Код возврата.
        /// </summary>
        public int ReturnCode
        {
            get { return GetReturnCode(); }
            set
            {
                _returnCode = value;
                ReturnCodeRetrieved = true;
            }
        }

        /// <summary>
        /// Raw server response.
        /// </summary>
        [NotNull]
        public byte[] RawAnswer { get; private set; }

        /// <summary>
        /// Raw client request.
        /// </summary>
        [NotNull]
        public byte[][] RawRequest { get; private set; }

        /// <summary>
        /// Relax return code check.
        /// </summary>
        public bool Relaxed { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServerResponse
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] byte[] rawAnswer,
                [NotNull] byte[][] rawRequest,
                bool relax
            )
        {
            Code.NotNull(connection, nameof(connection));
            Code.NotNull(rawAnswer, nameof(rawAnswer));
            Code.NotNull(rawRequest, nameof(rawRequest));

            Connection = connection;

            RawAnswer = rawAnswer;
            RawRequest = rawRequest;
            _stream = new MemoryStream(rawAnswer);
            Relaxed = relax;

            if (relax)
            {
                return;
            }

            CommandCode = RequireAnsiString();
            ClientID = RequireInt32();
            CommandNumber = RequireInt32();
            AnswerSize = GetInt32(0);
            ServerVersion = RequireAnsiString();

            // 5 пустых строк
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
            RequireAnsiString();
        }

        #endregion

        #region Private members

        private MemoryStream _stream;

        private int _returnCode;

        //private long _savedPosition;

        internal bool ReturnCodeRetrieved;

        #endregion

        #region Public methods

        /// <summary>
        /// Get ANSI string.
        /// </summary>
        [CanBeNull]
        public string GetAnsiString()
        {
            throw null;

            //MemoryStream memory = Connection.Executive.GetMemoryStream(GetType());
            //_savedPosition = _stream.Position;
            //while (true)
            //{
            //    int code = _stream.ReadByte();
            //    if (code < 0)
            //    {
            //        return null;
            //    }
            //    if (code == 0x0D)
            //    {
            //        code = _stream.ReadByte();
            //        if (code == 0x0A)
            //        {
            //            break;
            //        }
            //        memory.WriteByte(0x0D);
            //    }
            //    memory.WriteByte((byte)code);
            //}

            //byte[] bytes = memory.ToArray();
            //Connection.Executive.ReportMemoryUsage
            //    (
            //        GetType(),
            //        bytes.Length
            //    );
            //Connection.Executive.RecycleMemoryStream(memory);

            //string result = IrbisEncoding.Ansi.GetString
            //    (
            //        bytes,
            //        0,
            //        bytes.Length
            //    );

            //return result;
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
            Code.Positive(count, nameof(count));

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
        /// Get array of ANSI strings.
        /// </summary>
        /// <returns><c>null</c>if there is no lines in
        /// the server response, otherwise missing lines will
        /// be added (as empty lines).</returns>
        [CanBeNull]
        public string[] GetAnsiStringsPlus
            (
                int count
            )
        {
            Code.Positive(count, nameof(count));

            List<string> result = new List<string>(count);
            int index = 0;
            string line;
            for (; index < 1; index++)
            {
                line = GetAnsiString();
                if (ReferenceEquals(line, null))
                {
                    return null;
                }
                result.Add(line);
            }
            for (; index < count; index++)
            {
                line = GetAnsiString();
                result.Add
                    (
                        ReferenceEquals(line, null)
                        ? string.Empty
                        : line
                    );
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get dump.
        /// </summary>
        [NotNull]
        public byte[] GetDump()
        {
            throw null;

            //MemoryStream memory = Connection.Executive
            //    .GetMemoryStream(typeof(ServerResponse));
            //_stream.Position = _savedPosition;
            //while (true)
            //{
            //    int code = _stream.ReadByte();
            //    if (code < 0
            //        || code == 0x0D
            //        || code == 0x0A)
            //    {
            //        break;
            //    }
            //    memory.WriteByte((byte)code);
            //}

            //byte[] result = memory.ToArray();
            //Connection.Executive.RecycleMemoryStream(memory);

            //return result;
        }

        /// <summary>
        /// Get dump as string.
        /// </summary>
        [NotNull]
        public string GetDumpAsString()
        {
            throw null;

            //StringBuilder result = new StringBuilder();
            //int count = 0;

            //_stream.Position = _savedPosition;
            //while (true)
            //{
            //    int code = _stream.ReadByte();
            //    if (code < 0
            //       || code == 0x0D
            //       || code == 0x0A)
            //    {
            //        break;
            //    }

            //    if (count == 16)
            //    {
            //        count = 0;
            //        result.AppendLine();
            //    }

            //    if (count != 0)
            //    {
            //        result.Append(" ");
            //    }

            //    result.AppendFormat("{0:X2}", code);

            //    count++;
            //}

            //return result.ToString();
        }

        /// <summary>
        /// Get empty response.
        /// </summary>
        [NotNull]
        public static ServerResponse GetEmptyResponse
            (
                IIrbisConnection connection
            )
        {
            byte[] empty = EmptyArray<byte>.Value;

            ServerResponse result = new ServerResponse
                (
                    connection,
                    empty,
                    new[] { empty, empty },
                    true
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

            if (!NumericUtility.TryParseInt32(line, out result))
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
            Code.NotNull(encoding, nameof(encoding));

            StreamReader result = new StreamReader
                (
                    _stream,
                    encoding,
                    false,
                    1024,
                    true
                );

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
            Code.NotNull(encoding, nameof(encoding));

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
            Code.NotNull(record, nameof(record));

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
            if (Relaxed)
            {
                return _returnCode;
            }

            if (!ReturnCodeRetrieved)
            {
                _returnCode = RequireInt32();
                ReturnCodeRetrieved = true;
            }

            return _returnCode;
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
            Code.Nonnegative(offset, nameof(offset));
            Code.Nonnegative(length, nameof(length));

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
        public byte[] GetAnswerCopy()
        {
            if (ReferenceEquals(RawAnswer, null))
            {
                throw new IrbisException("packet is null");
            }

            byte[] result = RawAnswer.GetSpan((int)_stream.Position);

            return result;
        }

        /// <summary>
        /// Get stream with current state.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public Stream GetStream()
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
            Code.Nonnegative(offset, nameof(offset));
            Code.Nonnegative(length, nameof(length));

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
            throw null;

            //MemoryStream memory = Connection.Executive
            //    .GetMemoryStream(GetType());
            //_savedPosition = _stream.Position;
            //while (true)
            //{
            //    int code = _stream.ReadByte();
            //    if (code < 0)
            //    {
            //        return null;
            //    }
            //    if (code == 0x0D)
            //    {
            //        code = _stream.ReadByte();
            //        if (code == 0x0A)
            //        {
            //            break;
            //        }
            //        memory.WriteByte(0x0D);
            //    }
            //    memory.WriteByte((byte)code);
            //}

            //string result;
            //byte[] buffer = memory.ToArray();
            //Connection.Executive.ReportMemoryUsage
            //    (
            //        GetType(),
            //        buffer.Length
            //    );
            //Connection.Executive.RecycleMemoryStream(memory);

            //try
            //{
            //    result = IrbisEncoding.Utf8.GetString
            //        (
            //            buffer,
            //            0,
            //            buffer.Length
            //        );
            //}
            //catch (Exception inner)
            //{
            //    Log.TraceException
            //        (
            //            "ServerResponse::GetUtfString",
            //            inner
            //        );

            //    IrbisException outer = new IrbisException
            //        (
            //            "ServerResponse::GetUtfString failed",
            //            inner
            //        );
            //    BinaryAttachment attachment = new BinaryAttachment
            //        (
            //            "problem line",
            //            GetDump()
            //        );
            //    outer.Attach(attachment);
            //    throw outer;
            //}

            //return result;
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
            Code.Positive(count, nameof(count));

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
        /// Refuse an return code (for commands that
        /// doesn't return any codes).
        /// </summary>
        public void RefuseAnReturnCode()
        {
            ReturnCodeRetrieved = true;
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
            if (!NumericUtility.TryParseInt32(line, out int result))
            {
                Log.Error
                    (
                        "ServerResponse::RequireInt32: "
                        + "bad format="
                        + line.ToVisibleString()
                    );

                throw new IrbisNetworkException();
            }

            return result;
        }

        /// <summary>
        /// Get remaining length of the response.
        /// </summary>
        public int GetRemainingLength()
        {
            return (int)(RawAnswer.Length - _stream.Position);
        }

        /// <summary>
        /// Peek remaining text.
        /// </summary>
        [NotNull]
        public string PeekRemainingUtf8()
        {
            string result = IrbisEncoding.Utf8.GetString
                (
                    RawAnswer,
                    (int)_stream.Position,
                    (int)(RawAnswer.Length - _stream.Position)
                );

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            if (Relaxed)
            {
                return true;
            }

            Verifier<ServerResponse> verifier
                = new Verifier<ServerResponse>(this, throwOnError);

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
