using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

namespace UnitTests.AM.Text.Tokenizer
{
    [TestClass]
    public class TokenStreamTest
    {
        private TokenStream _GetStream()
        {
            Token[] result =
            {
                new Token(TokenKind.Word, "Hello", 1, 1), 
                new Token(TokenKind.Word, "Irbis", 2, 1), 
                new Token(TokenKind.Word, "World", 3, 1), 
            };

            return new TokenStream(result);
        }

        [TestMethod]
        public void TestTokenStream_Constructor()
        {
            TokenStream stream = new TokenStream
                (
                    new [] {"Hello", "Irbis", "World"}
                );
            stream.MoveNext();
            stream.MoveNext();
            stream.MoveNext();
            Assert.AreEqual(3, stream.Position);
        }

        [TestMethod]
        public void TestTokenStream_MoveNext()
        {
            TokenStream stream = _GetStream();
            Assert.AreEqual(0, stream.Position);
            Token token = stream.Current;
            Assert.AreEqual("Hello", token.Value);
            Assert.IsTrue(stream.HasNext);
            Assert.IsTrue(stream.MoveNext());
            Assert.AreEqual(1, stream.Position);
            token = stream.Current;
            Assert.AreEqual("Irbis", token.Value);
            Assert.IsTrue(stream.HasNext);
            Assert.IsTrue(stream.MoveNext());
            Assert.AreEqual(2, stream.Position);
            token = stream.Current;
            Assert.AreEqual("World", token.Value);
            Assert.IsFalse(stream.HasNext, "stream.HasNext");
            Assert.IsFalse(stream.MoveNext(), "stream.MoveNext()");
            Assert.AreEqual(3, stream.Position);
            token = stream.Current;
            Assert.IsNull(token);
            Assert.IsFalse(stream.HasNext, "stream.HasNext");
            Assert.IsFalse(stream.MoveNext(), "stream.MoveNext()");
            Assert.AreEqual(3, stream.Position);
            token = stream.Current;
            Assert.IsNull(token);
        }

        [TestMethod]
        public void TestTokenStream_Peek()
        {
            TokenStream stream = _GetStream();
            string text = stream.Peek();
            Assert.AreEqual("Irbis", text);
            text = stream.Peek();
            Assert.AreEqual("Irbis", text);
            stream.MoveNext();
            text = stream.Peek();
            Assert.AreEqual("World", text);
            stream.MoveNext();
            text = stream.Peek();
            Assert.AreEqual(null, text);
        }
    }
}
