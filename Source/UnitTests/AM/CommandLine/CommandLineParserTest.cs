using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class CommandLineParserTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void CommandLineParser_Parse1()
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

        [TestMethod]
        public void CommandLineParser_Parse2()
        {
            CommandLineParser parser = new CommandLineParser();
            string[] arguments = {"-p", "-q"};
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);

            arguments = new[] {"-p", "-q", "-p"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);

            arguments = new[] {"-p:one", "-q", "-p"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);
            Assert.AreEqual("one", parsed.GetValue("p", null));
        }

        [TestMethod]
        public void CommandLineParser_Parse3()
        {
            CommandLineParser parser = new CommandLineParser();
            string[] arguments = { "\"-p\"", "-q" };
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);

            arguments = new[] { "-p", "-q", "\"-p\"" };
            parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);

            arguments = new[] { "\"-p:one\"", "-q", "-p" };
            parsed = parser.Parse(arguments);
            Assert.AreEqual(0, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);
            Assert.AreEqual("one", parsed.GetValue("p", null));
        }

        [TestMethod]
        public void CommandLineParser_Parse4()
        {
            CommandLineParser parser = new CommandLineParser();
            string[] arguments = { "\"\"", "-q" };
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(1, parsed.PositionalArguments.Count);
            Assert.AreEqual(1, parsed.Switches.Count);

            arguments = new[] {"\"Long argument\"", "-q"};
            parsed = parser.Parse(arguments);
            Assert.AreEqual(1, parsed.PositionalArguments.Count);
            Assert.AreEqual(1, parsed.Switches.Count);
        }

        [TestMethod]
        public void CommandLineParser_Parse_WithResponse1()
        {
            string filePath = Path.Combine
                (
                    TestDataPath,
                    "response.txt"
                );

            CommandLineParser parser = new CommandLineParser();
            string[] arguments = { "@" + filePath, "-q" };
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(2, parsed.PositionalArguments.Count);
            Assert.AreEqual(3, parsed.Switches.Count);
        }

        [TestMethod]
        public void CommandLineParser_Parse_WithResponse2()
        {
            string filePath = Path.Combine
                (
                    TestDataPath,
                    "response.txt"
                );

            CommandLineParser parser = new CommandLineParser();
            string[] arguments = { "\"@" + filePath + "\"", "-q" };
            ParsedCommandLine parsed = parser.Parse(arguments);
            Assert.AreEqual(2, parsed.PositionalArguments.Count);
            Assert.AreEqual(3, parsed.Switches.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception1()
        {
            string[] arguments = {"-", "input"};
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception2()
        {
            string[] arguments = { "-:", "input" };
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception3()
        {
            string[] arguments = { "\"-p", "input" };
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception4()
        {
            string[] arguments = { "-p", "\"input" };
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception5()
        {
            string[] arguments = { "\"", "input" };
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CommandLineParser_Parse_Exception6()
        {
            string[] arguments = { null, "input" };
            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.Parse(arguments);
        }

        [TestMethod]
        public void CommandLineParser_ParseFile()
        {
            string filePath = Path.Combine
                (
                    TestDataPath,
                    "response.txt"
                );

            CommandLineParser parser = new CommandLineParser();
            ParsedCommandLine parsed = parser.ParseFile
                (
                    filePath,
                    Encoding.GetEncoding(0)
                );

            Assert.AreEqual(2, parsed.PositionalArguments.Count);
            Assert.AreEqual(2, parsed.Switches.Count);
        }
    }
}
