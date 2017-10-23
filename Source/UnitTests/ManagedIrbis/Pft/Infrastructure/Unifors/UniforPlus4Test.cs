using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus4Test
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(RecordField.Parse(1, "^aFirst"));
            record.Fields.Add(RecordField.Parse(1, "^aSecond"));
            record.Fields.Add(RecordField.Parse(1, "^aThird"));
            record.Fields.Add(RecordField.Parse(1, "^aFirst"));

            return record;
        }

        private void _4
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        private void _4A
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            Unifor unifor = new Unifor();
            string expression = input;
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlus4_GetField_1()
        {
            _4("+4", "");
            _4("+4Q", "");
            _4("+4Q1", "");
            _4("+4T", "");
            _4("+4N", "");
            _4("+4F", "");
            _4("+4T0", "1");
            _4("+4T1", "1");
            _4("+4T9", "1");
            _4("+4N0", "1");
            _4("+4N1", "1");
            _4("+4N9", "1");
            _4("+4F0", "^aFirst");
            _4("+4F1", "^aFirst");
            _4("+4F9", "^aFirst");

            _4A("+4T0", "");
        }
    }
}
