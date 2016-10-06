using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineSettingsTest
    {
        [TestMethod]
        public void TestCommandLineSettings_Construction()
        {
            Assert.AreEqual
                (
                    CommandLineSettings.DefaultArgumentDelimiter,
                    CommandLineSettings.ArgumentDelimiter
                );
            Assert.AreEqual
                (
                    CommandLineSettings.DefaultResponsePrefix,
                    CommandLineSettings.ResponsePrefix
                );
            Assert.AreEqual
                (
                    CommandLineSettings.DefaultSwitchPrefix,
                    CommandLineSettings.SwitchPrefix
                );
            Assert.AreEqual
                (
                    CommandLineSettings.DefaultValueSeparator,
                    CommandLineSettings.ValueSeparator
                );
        }
    }
}
