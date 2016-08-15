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

        [TestMethod]
        public void TestCommandLineSwitch_ToString()
        {
            CommandLineSwitch clSwitch = new CommandLineSwitch
            {
                Name = "test"
            };
            clSwitch
                .AddValue("first")
                .AddValue("second");

            string actual = clSwitch.ToString();
            Assert.AreEqual
                (
                    "-test:first -test:second",
                    actual
                );
        }
    }
}
