using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Fields;

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming

//using Counter= AM.Collections.DictionaryCounterInt32<string>;

namespace DumpFields
{
    class Program
    {
        private static readonly Counter _authors = new Counter("authors");
        private static readonly Counter _titles = new Counter("titles");
        private static readonly Counter _series = new Counter("series");
        private static readonly Counter _publishers = new Counter("publishers");

        private static DirectAccess64 _access;

        static void ProcessData
            (
                Counter counter,
                string title,
                int exemplarCount,
                int loanCount,
                double years//,
                //double meanLoan
            )
        {
            if (!string.IsNullOrEmpty(title))
            {
                counter.Add(title, exemplarCount, loanCount, years);
            }
        }

        static void ProcessData
            (
                Counter counter,
                string[] titles,
                int exemplarCount,
                int loanCount,
                double years//,
                //double meanLoan
            )
        {
            foreach (string title in titles)
            {
                if (!string.IsNullOrEmpty(title))
                {
                    counter.Add(title, exemplarCount, loanCount, years);
                }
            }
        }

        static DateTime GetDate(IEnumerable<ExemplarInfo> exemplars)
        {
            DateTime result = DateTime.MinValue;
            foreach (ExemplarInfo exemplar in exemplars)
            {
                DateTime date = IrbisDate.ConvertStringToDate(exemplar.Date);
                if (date != DateTime.MinValue)
                {
                    if (result == DateTime.MinValue)
                        result = date;
                    else if (date < result)
                        result = date;
                }
            }

            return result;
        }

        static void ProcessRecord(int mfn)
        {
            if ((mfn % 50) == 1)
                Console.Write($"\n{mfn-1:000,000,000}>");
            if ((mfn % 10) == 1)
                Console.Write(" ");

            MarcRecord record = _access.ReadRecord(mfn);
            if (ReferenceEquals(record, null))
            {
                Console.Write('-');
                return;
            }

            if (record.Deleted)
            {
                Console.Write('x');
                return;
            }

            string worksheet = record.FM(920);
            if (string.IsNullOrEmpty(worksheet))
            {
                Console.Write('~');
                return;
            }

            worksheet = worksheet.ToUpperInvariant();
            if (worksheet != "PAZK" && worksheet != "SPEC")
            {
                Console.Write('=');
                return;
            }

            ExemplarInfo[] exemplars = ExemplarInfo.Parse(record);
            if (exemplars.Length == 0)
            {
                Console.Write('.');
                return;
            }

            int exemplarCount = exemplars.Length;
            int loanCount = record.FM(999).SafeToInt32();
            DateTime date = GetDate(exemplars);
            if (date == DateTime.MinValue)
            {
                date = IrbisDate.ConvertStringToDate(record.FM(907, 'a'));
                if (date == DateTime.MinValue)
                {
                    Console.Write('.');
                    return;
                }
            }

            double years = (DateTime.Today - date).Days / 365.0;
            if (years < 1.0)
            {
                Console.Write('<');
                return;
            }
            //double meanLoan = (((double)loanCount) / exemplarCount) / years;

            ProcessData(_authors, record.FM(700, 'a'), exemplarCount, loanCount, years);
            ProcessData(_authors, record.FMA(701, 'a'), exemplarCount, loanCount, years);
            ProcessData(_titles, record.FM(200, 'a'), exemplarCount, loanCount, years);
            ProcessData(_titles, record.FMA(461, 'c'), exemplarCount, loanCount, years);
            ProcessData(_series, record.FM(225, 'a'), exemplarCount, loanCount, years);
            ProcessData(_series, record.FMA(46, 'a'), exemplarCount, loanCount, years);
            ProcessData(_publishers, record.FM(210, 'c'), exemplarCount, loanCount, years);
            ProcessData(_publishers, record.FMA(461, 'g'), exemplarCount, loanCount, years);

            Console.Write('*');
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            string masterName = args[0];
            DirectAccessMode mode = DirectAccessMode.ReadOnly;
            using (_access = new DirectAccess64(masterName, mode))
            {
                int maxMfn = _access.GetMaxMfn();
                Console.WriteLine(maxMfn);

                for (int mfn = 1; mfn < maxMfn; mfn++)
                {
                    try
                    {
                        ProcessRecord(mfn);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"MFN={mfn}: exception: {exception.Message}");
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            _authors.Save();
            _titles.Save();
            _series.Save();
            _publishers.Save();
        }
    }
}
