// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;

#endregion

// ReSharper disable LocalizableElement

namespace MassWriteOff
{
    class Program
    {
        private static char[] separators = {' ', '\t', ';'};

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
                        var parts = line.Trim()
                            .Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 0)
                        {
                            continue;
                        }
                        var number = parts[0];
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

                        string aktNumber = null;
                        if (parts.Length > 1)
                        {
                            aktNumber = parts[1];
                        }

                        exemplar.Status = "6";
                        var field = exemplar.Field.ThrowIfNull("exemplar.Field");
                        exemplar.ApplyToField(field);
                        if (!string.IsNullOrEmpty(aktNumber))
                        {
                            field.SetSubField('v', aktNumber);
                        }
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
