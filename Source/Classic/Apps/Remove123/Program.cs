using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;

using IrbisInterop;

using ManagedIrbis;
using ManagedIrbis.Server;

namespace Remove123
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            string iniPath = args[0];
            string database = args[1];

            using (IniFile iniFile = new IniFile
                (
                    iniPath,
                    IrbisEncoding.Ansi,
                    false
                ))
            {
                ServerIniFile serverIniFile = new ServerIniFile(iniFile);
                ServerConfiguration configuration
                    = ServerConfiguration.FromIniFile(serverIniFile);
                using (Irbis64Dll irbis = new Irbis64Dll(configuration))
                {
                    irbis.Layout = SpaceLayout.Version2014();

                    irbis.UseDatabase(database);
                    int maxMfn = irbis.GetMaxMfn();
                    Console.WriteLine("Max MFN={0}", maxMfn);

                    for (int mfn = 1; mfn < maxMfn; mfn++)
                    {
                        NativeRecord record;
                        try
                        {
                            irbis.ReadRecord(mfn);
                            record = irbis.GetRecord();
                        }
                        catch
                        {
                            continue;
                        }

                        NativeField[] fields = record.Fields
                            .Where(field => field.Tag < 4)
                            .ToArray();
                        foreach (NativeField field in fields)
                        {
                            record.Fields.Remove(field);
                        }
                        if (fields.Length != 0)
                        {
                            Console.WriteLine(mfn);
                            irbis.SetRecord(record);
                            irbis.WriteRecord(true, false);
                        }
                    }
                }
            }
        }
    }
}
