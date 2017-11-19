using System;

using AM.Text;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftSyntaxExceptionTest
    {
        [TestMethod]
        public void PftSyntaxException_Construction_1()
        {
            PftSyntaxException exception = new PftSyntaxException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_2()
        {
            PftToken token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            PftSyntaxException exception = new PftSyntaxException(token);
            Assert.AreEqual("Unexpected token: UnconditionalLiteral (1,1): Hello", exception.Message);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_3()
        {
            PftToken[] tokens =
            {
                new PftToken(PftTokenKind.UnconditionalLiteral, 1, 1, "Hello"),
                new PftToken(PftTokenKind.V, 2, 1, "v200"),
                new PftToken(PftTokenKind.Slash, 3, 1, "/")
            };
            PftTokenList tokenList = new PftTokenList(tokens);
            PftSyntaxException exception = new PftSyntaxException(tokenList);
            Assert.AreEqual("Unexpected end of file:UnconditionalLiteral (1,1): Hello V (2,1): v200 Slash (3,1): /", exception.Message);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_4()
        {
            PftToken[] tokens =
            {
                new PftToken(PftTokenKind.UnconditionalLiteral, 1, 1, "Hello"),
                new PftToken(PftTokenKind.V, 2, 1, "v200"),
                new PftToken(PftTokenKind.Slash, 3, 1, "/")
            };
            PftTokenList tokenList = new PftTokenList(tokens);
            Exception innerException = new Exception();
            PftSyntaxException exception = new PftSyntaxException(tokenList, innerException);
            Assert.AreEqual("Unexpected end of file: UnconditionalLiteral (1,1): Hello V (2,1): v200 Slash (3,1): /", exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_5()
        {
            const string message = "Message";
            Exception innerException = new Exception();
            PftSyntaxException exception = new PftSyntaxException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_6()
        {
            PftToken token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            Exception innerException = new Exception();
            PftSyntaxException exception = new PftSyntaxException(token, innerException);
            Assert.AreEqual("Unexpected token: UnconditionalLiteral (1,1): Hello", exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_7()
        {
            TextNavigator navigator = new TextNavigator("Hello\r\nWorld");
            navigator.ReadLine();
            PftSyntaxException exception = new PftSyntaxException(navigator);
            Assert.AreEqual("Syntax error at: Line=2, Column=1", exception.Message);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_8()
        {
            PftToken token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            PftUnconditionalLiteral literal = new PftUnconditionalLiteral(token);
            PftSyntaxException exception = new PftSyntaxException(literal);
            Assert.AreEqual("Syntax error at: 'Hello'", exception.Message);
        }
    }
}
