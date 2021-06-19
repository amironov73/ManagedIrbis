// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Text;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

internal class Program
{
    private const string RootPath = @"C:\KATA";
    private static DirectAccess64 _oldDatabase;
    // private static DirectAccess64 _newDatabase;
    private static IrbisConnection _connection;
    private static LocalProvider _provider;
    private static TextWriter _log;

    private static void WriteLog(string text)
    {
        Console.WriteLine();
        Console.WriteLine(text);
        _log.WriteLine(text);
        _log.Flush();
    }

    private static string IssueToString(MagazineIssueInfo issue) =>
        string.IsNullOrEmpty(issue.Volume)
            ? $"№ {issue.Number} - {issue.Year}"
            : $"Т {issue.Volume} - № {issue.Number} - {issue.Year}";

    private static void ShowDifference
        (
            MarcRecord goodRecord,
            MarcRecord preyRecord
        )
    {
        var difference = RecordComparator.FindDifference(goodRecord, preyRecord);
        foreach (var field in difference.FirstOnly)
        {
            _log.WriteLine($" > {field}");
        }

        foreach (var field in difference.SecondOnly)
        {
            _log.WriteLine($" < {field}");
        }
        _log.WriteLine();
    }

    private static int MergeRecords
        (
            MarcRecord goodRecord,
            MarcRecord preyRecord
        )
    {
        goodRecord.Version = preyRecord?.Version ?? 0;
        goodRecord.Database = _connection.Database;

        goodRecord.Database = _connection.Database;

        return 12345678;
    }

    private static bool MergeMagazines
        (
            MagazineInfo oldMagazine,
            MagazineInfo newMagazine
        )
    {
        bool result = false;

        var oldCum = oldMagazine.Cumulation;
        var newCum = newMagazine.Cumulation;
        if (oldCum == null && newCum != null)
        {
            oldMagazine.Cumulation = newCum;
            return true;
        }

        if (oldCum == null)
        {
            return false;
        }

        foreach (var oldLine in oldCum)
        {
            var year = oldLine.Year;
        }

        return result;
    }

    private static void ProcessIssue
        (
            string workingList,
            MarcRecord oldRecord
        )
    {
        var issue = MagazineIssueInfo.Parse(oldRecord);
        var magazineIndex = issue.MagazineCode;
        if (string.IsNullOrEmpty(magazineIndex))
        {
            Console.Write("n");
            return;
        }

        if (string.IsNullOrEmpty(issue.Index))
        {
            Console.Write("n");
            return;
        }

        var oldVersion = oldRecord.Version;
        var found = _connection.SearchRead("\"I={0}\"", magazineIndex);
        if (found.Length != 1)
        {
            Console.Write("n");
            return;
        }

        var magazine = MagazineInfo.Parse(found[0]);
        //found = _newDatabase.SearchReadSimple($"I={issue.Index}");
        found = _connection.SearchRead($"\"I={issue.Index}\"");
        if (found.Length != 1)
        {
            // Если не нашли в новой базе данных, надо переносить
            var newMfn = MergeRecords(oldRecord, null);
            WriteLog($"{workingList}\tнет\tПЕРЕНОС\t{oldRecord.Mfn}\t{oldVersion}\t{newMfn}\t1\t{magazine} - {IssueToString(issue)}");
            return;
        }

        var newRecord = found[0];
        var newVersion = newRecord.Version;
        if (oldVersion <= newVersion)
        {
            Console.Write("n");
            return;
        }

        // Если в старой базе версия выше, надо переносить
        MergeRecords(oldRecord, newRecord);
        WriteLog($"{workingList}\tнет\tОБНОВЛЕНИЕ\t{oldRecord.Mfn}\t{oldVersion}\t{newRecord.Mfn}\t{newVersion}\t{magazine} - {IssueToString(issue)}");
        ShowDifference(oldRecord, newRecord);
    }

    private static void ProcessMagazine
        (
            string workingList,
            MarcRecord oldRecord
        )
    {
        var oldMagazine = MagazineInfo.Parse(oldRecord);
        if (oldMagazine == null)
        {
            Console.Write("j");
            return;
        }

        var magazineIndex = oldMagazine.Index;
        if (string.IsNullOrEmpty(magazineIndex))
        {
            Console.Write("j");
            return;
        }

        var oldVersion = oldRecord.Version;
        var found = _connection.SearchRead($"\"I={magazineIndex}\"");
        if (found.Length != 1)
        {
            var newMfn = MergeRecords(oldRecord, null);
            WriteLog($"{workingList}\tнет\tПЕРЕНОС\t{oldRecord.Mfn}\t{oldVersion}\t{newMfn}\t1\t{oldMagazine}");
            return;
        }

        var newRecord = found[0];
        var newMagazine = MagazineInfo.Parse(newRecord);

        if (MergeMagazines(oldMagazine, newMagazine))
        {
            MergeRecords(oldRecord, newRecord);
            WriteLog($"{workingList}\tнет\tОБНОВЛЕНИЕ\t{oldRecord.Mfn}\t{oldVersion}\t{newRecord.Mfn}\t{newRecord.Version}\t{oldMagazine}");
            ShowDifference(oldRecord, newRecord);
            return;
        }

        Console.Write("j");
    }

    private static string GetArticleDescription
        (
            MarcRecord record
        )
    {
        return _provider.FormatRecord(record, "@brief");
    }

