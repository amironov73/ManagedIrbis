using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftParserTest
    {
        [NotNull]
        private PftProgram _Parse(string text)
        {
            PftLexer lexer = new PftLexer();
            PftTokenList list = lexer.Tokenize(text);
            PftParser parser = new PftParser(list);
            PftProgram result = parser.Parse();

            return result;
        }

        [TestMethod]
        public void PftParser_Construction_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            PftParser parser = new PftParser(list);
            Assert.AreSame(list, parser.Tokens);
        }

        [TestMethod]
        public void PftParser_EmptyProgram_1()
        {
            PftProgram program = _Parse(string.Empty);
            Assert.AreEqual(0, program.Children.Count);
        }
    }
}
