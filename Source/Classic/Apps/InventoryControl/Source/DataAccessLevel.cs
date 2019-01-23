// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DataAccessLevel.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AM;
using AM.Collections;
using AM.Text;
using AM.Text.Output;

using BLToolkit.Data.Linq;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Readers;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo

namespace InventoryControl
{
    [PublicAPI]
    public sealed class DataAccessLevel
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Клиент
        /// </summary>
        [NotNull]
        public IrbisConnection Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Анализируемое место хранения.
        /// </summary>
        public string Place { get; set; }

        public CeContext Db { get; private set; }

        #endregion

        #region Construction

        public DataAccessLevel
            (
                [NotNull] AbstractOutput output
            )
        {
            Code.NotNull(output, "output");

            _output = output;

            _client = IrbisConnectionUtility.GetClientFromConfig();
            _newspapers = new Dictionary<string, bool>();
            WriteLine("Подключились к ИРБИС-серверу");
        }

        #endregion

        #region Private members

        private IrbisConnection _client;

        private AbstractOutput _output;

        private Dictionary<string, bool> _newspapers;

        private static char[] _badCharacters = {'\0', '.', ',', '/', '(', ')'};

        #endregion

        #region Public methods

        [StringFormatMethod("format")]
        public void WriteLine
            (
                [NotNull] string format,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            _output?.WriteLine(format, args);
        }

        public ChairInfo[] ListPlaces()
        {
            ChairInfo[] result = ChairInfo.Read
                (
                    Client,
                    "mhr.mnu",
                    false
                )
                .Where(item => item.Code != "*")
                .ToArray();

            return result;
        }

        public void ExtractExemplars
            (
                Action<double> progress
            )
        {
            string place = Place;

            if (string.IsNullOrEmpty(place))
            {
                throw new ArgumentNullException();
            }

            EnsureDatabaseOpen();
            Db.TruncateExemplar();
            WriteLine
                (
                    "Очищаем таблицу"
                );

            string request = string.Format
                (
                    "\"MHR={0}\" + \"INP={0}-$\"",
                    place
                );

            int foundCount = Client.SearchCount(request);
            WriteLine
                (
                    "Всего найдено названий: {0}",
                    foundCount
                );

            int index = 0;
            int totalExemplars = 0;

            Dictionary<string, string> magazineBbk
                = new CaseInsensitiveDictionary<string>();
            BatchRecordReader batch = (BatchRecordReader) BatchRecordReader.Search
                (
                    Client,
                    Client.Database,
                    request,
                    500
                );
            foreach (MarcRecord record in batch)
            {
                if ((index % 100) == 0)
                {
                    WriteLine
                        (
                            "Загружено записей: {0} из {1}",
                            index,
                            foundCount
                        );

                    if (progress != null)
                    {
                        double percent = ((double)index) / foundCount;
                        progress(percent);
                    }
                }

                index++;

                Application.DoEvents();

                string worklist = record.FM(920);
                if (worklist.SameString("J")
                    || worklist.SameString("ASP"))
                {
                    continue;
                }

                if (IsNewspaper(record))
                {
                    continue;
                }

                if (NotComplete(record))
                {
                    continue;
                }

                ExemplarInfo2[] allExemplars = ExemplarInfo2.Parse(record);
                ExemplarInfo2[] goodExemplars = allExemplars
                    .Where(e => e.Place.SameString(place)
                    || e.RealPlace.SameString(place)
                    || StringUtility.SafeStarts(e.CheckedDate, place))
                    .Where(e => !e.Status.SameString("p"))
                    .ToArray();

                if (goodExemplars.Length != 0)
                {
                    string description = Client.FormatRecord("@sbrief", record.Mfn);

                    string bookIndex = record.FM(906)
                        ?? record.FM(621)
                        ?? record.FM(686);

                    foreach (ExemplarInfo2 ex in goodExemplars)
                    {
                        if (!string.IsNullOrEmpty(description))
                        {
                            description = description.Replace
                                (
                                    " Журнал,",
                                    string.Empty
                                )
                                .Replace
                                (
                                    " Журнал.",
                                    string.Empty
                                )
                                .Replace
                                (
                                    "(Введено оглавление)",
                                    string.Empty
                                )
                                .Trim();
                        }

                        ex.Description = description;
                        if (string.IsNullOrEmpty(ex.Year))
                        {
                            ex.Year = GetYear(record);
                        }

                        ex.Price = GetPrice(record, ex);
                        if (string.IsNullOrEmpty(ex.ShelfIndex))
                        {
                            ex.ShelfIndex = bookIndex;
                        }

                        ex.Index = record.FM(903);
                        ex.Bbk = record.FM(621) ?? record.FM(621, 'a');
                        ex.Issue = record.FM(936);

                        if (worklist.SafeContains("NJ") && string.IsNullOrEmpty(ex.Bbk))
                        {
                            string magazineReference = record.FM(933);
                            if (!string.IsNullOrEmpty(magazineReference))
                            {
                                string bbk;
                                if (!magazineBbk.TryGetValue(magazineReference, out bbk))
                                {
                                    MarcRecord magazineRecord
                                        = Client.SearchReadOneRecord("\"I={0}\"", magazineReference);
                                    if (!ReferenceEquals(magazineRecord, null))
                                    {
                                        bbk = magazineRecord.FM(621) ?? magazineRecord.FM(621, 'a');
                                        if (string.IsNullOrEmpty(bbk))
                                        {
                                            WriteLine("Журнал {0} без ББК", magazineRecord.FM(200, 'a'));
                                        }

                                        magazineBbk[magazineReference] = bbk;
                                    }
                                }

                                ex.Bbk = bbk;
                            }
                        }

                    }

                    try
                    {
                        Db.InsertBatch(goodExemplars);
                    }
                    catch (Exception ex)
                    {
                        //WriteLine(ex.ToString());

                        foreach (ExemplarInfo2 exemplar in goodExemplars)
                        {
                            WriteLine
                                (
                                    exemplar
                                    + " : MFN "
                                    + exemplar.Mfn
                                    + " : "
                                    + exemplar.Description
                                );
                        }

                        //return;
                    }

                }
                totalExemplars += goodExemplars.Length;
            }

            WriteLine
                (
                    "Всего загружено экземпляров: {0}",
                    totalExemplars
                );
        }

