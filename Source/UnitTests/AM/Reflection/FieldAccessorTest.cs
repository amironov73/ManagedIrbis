using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class FieldAccessorTest
    {
        [TestMethod]
        public void FieldAccessor_Get_Set_1()
        {
            CanaryClass canary = new CanaryClass
            {
                BooleanField = true,
                Int32Field = 123,
                StringField = "Hello"
            };

            FieldAccessor<CanaryClass, bool> booleanAccessor
                = new FieldAccessor<CanaryClass, bool>("BooleanField");
            FieldAccessor<CanaryClass, int> int32Accessor
                = new FieldAccessor<CanaryClass, int>("Int32Field");
            FieldAccessor<CanaryClass, string> stringAccessor
                = new FieldAccessor<CanaryClass, string>("StringField");

            Assert.AreEqual("BooleanField", booleanAccessor.FieldName);
            Assert.AreEqual("Int32Field", int32Accessor.FieldName);
            Assert.AreEqual("StringField", stringAccessor.FieldName);

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldAccessor_Get_Set_2()
        {
            FieldAccessor<CanaryClass, bool> wrongAccessor
                = new FieldAccessor<CanaryClass, bool>("NonexistentField"); 
        }
    }
}
