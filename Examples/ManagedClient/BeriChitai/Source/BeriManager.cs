// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BeriManager.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable UseStringInterpolation
// ReSharper disable UseNameofExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

public class BeriManager
{
    #region Constants

    /// <summary>
    /// Статус экземпляра.
    /// </summary>
    public const string StatusPrefix = "BERI=";

    /// <summary>
    /// Книга, доступная для заказа.
    /// </summary>
    public const string FreeBook = "0";

    /// <summary>
    /// Заказанная кем-либо книга.
    /// </summary>
    public const string ReservedBook = "1";

    /// <summary>
    /// Отданная читателю книга.
    /// </summary>
    public const string SurrenderedBook = "2";

    /// <summary>
    /// Дата бронирования.
    /// </summary>
    public const string DatePrefix = "DAB=";

    /// <summary>
    /// Читательский билет.
    /// </summary>
    public const string TicletPrefix = "CAB=";

    /// <summary>
    /// Дата выдачи.
    /// </summary>
    public const string IssuePrefix = "DAV=";

    /// <summary>
    /// Населенный пункт.
    /// </summary>
    public const string LocalityPrefix = "NAP=";

    #endregion

    #region Properties

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    [NotNull]
    public IrbisConnection Connection { get; set; }

    /// <summary>
    /// Формат библиографического описания.
    /// </summary>
    public string Format { get; set; }

    #endregion

    #region Construction

    public BeriManager
        (
            [NotNull] IrbisConnection connection
        )
    {
        Code.NotNull(connection, "connection");

        Connection = connection;
        Format = "@";
    }

    #endregion

    #region Private members

