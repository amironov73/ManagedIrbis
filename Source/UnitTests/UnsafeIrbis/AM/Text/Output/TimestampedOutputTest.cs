using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using UnsafeAM.Text.Output;

namespace UnitTests.UnsafeAM.Text.Output
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
