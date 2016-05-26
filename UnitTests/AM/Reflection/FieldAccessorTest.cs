using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class FieldAccessorTest
    {
        private CanaryClass canary;

        private FieldAccessor<CanaryClass, bool> booleanAccessor;
        private FieldAccessor<CanaryClass, int> int32Accessor;
        private FieldAccessor<CanaryClass, string> stringAccessor;

        [TestInitialize]
        public void Setup()
        {
            canary = new CanaryClass
            {
                BooleanField = true,
                Int32Field = 123,
                StringField = "Hello"
            };

            booleanAccessor = new FieldAccessor<CanaryClass, bool>
                    (
                        "BooleanField"
                    );
            int32Accessor = new FieldAccessor<CanaryClass, int>
                    (
                        "Int32Field"
                    );
            stringAccessor = new FieldAccessor<CanaryClass, string>
                    (
                        "StringField"
                    );
        }

        [TestMethod]
        public void TestFieldAccessorGetAndSet()
        {
            bool actualBoolean = booleanAccessor.Get(canary);
            Assert.AreEqual(canary.BooleanField, actualBoolean);

            int actualInt32 = int32Accessor.Get(canary);
            Assert.AreEqual(canary.Int32Field, actualInt32);

            string actualString = stringAccessor.Get(canary);
            Assert.AreEqual(canary.StringField, actualString);

            booleanAccessor.Set (canary, false);
            Assert.AreEqual(false, canary.BooleanField);

            int32Accessor.Set (canary, 1234);
            Assert.AreEqual(1234, canary.Int32Field);

            stringAccessor.Set (canary, "World");
            Assert.AreEqual("World", canary.StringField);
        }
    }
}
