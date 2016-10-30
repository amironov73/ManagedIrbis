using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fst;
using ManagedIrbis.ImportExport;

namespace FstTestRunner
{
    class Program
    {
        static void Main()
        {
            try
            {
                string recordText = File.ReadAllText
                    (
                        "record.txt",
                        IrbisEncoding.Ansi
                    );
                StringReader reader = new StringReader(recordText);
                MarcRecord record = PlainText.ReadRecord(reader)
                    .ThrowIfNull("record!");

                LocalFstProcessor processor = new LocalFstProcessor
                    (
                        "C:\\IRBIS64",
                        "SANDBOX"
                    );
                FstFile fstFile = processor.ReadFile("marc_irb.fst")
                    .ThrowIfNull("fstFile!");

                MarcRecord transformed = processor.TransformRecord
                    (
                        record,
                        fstFile
                    );
                string result = transformed.ToPlainText();
                Console.WriteLine(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
