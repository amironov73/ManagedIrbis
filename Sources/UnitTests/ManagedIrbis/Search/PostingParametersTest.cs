using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class PostingParametersTest
    {
        [TestMethod]
        public void TestPostingParameters_ApplyToCommand1()
        {
            PostingParameters parameters = new PostingParameters
            {
                Database = "IBIS",
                Term = "T=HELLO",
                Format = "@brief"
            };
            IrbisConnection connection = new IrbisConnection();
            ReadPostingsCommand command = new ReadPostingsCommand(connection);
            command.ApplyParameters(parameters);

            Assert.AreEqual(parameters.Database, command.Database);
            Assert.AreEqual(parameters.FirstPosting, command.FirstPosting);
            Assert.AreEqual(parameters.Format, command.Format);
            Assert.AreEqual(parameters.ListOfTerms, command.ListOfTerms);
            Assert.AreEqual(parameters.NumberOfPostings, command.NumberOfPostings);
            Assert.AreEqual(parameters.Term, command.Term);
        }

        [TestMethod]
        public void TestPostingParameters_ApplyToCommand2()
        {
            PostingParameters parameters = new PostingParameters
            {
                Database = "IBIS",
                ListOfTerms = new[] {"T=HELLO", "T=WORLD"},
                Format = "@brief"
            };
            IrbisConnection connection = new IrbisConnection();
            ReadPostingsCommand command = new ReadPostingsCommand(connection);
            command.ApplyParameters(parameters);

            Assert.AreEqual(parameters.Database, command.Database);
            Assert.AreEqual(parameters.FirstPosting, command.FirstPosting);
            Assert.AreEqual(parameters.Format, command.Format);
            Assert.AreEqual(parameters.ListOfTerms, command.ListOfTerms);
            Assert.AreEqual(parameters.NumberOfPostings, command.NumberOfPostings);
            Assert.AreEqual(parameters.Term, command.Term);
        }

        [TestMethod]
        public void TestPostingParameters_Clone()
        {
            PostingParameters expected = new PostingParameters
            {
                Database = "IBIS",
                Term = "T=HELLO",
                Format = "@brief"
            };
            PostingParameters actual = expected.Clone();

            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.FirstPosting, actual.FirstPosting);
            Assert.AreEqual(expected.Format, actual.Format);
            Assert.AreEqual(expected.ListOfTerms, actual.ListOfTerms);
            Assert.AreEqual(expected.NumberOfPostings, actual.NumberOfPostings);
            Assert.AreEqual(expected.Term, actual.Term);
        }

        [TestMethod]
        public void TestPostingParameters_Verify()
        {
            PostingParameters parameters = new PostingParameters();
            Assert.IsFalse(parameters.Verify(false));

            parameters = new PostingParameters
            {
                Database = "IBIS",
                Term = "T=HELLO",
                Format = "@brief"
            };
            Assert.IsTrue(parameters.Verify(false));
        }


        [TestMethod]
        public void TestPostingParameters_GatherFromCommand1()
        {
            IrbisConnection connection = new IrbisConnection();
            ReadPostingsCommand command = new ReadPostingsCommand(connection)
            {
                Database = "IBIS",
                Term = "T=HELLO",
                Format = "@brief"
            };
            PostingParameters parameters = command.GatherParameters();

            Assert.AreEqual(command.Database, parameters.Database);
            Assert.AreEqual(command.FirstPosting, parameters.FirstPosting);
            Assert.AreEqual(command.Format, parameters.Format);
            Assert.AreEqual(command.ListOfTerms, parameters.ListOfTerms);
            Assert.AreEqual(command.NumberOfPostings, parameters.NumberOfPostings);
            Assert.AreEqual(command.Term, parameters.Term);
        }

        [TestMethod]
        public void TestPostingParameters_GatherFromCommand2()
        {
            IrbisConnection connection = new IrbisConnection();
            ReadPostingsCommand command = new ReadPostingsCommand(connection)
            {
                Database = "IBIS",
                ListOfTerms = new[] { "T=HELLO", "T=WORLD" },
                Format = "@brief"
            };
            PostingParameters parameters = command.GatherParameters();

            Assert.AreEqual(command.Database, parameters.Database);
            Assert.AreEqual(command.FirstPosting, parameters.FirstPosting);
            Assert.AreEqual(command.Format, parameters.Format);
            Assert.AreEqual(command.ListOfTerms, parameters.ListOfTerms);
            Assert.AreEqual(command.NumberOfPostings, parameters.NumberOfPostings);
            Assert.AreEqual(command.Term, parameters.Term);
        }
    }
}
