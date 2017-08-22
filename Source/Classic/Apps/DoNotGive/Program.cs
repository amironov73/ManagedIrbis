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
        private static string _fromNumber, _toNumber, _fond;
        private static IrbisProvider _provider;

        static void _ProcessNumber
            (
                NumberText number
            )
        {
            string n1 = number.ToString();
            int[] found = _provider.Search
                (
                    "\"IN="
                    + n1
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
                foreach (RecordField field in record.Fields.GetField("910"))
                {
                    string n2 = field.GetFirstSubFieldValue('b');
                    if (!n2.SameString(n1))
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
                    if (status == "5")
                    {
                        continue;
                    }

                    Console.WriteLine(n1);
                    field.SetSubField('a', "5");
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
            if (args.Length != 4)
            {
                return;
            }

            _connectionString = args[0];
            _fond = args[1];
            _fromNumber = args[2];
            _toNumber = args[3];

            NativeIrbisProvider.Register();

            try
            {
                using (_provider = ProviderManager
                    .GetAndConfigureProvider(_connectionString))
                {
                    NumberText currentNumber = _fromNumber;
                    while (currentNumber < _toNumber)
                    {
                        _ProcessNumber(currentNumber);
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
