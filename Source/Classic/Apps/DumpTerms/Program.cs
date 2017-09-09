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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

#endregion

namespace DumpTerms
{
    class Program
    {
        static void DumpTerm
            (
                [NotNull] IrbisConnection connection,
                [NotNull] TermInfo term
            )
        {
            Console.WriteLine(term.Text);

            PostingParameters parameters = new PostingParameters
            {
                Database = connection.Database,
                Term = term.Text
            };
            TermPosting[] postings = connection.ReadPostings(parameters);
            Console.WriteLine("\tPostings: {0}", postings.Length);
            foreach (TermPosting posting in postings)
            {
                Console.WriteLine
                    (
                        "\tMFN={0} Tag={1} Occ={2} Count={3}",
                        posting.Mfn,
                        posting.Tag,
                        posting.Occurrence,
                        posting.Count
                    );

                try
                {
                    MarcRecord record = connection.ReadRecord(posting.Mfn);
                    RecordField field = record.Fields
                        .GetField(posting.Tag)
                        .GetOccurrence(posting.Occurrence - 1);
                    if (ReferenceEquals(field, null))
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine
                        (
                            "\t{0}",
                            field.ToText()
                        );
                        Console.WriteLine();
                    }
                }
                catch
                {
                    // Nothing to do here
                }
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <connectionString> <termPrefix>");

                return;
            }

            string connectionString = args[0];
            string termPrefix = args[1];

            try
            {
                using (IrbisConnection connection
                    = new IrbisConnection(connectionString))
                {
                    TermParameters parameters = new TermParameters
                    {
                        Database = connection.Database,
                        StartTerm = termPrefix,
                        NumberOfTerms = 100
                    };
                    TermInfo[] terms = connection.ReadTerms(parameters);
                    Console.WriteLine("Found terms: {0}", terms.Length);
                    foreach (TermInfo term in terms)
                    {
                        DumpTerm(connection, term);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
