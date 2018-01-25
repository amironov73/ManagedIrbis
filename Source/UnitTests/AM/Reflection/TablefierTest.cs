using System;
using System.IO;

using AM.Reflection;
using AM.Text;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.AM.Reflection
{
    [TestClass]
    public class TablefierTest
    {
        [NotNull]
        private CanaryClass[] _GetData()
        {
            return new []
            {
                new CanaryClass
                {
                    BooleanProperty = false,
                    Int32Property = 1,
                    StringProperty = "Knowledge"
                },
                new CanaryClass
                {
                    BooleanProperty = true,
                    Int32Property = 123,
                    StringProperty = "Is"
                },
                new CanaryClass
                {
                    BooleanProperty = false,
                    Int32Property = 321,
                    StringProperty = "Power"
                },
                new CanaryClass
                {
                    BooleanProperty = true,
                    Int32Property = 5,
                    StringProperty = "Itself"
                },
            };
        }

        [TestMethod]
        public void Tablefier_Print_1()
        {
            CanaryClass[] data = _GetData();

            StringWriter writer = new StringWriter();
            Tablefier tablefier = new Tablefier();
            tablefier.Print(writer, data);
            string actual = writer.ToString().DosToUnix();
            string expected =
                  "Int32Property BooleanProperty StringProperty NoSetterProperty\n"
                + "------------- --------------- -------------- ----------------\n"
                + "            1 False           Knowledge                   123\n"
                + "          123 True            Is                          123\n"
                + "          321 False           Power                       123\n"
                + "            5 True            Itself                      123\n";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tablefier_Print_2()
        {
            CanaryClass[] data = _GetData();

            StringWriter writer = new StringWriter();
            Tablefier tablefier = new Tablefier();
            string[] properties = { "Int32Property", "StringProperty" };
            tablefier.Print(writer, data, properties);
            string actual = writer.ToString().DosToUnix();
            string expected =
                  "Int32Property StringProperty\n"
                + "------------- --------------\n"
                + "            1 Knowledge     \n"
                + "          123 Is            \n"
                + "          321 Power         \n"
                + "            5 Itself        \n";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tablefier_Print_3()
        {
            CanaryClass[] data = _GetData();

            StringWriter writer = new StringWriter();
            Tablefier tablefier = new Tablefier();
            string[] properties = new string[0];
            tablefier.Print(writer, data, properties);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Tablefier_Print_4()
        {
            CanaryClass[] data = _GetData();

            StringWriter writer = new StringWriter();
            Tablefier tablefier = new Tablefier();
            string[] properties = { "Int32Property", "NoSuchProperty" };
            tablefier.Print(writer, data, properties);
        }

        [TestMethod]
        public void Tablefier_Print_5()
        {
            CanaryClass[] data = _GetData();

            Tablefier tablefier = new Tablefier();
            string[] properties = { "Int32Property", "StringProperty" };
            string actual = tablefier.Print(data, properties).DosToUnix();
            string expected =
                "Int32Property StringProperty\n"
                + "------------- --------------\n"
                + "            1 Knowledge     \n"
                + "          123 Is            \n"
                + "          321 Power         \n"
                + "            5 Itself        \n";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Tablefier_Print_6()
        {
            CanaryClass[] data = _GetData();

            Tablefier tablefier = new Tablefier();
            string actual = tablefier.Print(data).DosToUnix();
            string expected =
                "Int32Property BooleanProperty StringProperty NoSetterProperty\n"
                + "------------- --------------- -------------- ----------------\n"
                + "            1 False           Knowledge                   123\n"
                + "          123 True            Is                          123\n"
                + "          321 False           Power                       123\n"
                + "            5 True            Itself                      123\n";
            Assert.AreEqual(expected, actual);
        }
    }
}
