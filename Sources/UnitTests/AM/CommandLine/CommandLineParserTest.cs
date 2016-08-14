using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineParserTest
    {
        [TestMethod]
        public void TestCommandLineParser_Parse()
        {
            CommandLineParser parser = new CommandLineParser();
            string[] arguments = new string[0];
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.Switches.Count);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);

            arguments = new[] {"input.txt", "output.txt"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.Switches.Count);
            Assert.AreEqual(2, parsed.PositionalArguments.Count);

            arguments = new[] {"-input:one.txt", "-output:two.txt"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(2, parsed.Switches.Count);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.IsTrue(parsed.HaveSwitch("input"));
            Assert.IsTrue(parsed.HaveSwitch("output"));
            Assert.IsFalse(parsed.HaveSwitch("noSuchSwitch"));
            Assert.IsNull(parsed.GetArgument(0,null));
            Assert.AreEqual("one.txt", parsed.GetValue("input", null));
            Assert.AreEqual("two.txt", parsed.GetValue("output", null));

            arguments = new[] {"-input:input.txt", "output.txt"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(1, parsed.Switches.Count);
            Assert.AreEqual(1, parsed.PositionalArguments.Count);
            Assert.IsTrue(parsed.HaveSwitch("input"));
            Assert.IsFalse(parsed.HaveSwitch("output"));
            Assert.IsFalse(parsed.HaveSwitch("noSuchSwitch"));
            Assert.AreEqual("input.txt", parsed.GetValue("input", null));
            Assert.AreEqual("output.txt", parsed.GetArgument(0, null));
        }
    }
}
