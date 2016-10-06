using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

namespace UnitTests.AM.Text.Output
{
    [TestClass]
    public class TimestampedOutputTest
    {
        [TestMethod]
        public void TestTimestampedOutput()
        {
            TextOutput innerOutput = new TextOutput();
            TimestampedOutput output = new TimestampedOutput(innerOutput);

            output.Write("Hello");

            string actual = innerOutput.ToString();
            Assert.IsNotNull(actual);
        }
    }
}
