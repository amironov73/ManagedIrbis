using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusETest
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(RecordField.Parse(910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^A0^B559^C19990924^H107256G^=2^U2004/7"));
            record.Fields.Add(RecordField.Parse(910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60"));
            record.Fields.Add(RecordField.Parse(910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ"));

            return record;
        }

        private void _E
            (
                int index,
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Index = index,
                Record = _GetRecord()
            };
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlusE_GetFieldIndex_1()
        {
            _E(0, "+E910#1", "1");
            _E(0, "+E910#2", "2");
            _E(0, "+E910#3", "3");
            _E(0, "+E910#33", "");
            _E(0, "+E910#",  "1");
            _E(0, "+E910#q",  "");
            _E(0, "+E910#*", "1");
            _E(1, "+E910#*", "2");
            _E(2, "+E910#*", "3");
            _E(33, "+E910#*", "");
        }
    }
}
