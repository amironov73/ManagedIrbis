using System;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class UniforTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] string expression,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            PftNode node = new PftNode();
            Unifor unifor = new Unifor();
            unifor.Execute(context, node, expression);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(new RecordField(1, "11|22|33|44"));
            result.Fields.Add(new RecordField(2, "^A11^B00^A22^B99^A33"));
            result.Fields.Add(new RecordField(3, "^1100^AQQQ^BWWW^1200^AEEE^BRRR"));
            result.Fields.Add(new RecordField(4));

            return result;
        }

        [TestMethod]
        public void Unifor_Construction_1()
        {
            Unifor unifor = new Unifor();
            Assert.AreEqual("unifor", unifor.Name);

            Assert.IsNotNull(Unifor.Registry);
            Assert.AreEqual(95, Unifor.Registry.Count);
            Assert.IsFalse(Unifor.ThrowOnUnknown);
            Assert.IsFalse(Unifor.ThrowOnEmpty);
        }

        [TestMethod]
        public void Unifor_FindAction_1()
        {
            string expression = "+9V";
            Action<PftContext, PftNode, string> action = Unifor.FindAction(ref expression);
            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void Unifor_Execute_1()
        {
            MarcRecord record = _GetRecord();
            string expression = "+9V";
            string expected = "64";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void Unifor_Execute_2()
        {
            bool save = Unifor.ThrowOnUnknown;
            try
            {
                Unifor.ThrowOnUnknown = true;
                MarcRecord record = _GetRecord();
                string expression = "*";
                string expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnUnknown = save;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void Unifor_Execute_3()
        {
            bool save = Unifor.ThrowOnEmpty;
            try
            {
                Unifor.ThrowOnEmpty = true;
                MarcRecord record = _GetRecord();
                string expression = "";
                string expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnEmpty = save;
            }
        }

        [TestMethod]
        public void Unifor_Execute_3a()
        {
            bool save = Unifor.ThrowOnEmpty;
            try
            {
                Unifor.ThrowOnEmpty = false;
                MarcRecord record = _GetRecord();
                string expression = "";
                string expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Unifor.ThrowOnEmpty = save;
            }
        }
    }
}
