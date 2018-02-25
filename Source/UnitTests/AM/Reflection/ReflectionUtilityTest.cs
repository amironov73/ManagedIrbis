using System;
using System.ComponentModel;
using System.Reflection;

using AM.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class ReflectionUtilityTest
    {
        [TestMethod]
        public void ReflectionUtility_GetCustomAttribute_1()
        {
            DisplayNameAttribute attribute
                = ReflectionUtility.GetCustomAttribute<DisplayNameAttribute>(typeof(CanaryClass));
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void ReflectionUtility_GetCustomAttribute_2()
        {
            DisplayNameAttribute attribute
                = ReflectionUtility.GetCustomAttribute<DisplayNameAttribute>(typeof(CanaryClass), true);
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void ReflectionUtility_GetCustomAttribute_3()
        {
            FieldInfo fieldInfo = typeof(CanaryClass).GetField("Int32Field");
            DisplayNameAttribute attribute
                = ReflectionUtility.GetCustomAttribute<DisplayNameAttribute>(fieldInfo);
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void ReflectionUtility_GetCustomAttribute_4()
        {
            PropertyInfo propertyInfo = typeof(CanaryClass).GetProperty("Int32Property");
            DisplayNameAttribute attribute
                = ReflectionUtility.GetCustomAttribute<DisplayNameAttribute>(propertyInfo);
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void ReflectionUtility_GetCustomAttribute_5()
        {
            MethodInfo methodInfo = typeof(CanaryClass).GetMethod("MethodOne");
            DisplayNameAttribute attribute
                = ReflectionUtility.GetCustomAttribute<DisplayNameAttribute>(methodInfo);
            Assert.IsNull(attribute);
        }

        [TestMethod]
        public void ReflectionUtility_GetFieldValue_1()
        {
            CanaryClass canary = new CanaryClass
            {
                StringField = "Text"
            };
            string actual = (string) ReflectionUtility.GetFieldValue(canary, "StringField");
            Assert.AreEqual(canary.StringField, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_GetFieldValue_2()
        {
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.GetFieldValue(canary, "NoSuchField");
        }

        [TestMethod]
        public void ReflectionUtility_GetPropertyValue_1()
        {
            CanaryClass canary = new CanaryClass
            {
                StringProperty = "Text"
            };
            string actual = (string) ReflectionUtility.GetPropertyValue(canary, "StringProperty");
            Assert.AreEqual(canary.StringProperty, actual);
        }

        [TestMethod]
        public void ReflectionUtility_GetPropertyValue_2()
        {
            CanaryClass canary = new CanaryClass
            {
                BooleanProperty = true
            };
            bool actual = (bool) ReflectionUtility.GetPropertyValue(canary, "BooleanProperty");
            Assert.AreEqual(canary.BooleanProperty, actual);
        }

        [TestMethod]
        public void ReflectionUtility_GetPropertyValue_3()
        {
            CanaryClass canary = new CanaryClass();
            int actual = (int) ReflectionUtility.GetPropertyValue(canary, "NoSetterProperty");
            Assert.AreEqual(canary.NoSetterProperty, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_GetPropertyValue_4()
        {
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.GetPropertyValue(canary, "NoSuchProperty");
        }

        [TestMethod]
        public void ReflectionUtility_SetFieldValue_1()
        {
            string expected = "Text";
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetFieldValue(canary, "StringField", expected);
            Assert.AreEqual(expected, canary.StringField);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_SetFieldValue_2()
        {
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetFieldValue(canary, "NoSuchField", "Text");
        }

        [TestMethod]
        public void ReflectionUtility_SetPropertyValue_1()
        {
            string expected = "Text";
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetPropertyValue(canary, "StringProperty", expected);
            Assert.AreEqual(expected, canary.StringProperty);
        }

        [TestMethod]
        public void ReflectionUtility_SetPropertyValue_2()
        {
            int expected = 12345;
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetPropertyValue(canary, "Int32Property", expected);
            Assert.AreEqual(expected, canary.Int32Property);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_SetPropertyValue_3()
        {
            int expected = 12345;
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetPropertyValue(canary, "NoSetterProperty", expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ReflectionUtility_SetPropertyValue_4()
        {
            int expected = 12345;
            CanaryClass canary = new CanaryClass();
            ReflectionUtility.SetPropertyValue(canary, "NoSuchProperty", expected);
        }

        [TestMethod]
        public void ReflectionUtility_GetPropertiesAndFields_1()
        {
            PropertyOrField[] all = ReflectionUtility.GetPropertiesAndFields
                (
                    typeof(CanaryClass),
                    BindingFlags.Instance | BindingFlags.Public
                );
            Assert.AreEqual(7, all.Length);
        }

        [TestMethod]
        public void ReflectionUtility_HasAttribute_1()
        {
            Assert.IsFalse(ReflectionUtility.HasAttribute<DisplayNameAttribute>
                (
                    typeof(CanaryClass).GetMethod("MethodOne")
                ));
        }

        [TestMethod]
        public void ReflectionUtility_HasAttribute_2()
        {
            Assert.IsFalse(ReflectionUtility.HasAttribute<DisplayNameAttribute>
                (
                    typeof(CanaryClass).GetProperty("StringProperty")
                ));
            Assert.IsTrue(ReflectionUtility.HasAttribute<DisplayNameAttribute>
                (
                    typeof(CanaryClass).GetProperty("Int32Property")
                ));
        }

        [TestMethod]
        public void ReflectionUtility_HasAttribute_3()
        {
            Assert.IsFalse(ReflectionUtility.HasAttribute<DisplayNameAttribute>
                (
                    typeof(CanaryClass),
                    true
                ));
        }
    }
}
