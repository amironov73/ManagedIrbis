using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

using AM;
using AM.Reflection;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class PropertyOrFieldTest
    {
        [TestMethod]
        public void PropertyOrField_Construction_1()
        {
            Type type = typeof(CanaryClass);
            MemberInfo member = type.GetProperty("Int32Property");
            Assert.IsNotNull(member);
            PropertyOrField property = new PropertyOrField(member);
            Assert.IsNotNull(property);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PropertyOrField_Construction_2()
        {
            Type type = typeof(CanaryClass);
            MemberInfo member = type.GetMethod("MethodOne");
            Assert.IsNotNull(member);
            PropertyOrField property = new PropertyOrField(member);
            Assert.IsNotNull(property);
        }

        [TestMethod]
        public void PropertyOrField_Get_Set_1()
        {
            CanaryClass canary = new CanaryClass
            {
                Int32Field = 123
            };
            Type type = canary.GetType();
            FieldInfo info = type.GetField("Int32Field");
            Assert.IsNotNull(info);
            PropertyOrField field = new PropertyOrField(info);
            Assert.AreSame(info, field.FieldInfo);
            Assert.IsNull(field.PropertyInfo);
            Assert.AreEqual("Int32Field", field.Name);
            Assert.IsTrue(field.MemberType == typeof(int));
            Assert.IsFalse(field.IsProperty);
            Assert.IsFalse(field.IsIndexed);
            Assert.IsFalse(field.ReadOnly);

            Assert.AreEqual(123, field.GetValue(canary));

            field.SetValue(canary, 321);
            Assert.AreEqual(321, canary.Int32Field);

            Assert.IsFalse(field.HaveAttribute<DisplayNameAttribute>(false));

            Assert.AreEqual("Int32Field", field.ToString());
        }

        [TestMethod]
        public void PropertyOrField_Get_Set_2()
        {
            CanaryClass canary = new CanaryClass
            {
                Int32Property = 123
            };
            Type type = canary.GetType();
            PropertyInfo info = type.GetProperty("Int32Property");
            Assert.IsNotNull(info);
            PropertyOrField property = new PropertyOrField(info);
            Assert.AreSame(info, property.PropertyInfo);
            Assert.IsNull(property.FieldInfo);
            Assert.AreEqual("Int32Property", property.Name);
            Assert.IsTrue(property.MemberType == typeof(int));
            Assert.IsTrue(property.IsProperty);
            Assert.IsFalse(property.IsIndexed);
            Assert.IsFalse(property.ReadOnly);

            Assert.AreEqual(123, property.GetValue(canary));

            property.SetValue(canary, 321);
            Assert.AreEqual(321, canary.Int32Property);

            Assert.IsTrue(property.HaveAttribute<DisplayNameAttribute>(false));
            DisplayNameAttribute attribute = property.GetCustomAttribute<DisplayNameAttribute>(false);
            Assert.IsNotNull(attribute);
            Assert.AreEqual("CanaryProperty", attribute.DisplayName);

            Assert.AreEqual("Int32Property", property.ToString());
        }

        [TestMethod]
        public void PropertyOrField_CompareTo_1()
        {
            Type type = typeof(CanaryClass);
            FieldInfo info = type.GetField("Int32Field");
            Assert.IsNotNull(info);
            PropertyOrField first = new PropertyOrField(info);
            PropertyOrField second = new PropertyOrField(info);
            Assert.IsTrue(((IComparable<PropertyOrField>)first).CompareTo(second) == 0);

            info = type.GetField("BooleanField");
            Assert.IsNotNull(info);
            second = new PropertyOrField(info);
            Assert.IsTrue(((IComparable<PropertyOrField>)first).CompareTo(second) != 0);
        }
    }
}

