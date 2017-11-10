using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM
{
    [TestClass]
    public class GenderUtilityTest
    {
        [TestMethod]
        public void GenderUtility_Parse_1()
        {
            Assert.AreEqual(Gender.Male, GenderUtility.Parse("м"));
            Assert.AreEqual(Gender.Female, GenderUtility.Parse("ж"));
            Assert.AreEqual(Gender.NotSet, GenderUtility.Parse(""));
            Assert.AreEqual(Gender.NotSet, GenderUtility.Parse(null));
            Assert.AreEqual(Gender.NotSet, GenderUtility.Parse(";"));
        }

        [TestMethod]
        public void GenderUtility_ToString_1()
        {
            Assert.AreEqual("мужской", GenderUtility.ToString(Gender.Male));
            Assert.AreEqual("женский", GenderUtility.ToString(Gender.Female));
            Assert.AreEqual("н/д", GenderUtility.ToString(Gender.NotSet));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenderUtility_ToString_2()
        {
            GenderUtility.ToString((Gender) 10);
        }

        [TestMethod]
        public void GenderUtility_Vefify_1()
        {
            Assert.IsTrue(GenderUtility.Verify(Gender.NotSet, false));
            Assert.IsTrue(GenderUtility.Verify(Gender.Male, false));
            Assert.IsTrue(GenderUtility.Verify(Gender.Female, false));
            Assert.IsFalse(GenderUtility.Verify((Gender)10, false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenderUtility_Vefify_2()
        {
            Assert.IsFalse(GenderUtility.Verify((Gender)10, true));
        }
    }
}
