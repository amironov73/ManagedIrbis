using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermParametersTest
    {
        private TermParameters _GetParameters()
        {
            TermParameters result  = new TermParameters
            {
                Database = "IBIS",
                Format = "@brief",
                NumberOfTerms = 10,
                ReverseOrder = false,
                StartTerm = "T=HELLLO"
            };

            return result;
        }

        private void _TestSerialization
            (
                TermParameters first
            )
        {
            byte[] bytes = first.SaveToMemory();

            TermParameters second = bytes
                .RestoreObjectFromMemory<TermParameters>();

            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Format, second.Format);
            Assert.AreEqual(first.NumberOfTerms, second.NumberOfTerms);
            Assert.AreEqual(first.ReverseOrder, second.ReverseOrder);
            Assert.AreEqual(first.StartTerm, second.StartTerm);
        }

        [TestMethod]
        public void TestTermParameters_Serialization()
        {
            TermParameters parameters = new TermParameters();
            _TestSerialization(parameters);

            parameters = _GetParameters();
            _TestSerialization(parameters);
        }

        [TestMethod]
        public void TestTermParameters_ApplyToCommand()
        {
            TermParameters parameters = _GetParameters();
            IrbisConnection connection = new IrbisConnection();
            ReadTermsCommand command = new ReadTermsCommand(connection);
            command.ApplyParameters(parameters);

            Assert.AreEqual(parameters.Database, command.Database);
            Assert.AreEqual(parameters.Format, command.Format);
            Assert.AreEqual(parameters.NumberOfTerms, command.NumberOfTerms);
            Assert.AreEqual(parameters.ReverseOrder, command.ReverseOrder);
            Assert.AreEqual(parameters.StartTerm, command.StartTerm);
        }

        [TestMethod]
        public void TestTermParameters_GatherFromCommand()
        {
            IrbisConnection connection = new IrbisConnection();
            ReadTermsCommand command = new ReadTermsCommand(connection)
            {
                Database = "IBIS",
                Format = "@brief",
                NumberOfTerms = 10,
                StartTerm = "T=HELLO"
            };
            TermParameters parameters = command.GatherParameters();

            Assert.AreEqual(command.Database, parameters.Database);
            Assert.AreEqual(command.Format, parameters.Format);
            Assert.AreEqual(command.NumberOfTerms, parameters.NumberOfTerms);
            Assert.AreEqual(command.ReverseOrder, parameters.ReverseOrder);
            Assert.AreEqual(command.StartTerm, parameters.StartTerm);
        }

        [TestMethod]
        public void TestTermParameters_Clone()
        {
            TermParameters expected = _GetParameters();
            TermParameters actual = expected.Clone();

            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.Format, actual.Format);
            Assert.AreEqual(expected.NumberOfTerms, actual.NumberOfTerms);
            Assert.AreEqual(expected.ReverseOrder, actual.ReverseOrder);
            Assert.AreEqual(expected.StartTerm, actual.StartTerm);
        }

        [TestMethod]
        public void TestTermParameters_Verify()
        {
            TermParameters parameters = _GetParameters();
            Assert.IsTrue(parameters.Verify(false));
        }
    }
}