        public void DropDatabase
            (
                string filename
            )
        {
            if (Db != null)
            {
                Db.Dispose();
                Db = null;
            }

            if (!string.IsNullOrEmpty(filename))
            {
                if (File.Exists(filename))
                {
                    WriteLine
                        (
                            "Удаляем БД: {0}",
                            filename
                        );
                    File.Delete(filename);
                }
            }
        }

        public string FixFileName(string name)
        {
            return Transliterator.Transliterate(name)
                   + ".sdf";
        }

        public void CreateDatabase
            (
                string filename
            )
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            WriteLine
                (
                    "Создаём БД: {0}", filename
                );

            if (File.Exists(filename))
            {
                // Уже есть база данных
                return;
            }

            string connectionString = string.Format
                (
                    "Data Source={0};",
                    filename
                );
            using (SqlCeEngine engine = new SqlCeEngine
                (
                    connectionString
                ))
            {
                engine.CreateDatabase();
            }

            string commands = File.ReadAllText("database.sqlce");

            using (CeContext db = new CeContext(filename))
            {
                db.SetCommand(commands)
                    .ExecuteNonQuery();
            }

            WriteLine
                (
                    "Успешно инициализирована БД {0}",
                    filename
                );
        }

        public void CloseDatabase()
        {
            if (Db != null)
            {
                WriteLine
                    (
                        "Закрываем БД: {0}", Db.DatabaseName
                    );

                Db.Dispose();
                Db = null;
            }
        }

        public void OpenDatabase
            (
                string filename
            )
        {
            CloseDatabase();

            if (!string.IsNullOrEmpty(filename))
            {
                if (!File.Exists(filename))
                {
                    CreateDatabase(filename);
                }

                WriteLine
                    (
                        "Открываем БД: {0}",
                        filename
                    );
                Db = new CeContext(filename);
                WriteLine
                    (
                        "Экземпляров в БД: {0}",
                        Db.Exemplar.Count()
                    );

            }
        }

        public void SwitchDatabase
            (
                string filename
            )
        {
            if (string.IsNullOrEmpty(filename))
            {
                CloseDatabase();
            }

            if (Db == null)
            {
                OpenDatabase(filename);
            }
            else
            {
                if (Db.DatabaseName != filename)
                {
                    OpenDatabase(filename);
                }
            }
        }

        public bool EnsureDatabaseOpen()
        {
            if (Db == null)
            {
                SwitchDatabase(FixFileName(Place));
            }
            return (Db != null);
        }

        public int GetExemplarCount()
        {
            EnsureDatabaseOpen();
            return Db.Exemplar.Count();
        }

        public BookInfo BookFromExemplar
            (
                ExemplarInfo2 exemplar
            )
        {
            BookInfo result = new BookInfo
            {
                Mfn = exemplar.Mfn,
                Ksu = exemplar.KsuNumber1,
                Number = exemplar.Number,
                Description = exemplar.Description,
                Year = exemplar.Year,
                Price = exemplar.Price,
                Barcode = exemplar.Barcode,
                Sign = exemplar.ShelfIndex,
                Issue = exemplar.Issue,
                Index = exemplar.Index,
                Bbk = exemplar.Bbk
            };

            char firstChar = result.Description.FirstChar();
            if (_badCharacters.Contains(firstChar))
            {
                result.Description = "!!! MFN=" + result.Mfn + ": "
                    + result.Description.ToVisibleString();
            }

            if (!string.IsNullOrEmpty(exemplar.CheckedDate))
            {
                DateTime date;
                if (DateTime.TryParseExact
                    (
                        exemplar.CheckedDate,
                        "yyyyMMdd",
                        CultureInfo.CurrentCulture,
                        DateTimeStyles.None,
                        out date
                    ))
                {
                    result.CheckedDate = date;
                }
            }

            return result;
        }

        public ExemplarInfo2[] GetAllExemplars()
        {
            EnsureDatabaseOpen();
            return Db.Exemplar.ToArray();
        }

        public BookInfo[] GetAllBooks()
        {
            return GetAllExemplars()
                .Select(BookFromExemplar)
                .ToArray();
        }

        public BookInfo[] GetSeenBooks
            (
                string place
            )
        {
            EnsureDatabaseOpen();
            return Db.Exemplar
                .ToArray()
                .Where(e => e.RealPlace == place)
                .Where(e => e.Status != "6")
                .Select(BookFromExemplar)
                .ToArray();
        }

        public BookInfo[] GetBooksOnHands
            (
                string place
            )
        {
            EnsureDatabaseOpen();
            return Db.Exemplar
                .ToArray()
                .Where(e => e.Status == "1")
                .Select(BookFromExemplar)
                .ToArray();
        }

        public BookInfo[] GetSeenPlusOnHands
            (
                string place
            )
        {
            EnsureDatabaseOpen();
            return Db.Exemplar
                .ToArray()
                .Where(e => e.RealPlace == place)
                .Where(e => e.Status != "6")
                .Select(BookFromExemplar)
                .Concat
                (
                    Db.Exemplar
                        .ToArray()
                        .Where(e => e.Place == place)
                        .Where(e => e.RealPlace != place)
                        .Where(e => e.Status == "1")
                        .Select(BookFromExemplar)
                )
                //.Distinct(BookInfo.MfnComparer)
                .ToArray();
        }

        public string GetShelfIndex
            (
                string number
            )
        {
            if (string.IsNullOrEmpty(number))
            {
                return null;
            }
            MarcRecord found = Client.SearchReadOneRecord
                (
                    "\"IN={0}\"",
                    number
                );
            if (found == null)
            {
                return null;
            }

            return found.FM(906)
                ?? found.FM(621)
                ?? found.FM(686);
        }

        public static bool IsGoodBarcode
            (
                string barcode
            )
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return false;
            }
            return barcode.StartsWith("E0");
        }

        public BookInfo[] GetMissingBooks
            (
                string place
            )
        {
            EnsureDatabaseOpen();
            BookInfo[] result = Db.Exemplar
                .ToArray()
                .Where(e => e.Place == place)
                .Where(e => e.RealPlace != place)
                .Where(e => (e.Status != "1" && e.Status != "6"))
                .Select(BookFromExemplar)
                .ToArray();

            WriteLine
                (
                    "Предварительно отобрано: {0}",
                    result.Length
                );

            WriteLine("Получаем базу CONFIG");
            ConfigDatabase config = new ConfigDatabase(Client);

            List<BookInfo> result2 = new List<BookInfo>(result.Length);

            foreach (BookInfo book in result)
            {
                if (config.BookInWork(book.Number))
                {
                    continue;
                }

                book.Sign = string.Format
                    (
                        "{0}",
                        book.Mfn
                    );


                if (!string.IsNullOrEmpty(book.Number))
                {
                    book.Number = book.Number.Trim();
                }

                bool needLongWork =
                    string.IsNullOrEmpty(book.Issue)
                    && !book.Number.StartsWith("П")
                    && !book.Number.OneOf("0", "1", "2", "3");

                if (needLongWork)
                {
                    MarcRecord found = Client.SearchReadOneRecord
                        (
                            "\"IN={0}\"",
                            book.Number
                        );
                    string shelf = "?";
                    if (found != null)
                    {
                        shelf = found.FM(906)
                                ?? found.FM(621)
                                ?? found.FM(686);
                    }
                    book.Remark = string.Concat
                        (
                            shelf,
                            " : ",
                            IsGoodBarcode(book.Barcode)
                                ? "есть"
                                : "нет"
                        );
                }

                result2.Add(book);
            }

            return result2.ToArray();
        }

        public bool IsDefectBook
            (
                ExemplarInfo2 exemplar
            )
        {
            return string.IsNullOrEmpty(exemplar.Description)
                   || string.IsNullOrEmpty(exemplar.Price)
                   || string.IsNullOrEmpty(exemplar.Year);
        }

        public BookInfo[] GetDefectBooks
            (
                string place
            )
        {
            EnsureDatabaseOpen();
            return Db.Exemplar
                .ToArray()
                .Where(IsDefectBook)
                .Select(BookFromExemplar)
                .ToArray();
        }

        private string GetPrice
            (
                MarcRecord record,
                ExemplarInfo2 exemplar
            )
        {
            if (!string.IsNullOrEmpty(exemplar.Price))
            {
                return exemplar.Price;
            }
            string price = record.FM(10, 'd');
            if (!string.IsNullOrEmpty(price))
            {
                return price;
            }
            return string.Empty;
        }

        private string GetYear
            (
                MarcRecord record
            )
        {
            string result = record.FM(210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'h');
            }

            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(461, 'z');
            }

            if (string.IsNullOrEmpty(result))
            {
                result = record.FM(934);
            }

            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            Match match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }

            return result;
        }

        public void SortBooks
            (
                BookInfo[] books,
                bool sortByTitle,
                int startNumber
            )
        {
            WriteLine("Сортировка...");

            if (sortByTitle)
            {
                Array.Sort
                    (
                        books,
                        _BookComparisonByTitle
                    );
            }
            else
            {
                Array.Sort
                    (
                        books,
                        _BookComparisonByNumber
                    );
            }

            for (int i = 0; i < books.Length; )
            {
                books[i].Ordinal = ++i + startNumber - 1;
            }

            WriteLine("Отсортировано");
        }

        private static int _BookComparisonByNumber
            (
                BookInfo left,
                BookInfo right
            )
        {
            int result = NumberText.Compare
                (
                    left.Number,
                    right.Number
                );
            if (result == 0)
            {
                result = string.Compare
                    (
                        left.Description,
                        right.Description,
                        CultureInfo.CurrentCulture,
                        CompareOptions.IgnoreCase
                    );
            }
            return result;
        }

        private static int _BookComparisonByTitle
            (
                BookInfo left,
                BookInfo right
            )
        {
            int result = NumberText.Compare
                (
                    left.Description,
                    right.Description
                //CultureInfo.CurrentCulture,
                //CompareOptions.IgnoreCase
                );
            if (result == 0)
            {
                result = NumberText.Compare
                    (
                        left.Number,
                        right.Number
                    );
            }

            return result;
        }

        public bool NotComplete
            (
                MarcRecord record
            )
        {
            RecordField found = record.Fields
                .GetField(907)
                .GetField('c', "ОБРНЗ")
                .FirstOrDefault();

            return (found != null);
        }

        public bool IsNewspaper
            (
                MarcRecord record
            )
        {
            string worklist = record.FM(920);
            if (string.IsNullOrEmpty(worklist))
            {
                return false;
            }

            if (worklist.SameString("NJP"))
            {
                return true;
            }

            if (!worklist.SameString("NJ"))
            {
                return false;
            }

            string index = record.FM(933);
            if (string.IsNullOrEmpty(index))
            {
                return false;
            }

            bool result;
            if (_newspapers.TryGetValue(index, out result))
            {
                return result;
            }

            MarcRecord main = Client.SearchReadOneRecord
                (
                    "\"I={0}\"",
                    index
                );
            if (ReferenceEquals(main, null))
            {
                return false;
            }

            string kind = main.FM(110, 'b');
            result = kind.SameString("c");
            _newspapers[index] = result;
            return result;
        }

        public bool MoveBook
            (
                BookInfo book,
                string oldPlace,
                string newPlace,
                ConfigDatabase config
            )
        {
            string number = book.Number;
            MarcRecord[] found = Client.SearchRead
                (
                    "\"IN={0}\"",
                    number
                );
            if (found.Length > 1)
            {
                WriteLine
                    (
                        "IN={0}: найдено книг: {1}",
                        number,
                        found.Length
                    );
                return false;
            }
            if (found.Length == 0)
            {
                WriteLine
                    (
                        "IN={0}: экземпляр не найден",
                        number
                    );
                return false;
            }

            MarcRecord record = found[0];
            RecordField field = record.Fields
                .FirstOrDefault(f => f.GetFirstSubFieldValue('b')
                    .SameString(number));
            if (ReferenceEquals(field, null))
            {
                WriteLine
                    (
                        "IN={0}: не найден",
                        number
                    );
                return false;
            }

            if (config.BookInWork(number))
            {
                WriteLine
                    (
                        "IN={0}: экземпляр в обработке",
                        number
                    );
                return false;
            }

            string currentPlace = field.GetFirstSubFieldValue('d');
            if (currentPlace.SameString(newPlace))
            {
                WriteLine
                    (
                        "IN={0}: уже в {1}",
                        number,
                        newPlace
                    );
                return true;
            }
            if (!currentPlace.SameString(oldPlace))
            {
                WriteLine
                    (
                        "IN={0}: неожиданное место хранения: {1}",
                        number,
                        currentPlace
                    );
                return false;
            }

            field.SetSubField('d', newPlace);
            field.SetSubField('a', "5");
            Client.WriteRecord(record, false, true);
            WriteLine
                (
                    "IN={0}: перенесен в {1}",
                    number,
                    newPlace
                );

            return true;
        }

        public void WriteDelimiter()
        {
            WriteLine
                (
                    new string('=', 60)
                );
        }

        public void MoveBooks
            (
                BookInfo[] books,
                string oldPlace,
                string newPlace
            )
        {
            EnsureDatabaseOpen();
            if (ReferenceEquals(books, null))
            {
                throw new ArgumentNullException(nameof(books));
            }
            if (string.IsNullOrEmpty(oldPlace))
            {
                throw new ArgumentNullException(nameof(oldPlace));
            }
            if (string.IsNullOrEmpty(newPlace))
            {
                throw new ArgumentNullException(nameof(newPlace));
            }

            WriteDelimiter();
            WriteLine("Начали перемещение");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            WriteLine("Получаем базу CONFIG");
            ConfigDatabase config = new ConfigDatabase(Client);

            int countMoved = 0;

            foreach (BookInfo book in books)
            {
                if (MoveBook
                    (
                        book,
                        oldPlace,
                        newPlace,
                        config
                    ))
                {
                    countMoved++;
                }
            }

            stopwatch.Stop();
            WriteLine
                (
                    "Закончили перемещение: {0}",
                    stopwatch.Elapsed
                );
            WriteLine
                (
                    "Перемещено экземпляров: {0}",
                    countMoved
                );
            WriteDelimiter();
        }

        public void StatByDay
            (
                BookInfo[] books
            )
        {
            if (ReferenceEquals(books, null))
            {
                throw new ArgumentNullException(nameof(books));
            }

            WriteDelimiter();

            var grouped = books.GroupBy(b => b.CheckedDate)
                .OrderBy(g => g.Key);
            foreach (var g in grouped)
            {
                WriteLine
                    (
                        "{0} ({1}): {2}",
                        g.Key.ToShortDateString(),
                        g.Key.ToString("ddd"),
                        g.Count()
                    );
            }

            WriteDelimiter();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            CloseDatabase();
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
                WriteLine("Отключились от ИРБИС-сервера");
            }

            if (_output != null)
            {
                _output.Dispose();
                _output = null;
            }
        }

        #endregion
    }
}
