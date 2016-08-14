using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineSwitchTest
    {
        [TestMethod]
        public void TestCommandLineSwitch_Construction()
        {
            CommandLineSwitch clSwitch = new CommandLineSwitch();

            Assert.IsNotNull(clSwitch.Name);
            Assert.IsNull(clSwitch.Value);
            Assert.IsNotNull(clSwitch.Values);
            Assert.AreEqual(0, clSwitch.Values.Count);
        }
    }
}
