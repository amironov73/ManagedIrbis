using System;
using System.IO;

using AM;
using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace UnitTests.AM
{
    [TestClass]
    public class ObjectDumperTest
    {
        class MyClass1
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        class MyClass2
        {
            public MyClass1 First { get; set; }
            public MyClass1 Second { get; set; }
        }

        private void _Write
            (
                object obj,
                string expected
            )
        {
            StringWriter writer = new StringWriter();
            ObjectDumper.Write(obj, 0, writer);
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ObjectDumper_Write_1()
        {
            _Write(null, "(null)\n");
            _Write("Hello", "Hello\n");
            _Write(1, "1\n");
            _Write(-1, "-1\n");
            _Write(1.23, "1.23\n");
        }

        [TestMethod]
        public void ObjectDumper_Write_2()
        {
            MyClass1 obj = new MyClass1();
            _Write(obj, "{X=0    Y=0}\n");
            obj = new MyClass1 { X = 1, Y = 2 };
            _Write(obj, "{X=1    Y=2}\n");
        }

        [TestMethod]
        public void ObjectDumper_Write_3()
        {
            int[] array = { 1, 2, 3 };
            _Write(array, "[0] 1\n[1] 2\n[2] 3\n");
        }

        [TestMethod]
        public void ObjectDumper_Write_4()
        {
            MyClass1[] array =
            {
                new MyClass1 {X = 1, Y = 2},
                new MyClass1 {X = 3, Y = 4}
            };
            _Write(array, "[0] {X=1        Y=2}\n[1] {X=3        Y=4}\n");
        }

        [TestMethod]
        public void ObjectDumper_Write_5()
        {
            MyClass2 obj = new MyClass2
            {
                First = new MyClass1
                {
                    X = 1,
                    Y = 2
                },
                Second = new MyClass1
                {
                    X = 3,
                    Y = 4
                }
            };
            _Write(obj, "{First={X=1     Y=2}\n        Second={X=3     Y=4}\n}\n");
        }
    }
}
