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
    public class PftLexerTest
    {
        private void _Single
            (
                [NotNull] string text,
                PftTokenKind kind
            )
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(kind, token.Kind);
            Assert.AreEqual(text, token.Text);
        }

        [TestMethod]
        public void PftLexer_Empty_1()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(string.Empty);
            Assert.IsTrue(tokens.IsEof);
        }

        [TestMethod]
        public void PftLexer_SkipWhitespace_1()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(" \t\r\n\v ");
            Assert.IsTrue(tokens.IsEof);
        }

        [TestMethod]
        public void PftLexer_SkipWhitespace_2()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize("     'Hello'   ");
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.UnconditionalLiteral, token.Kind);
        }

        [TestMethod]
        public void PftLexer_UnconditionalLiteral_1()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize("'Hello'");
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.UnconditionalLiteral, token.Kind);
            Assert.AreEqual("Hello", token.Text);
        }

        [TestMethod]
        public void PftLexer_ConditionalLiteral_1()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize("\"Hello\"");
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.ConditionalLiteral, token.Kind);
            Assert.AreEqual("Hello", token.Text);
        }

        [TestMethod]
        public void PftLexer_RepeatableLiteral_1()
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize("|Hello|");
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.RepeatableLiteral, token.Kind);
            Assert.AreEqual("Hello", token.Text);
        }

        [TestMethod]
        public void PftLexer_Bang_1()
        {
            _Single("!", PftTokenKind.Bang);
        }

        [TestMethod]
        public void PftLexer_NotEqual_1()
        {
            _Single("!=", PftTokenKind.NotEqual2);
            _Single("!==", PftTokenKind.NotEqual2);
            _Single("!~", PftTokenKind.NotEqual2);
            _Single("!~~", PftTokenKind.NotEqual2);
        }

        [TestMethod]
        public void PftLexer_Colon_1()
        {
            _Single(":", PftTokenKind.Colon);
            _Single("::", PftTokenKind.Colon);
        }

        [TestMethod]
        public void PftLexer_Semicolon_1()
        {
            _Single(";", PftTokenKind.Semicolon);
        }

        [TestMethod]
        public void PftLexer_Comma_1()
        {
            _Single(",", PftTokenKind.Comma);
        }

        [TestMethod]
        public void PftLexer_Backslash_1()
        {
            _Single("\\", PftTokenKind.Backslash);
        }

        [TestMethod]
        public void PftLexer_Equals_1()
        {
            _Single("=", PftTokenKind.Equals);
            _Single("==", PftTokenKind.Equals);
        }

        [TestMethod]
        public void PftLexer_Hash_1()
        {
            _Single("#", PftTokenKind.Hash);
        }

        [TestMethod]
        public void PftLexer_Percent_1()
        {
            _Single("%", PftTokenKind.Percent);
        }

        [TestMethod]
        public void PftLexer_LeftCurly_1()
        {
            _Single("{", PftTokenKind.LeftCurly);
        }

        [TestMethod]
        public void PftLexer_LeftCurly_2()
        {
            string text = "{Hello";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(2, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.LeftCurly, token.Kind);
            Assert.AreEqual("{", token.Text);
        }

        [TestMethod]
        public void PftLexer_LeftCurly_3()
        {
            string text = "{{Hello";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(3, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.LeftCurly, token.Kind);
            Assert.AreEqual("{", token.Text);
        }

        [TestMethod]
        public void PftLexer_TripleCuryl_1()
        {
            string text = "{{{Hello}}}";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.TripleCurly, token.Kind);
            Assert.AreEqual("Hello", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_TripleCuryl_2()
        {
            string text = "{{{Hello}}";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_LeftSquare_1()
        {
            _Single("[", PftTokenKind.LeftSquare);
            _Single("[[[", PftTokenKind.EatOpen);
        }

        [TestMethod]
        public void PftLexer_LeftParenthesis_1()
        {
            _Single("(", PftTokenKind.LeftParenthesis);
        }

        [TestMethod]
        public void PftLexer_RightCurly_1()
        {
            _Single("}", PftTokenKind.RightCurly);
        }

        [TestMethod]
        public void PftLexer_RightSquare_1()
        {
            _Single("]", PftTokenKind.RightSquare);
            _Single("]]]", PftTokenKind.EatClose);
        }

        [TestMethod]
        public void PftLexer_RightParenthesis_1()
        {
            _Single(")", PftTokenKind.RightParenthesis);
        }

        [TestMethod]
        public void PftLexer_Plus_1()
        {
            _Single("+", PftTokenKind.Plus);
        }

        [TestMethod]
        public void PftLexer_Minus_1()
        {
            _Single("-", PftTokenKind.Minus);
        }

        [TestMethod]
        public void PftLexer_Star_1()
        {
            _Single("*", PftTokenKind.Star);
        }

        [TestMethod]
        public void PftLexer_Tilda_1()
        {
            _Single("~", PftTokenKind.Tilda);
            _Single("~~", PftTokenKind.Tilda);
        }

        [TestMethod]
        public void PftLexer_Question_1()
        {
            _Single("?", PftTokenKind.Question);
        }

        [TestMethod]
        public void PftLexer_Slash_1()
        {
            _Single("/", PftTokenKind.Slash);
        }

        [TestMethod]
        public void PftLexer_Comment_1()
        {
            string text = "/* Comment\n  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Comment, token.Kind);
            Assert.AreEqual(" Comment", token.Text);
        }

        [TestMethod]
        public void PftLexer_Less_1()
        {
            _Single("<", PftTokenKind.Less);
            _Single("<=", PftTokenKind.LessEqual);
            _Single("<>", PftTokenKind.NotEqual1);
        }

        [TestMethod]
        public void PftLexer_Less_2()
        {
            string text = "<<Hello";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(3, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Less, token.Kind);
            Assert.AreEqual("<", token.Text);
        }

        [TestMethod]
        public void PftLexer_TripleLess_1()
        {
            string text = "<<<Value>>>  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.TripleLess, token.Kind);
            Assert.AreEqual("Value", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_TripleLess_2()
        {
            string text = "<<<Value>>";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_More_1()
        {
            _Single(">", PftTokenKind.More);
            _Single(">=", PftTokenKind.MoreEqual);
        }

        [TestMethod]
        public void PftLexer_Unifor_1()
        {
            string text = "&unifor  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Unifor, token.Kind);
            Assert.AreEqual("unifor", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_Unifor_2()
        {
            string text = "&  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_Variable_1()
        {
            string text = "$min123  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Variable, token.Kind);
            Assert.AreEqual("min123", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_Variable_2()
        {
            string text = "$  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_At_1()
        {
            string text = "@min123  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.At, token.Kind);
            Assert.AreEqual("min123", token.Text);
        }

        [TestMethod]
        public void PftLexer_At_3()
        {
            string text = "\x1Cmin123\x1D  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.At, token.Kind);
            Assert.AreEqual("min123", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_At_4()
        {
            string text = "\x1C\x1D  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_At_5()
        {
            string text = "\x1Cmin123";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_At_6()
        {
            string text = "\u221Fmin123\u2194  ";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.At, token.Kind);
            Assert.AreEqual("min123", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_At_2()
        {
            string text = "@  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_Hat_1()
        {
            _Single("^", PftTokenKind.Hat);
        }

        [TestMethod]
        public void PftLexer_A_1()
        {
            _Single("a", PftTokenKind.A);
            _Single("again", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_C_1()
        {
            string text = "c123";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.C, token.Kind);
            Assert.AreEqual("123", token.Text);
        }

        [TestMethod]
        public void PftLexer_C_2()
        {
            _Single("c", PftTokenKind.Identifier);
            _Single("cat", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_D_1()
        {
            _Single("d", PftTokenKind.Identifier);
            _Single("d300", PftTokenKind.V);
            _Single("d200^a", PftTokenKind.V);
            _Single("data", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_F_1()
        {
            _Single("f", PftTokenKind.F);
            _Single("fact", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_G_1()
        {
            _Single("g", PftTokenKind.Identifier);
            _Single("g300", PftTokenKind.V);
            _Single("g200^a", PftTokenKind.V);
            _Single("geo", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_L_1()
        {
            _Single("l", PftTokenKind.L);
            _Single("leo", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_M_1()
        {
            _Single("m", PftTokenKind.Identifier);
            _Single("my", PftTokenKind.Identifier);
            _Single("mfu", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_Mfn_1()
        {
            string text = "mfn";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Mfn, token.Kind);
            Assert.AreEqual("mfn", token.Text);
        }

        [TestMethod]
        public void PftLexer_Mfn_2()
        {
            string text = "mfn(5)";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.Mfn, token.Kind);
            Assert.AreEqual("mfn(5)", token.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_Mfn_3()
        {
            string text = "mfn(5";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_Mpl_1()
        {
            _Single("mpl", PftTokenKind.Mpl);
            _Single("mhl", PftTokenKind.Mpl);
            _Single("mdl", PftTokenKind.Mpl);
            _Single("mpu", PftTokenKind.Mpl);
            _Single("mhu", PftTokenKind.Mpl);
            _Single("mdu", PftTokenKind.Mpl);
        }

        [TestMethod]
        public void PftLexer_Mpl_2()
        {
            _Single("mp1", PftTokenKind.Identifier);
            _Single("mha", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_N_1()
        {
            _Single("n", PftTokenKind.Identifier);
            _Single("n300", PftTokenKind.V);
            _Single("n200^a", PftTokenKind.V);
            _Single("none", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_P_1()
        {
            _Single("p", PftTokenKind.P);
            _Single("para", PftTokenKind.Identifier);
            _Single("p2", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_S_1()
        {
            _Single("s", PftTokenKind.S);
            _Single("single", PftTokenKind.Identifier);
            _Single("s2", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_V_1()
        {
            _Single("v", PftTokenKind.Identifier);
            _Single("v300", PftTokenKind.V);
            _Single("v200^a", PftTokenKind.V);
            _Single("vote", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_X_1()
        {
            string text = "c123";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.C, token.Kind);
            Assert.AreEqual("123", token.Text);
        }

        [TestMethod]
        public void PftLexer_X_2()
        {
            _Single("c", PftTokenKind.Identifier);
            _Single("cat", PftTokenKind.Identifier);
        }

    }
}
