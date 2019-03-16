using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

public static class Utility
{
  public static Encoding AnsiEncoding()
  {
    return Encoding.GetEncoding(1251);
  }

  public static Encoding UtfEncoding()
  {
    return Encoding.UTF8;
  }

  public static string IntToString(int value)
  {
    return value.ToString(CultureInfo.InvariantCulture);
  }

  public static int StringToInt(string value)
  {
    if (!int.TryParse(value, out var result))
    {
      result = 0;
    }

    return result;
  }
}

public sealed class ChunkCollector
{
  #region Properties

  /// <summary>
  /// Total length of the data.
  /// </summary>
  public int Length
  {
    get
    {
      var result = 0;
      foreach (var memory in _accumulator)
      {
        result += memory.Length;
      }

      return result;
    }
  }

  #endregion

  #region Construction

  /// <summary>
  /// Constructor.
  /// </summary>
  public ChunkCollector()
  {
    _accumulator = new List<ReadOnlyMemory<byte>>();
  }

  #endregion

  #region Private members

  private readonly List<ReadOnlyMemory<byte>> _accumulator;

  private static readonly byte[] _newLine = {10};

  #endregion

  #region Public methods

  /// <summary>
  /// Add the chunk.
  /// </summary>
  public ChunkCollector Add(ReadOnlyMemory<byte> chunk)
  {
    _accumulator.Add(chunk);

    return this;
  }

  /// <summary>
  /// Add the array.
  /// </summary>
  public ChunkCollector Add(byte[] array)
  {
    return Add(new ReadOnlyMemory<byte>(array));
  }

  /// <summary>
  /// Copy data from the stream.
  /// </summary>
  public async Task CopyFromAsync(Stream stream, int bufferSize)
  {
    while (true)
    {
      var buffer = new byte[bufferSize];
      var read = await stream.ReadAsync(buffer, 0, bufferSize);
      if (read <= 0)
      {
        break;
      }

      var chunk = new ReadOnlyMemory<byte>(buffer, 0, read);
      Add(chunk);
    }
  }

  /// <summary>
  /// Debug print.
  /// </summary>
  public void Debug(TextWriter writer)
  {
    foreach (var memory in _accumulator)
    {
      foreach (var b in memory.Span)
      {
        writer.Write($" {b:X2}");
      }
    }
  }

  /// <summary>
  /// Get collected data.
  /// </summary>
  public ReadOnlyMemory<byte>[] GetChunks(int prefixLength = 0)
  {
    var length = _accumulator.Count;
    var result = new ReadOnlyMemory<byte>[length + prefixLength];
    for (int i = 0; i < length; i++)
    {
      result[prefixLength + i] = _accumulator[i];
    }

    return result;
  }

  /// <summary>
  /// Add new line symbol.
  /// </summary>
  public ChunkCollector NewLine()
  {
    return Add(_newLine);
  }

  #endregion

  #region Object members

  /// <inheritdoc />
  public override string ToString()
  {
    var result = new StringBuilder(Length);
    foreach (var memory in _accumulator)
    {
      result.Append(Encoding.Default.GetString(memory.Span));
    }

    return result.ToString();
  }

  #endregion
}

/// <summary>
/// Чтение чанков.
/// </summary>
public sealed class ChunkReader
{
  #region Properties

  public ChunkCollector Chunks { get; }

  public int Length { get; }

  public bool EOT { get; private set; }

  #endregion

  #region Construction

  /// <summary>
  /// Constructor.
  /// </summary>
  public ChunkReader(ChunkCollector chunks)
  {
    Chunks = chunks;
    Length = chunks.Length;
    _memory = chunks.GetChunks();
    if (_memory.Length == 0)
    {
      EOT = true;
    }
    else
    {
      _currentChunk = _memory.FirstOrDefault();
      _currentIndex = 0;
      _currentOffset = 0;
    }
  }

  #endregion

  #region Private members

  private readonly ReadOnlyMemory<byte>[] _memory;
  private ReadOnlyMemory<byte> _currentChunk;
  private int _currentIndex, _currentOffset;

  #endregion

  #region Public methods

  public byte Peek()
  {
    if (EOT)
    {
      return 0;
    }

    return _currentChunk.Span[_currentOffset];
  }

  public byte ReadByte()
  {
    if (EOT)
    {
      return 0;
    }

    byte result = _currentChunk.Span[_currentOffset];
    _currentOffset++;
    if (_currentOffset > _currentChunk.Length)
    {
      _currentOffset = 0;
      _currentIndex++;
      if (_currentIndex >= _memory.Length)
      {
        EOT = true;
      }
      else
      {
        _currentChunk = _memory[_currentIndex];
      }
    }

    return result;
  }

