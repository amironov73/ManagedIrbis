using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class ParsedCommandLineTest
    {
        [TestMethod]
        public void TestParsedCommandLine_Construction()
        {
            ParsedCommandLine parsed = new ParsedCommandLine();

            Assert.IsNotNull(parsed.PositionalArguments);
            Assert.IsNotNull(parsed.Switches);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(0, parsed.Switches.Count);
        }

        [TestMethod]
        public void ParsedCommandLine_ToString()
        {
            ParsedCommandLine parsed = new ParsedCommandLine();
            Assert.AreEqual(string.Empty, parsed.ToString());

            parsed.AddSwitch("t");
            Assert.AreEqual
                (
                    "-t",
                    parsed.ToString()
                );

            parsed.AddSwitch("out", "Program.exe");
            Assert.AreEqual
                (
                    "-t -out:Program.exe",
                    parsed.ToString()
                );

            parsed.PositionalArguments.Add("source.cs");
            Assert.AreEqual
                (
                    "-t -out:Program.exe source.cs",
                    parsed.ToString()
                );

            parsed.PositionalArguments.Add("another source.cs");
            Assert.AreEqual
                (
                    "-t -out:Program.exe source.cs \"another source.cs\"",
                    parsed.ToString()
                );

            parsed.PositionalArguments.Clear();
            parsed.AddSwitch("p", "very long path");
            Assert.AreEqual
                (
                    "-t -out:Program.exe \"-p:very long path\"",
                    parsed.ToString()
                );
        }
    }
}
