using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusFTest
    {
        private void _Execute
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            context.Output.Write(input);
            Unifor unifor = new Unifor();
            unifor.Execute(context, null, "+F");
            string actual = context.GetProcessedOutput();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlusF_CleanRtf_1()
        {
            _Execute("", "");
            _Execute("Привет, мир", "Привет, мир");
            _Execute("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fswiss\\fcharset0 Arial;}\r\n{\\f1\\fnil\\fprq1\\fcharset0 Courier New;}{\\f2\\fswiss\\fprq2\\fcharset0 Arial;}}\r\n{\\colortbl ;\\red0\\green128\\blue0;\\red0\\green0\\blue0;}\r\n{\\*\\generator Msftedit 5.41.21.2508;}\r\n\\viewkind4\\uc1\\pard\\f0\\fs20 The \\i Greek \\i0 word for psyche is spelled \\cf1\\f1\\u968?\\u965?\\u967?\\u942?\\cf2\\f2 . The Greek letters are encoded in Unicode.\\par\r\nThese characters are from the extended \\b ASCII \\b0 character set (Windows code page 1252):  \\\'e2\\\'e4\\u1233?\\\'e5\\cf0\\par }", "The Greek word for psyche is spelled ψυχή. The Greek letters are encoded in Unicode.\nThese characters are from the extended ASCII character set (Windows code page 1252):  âäӑå\n");
        }
    }
}
