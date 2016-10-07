using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class SearchParametersTest
    {
        private void _TestSerialization
            (
                SearchParameters first
            )
        {
            byte[] bytes = first.SaveToMemory();

            SearchParameters second
                = bytes.RestoreObjectFromMemory<SearchParameters>();

            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.FirstRecord, second.FirstRecord);
            Assert.AreEqual(first.FormatSpecification, second.FormatSpecification);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            Assert.AreEqual(first.MinMfn, second.MinMfn);
            Assert.AreEqual(first.NumberOfRecords, second.NumberOfRecords);
            Assert.AreEqual(first.SearchExpression, second.SearchExpression);
            Assert.AreEqual(first.SequentialSpecification, second.SequentialSpecification);
        }

        [TestMethod]
        public void TestSearchParameters_Serialization()
        {
            SearchParameters parameters = new SearchParameters();
            _TestSerialization(parameters);

            parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "T=A$",
                FormatSpecification = "@brief"
            };
            _TestSerialization(parameters);
        }

        [TestMethod]
        public void TestSearchParameters_Clone()
        {
            SearchParameters expected = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "T=A$",
                FormatSpecification = "@brief"
            };
            SearchParameters actual = expected.Clone();

            Assert.AreEqual(expected.Database, actual.Database);
            Assert.AreEqual(expected.FirstRecord, actual.FirstRecord);
            Assert.AreEqual(expected.FormatSpecification, actual.FormatSpecification);
            Assert.AreEqual(expected.MaxMfn, actual.MaxMfn);
            Assert.AreEqual(expected.MinMfn, actual.MinMfn);
            Assert.AreEqual(expected.NumberOfRecords, actual.NumberOfRecords);
            Assert.AreEqual(expected.SearchExpression, actual.SearchExpression);
            Assert.AreEqual(expected.SequentialSpecification, actual.SequentialSpecification);
        }

        [TestMethod]
        public void TestSearchParameters_Verify()
        {
            SearchParameters parameters = new SearchParameters
            {
                Database = "IBIS",
                SearchExpression = "T=A$",
                FormatSpecification = "@brief"
            };
            Assert.IsTrue(parameters.Verify(false));
        }
    }
}