    [CanBeNull]
    private static string PrepareDescription
        (
            [CanBeNull] string description
        )
    {
        if (string.IsNullOrEmpty(description))
        {
            return description;
        }

        string result = description.Replace
            (
                "</><dd> (Нет сведений об экземплярах)<br>",
                string.Empty
            );
        result = result.Replace
            (
                "<b> Нет сведений об экземплярах</b>",
                string.Empty
            );

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Расширение информации о книгах.
    /// </summary>
    public void ExtendInfo
        (
            [NotNull][ItemNotNull] IEnumerable<BeriInfo> books
        )
    {
        Code.NotNull(books, "books");

        Connection.Connect();
        BeriInfo[] array = books.ToArray();

        if (!string.IsNullOrEmpty(Format))
        {
            foreach (BeriInfo book in array)
            {
                if (string.IsNullOrEmpty(book.Description)
                    && !ReferenceEquals(book.Record, null))
                {
                    string description = Connection.FormatRecord
                        (
                            Format,
                            book.Record.Mfn
                        );
                    description = PrepareDescription(description);
                    book.Description = description;
                }
            }
        }

        Connection.PushDatabase(StandardDatabases.Readers);
        try
        {
            ReaderManager readerManager = new ReaderManager(Connection);
            foreach (BeriInfo book in array)
            {
                string ticket = book.Ticket;
                if (ReferenceEquals(book.Reader, null)
                    && !string.IsNullOrEmpty(ticket))
                {
                    ReaderInfo reader = readerManager.GetReader(ticket);
                    book.Reader = reader;
                    if (!ReferenceEquals(reader, null))
                    {
                        reader.Description = Connection.FormatRecord("@", reader.Mfn);
                    }
                }
            }
        }
        finally
        {
            Connection.PopDatabase();
        }
    }

    /// <summary>
    /// Получение списка книг с указанным статусом.
    /// </summary>
    public BeriInfo[] GetBooksWithStatus(string status)
    {
        Connection.Connect();

        string expression = string.Format
            (
                "\"{0}{1}\"",
                StatusPrefix,
                status
            );

        MarcRecord[] records = BatchRecordReader.Search
            (
                Connection,
                Connection.Database,
                expression,
                500
            ).ToArray();

        BeriInfo[] result = records
            .SelectMany(record => BeriInfo.Parse(record))
            .ToArray();

        return result;
    }

    /// <summary>
    /// Получение списка доступных для заказа книг.
    /// </summary>
    [NotNull]
    [ItemNotNull]
    public BeriInfo[] GetFreeBooks()
    {
        return GetBooksWithStatus(FreeBook);
    }

    /// <summary>
    /// Получение списка заказанных книг.
    /// </summary>
    public BeriInfo[] GetReservedBooks()
    {
        BeriInfo[] result = GetBooksWithStatus(ReservedBook);
        ExtendInfo(result);

        return result;
    }

    /// <summary>
    /// Получение списка выданных читателю книг.
    /// </summary>
    public BeriInfo[] GetSurrenderedBooks()
    {
        BeriInfo[] result = GetBooksWithStatus(SurrenderedBook);
        ExtendInfo(result);

        return result;
    }

    /// <summary>
    /// Может ли читатель получать книги.
    /// </summary>
    public bool IsReaderEnabled
        (
            [NotNull] ReaderInfo reader
        )
    {
        Code.NotNull(reader, "reader");

        return string.IsNullOrEmpty(reader.Rights);
    }

    /// <summary>
    /// Создание заказа на книгу.
    /// </summary>
    public bool CreateBooking
        (
            int mfn,
            [NotNull] ReaderInfo reader
        )
    {
        Code.Positive(mfn, nameof(mfn));
        Code.NotNull(reader, nameof(reader));

        MarcRecord record = Connection.ReadRecord(mfn);
        if (ReferenceEquals(record, null))
        {
            return false;
        }

        BeriInfo[] already = BeriInfo.Parse(record);
        if (already.Length != 0)
        {
            return false;
        }

        string ticket = reader.Ticket;
        if (string.IsNullOrEmpty(ticket))
        {
            return false;
        }

        RecordField field = new RecordField(BeriInfo.BeriTag)
            .AddSubField('a', IrbisDate.TodayText)
            .AddSubField('b', ticket);
        record.AddField(field);

        Connection.WriteRecord(record, false, true, false);

        return true;
    }

    /// <summary>
    /// Находим книгу по указанному индексу.
    /// </summary>
    [CanBeNull]
    public BeriInfo[] GetBook
        (
            [NotNull] string index
        )
    {
        Code.NotNullNorEmpty(index, "index");

        MarcRecord record = Connection.SearchReadOneRecord("\"I={0}\"", index);
        if (ReferenceEquals(record, null))
        {
            return null;
        }

        BeriInfo[] result = BeriInfo.Parse(record);

        return result;
    }

    /// <summary>
    /// Регистрация выдачи книги.
    /// </summary>
    public bool GiveBook
        (
            [NotNull] BeriInfo book
        )
    {
        Code.NotNull(book, "book");

        MarcRecord record = book.Record.ThrowIfNull("book.Record");
        RecordField field = book.Field.ThrowIfNull("book.Field");
        if (!ReferenceEquals(field.Record, record))
        {
            throw new IrbisException("the field doesn't belong the record");
        }

        field.SetSubField('c', IrbisDate.TodayText);

        Connection.Connect();
        Connection.WriteRecord(record, true);

        return true;
    }

    /// <summary>
    /// Отменить заказ на книгу.
    /// </summary>
    public bool CancelBooking
        (
            [NotNull] BeriInfo book
        )
    {
        Code.NotNull(book, "book");

        ReaderInfo reader = book.Reader.ThrowIfNull("book.Reader");
        MarcRecord record = book.Record.ThrowIfNull("book.Record");
        RecordField field = book.Field.ThrowIfNull("book.Field");
        if (!ReferenceEquals(field.Record, record))
        {
            throw new IrbisException("the field doesn't belong the record");
        }

        record.Fields.Remove(field);

        Connection.Connect();
        Connection.WriteRecord(record, true);

        return true;
    }

    #endregion
}
