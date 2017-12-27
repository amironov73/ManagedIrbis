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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using AM.Text;
using IrbisInterop;
using ManagedIrbis;
using ManagedIrbis.Client;

#endregion

namespace DoNotGive
{
    class Program
    {
        private static string _connectionString;
        private static string _fileName, _fond;
        private static IrbisProvider _provider;

        static void _ProcessNumber
            (
                string number
            )
        {
            int[] found = _provider.Search
                (
                    "\"IN="
                    + number
                    + "\""
                );
            foreach (int mfn in found)
            {
                MarcRecord record = _provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    continue;
                }

                bool modified = false;
                foreach (RecordField field in record.Fields.GetField(910))
                {
                    string n2 = field.GetFirstSubFieldValue('b');
                    if (!n2.SameString(number))
                    {
                        continue;
                    }

                    string fond = field.GetFirstSubFieldValue('d');
                    if (!string.IsNullOrEmpty(fond)
                        && !fond.SameString(_fond))
                    {
                        continue;
                    }

                    string status = field.GetFirstSubFieldValue('a');
                    if (status == "0")
                    {
                        continue;
                    }

                    Console.WriteLine(number);
                    field.SetSubField('d', _fond);
                    field.SetSubField('a', "0");
                    modified = true;
                }

                if (modified)
                {
                    _provider.WriteRecord(record);
                }
            }
        }

        static void Main
            (
                string[] args
            )
        {
            if (args.Length != 3)
            {
                return;
            }

            _connectionString = args[0];
            _fond = args[1];
            _fileName = args[2];
            string[] numbers = File.ReadAllLines(_fileName)
                .NonEmptyLines().ToArray();

            NativeIrbisProvider.Register();

            try
            {
                using (_provider = ProviderManager
                    .GetAndConfigureProvider(_connectionString))
                {
                    foreach (string number in numbers)
                    {
                        _ProcessNumber(number);
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
