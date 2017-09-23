using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ExemplarInfoComparerTest
    {
        [NotNull]
        private ExemplarInfo _GetFirst()
        {
            return new ExemplarInfo
            {
                Number = "1234",
                Description = "First description"
            };
        }

        [NotNull]
        private ExemplarInfo _GetSecond()
        {
            return new ExemplarInfo
            {
                Number = "234",
                Description = "Second description"
            };
        }

        [TestMethod]
        public void ExemplarInfoComparer_ByNumber_1()
        {
            ExemplarInfo first = _GetFirst();
            ExemplarInfo second = _GetSecond();
            IComparer<ExemplarInfo> comparer = ExemplarInfoComparer.ByNumber();

            Assert.IsTrue
                (
                    comparer.Compare(first, second) > 0
                );
        }

        [TestMethod]
        public void ExemplarInfoComparer_ByDescription_1()
        {
            ExemplarInfo first = _GetFirst();
            ExemplarInfo second = _GetSecond();
            IComparer<ExemplarInfo> comparer = ExemplarInfoComparer.ByDescription();

            Assert.IsTrue
            (
                comparer.Compare(first, second) < 0
            );
        }
    }
}