    private static void ProcessArticle
        (
            string workingList,
            MarcRecord oldRecord
        )
    {
        var oldArticle = MagazineArticleInfo.ParseAsp(oldRecord);

        var index = oldRecord.FM(903);
        if (string.IsNullOrEmpty(index))
        {
            Console.Write("a");
            return;
        }

        var oldVersion = oldRecord.Version;
        var found = _connection.SearchRead($"\"I={index}\"");
        if (found.Length != 1)
        {
            var newMfn = MergeRecords(oldRecord, null);
            WriteLog($"{workingList}\tнет\tПЕРЕНОС\t{oldRecord.Mfn}\t{oldVersion}\t{newMfn}\t1\t{GetArticleDescription(oldRecord)}");
            return;
        }

        var newRecord = found[0];
        var newVersion = newRecord.Version;
        if (oldVersion <= newVersion)
        {
            Console.Write("a");
            return;
        }

        MergeRecords(oldRecord, newRecord);
        WriteLog($"{workingList}\tнет\tОБНОВЛЕНИЕ\t{oldRecord.Mfn}\t{oldVersion}\t{newRecord.Mfn}\t{newVersion}\t{GetArticleDescription(oldRecord)}");
        ShowDifference(oldRecord, newRecord);
    }

    private static string ChooseExemplar
        (
            MarcRecord record
        )
    {
        var exemplars = ExemplarInfo.Parse(record);
        if (exemplars.Length == 0)
        {
            // книга без экземпляров, фиг с ней
            return null;
        }

        foreach (var exemplar in exemplars)
        {
            var barcode = exemplar.Barcode;
            if (!string.IsNullOrEmpty(barcode))
            {
                return barcode;
            }

            var number = exemplar.Number;
            if (string.IsNullOrEmpty(number))
            {
                continue;
            }

            number = number.ToUpperInvariant();
            if (number.Length < 4)
            {
                continue;
            }

            return number;
        }

        return null;
    }

    private static void ProcessBook
        (
            string workingList,
            MarcRecord oldRecord
        )
    {
        var book = new BookInfo(_provider, oldRecord);
        var exemplar = ChooseExemplar(oldRecord);
        if (exemplar == null)
        {
            // книга без экземпляров, фиг с ней
            Console.Write("p");
            return;
        }

        var oldVersion = oldRecord.Version;
        //var found = _newDatabase.SearchReadSimple($"IN={exemplar}");
        var found = _connection.SearchRead($"\"IN={exemplar}\"");
        if (found.Length == 0)
        {
            // Если не нашли в новой базе надо переносить
            var newMfn = MergeRecords(oldRecord, null);
            WriteLog($"{workingList}\t{exemplar}\tПЕРЕНОС\t{oldRecord.Mfn}\t{oldVersion}\t{newMfn}\t1\t{book.Description}");
            return;
        }

        var newRecord = found[0];
        var newVersion = newRecord.Version;
        if (oldVersion <= newVersion)
        {
            Console.Write("p");
            return;
        }

        // Если в старой базе версия новее, надо переносить
        MergeRecords(oldRecord, newRecord);
        WriteLog($"{workingList}\t{exemplar}\tОБНОВЛЕНИЕ\t{oldRecord.Mfn}\t{oldVersion}\t{newRecord.Mfn}\t{newVersion}\t{book.Description}");
        ShowDifference(oldRecord, newRecord);
    }

    private static void ProcessRecord(MarcRecord oldRecord)
    {
        if (oldRecord == null)
        {
            Console.Write("-");
            return;
        }

        if (oldRecord.Deleted)
        {
            Console.Write("*");
            return;
        }

        var workingList = oldRecord.FM(920);
        if (string.IsNullOrEmpty(workingList))
        {
            Console.Write("=");
            return;
        }

        workingList = workingList.ToUpperInvariant();
        if (workingList == "NJ" || workingList == "NJK" || workingList == "NJP")
        {
            ProcessIssue(workingList, oldRecord);
            return;
        }

        if (workingList == "J")
        {
            ProcessMagazine(workingList, oldRecord);
            return;
        }

        if (workingList == "ASP")
        {
            ProcessArticle(workingList, oldRecord);
            return;
        }

        if (workingList == "PAZK" || workingList == "SPEC"
            || workingList == "PVK" || workingList == "IBIS"
            || workingList == "AUNTD")
        {
            ProcessBook(workingList, oldRecord);
            return;
        }

        Console.Write(".");
    }

    private static void ProcessDatabase()
    {
        var maxMfn = _oldDatabase.GetMaxMfn();
        Console.WriteLine($"Max MFN={maxMfn}");

        var minMfn = Math.Max(1, maxMfn - 10000);
        for (var mfn = minMfn; mfn < maxMfn; mfn++)
        {
            try
            {
                var record = _oldDatabase.ReadRecord(mfn);
                ProcessRecord(record);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        //Parallel.For(minMfn, maxMfn, mfn =>
        //{
        //    try
        //    {
        //        var record = _oldDatabase.ReadRecord(mfn);
        //        ProcessRecord(record);
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }
        //});

    }

    public static void Main(string[] args)
    {
        _log = new StreamWriter("katastropher.txt", false, Encoding.UTF8);

        var oldPath = Path.Combine(RootPath, @"Datai\IBIS_OLD\ibis_old.mst");
        _oldDatabase = new DirectAccess64(oldPath, DirectAccessMode.ReadOnly);

        //var newPath = Path.Combine(RootPath, @"Datai\IBIS_NEW\ibis_new.mst");
        //_newDatabase = new DirectAccess64(newPath, DirectAccessMode.ReadOnly);

        _connection = new IrbisConnection("user=librarian;password=secret;db=IBIS_NEW;");

        _provider = new LocalProvider(RootPath, DirectAccessMode.ReadOnly, true)
        {
            Database = "IBIS_OLD"
        };

        ProcessDatabase();

        _provider.Dispose();
        _oldDatabase.Dispose();
        //_newDatabase.Dispose();
        _connection.Dispose();
        _log.Dispose();

        Console.WriteLine();
        Console.WriteLine("ALL DONE");
    }
}
