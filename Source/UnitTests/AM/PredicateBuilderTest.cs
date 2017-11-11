using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class PredicateBuilderTest
    {
        [TestMethod]
        public void PredicateBuilder_True_1()
        {
            var trueFunc = PredicateBuilder.True<int>().Compile();
            Assert.IsTrue(trueFunc(0));
        }

        [TestMethod]
        public void PredicateBuilder_True_2()
        {
            var trueFunc = PredicateBuilder.True().Compile();
            Assert.IsTrue(trueFunc());
        }

        [TestMethod]
        public void PredicateBuilder_False_1()
        {
            var falseFunc = PredicateBuilder.False<int>().Compile();
            Assert.IsFalse(falseFunc(0));
        }

        [TestMethod]
        public void PredicateBuilder_False_2()
        {
            var falseFunc = PredicateBuilder.False().Compile();
            Assert.IsFalse(falseFunc());
        }

        [TestMethod]
        public void PredicateBuilder_Create_1()
        {
            var func = PredicateBuilder.Create<int>(x => x > 0).Compile();
            Assert.IsTrue(func(10));
            Assert.IsFalse(func(-10));
        }

        [TestMethod]
        public void PredicateBuilder_Or_1()
        {
            var func1 = PredicateBuilder.Create<int>(x => x > 0);
            var func2 = PredicateBuilder.Create<int>(x => x < -100);
            var orFunc = func1.Or(func2).Compile();
            Assert.IsTrue(orFunc(10));
            Assert.IsTrue(orFunc(-110));
            Assert.IsFalse(orFunc(-10));
        }

        [TestMethod]
        public void PredicateBuilder_And_1()
        {
            var func1 = PredicateBuilder.Create<int>(x => x > 0);
            var func2 = PredicateBuilder.Create<int>(x => x < 100);
            var andFunc = func1.And(func2).Compile();
            Assert.IsTrue(andFunc(10));
            Assert.IsFalse(andFunc(-10));
            Assert.IsFalse(andFunc(110));
        }

        [TestMethod]
        public void PredicateBuilder_Not_1()
        {
            var func = PredicateBuilder.Create<int>(x => x > 0);
            var notFunc = func.Not().Compile();
            Assert.IsTrue(notFunc(-10));
            Assert.IsFalse(notFunc(10));
        }
    }
}
