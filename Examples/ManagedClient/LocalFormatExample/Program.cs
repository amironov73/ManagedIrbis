using System;
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;

namespace LocalFormatExample
{
    class Program
    {
        private const string LocalRootPath = "C:\\IRBIS64_2015";

        private const string ScriptText 
            = "v200^a, \" : \"v200^e, \" / \"v200^f";

        static void Main()
        {
            try
            {
                if (!Directory.Exists(LocalRootPath))
                {
                    throw new ApplicationException
                        (
                            "Root path doesn't exist!"
                        );
                }

                using (LocalClient client 
                    = new LocalClient(LocalRootPath))
                {
                    client.Database = "IBIS";

                    MarcRecord record = client.ReadRecord
                        (
                            client.GetMaxMfn() / 2
                        );
                    if (ReferenceEquals(record, null))
                    {
                        throw new ApplicationException
                            (
                                "Can't read record"
                            );
                    }

                    // See the record content (for debug)
                    // Console.WriteLine(record.ToPlainText());

                    using (PftFormatter formatter = new PftFormatter())
                    {
                        formatter.SetEnvironment(client);

                        formatter.ParseProgram(ScriptText);

                        // We can use @file syntax
                        // formatter.ParseProgram("@brief");

                        string result = formatter.Format(record);

                        Console.WriteLine(result);
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
