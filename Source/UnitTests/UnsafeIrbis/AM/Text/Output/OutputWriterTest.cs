using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Text.Output;

namespace UnitTests.UnsafeAM.Text.Output
{
    [TestClass]
    public class OutputWriterTest
    {
        [TestMethod]
        public void TestOutputWriter1()
        {
            const string expected = "Quick brown fox";

            TextOutput output = new TextOutput();
            OutputWriter writer = new OutputWriter(output);
            writer.WriteLine(expected);

            string actual = output.ToString()
                .TrimEnd();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOutputWriter2()
        {
            const int value = 235;
            string expected = value.ToString();

            TextOutput output = new TextOutput();
            OutputWriter writer = new OutputWriter(output);
            
            writer.WriteLine(value);

            string actual = output.ToString()
                .TrimEnd();

            Assert.AreEqual(expected, actual);
        }
    }
}
