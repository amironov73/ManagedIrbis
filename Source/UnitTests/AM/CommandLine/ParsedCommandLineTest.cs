using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.CommandLine;

namespace UnitTests.AM.CommandLine
{
    [TestClass]
    public class ParsedCommandLineTest
    {
        [TestMethod]
        public void ParsedCommandLine_Construction()
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

            parsed.Switches.Clear();
            parsed
                .AddSwitch("t", "one")
                .AddSwitch("t", "two");
            Assert.AreEqual
                (
                    "-t:one -t:two",
                    parsed.ToString()
                );

            parsed.AddSwitch("p", "three");
            Assert.AreEqual
                (
                    "-t:one -t:two -p:three",
                    parsed.ToString()
                );
        }

        [TestMethod]
        public void ParsedCommandLine_Merge()
        {
            ParsedCommandLine first = new ParsedCommandLine();
            first
                .AddSwitch("t", "one")
                .AddSwitch("p", "two");

            ParsedCommandLine second = new ParsedCommandLine();
            second
                .AddSwitch("t", "three")
                .AddSwitch("q", "four");

            first.Merge(second);

            Assert.AreEqual
                (
                    "-t:one -t:three -p:two -q:four",
                    first.ToString()
                );
        }

        [TestMethod]
        public void ParsedCommandLine_GetValues()
        {
            ParsedCommandLine parsed = new ParsedCommandLine();
            parsed
                .AddSwitch("t", "one")
                .AddSwitch("p", "two")
                .AddSwitch("t", "three")
                .AddSwitch("q", "four");

            string[] actual = parsed.GetValues("t");
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("one", actual[0]);
            Assert.AreEqual("three", actual[1]);

            actual = parsed.GetValues("p");
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("two", actual[0]);

            actual = parsed.GetValues("noSuchSwitch");
            Assert.AreEqual(0, actual.Length);
        }
    }
}
