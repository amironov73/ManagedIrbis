using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.ImportExport;

namespace UnitTests.Experiments
{
    //[TestClass]
    public class IsoExperiments
    {
        //[TestMethod]
        public void DumpTestRecord()
        {
            string path = @"D:\Projects\ManagedClient.45\TestData\Test1.iso";
            MarcRecord record;
            using (FileStream stream = File.OpenRead(path))
            {
                record = Iso2709.ReadRecord(stream, IrbisEncoding.Ansi);
            }
            Assert.IsNotNull(record);
            string sourceCode = record.ToSourceCode();
            string tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, sourceCode, Encoding.UTF8);
        }
    }
}
