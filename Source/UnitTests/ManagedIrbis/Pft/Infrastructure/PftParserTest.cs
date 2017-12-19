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
        private PftProgram _Parse
            (
                [NotNull] string text
            )
        {
            PftLexer lexer = new PftLexer();
            PftTokenList list = lexer.Tokenize(text);
            PftParser parser = new PftParser(list);
            PftProgram result = parser.Parse();

            return result;
        }

        private void _Compare
            (
                [NotNull] PftProgram expected,
                [NotNull] PftProgram actual
            )
        {
            PftSerializationUtility.VerifyDeserializedProgram(expected, actual);
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

        [TestMethod]
        public void PftParser_ParseA_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftA
                        {
                            Field = new PftV("v200^a")
                        }
                    }
                }
            };
            PftProgram actual = _Parse("if a(v200^a) then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_ParseA_2()
        {
            _Parse("if a(d200^a) then fi");
        }

        [TestMethod]
        public void PftParser_ParseAbs_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftAbs
                        {
                            Children =
                            {
                                new PftMinus
                                {
                                    Children = { new PftNumericLiteral(123) }
                                }
                            }
                        },
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                }
            };
            PftProgram actual = _Parse("f(abs(-123),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_UnconditionalLiteral_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello")
                }
            };
            PftProgram actual = _Parse("'Hello'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_UnconditionalLiteral_2()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello, "),
                    new PftUnconditionalLiteral("world!"),
                }
            };
            PftProgram actual = _Parse("'Hello, ''world!'");
            _Compare(expected, actual);
        }
    }
}