  public byte[] ReadLine()
  {
    var result = new MemoryStream();
    while (true)
    {
      var one = ReadByte();
      if (one == 0)
      {
        break;
      }

      if (one == 13)
      {
        if (Peek() == 10)
        {
          ReadByte();
        }

        break;
      }

      if (one == 10)
      {
        break;
      }

      result.WriteByte(one);
    }

    return result.ToArray();
  }

  public string ReadLine(Encoding encoding)
  {
    byte[] bytes = ReadLine();
    if (bytes.Length == 0)
    {
      return string.Empty;
    }

    return encoding.GetString(bytes);
  }

  public byte[] RemainingBytes()
  {
    if (EOT)
    {
      return Array.Empty<byte>();
    }

    var length = _currentChunk.Length - _currentOffset;

    for (var i = _currentIndex + 1; i < _memory.Length; i++)
    {
      length += _memory[i].Length;
    }

    if (length == 0)
    {
      EOT = true;

      return Array.Empty<byte>();
    }

    var result = new byte[length];
    var offset = 0;
    var tail = length;
    _currentChunk.Slice(_currentOffset).CopyTo(new Memory<byte>(result));
    offset += _currentChunk.Length - _currentOffset;
    tail -= offset;
    for (var i = _currentIndex + 1; i < _memory.Length; i++)
    {
      var chunk = _memory[i];
      chunk.CopyTo(new Memory<byte>(result, offset, tail));
      offset += chunk.Length;
      tail -= chunk.Length;
    }

    return result;
  }

  public string RemainingText(Encoding encoding)
  {
    var bytes = RemainingBytes();
    if (bytes.Length == 0)
    {
      return string.Empty;
    }

    return encoding.GetString(bytes);
  }

  #endregion
}

/// <summary>
/// Асинхронно отрабатывает работу с TCP-сервером.
/// </summary>
public struct AsyncExecutor
{
  #region Properties

  public string Host { get; }

  public int Port { get; }

  #endregion

  #region Construction

  /// <summary>
  /// Constructor.
  /// </summary>
  public AsyncExecutor(string host, int port)
  {
    Host = host;
    Port = port;
  }

  #endregion

  #region Public methods

  public async Task<ChunkCollector> Execute(ChunkCollector output)
  {
    // output.Debug(Out);

    var client = new TcpClient();
    await client.ConnectAsync(Host, Port);
    var stream = client.GetStream();

    var length = output.Length;
    var prefix = Encoding.ASCII.GetBytes($"{length}\n");
    var chunks = output.GetChunks(1);
    chunks[0] = new ReadOnlyMemory<byte>(prefix);
    foreach (var chunk in chunks)
    {
      await stream.WriteAsync(chunk);
    }

    var result = new ChunkCollector();
    await result.CopyFromAsync(stream, 2048);
    // result.Debug(Out);

    return result;
  }

  #endregion
}

public sealed class ClientQuery
{
  public ChunkCollector Buffer { get; }

  private readonly Encoding _ansi;

  private readonly Encoding _utf;

  public ClientQuery(AsyncConnection connection, string command)
  {
    Buffer = new ChunkCollector();
    _ansi = Utility.AnsiEncoding();
    _utf = Utility.UtfEncoding();

    AddAnsi(command).NewLine();
    AddAnsi(connection.Workstation).NewLine();
    AddAnsi(command).NewLine();
    Add(connection.ClientId).NewLine();
    Add(connection.QueryId).NewLine();
    AddAnsi(connection.Password).NewLine();
    AddAnsi(connection.Username).NewLine();
    NewLine();
    NewLine();
    NewLine();
  }

  public ClientQuery Add(int value)
  {
    return AddAnsi(Utility.IntToString(value));
  }

  public ClientQuery AddAnsi<T>(T value)
  {
    byte[] converted = _ansi.GetBytes(value.ToString());
    Buffer.Add(converted);

    return this;
  }

  public ClientQuery AddUtf<T>(T value)
  {
    byte[] converted = _utf.GetBytes(value.ToString());
    Buffer.Add(converted);

    return this;
  }

  public ClientQuery NewLine()
  {
    Buffer.NewLine();

    return this;
  }
}

public sealed class ServerResponse
{
  public string Command { get; }
  public int ClientId { get; }
  public int QueryId { get; }
  public int ReturnCode { get; private set; }
  public int AnswerSize { get; }
  public string ServerVersion { get; }

  private readonly ChunkCollector _answer;
  private readonly ChunkReader _reader;

  private readonly Encoding _ansi;

  private readonly Encoding _utf;

  public ServerResponse(ChunkCollector answer)
  {
    _answer = answer;
    _reader = new ChunkReader(_answer);
    _ansi = Utility.AnsiEncoding();
    _utf = Utility.UtfEncoding();
    
    Command = ReadAnsi();
    ClientId = ReadInteger();
    QueryId = ReadInteger();
    AnswerSize = ReadInteger();
    ServerVersion = ReadAnsi();
    ReadAnsi();
    ReadAnsi();
    ReadAnsi();
    ReadAnsi();
    ReadAnsi();
  }

