using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Text;

using ManagedIrbis;

namespace RangeStat
{
    class Program
    {
        private static IrbisConnection connection;
        private static NumberText fromNumber;
        private static NumberText toNumber;

        static void ProcessNumber
            (
                string number
            )
        {
            MarcRecord[] found = connection.SearchRead
                (
                    "\"IN={0}\"",
                    number
                );

            if (found.Length == 0)
            {
                return;
            }

            MarcRecord record = found[0];
            string description = connection.FormatRecord
                (
                    "@sbrief", 
                    record.Mfn
                );
            int count = record.FM("999").SafeToInt32();
            Console.WriteLine
                (
                    "{0}\t{1}\t{2}",
                    number,
                    count,
                    description
                );
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                return;
            }

            string connectionString = args[0];
            fromNumber = args[1];
            toNumber = args[2];

            try
            {
                using (connection = new IrbisConnection(connectionString))
                {
                    NumberText currentNumber = fromNumber;
                    while (currentNumber < toNumber)
                    {
                        ProcessNumber(currentNumber.ToString());
                        currentNumber = currentNumber.Increment();
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
