using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable UnusedParameter.Local

namespace UnitTests.AM
{
    [TestClass]
    public class ConversionUtilityTest
    {
        class SourceConverter
            : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return true;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                return Activator.CreateInstance(destinationType);
            }
        }

        class TargetConverter
            : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return true;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return new TargetType2();
            }
        }

        [TypeConverter(typeof(SourceConverter))]
        class SourceType
        {
        }

        class SourceType2
        {
            public static explicit operator TargetType2 (SourceType2 arg)
            {
                return new TargetType2();
            }
        }

        class SourceType3
        {
            public static explicit operator SourceType2(SourceType3 arg)
            {
                return new SourceType2();
            }

            public static explicit operator TargetType (SourceType3 arg)
            {
                return new TargetType();
            }
        }

        class TargetType
        {
        }

        [TypeConverter(typeof(TargetConverter))]
        class TargetType2
        {
        }

        [TestMethod]
        public void ConversionUtility_CanConvertTo_1()
        {
            Assert.IsTrue(ConversionUtility.CanConvertTo<bool>(1));
            Assert.IsTrue(ConversionUtility.CanConvertTo<int>("1"));
        }

        [TestMethod]
        public void ConversionUtility_CanConvertTo_2()
        {
            // ReferenceEquals(targetType, sourceType)
            Assert.IsTrue(ConversionUtility.CanConvertTo<bool>(true));
            Assert.IsTrue(ConversionUtility.CanConvertTo<int>(1));
        }

        [TestMethod]
        public void ConversionUtility_CanConvertTo_3()
        {
            // targetType.IsAssignableFrom(sourceType)
            Assert.IsTrue(ConversionUtility.CanConvertTo<Stream>(new MemoryStream()));
        }

        //[TestMethod]
        //public void ConversionUtility_CanConvertTo_4()
        //{
        //    // converterFrom.CanConvertTo(targetType)
        //    Assert.IsTrue(ConversionUtility.CanConvertTo<TargetType>(new SourceType()));
        //}

        //[TestMethod]
        //public void ConversionUtility_CanConvertTo_5()
        //{
        //    // converterTo.CanConvertFrom(sourceType)
        //    Assert.IsTrue(ConversionUtility.CanConvertTo<TargetType2>(new SourceType2()));
        //}

        [TestMethod]
        public void ConversionUtility_CanConvertTo_6()
        {
            Assert.IsFalse(ConversionUtility.CanConvertTo<int>(new SourceType2()));
            Assert.IsFalse(ConversionUtility.CanConvertTo<int>(new TargetType()));
        }

        [TestMethod]
        public void ConversionUtility_ConvertTo_1()
        {
            Assert.AreEqual(true, ConversionUtility.ConvertTo<bool>(1));
            Assert.AreEqual(1, ConversionUtility.ConvertTo<int>("1"));
        }

        [TestMethod]
        public void ConversionUtility_ConvertTo_2()
        {
            // ReferenceEquals(value, null)
            Assert.AreEqual(false, ConversionUtility.ConvertTo<bool>(null));
            Assert.AreEqual(0, ConversionUtility.ConvertTo<int>(null));
        }

        [TestMethod]
        public void ConversionUtility_ConvertTo_3()
        {
            // targetType == typeof(string)
            Assert.AreEqual("False", ConversionUtility.ConvertTo<string>(false));
            Assert.AreEqual("0", ConversionUtility.ConvertTo<string>(0));
        }

        [TestMethod]
        public void ConversionUtility_ConvertTo_4()
        {
            // targetType.IsAssignableFrom(sourceType)
            MemoryStream memoryStream = new MemoryStream();
            Stream stream = memoryStream;
            Assert.AreSame(stream, ConversionUtility.ConvertTo<Stream>(memoryStream));
        }

        //[TestMethod]
        //public void ConversionUtility_ConvertTo_5()
        //{
        //    // converterFrom.CanConvertTo(targetType)
        //    Assert.IsNotNull(ConversionUtility.ConvertTo<TargetType>(new SourceType()));
        //}

        //[TestMethod]
        //public void ConversionUtility_ConvertTo_6()
        //{
        //    // converterTo.CanConvertFrom(sourceType)
        //    Assert.IsNotNull(ConversionUtility.ConvertTo<TargetType2>(new SourceType2()));
        //}

        //[TestMethod]
        //public void ConversionUtility_ConvertTo_7()
        //{
        //    // change type operator
        //    Assert.IsNotNull(ConversionUtility.ConvertTo<TargetType2>(new SourceType3()));
        //}

        [TestMethod]
        [ExpectedException(typeof(ArsMagnaException))]
        public void ConversionUtility_ConvertTo_8()
        {
            ConversionUtility.ConvertTo<TargetType>(new SourceType2());
        }

        [TestMethod]
        public void ConversionUtility_ToBoolean_1()
        {
            Assert.AreEqual(true, ConversionUtility.ToBoolean("true"));
            Assert.AreEqual(true, ConversionUtility.ToBoolean("yes"));
            Assert.AreEqual(false, ConversionUtility.ToBoolean("false"));
            Assert.AreEqual(false, ConversionUtility.ToBoolean("no"));
        }

        [TestMethod]
        public void ConversionUtility_ToBoolean_2()
        {
            // value is bool
            Assert.AreEqual(true, ConversionUtility.ToBoolean(true));
            Assert.AreEqual(false, ConversionUtility.ToBoolean(false));
        }

        [TestMethod]
        public void ConversionUtility_ToBoolean_3()
        {
            // value is IConverttible
            Assert.AreEqual(true, ConversionUtility.ToBoolean(1));
            Assert.AreEqual(false, ConversionUtility.ToBoolean(0));
        }

        //[TestMethod]
        //public void ConversionUtility_ToBoolean_4()
        //{
        //    // TypeConverter
        //    Assert.AreEqual(false, ConversionUtility.ToBoolean(new SourceType()));
        //}

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConversionUtility_ToBoolean_5()
        {
            ConversionUtility.ToBoolean(new SourceType2());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ConversionUtility_ToBoolean_6()
        {
            ConversionUtility.ToBoolean("bullshit");
        }
    }
}
