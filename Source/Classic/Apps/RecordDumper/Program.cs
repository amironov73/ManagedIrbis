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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace RecordDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine
                    (
                        "RecordDumper <connection-string> "
                        + "<search-expression> <output-file>"
                    );

                return;
            }

            string connectionString = args[0];
            string searchExpression = args[1];
            string outputFileName = args[2];

            try
            {
                JObject config
                    = JsonUtility.ReadObjectFromFile("dumperConfig.json");
                int etalonTag = config["etalonTag"].Value<int>();
                int[] excludeTags = config["excludeTags"]
                    .Values<int>().ToArray();

                using (IrbisProvider provider = ProviderManager
                    .GetAndConfigureProvider(connectionString))
                using (StreamWriter writer
                    = new StreamWriter(outputFileName, false))
                {
                    int[] found = provider.Search(searchExpression);
                    Console.WriteLine("Found: {0}", found.Length);

                    foreach (int mfn in found)
                    {
                        MarcRecord record = provider.ReadRecord(mfn);
                        if (ReferenceEquals(record, null))
                        {
                            continue;
                        }
                        string title = record.FM(2022);
                        if (string.IsNullOrEmpty(title))
                        {
                            continue;
                        }
                        string worksheet = record.FM(920);
                        if (string.IsNullOrEmpty(worksheet))
                        {
                            continue;
                        }

                        Console.WriteLine("MFN={0}: {1}", mfn, title);
                        writer.WriteLine("<h3>{0}</h3>", title);

                        RecordField[] fields = record.Fields
                            .Where(field => field.Tag.OneOf(excludeTags))
                            .ToArray();
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
