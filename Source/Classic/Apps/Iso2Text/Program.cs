using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.ImportExport;

namespace Iso2Text
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"D:\Temp45\1-31973.MRC";
            string outputPath = @"D:\Temp45\1-31973.txt";

            int counter = 0;
            using (IsoReader reader = new IsoReader(sourcePath, IrbisEncoding.Oem))
            using (TextWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                foreach (MarcRecord record in reader)
                {
                    if (record == null)
                    {
                        continue;
                    }

                    PlainText.WriteRecord(writer, record);
                    Console.Write('.');
                    counter++;
                }
            }

            Console.WriteLine();
            Console.WriteLine(counter);
            Console.WriteLine();
        }
    }
}
