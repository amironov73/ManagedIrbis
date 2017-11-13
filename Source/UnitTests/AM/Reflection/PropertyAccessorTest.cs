using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class PropertyAccessorTest
    {
        private CanaryClass canary;

        private PropertyAccessor<CanaryClass, bool> booleanAccessor;
        private PropertyAccessor<CanaryClass, int> int32Accessor;
        private PropertyAccessor<CanaryClass, string> stringAccessor;

        [TestInitialize]
        public void Setup ()
        {
            canary = new CanaryClass
            {
                BooleanProperty = true,
                Int32Property = 123,
                StringProperty = "Hello"
            };

            booleanAccessor = new PropertyAccessor<CanaryClass, bool>
                    (
                        canary,
                        "BooleanProperty"
                    );
            int32Accessor = new PropertyAccessor<CanaryClass, int>
                    (
                        canary,
                        "Int32Property"
                    );
            stringAccessor = new PropertyAccessor<CanaryClass, string>
                    (
                        canary,
                        "StringProperty"
                    );
        }

        [TestMethod]
        public void PropertyAccessor_Get_Set_1()
        {
            Assert.AreEqual("BooleanProperty", booleanAccessor.Name);
            Assert.AreEqual("Int32Property", int32Accessor.Name);
            Assert.AreEqual("StringProperty", stringAccessor.Name);

            bool actualBoolean = booleanAccessor.Value;
            Assert.AreEqual(canary.BooleanProperty, actualBoolean);

            int actualInt32 = int32Accessor.Value;
            Assert.AreEqual(canary.Int32Property, actualInt32);

            string actualString = stringAccessor.Value;
            Assert.AreEqual(canary.StringProperty, actualString);

            booleanAccessor.Value = false;
            Assert.AreEqual(false, canary.BooleanProperty);

            int32Accessor.Value = 1234;
            Assert.AreEqual(1234, canary.Int32Property);

            stringAccessor.Value = "World";
            Assert.AreEqual("World", canary.StringProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void PropertyAccessor_Set_1()
        {
            PropertyAccessor<CanaryClass, int> accessor
                = new PropertyAccessor<CanaryClass, int>(canary, "NoSetterProperty");
            accessor.Value = 321;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PropertyAccessor_Construction_1()
        {
            PropertyAccessor<CanaryClass, bool> wrongAccessor
                = new PropertyAccessor<CanaryClass, bool>
                    (
                        canary,
                        "NonexistentProperty"
                    );
            Assert.AreEqual("NonexistentProperty", wrongAccessor.Name);
        }

        [TestMethod]
        public void PropertyAccessor_Events_1()
        {
            bool booleanGet = false, booleanSet = false;
            booleanAccessor.GettingValue += (target, value) => { booleanGet = true; };
            booleanAccessor.SettingValue += (target, value) => { booleanSet = true; };
            booleanAccessor.Value = !booleanAccessor.Value;
            Assert.IsTrue(booleanGet);
            Assert.IsTrue(booleanSet);

            bool int32Get = false, int32Set = false;
            int32Accessor.GettingValue += (target, value) => { int32Get = true; };
            int32Accessor.SettingValue += (target, value) => { int32Set = true; };
            int32Accessor.Value = int32Accessor.Value * 2;
            Assert.IsTrue(int32Get);
            Assert.IsTrue(int32Set);

            bool stringGet = false, stringSet = false;
            stringAccessor.GettingValue += (target, value) => { stringGet = true; };
            stringAccessor.SettingValue += (target, value) => { stringSet = true; };
            stringAccessor.Value = stringAccessor.Value + "World";
            Assert.IsTrue(stringGet);
            Assert.IsTrue(stringSet);
        }

        [TestMethod]
        public void PropertyAccessor_SetTarget_1()
        {
            CanaryClass newCanary = new CanaryClass
            {
                BooleanProperty = false
            };
            bool flag = false;
            booleanAccessor.TargetChanged += (accessor, target) =>
            {
                flag = true;
            };
            booleanAccessor.SetTarget(newCanary);
            Assert.IsTrue(flag);

            flag = false;
            booleanAccessor.SetTarget(canary);
            Assert.IsTrue(flag);
        }
    }
}
