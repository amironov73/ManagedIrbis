using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class ReferenceTest
    {
        [TestMethod]
        public void Reference_Construction1()
        {
            Reference<string> reference = new Reference<string>();
            Assert.IsNull(reference.Target);
            Assert.AreEqual(1, reference.Counter);
        }

        [TestMethod]
        public void Reference_Construction2()
        {
            const string expected1 = "abc";
            Reference<string> reference = new Reference<string>(expected1);
            Assert.AreEqual(expected1, reference.Target);
        }

        [TestMethod]
        public void Reference_Value()
        {
            const string expected1 = "abc", expected2 = "cba";
            Reference<string> reference = new Reference<string>(expected1);
            int count = 0;
            reference.TargetChanged += (sender, args) => { count++; };
            Assert.AreEqual(expected1, reference.Target);
            reference.Target = expected2;
            Assert.AreEqual(expected2, reference.Target);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void Reference_Counter()
        {
            const string expected1 = "abc";
            Reference<string> reference = new Reference<string>(expected1);
            Assert.AreEqual(expected1, reference.Target);
            Assert.AreEqual(1, reference.Counter);
            Assert.AreEqual(1, reference.ResetCounter());
            Assert.AreEqual(0, reference.Counter);
        }

        [TestMethod]
        public void Reference_Implicit1()
        {
            const string expected = "abc";
            Reference<string> reference = new Reference<string>(expected);
            string actual = reference;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Reference_Implicit2()
        {
            const string expected = "abc";
            Reference<string> reference = expected;
            Assert.AreEqual(expected, reference.Target);
        }

        [TestMethod]
        public void Reference_ToString()
        {
            const string expected = "abc";
            Reference<string> reference = new Reference<string>(expected);
            Assert.AreEqual(expected, reference.ToString());
        }
    }
}
