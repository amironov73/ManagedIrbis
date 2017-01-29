// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TinyClient.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

#endregion

namespace TinyClient
{
    public sealed class SubField
    {
        public char Code { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return "^" + Code + Value;
        }
    }

    public sealed class SubFieldCollection
        : IEnumerable
    {
        private readonly ArrayList _list;

        public SubFieldCollection()
        {
            _list = new ArrayList();
        }

        public SubField this[int index]
        {
            get { return (SubField)_list[index]; }
            set { _list[index] = value; }
        }

        public void Add(SubField item)
        {
            _list.Add(item);
        }

        public void Remove(SubField item)
        {
            _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    public sealed class RecordField
    {
        private readonly SubFieldCollection _subFields;

        public string Tag { get; set; }

        public string Value { get; set; }

        public SubFieldCollection SubFields
        {
            get { return _subFields; }
        }

        public RecordField(string tag)
        {
            Tag = tag;
            _subFields = new SubFieldCollection();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(Tag);
            result.Append("#");
            if (!string.IsNullOrEmpty(Value))
            {
                result.Append(Value);
            }
            foreach (SubField subField in SubFields)
            {
                result.Append(subField);
            }
            return result.ToString();
        }
    }

    public sealed class FieldCollection
        : IEnumerable
    {
        private readonly ArrayList _list;

        public FieldCollection()
        {
            _list = new ArrayList();
        }

        public RecordField this[int index]
        {
            get { return (RecordField)_list[index]; }
            set { _list[index] = value; }
        }

        public void Add(RecordField item)
        {
            _list.Add(item);
        }

        public void Remove(RecordField item)
        {
            _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    public sealed class MarcRecord
    {
        private readonly FieldCollection _fields;

        public FieldCollection Fields
        {
            get { return _fields; }
        }

        public int Mfn { get; set; }

        public int Flags { get; set; }

        public string Database { get; set; }

        public MarcRecord()
        {
            _fields = new FieldCollection();
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (RecordField field in Fields)
            {
                result.Append(field);
                result.AppendLine();
            }
            return result.ToString();
        }
    }

    public sealed class IrbisClient
    {
        public IPAddress Host { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public char Workstation { get; set; }

        public string Database { get; set; }

        public int ClientID { get; set; }

        public int QueryID { get; set; }

        public IrbisClient()
        {
            Host = IPAddress.Loopback;
            Port = 6666;
            Workstation = 'C';
            Database = "IBIS";
            ClientID = new Random().Next(100000, 900000);
            QueryID = 0;
        }

        private Response ExecuteQuery(Query query)
        {
            using (TcpClient connection = new TcpClient())
            {
                connection.Connect(Host, Port);
                byte[] data = query.Encode();
                Socket socket = connection.Client;
                socket.Send(data);

                return new Response(socket);
            }
        }

        public string[] Connect()
        {
            Query query = new Query(this, "A");
            query.AddAnsi(Username);
            query.AddAnsi(Password);
            Response response = ExecuteQuery(query);
            response.CheckReturnCode();
            return response.ReadRemainingAnsi();
        }

        public void Dispose()
        {
            Query query = new Query(this, "B");
            query.AddAnsi(Username);
            ExecuteQuery(query);
        }

        public string FormatRecord(string format, int mfn)
        {
            return string.Empty;
        }

        public string FormatRecord(string format, MarcRecord record)
        {
            return string.Empty;
        }

        public int GetMaxMfn()
        {
            Query query = new Query(this, "O");
            query.AddAnsi(Database);
            Response response = ExecuteQuery(query);
            response.CheckReturnCode();
            return response.ReturnCode;
        }

        public void Nop()
        {
            Query query = new Query(this, "N");
            ExecuteQuery(query);
        }

        public MarcRecord ReadRecord(int mfn)
        {
            return null;
        }

        public string ReadTextFile(string specification)
        {
            Query query = new Query(this, "L");
            query.AddAnsi(specification);
            Response response = ExecuteQuery(query);
            return response.ReadAnsi();
        }

        public int[] Search(string expression)
        {
            return new int[0];
        }

        public MarcRecord WriteRecord(MarcRecord record)
        {
            return record;
        }
    }

    sealed class Query
    {
        private readonly MemoryStream _stream;

        public Query(IrbisClient client, string command)
        {
            _stream = new MemoryStream();
            AddAnsi(command);
            AddAnsi(client.Workstation.ToString());
            AddAnsi(command);
            Add(client.ClientID);
            Add(client.QueryID);
            client.QueryID++;
            AddAnsi(client.Password);
            AddAnsi(client.Username);
            AddLineFeed();
            AddLineFeed();
            AddLineFeed();
        }

        public void Add(bool value)
        {
            string text = value ? "1" : "0";
            AddAnsi(text);
        }

        public void Add(int value)
        {
            string text = value.ToString(CultureInfo.InvariantCulture);
            AddAnsi(text);
        }

        public void AddAnsi(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = Utility.Ansi.GetBytes(text);
                _stream.Write(bytes, 0, bytes.Length);
            }
            AddLineFeed();
        }

        public void AddLineFeed()
        {
            _stream.Write(Utility.LF, 0, Utility.LF.Length);
        }

        public void AddUtf(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = Utility.Utf.GetBytes(text);
                _stream.Write(bytes, 0, bytes.Length);
            }
            AddLineFeed();
        }

        public byte[] Encode()
        {
            byte[] buffer = _stream.ToArray();
            byte[] prefix = Utility.Ansi.GetBytes
                (
                    buffer.Length.ToString(CultureInfo.InvariantCulture)
                    + "\n"
                );
            byte[] result = new byte[prefix.Length + buffer.Length];
            Array.Copy(prefix,result,prefix.Length);
            Array.Copy(buffer,0,result,prefix.Length,buffer.Length);

            return result;
        }
    }

    sealed class Response
    {
        private readonly MemoryStream _stream;

        public string Command { get; set; }

        public int ClientID { get; set; }

        public int QueryID { get; set; }

        public int ReturnCode { get; set; }

        public Response(Socket socket)
        {
            _stream = new MemoryStream();

            byte[] buffer = new byte[32 * 1024];
            while (true)
            {
                int read = socket.Receive(buffer);
                if (read <= 0)
                {
                    break;
                }
                _stream.Write(buffer, 0, read);
            }

            _stream.Seek(0, SeekOrigin.Begin);

            Command = ReadAnsi();
            ClientID = ReadInt32();
            QueryID = ReadInt32();
            for (int i = 0; i < 7; i++)
            {
                ReadAnsi();
            }
        }

        public void CheckReturnCode(params int[] allowed)
        {
            if (GetReturnCode() < 0)
            {
                if (Array.IndexOf(allowed, ReturnCode) < 0)
                {
                    throw new IrbisException(ReturnCode);
                }
            }
        }

        public byte[] GetLine()
        {
            MemoryStream result = new MemoryStream();
            while (true)
            {
                int one = _stream.ReadByte();
                if (one < 0)
                {
                    break;
                }
                if (one == 0x0D)
                {
                    one = _stream.ReadByte();
                    if (one == 0x0A)
                    {
                        break;
                    }
                }
                else
                {
                    result.WriteByte((byte)one);
                }
            }

            return result.ToArray();
        }

        public int GetReturnCode()
        {
            ReturnCode = ReadInt32();
            return ReturnCode;
        }

        public string ReadAnsi()
        {
            byte[] line = GetLine();
            return Utility.Ansi.GetString(line);
        }

        public int ReadInt32()
        {
            string text = ReadAnsi();
            int result;
            int.TryParse(text, out result);
            return result;
        }

        public string[] ReadRemainingAnsi()
        {
            return new string[0];
        }

        public string[] ReadRemainingUtf()
        {
            return new string[0];
        }

        public string ReadUtf()
        {
            byte[] line = GetLine();
            return Utility.Ansi.GetString(line);
        }
    }

    public sealed class Utility
    {
        private static readonly Encoding _utf8 = new UTF8Encoding(false, false);
        private static readonly Encoding _cp1251 = Encoding.GetEncoding(1251);

        public static Encoding Utf { get { return _utf8; } }
        public static Encoding Ansi { get { return _cp1251; } }

        public static byte[] CRLF = { 0x13, 0x10 };
        public static byte[] LF = { 0x10 };
    }

    public sealed class IrbisException : Exception
    {
        public int ErrorCode { get; set; }

        public IrbisException()
        {
        }

        public IrbisException(int code)
        {
            ErrorCode = code;
        }

        public IrbisException(int code, string message) : base(message)
        {
            ErrorCode = code;
        }

        public IrbisException(string message) : base(message)
        {
        }
    }
}
