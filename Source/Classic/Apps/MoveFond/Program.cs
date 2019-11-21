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
using AM.Configuration;

using ManagedIrbis;
using ManagedIrbis.Fields;

#endregion

// ReSharper disable LocalizableElement

namespace MoveFond
{
    class Program
    {
        static IrbisConnection _connection;
        private static int _counter = 0;

        static void ProcessRecord
            (
                MarcRecord record
            )
        {
            bool move = false;

            var exemplars = ExemplarInfo.Parse(record)
                .Where(field => field.Place.OneOf("Ф501", "Ф502"))
                .Where(field => field.RealPlace.SameString(field.Place))
                .Where(field => !string.IsNullOrEmpty(field.CheckedDate))
                .ToArray();
            foreach (var exemplar in exemplars)
            {
                var field = exemplar.Field.ThrowIfNull();
                field.SetSubField('d', "Ф404");
                field.SetSubField('!', "Ф404");

                move = true;
            }

            if (move)
            {
                _counter++;
                _connection.WriteRecord(record);
                Console.Write('!');
            }
        }

        static void Main(string[] args)
        {
            var connectionString = ConfigurationUtility
                .GetString("connection-string")
                .ThrowIfNull("connectionString");

            try
            {
                using (_connection = new IrbisConnection(connectionString))
                {
                    var found = _connection.SearchRead("MHR=Ф501 + MHR=Ф502");
                    foreach (var record in found)
                    {
                        ProcessRecord(record);
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Total = {_counter}");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
