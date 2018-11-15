using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;

// ReSharper disable LocalizableElement

namespace MassWriteOff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            try
            {
                var connectionString = args[0];
                var lines = File.ReadAllLines(args[1]);

                using (var connection = new IrbisConnection(connectionString))
                {
                    var manager = new ExemplarManager(connection);

                    foreach (string line in lines)
                    {
                        var number = line.Trim();
                        if (string.IsNullOrEmpty(number))
                        {
                            continue;
                        }

                        Console.Write($"{number}: ", number);
                        var exemplar = manager.ReadExtend(number);
                        if (ReferenceEquals(exemplar, null))
                        {
                            Console.WriteLine("NOT FOUND");
                            continue;
                        }

                        if (exemplar.Status == "6")
                        {
                            Console.WriteLine("ALREADY SET");
                            continue;
                        }

                        exemplar.Status = "6";
                        var field = exemplar.Field.ThrowIfNull("exemplar.Field");
                        exemplar.ApplyToField(field);
                        var record = exemplar.Record.ThrowIfNull("exemplar.Record");
                        connection.WriteRecord(record);
                        Console.WriteLine($"[{record.Mfn}] {exemplar.Description}");
                    }
                }

                Console.WriteLine("DONE");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
