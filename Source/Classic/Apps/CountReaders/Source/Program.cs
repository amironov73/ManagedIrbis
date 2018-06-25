// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Readers;
using ManagedIrbis.Search;

#endregion

// ReSharper disable LocalizableElement
// ReSharper disable InconsistentNaming
// ReSharper disable UseStringInterpolation
// ReSharper disable InlineOutVariableDeclaration

namespace CountReaders
{
    class Program
    {
        private static IrbisConnection connection;

        static void AnalyzeYear
            (
                int year
            )
        {
            string expression = string.Format("RD={0}$", year.ToInvariantString());
            int[] found = connection.Search(expression);
            List<MarcRecord> records = new BatchRecordReader
                    (
                        connection,
                        connection.Database,
                        500,
                        true,
                        found
                    )
                    .ReadAll(true);

            foreach (MarcRecord record in records)
            {
                ReaderInfo reader = ReaderInfo.Parse(record);

                DateTime registrationDate = reader.RegistrationDate;
                DateTime lastVisitDate = reader.LastVisitDate;
                if (lastVisitDate == DateTime.MinValue)
                {
                    lastVisitDate = reader.LastRegistrationDate;
                }
                if (lastVisitDate == DateTime.MinValue)
                {
                    if (!ReferenceEquals(reader.Enrollment, null)
                        && reader.Enrollment.Length != 0)
                    {
                        lastVisitDate = reader.Enrollment.Max(e => e.Date);
                    }
                }
                if (lastVisitDate == DateTime.MinValue)
                {
                    lastVisitDate = registrationDate;
                }
                int visitDays = (int) (lastVisitDate - registrationDate).TotalDays;
                if (visitDays == 0)
                {
                    visitDays = 1;
                }
                int visits = reader.Visits.Length;
                double rate = (double) visits / visitDays;
                Console.WriteLine
                    (
                        "{0}\t{1}\t{2}\t{3}\t{4}\t{5:F3}",
                        reader.Ticket,
                        registrationDate.ToShortDateString(),
                        visitDays,
                        reader.LastVisitDate.ToShortDateString(),
                        visits,
                        rate
                    );
            }
        }

        static void AnalyzeTerms()
        {
            TermParameters parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = "RD=",
                NumberOfTerms = 10000
            };
            TermInfo[] terms = connection.ReadTerms(parameters);
            terms = terms.Where(ti => ti.Text.SafeStarts("RD=201"))
                .ToArray();
            terms = TermInfo.TrimPrefix(terms, "RD=");
            foreach (TermInfo term in terms)
            {
                DateTime date = IrbisDate.ConvertStringToDate(term.Text);
                Console.WriteLine("{0:yyyy-MM-dd}\t{1}", date, term.Count);
            }

        }

        static void Main()
        {
            try
            {
                using (connection = IrbisConnectionUtility.GetClientFromConfig())
                {
                    for (int year = 2010; year < 2018; year++)
                    {
                        AnalyzeYear(year);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
