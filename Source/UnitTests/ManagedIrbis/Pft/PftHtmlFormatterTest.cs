using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftHtmlFormatterTest
    {
        [TestMethod]
        public void PftHtmlFormatter_Construction_1()
        {
            PftHtmlFormatter formatter = new PftHtmlFormatter();
            Assert.IsNotNull(formatter.Separator);
            Assert.AreEqual("<%", formatter.Separator.Open);
            Assert.AreEqual("%>", formatter.Separator.Close);
        }

        [TestMethod]
        public void PftHtmlFormatter_Construction_2()
        {
            PftContext context = new PftContext(null);
            PftHtmlFormatter formatter = new PftHtmlFormatter(context);
            Assert.IsNotNull(formatter.Separator);
            Assert.AreEqual("<%", formatter.Separator.Open);
            Assert.AreEqual("%>", formatter.Separator.Close);
            Assert.AreSame(context, formatter.Context);
        }

        [TestMethod]
        public void PftHtmlFormatter_ParseProgram_1()
        {
            PftHtmlFormatter formatter = new PftHtmlFormatter();
            string source = "<body><p> <% 'Hello, world!' %></p></body> ";
            formatter.ParseProgram(source);
            PftProgram actual = formatter.Program;
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftVerbatim("<body><p> "),
                    new PftUnconditionalLiteral("Hello, world!"),
                    new PftVerbatim("</p></body> "),
                }
            };
            PftSerializationUtility.VerifyDeserializedProgram(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftHtmlFormatter_ParseProgram_2()
        {
            PftHtmlFormatter formatter = new PftHtmlFormatter();
            string source = "<body><p> <% 'Hello, world!' ";
            formatter.ParseProgram(source);
        }
    }
}