  public void CheckReturnCode(params int[] goodCodes)
  {
    if (GetReturnCode() < 0)
    {
      if (!goodCodes.Contains(ReturnCode))
      {
        throw new Exception();
      }
    }
  }

  public void Debug()
  {
    _answer.Debug(Console.Out);
  }

  public string GetLine(Encoding encoding) => _reader.ReadLine(encoding);

  public int GetReturnCode()
  {
    return ReturnCode = ReadInteger();
  }

  public string ReadAnsi() => GetLine(_ansi);

  public int ReadInteger()
  {
    return Utility.StringToInt(GetLine(_ansi));
  }

  public string[] ReadRemainingAnsiLines()
  {
    List<string> result = new List<string>();

    while (!_reader.EOT)
    {
      string line = _reader.ReadLine(_ansi);
      result.Add(line);
    }

    return result.ToArray();
  }

  public string ReadRemainingAnsiText() => _reader.RemainingText(_ansi);

  public string[] ReadRemainingUtfLines()
  {
    List<string> result = new List<string>();

    while (!_reader.EOT)
    {
      string line = _reader.ReadLine(_utf);
      result.Add(line);
    }

    return result.ToArray();
  }

  public string ReadRemainingUtfText() => _reader.RemainingText(_utf);

  public string ReadUtf() => GetLine(_utf);
}

public sealed class AsyncConnection
  : IDisposable
{
  public string Host { get; set; } = "127.0.0.1";
  public int Port { get; set; } = 6666;
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public string Database { get; set; } = "IBIS";
  public string Workstation { get; set; } = "C";
  public int ClientId { get; private set; }
  public int QueryId { get; private set; }
  public string ServerVersion { get; private set; }
  public object IniFile { get; private set; }
  public int Interval { get; private set; }
  
  public bool Connected { get; private set; }
  
  public async Task<bool> Connect()
  {
    if (Connected)
    {
      return true;
    }

    AGAIN:
    ClientId = new Random().Next(100000, 999999);
    QueryId = 1;
    var query = new ClientQuery(this, "A");
    query.AddAnsi(Username).NewLine();
    query.AddAnsi(Password);

    var response = await Execute(query);
    if (response.GetReturnCode() == -3337)
    {
      goto AGAIN;
    }

    if (response.ReturnCode < 0)
    {
      return false;
    }

    Connected = true;
    ServerVersion = response.ServerVersion;
    Interval = response.ReadInteger();

    return true;
  }

  public async Task<bool> Disconnect()
  {
    if (!Connected)
    {
      return true;
    }

    var query = new ClientQuery(this, "B");
    query.AddAnsi(Username);
    await Execute(query);
    Connected = false;

    return true;
  }
  
  public async Task<ServerResponse> Execute(ClientQuery query)
  {
    AsyncExecutor executor = new AsyncExecutor(Host, Port);
    var answer = await executor.Execute(query.Buffer);
    var result = new ServerResponse(answer);
    QueryId++;

    return result;
  }

  public async Task<string> FormatRecord(string format, int mfn)
  {
    if (!Connected)
    {
      return null;
    }

    var query = new ClientQuery(this, "G");
    query.AddAnsi(Database).NewLine();
    query.AddAnsi(format).NewLine();
    query.Add(1).NewLine();
    query.Add(mfn).NewLine();
    var response = await Execute(query);
    response.CheckReturnCode();
    string result = response.ReadRemainingUtfText();

    return result;
  }
  
  public async Task<int> GetMaxMfn(string database = null)
  {
    if (!Connected)
    {
      return 0;
    }

    database = database ?? Database;
    var query = new ClientQuery(this, "O");
    query.AddAnsi(database);
    var response = await Execute(query);
    response.CheckReturnCode();

    return response.ReturnCode;
  }
  
  /// <inheritdoc />
  public void Dispose()
  {
    Disconnect().Wait();
  }
}

class Program
{
  static async Task AsyncMain()
  {
    using (var connection = new AsyncConnection())
    {
      connection.Username = "librarian";
      connection.Password = "secret";

      if (!await connection.Connect())
      {
        WriteLine("Не удалось подключиться!");
        return;
      }

      WriteLine($"VERSION: {connection.ServerVersion}");
      WriteLine($"INTERVAL: {connection.Interval}");

      var maxMfn = await connection.GetMaxMfn();
      WriteLine($"Max MFN: {maxMfn}");

      var text = await connection.FormatRecord("@brief", 123);
      WriteLine($"FORMATTED: {text}");

      await connection.Disconnect();
    }
  }
  
  static void Main()
  {
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    try
    {
      AsyncMain().Wait();
    }
    catch (Exception exception)
    {
      WriteLine(exception);
    }
  }
}
