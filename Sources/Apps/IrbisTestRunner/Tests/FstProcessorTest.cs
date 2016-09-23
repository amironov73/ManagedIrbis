/* FstProcessorTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fst;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class FstProcessorTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        private FstFile _GetFstFile()
        {
            FstFile result = new FstFile();

            result.Lines.AddRange
                (
                    new[]
                    {
                        new FstLine { Tag = "200", Format = "v200" },
                        new FstLine { Tag = "210", Format = "v210" },
                        new FstLine { Tag = "215", Format = "v215" },
                        new FstLine { Tag = "900", Format = "v900" } 
                    }
                );

            return result;
        }

        #endregion

        #region Public methods

        [TestMethod]
        public void FstProcessor_TransformRecord()
        {
            IrbisConnection connection = Connection.ThrowIfNull();

            MarcRecord record = _GetRecord();
            FstFile fstFile = _GetFstFile();
            FstProcessor processor = new FstProcessor(connection);
            MarcRecord transformed = processor.TransformRecord
                (
                    record,
                    fstFile
                );
            Write(transformed);
        }

        #endregion
    }
}
