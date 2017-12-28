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
    public class UmarciTest
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
            Umarci umarci = new Umarci();
            umarci.Execute(context, node, expression);
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
        public void Umarci_Construction_1()
        {
            Umarci umarci = new Umarci();
            Assert.AreEqual("umarci", umarci.Name);

            Assert.IsNotNull(Umarci.Registry);
            Assert.AreEqual(5, Umarci.Registry.Count);
            Assert.IsFalse(Umarci.ThrowOnUnknown);
        }

        [TestMethod]
        public void Umarci_FindAction_1()
        {
            string expression = "12#a#1";
            Action<PftContext, PftNode, string> action = Umarci.FindAction(ref expression);
            Assert.IsNotNull(action);
        }

        [TestMethod]
        public void Umarci_Umarci0_1()
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            PftNode node = new PftNode();
            string expression = "0";
            Umarci.Umarci0(context, node, expression);
            string expected = "";
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Umarci_Umarci1_1()
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            PftNode node = new PftNode();
            string expression = "2#a#1";
            Umarci.Umarci1(context, node, expression);
            string expected = "11";
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Umarci_Umarci2_1()
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            PftNode node = new PftNode();
            string expression = "1#|";
            Umarci.Umarci2(context, node, expression);
            string expected = "3";
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Umarci_Umarci3_1()
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            PftNode node = new PftNode();
            string expression = "1#2#|";
            Umarci.Umarci3(context, node, expression);
            string expected = "22";
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Umarci_Umarci4_1()
        {
            PftContext context = new PftContext(null)
            {
                Record = _GetRecord()
            };
            PftNode node = new PftNode();
            string expression = "3/100^a";
            Umarci.Umarci4(context, node, expression);
            string expected = "QQQ";
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Umarci_Execute_1()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a#1";
            string expected = "11";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1a()
        {
            MarcRecord record = _GetRecord();
            string expression = "1";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1b()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1c()
        {
            MarcRecord record = _GetRecord();
            string expression = "1#a#1";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1d()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a#q";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1e()
        {
            MarcRecord record = _GetRecord();
            string expression = "1222#a#1";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1f()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a#33";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1g()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a#3";
            string expected = "33";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1h()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#q#3";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_1i()
        {
            MarcRecord record = _GetRecord();
            string expression = "12#a#-1";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2()
        {
            MarcRecord record = _GetRecord();
            string expression = "21#|";
            string expected = "3";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2a()
        {
            MarcRecord record = _GetRecord();
            string expression = "2";
            string expected = "0";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2b()
        {
            MarcRecord record = _GetRecord();
            string expression = "21";
            string expected = "0";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2c()
        {
            MarcRecord record = _GetRecord();
            string expression = "2#|";
            string expected = "0";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2d()
        {
            MarcRecord record = _GetRecord();
            string expression = "2111#|";
            string expected = "0";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_2e()
        {
            MarcRecord record = _GetRecord();
            string expression = "24#|";
            string expected = "0";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#2#|";
            string expected = "22";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3a()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#0#|";
            string expected = "11|22|33|44";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3b()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#1#|";
            string expected = "11";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3c()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#3#|";
            string expected = "33";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3d()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#4#|";
            string expected = "44";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3e()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#5#|";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3f()
        {
            MarcRecord record = _GetRecord();
            string expression = "3";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3g()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#0";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3h()
        {
            MarcRecord record = _GetRecord();
            string expression = "3#0#|";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3i()
        {
            MarcRecord record = _GetRecord();
            string expression = "3100#0#|";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3j()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#q#|";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3k()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#0#?";
            string expected = "11|22|33|44";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3l()
        {
            MarcRecord record = _GetRecord();
            string expression = "31#1#?";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_3m()
        {
            MarcRecord record = _GetRecord();
            string expression = "34#1#|";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4()
        {
            MarcRecord record = _GetRecord();
            string expression = "43/100^a";
            string expected = "QQQ";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4a()
        {
            MarcRecord record = _GetRecord();
            string expression = "4";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4b()
        {
            MarcRecord record = _GetRecord();
            string expression = "43";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4c()
        {
            MarcRecord record = _GetRecord();
            string expression = "43/";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4d()
        {
            MarcRecord record = _GetRecord();
            string expression = "4444/200^a";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_4e()
        {
            MarcRecord record = _GetRecord();
            string expression = "43/222^a";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_5()
        {
            MarcRecord record = _GetRecord();
            string expression = "";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        public void Umarci_Execute_6()
        {
            MarcRecord record = _GetRecord();
            string expression = "QQQ";
            string expected = "";
            _Execute(record, expression, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void Umarci_Execute_6a()
        {
            bool save = Umarci.ThrowOnUnknown;
            try
            {
                Umarci.ThrowOnUnknown = true;
                MarcRecord record = _GetRecord();
                string expression = "QQQ";
                string expected = "";
                _Execute(record, expression, expected);
            }
            finally
            {
                Umarci.ThrowOnUnknown = save;
            }
        }
    }
}
