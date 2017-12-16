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

        private void _Single
            (
                [NotNull] string text,
                [NotNull] string expected,
                PftTokenKind kind
            )
        {
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(kind, token.Kind);
            Assert.AreEqual(expected, token.Text);
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
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_UnconditionalLiteral_2()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("'Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_UnconditionalLiteral_3()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("'");
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
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_ConditionalLiteral_2()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("\"Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_ConditionalLiteral_3()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("\"");
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
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_RepeatableLiteral_2()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("|Hello");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_RepeatableLiteral_3()
        {
            PftLexer lexer = new PftLexer();
            lexer.Tokenize("|");
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
        public void PftLexer_At_2()
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
        public void PftLexer_At_3()
        {
            string text = "\x1C\x1D  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_At_4()
        {
            string text = "\x1Cmin123";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        public void PftLexer_At_5()
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
        public void PftLexer_At_6()
        {
            string text = "@  ";
            PftLexer lexer = new PftLexer();
            lexer.Tokenize(text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_At_7()
        {
            string text = "@";
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
            string text = "x123";
            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            Assert.AreEqual(1, tokens.Length);
            PftToken token = tokens.Current;
            Assert.AreEqual(PftTokenKind.X, token.Kind);
            Assert.AreEqual("123", token.Text);
        }

        [TestMethod]
        public void PftLexer_X_2()
        {
            _Single("x", PftTokenKind.Identifier);
            _Single("xerox", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_Digits_1()
        {
            _Single("123", PftTokenKind.Number);
            _Single("123.12", PftTokenKind.Number);
            _Single("123.12e10", PftTokenKind.Number);
            _Single("123.12e-10", PftTokenKind.Number);
            _Single(".12", PftTokenKind.Number);
            _Single(".12e-10", PftTokenKind.Number);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_Digits_2()
        {
            _Single("123E", PftTokenKind.Number);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftLexer_Digits_3()
        {
            _Single(".E", PftTokenKind.Number);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_A()
        {
            _Single("ab", PftTokenKind.Identifier);
            _Single("aba", PftTokenKind.Identifier);
            _Single("abs", PftTokenKind.Abs);
            _Single("absa", PftTokenKind.Identifier);
            _Single("abz", PftTokenKind.Identifier);

            _Single("al", PftTokenKind.Identifier);
            _Single("ala", PftTokenKind.Identifier);
            _Single("all", PftTokenKind.All);
            _Single("alla", PftTokenKind.Identifier);
            _Single("alz", PftTokenKind.Identifier);

            _Single("an", PftTokenKind.Identifier);
            _Single("ana", PftTokenKind.Identifier);
            _Single("and", PftTokenKind.And);
            _Single("anda", PftTokenKind.Identifier);
            _Single("anz", PftTokenKind.Identifier);

            _Single("и", "and", PftTokenKind.And);
            _Single("иго", PftTokenKind.Identifier);

            _Single("any", PftTokenKind.Any);
            _Single("anya", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_B()
        {
            _Single("blan", PftTokenKind.Identifier);
            _Single("blana", PftTokenKind.Identifier);
            _Single("blank", PftTokenKind.Blank);
            _Single("blanka", PftTokenKind.Identifier);
            _Single("blanz", PftTokenKind.Identifier);

            _Single("brea", PftTokenKind.Identifier);
            _Single("breaa", PftTokenKind.Identifier);
            _Single("break", PftTokenKind.Break);
            _Single("breaka", PftTokenKind.Identifier);
            _Single("breaz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_C()
        {
            _Single("cei", PftTokenKind.Identifier);
            _Single("ceia", PftTokenKind.Identifier);
            _Single("ceil", PftTokenKind.Ceil);
            _Single("ceila", PftTokenKind.Identifier);
            _Single("ceiz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_D()
        {
            _Single("di", PftTokenKind.Identifier);
            _Single("dia", PftTokenKind.Identifier);
            _Single("div", PftTokenKind.Div);
            _Single("diva", PftTokenKind.Identifier);
            _Single("diz", PftTokenKind.Identifier);

            _Single("da", PftTokenKind.Identifier);
            _Single("do", PftTokenKind.Do);
            _Single("doa", PftTokenKind.Identifier);
            _Single("dz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_E()
        {
            _Single("els", PftTokenKind.Identifier);
            _Single("elsa", PftTokenKind.Identifier);
            _Single("else", PftTokenKind.Else);
            _Single("elsea", PftTokenKind.Identifier);
            _Single("elsz", PftTokenKind.Identifier);

            _Single("иначе", "else", PftTokenKind.Else);

            _Single("empt", PftTokenKind.Identifier);
            _Single("empta", PftTokenKind.Identifier);
            _Single("empty", PftTokenKind.Empty);
            _Single("emptya", PftTokenKind.Identifier);
            _Single("emptz", PftTokenKind.Identifier);

            _Single("en", PftTokenKind.Identifier);
            _Single("ena", PftTokenKind.Identifier);
            _Single("end", PftTokenKind.End);
            _Single("enda", PftTokenKind.Identifier);
            _Single("endz", PftTokenKind.Identifier);

            _Single("конец", "end", PftTokenKind.End);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_F()
        {
            _Single("fm", PftTokenKind.Identifier);
            _Single("fma", PftTokenKind.Identifier);
            _Single("fmt", PftTokenKind.Fmt);
            _Single("fmta", PftTokenKind.Identifier);
            _Single("fmz", PftTokenKind.Identifier);

            _Single("fals", PftTokenKind.Identifier);
            _Single("falsa", PftTokenKind.Identifier);
            _Single("false", PftTokenKind.False);
            _Single("falsea", PftTokenKind.Identifier);
            _Single("falsz", PftTokenKind.Identifier);

            _Single("ложь", "false", PftTokenKind.False);

            _Single("f", PftTokenKind.F);
            _Single("fa", PftTokenKind.Identifier);
            _Single("fi", PftTokenKind.Fi);
            _Single("fia", PftTokenKind.Identifier);
            _Single("fz", PftTokenKind.Identifier);

            _Single("все", "fi", PftTokenKind.Fi);
            _Single("всё", "fi", PftTokenKind.Fi);
            _Single("илсе", "fi", PftTokenKind.Fi);
            _Single("фи", "fi", PftTokenKind.Fi);

            _Single("firs", PftTokenKind.Identifier);
            _Single("firsa", PftTokenKind.Identifier);
            _Single("first", PftTokenKind.First);
            _Single("firsta", PftTokenKind.Identifier);
            _Single("firsz", PftTokenKind.Identifier);

            _Single("floo", PftTokenKind.Identifier);
            _Single("flooa", PftTokenKind.Identifier);
            _Single("floor", PftTokenKind.Floor);
            _Single("floora", PftTokenKind.Identifier);
            _Single("flooz", PftTokenKind.Identifier);

            _Single("fo", PftTokenKind.Identifier);
            _Single("foa", PftTokenKind.Identifier);
            _Single("for", PftTokenKind.For);
            _Single("fora", PftTokenKind.Identifier);
            _Single("foz", PftTokenKind.Identifier);

            _Single("для", "for", PftTokenKind.For);

            _Single("foreac", PftTokenKind.Identifier);
            _Single("foreaca", PftTokenKind.Identifier);
            _Single("foreach", PftTokenKind.ForEach);
            _Single("foreacha", PftTokenKind.Identifier);
            _Single("foreacz", PftTokenKind.Identifier);

            _Single("fra", PftTokenKind.Identifier);
            _Single("fraa", PftTokenKind.Identifier);
            _Single("frac", PftTokenKind.Frac);
            _Single("fraca", PftTokenKind.Identifier);
            _Single("fraz", PftTokenKind.Identifier);

            _Single("fro", PftTokenKind.Identifier);
            _Single("froa", PftTokenKind.Identifier);
            _Single("from", PftTokenKind.From);
            _Single("froma", PftTokenKind.Identifier);
            _Single("froz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_G()
        {
            _Single("globa", PftTokenKind.Identifier);
            _Single("globaa", PftTokenKind.Identifier);
            _Single("global", PftTokenKind.Global);
            _Single("globala", PftTokenKind.Identifier);
            _Single("globaz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_H()
        {
            _Single("hav", PftTokenKind.Identifier);
            _Single("hava", PftTokenKind.Identifier);
            _Single("have", PftTokenKind.Have);
            _Single("havea", PftTokenKind.Identifier);
            _Single("havz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_I()
        {
            _Single("i", PftTokenKind.Identifier);
            _Single("ia", PftTokenKind.Identifier);
            _Single("if", PftTokenKind.If);
            _Single("ifa", PftTokenKind.Identifier);
            _Single("ifz", PftTokenKind.Identifier);

            _Single("если", "if", PftTokenKind.If);
            
            _Single("ia", PftTokenKind.Identifier);
            _Single("in", PftTokenKind.In);
            _Single("ina", PftTokenKind.Identifier);
            _Single("inz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_L()
        {
            _Single("l", PftTokenKind.L);

            _Single("las", PftTokenKind.Identifier);
            _Single("lasa", PftTokenKind.Identifier);
            _Single("last", PftTokenKind.Last);
            _Single("lasta", PftTokenKind.Identifier);
            _Single("lasz", PftTokenKind.Identifier);

            _Single("loca", PftTokenKind.Identifier);
            _Single("locaa", PftTokenKind.Identifier);
            _Single("local", PftTokenKind.Local);
            _Single("locala", PftTokenKind.Identifier);
            _Single("locaz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_N()
        {
            _Single("n", PftTokenKind.Identifier);
            _Single("na", PftTokenKind.Identifier);
            _Single("nl", PftTokenKind.Nl);
            _Single("nla", PftTokenKind.Identifier);
            _Single("nz", PftTokenKind.Identifier);

            _Single("no", PftTokenKind.Identifier);
            _Single("noa", PftTokenKind.Identifier);
            _Single("not", PftTokenKind.Not);
            _Single("nota", PftTokenKind.Identifier);
            _Single("noz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_O()
        {
            _Single("o", PftTokenKind.Identifier);
            _Single("oa", PftTokenKind.Identifier);
            _Single("or", PftTokenKind.Or);
            _Single("ora", PftTokenKind.Identifier);
            _Single("orz", PftTokenKind.Identifier);

            _Single("или", "or", PftTokenKind.Or);

            _Single("orde", PftTokenKind.Identifier);
            _Single("ordea", PftTokenKind.Identifier);
            _Single("order", PftTokenKind.Order);
            _Single("ordera", PftTokenKind.Identifier);
            _Single("ordez", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_P()
        {
            _Single("paralle", PftTokenKind.Identifier);
            _Single("parallea", PftTokenKind.Identifier);
            _Single("parallel", PftTokenKind.Parallel);
            _Single("parallela", PftTokenKind.Identifier);
            _Single("parallez", PftTokenKind.Identifier);

            _Single("po", PftTokenKind.Identifier);
            _Single("poa", PftTokenKind.Identifier);
            _Single("pow", PftTokenKind.Pow);
            _Single("powa", PftTokenKind.Identifier);
            _Single("poz", PftTokenKind.Identifier);

            _Single("pro", PftTokenKind.Identifier);
            _Single("proa", PftTokenKind.Identifier);
            _Single("proc", PftTokenKind.Proc);
            _Single("proca", PftTokenKind.Identifier);
            _Single("proz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_R()
        {
            _Single("rav", PftTokenKind.Identifier);
            _Single("rava", PftTokenKind.Identifier);
            _Single("ravr", PftTokenKind.Ravr);
            _Single("ravra", PftTokenKind.Identifier);
            _Single("ravz", PftTokenKind.Identifier);

            _Single("re", PftTokenKind.Identifier);
            _Single("rea", PftTokenKind.Identifier);
            _Single("ref", PftTokenKind.Ref);
            _Single("refa", PftTokenKind.Identifier);
            _Single("rez", PftTokenKind.Identifier);

            _Single("rma", PftTokenKind.Identifier);
            _Single("rmaa", PftTokenKind.Identifier);
            _Single("rmax", PftTokenKind.Rmax);
            _Single("rmaxa", PftTokenKind.Identifier);
            _Single("rmaz", PftTokenKind.Identifier);

            _Single("rmi", PftTokenKind.Identifier);
            _Single("rmia", PftTokenKind.Identifier);
            _Single("rmin", PftTokenKind.Rmin);
            _Single("rmina", PftTokenKind.Identifier);
            _Single("rmiz", PftTokenKind.Identifier);

            _Single("roun", PftTokenKind.Identifier);
            _Single("rouna", PftTokenKind.Identifier);
            _Single("round", PftTokenKind.Round);
            _Single("rounda", PftTokenKind.Identifier);
            _Single("rounz", PftTokenKind.Identifier);

            _Single("rsu", PftTokenKind.Identifier);
            _Single("rsua", PftTokenKind.Identifier);
            _Single("rsum", PftTokenKind.Rsum);
            _Single("rsuma", PftTokenKind.Identifier);
            _Single("rsuz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_S()
        {
            _Single("s", PftTokenKind.S);

            _Single("selec", PftTokenKind.Identifier);
            _Single("seleca", PftTokenKind.Identifier);
            _Single("select", PftTokenKind.Select);
            _Single("selecta", PftTokenKind.Identifier);
            _Single("selecz", PftTokenKind.Identifier);

            _Single("sig", PftTokenKind.Identifier);
            _Single("siga", PftTokenKind.Identifier);
            _Single("sign", PftTokenKind.Sign);
            _Single("signa", PftTokenKind.Identifier);
            _Single("sigz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_T()
        {
            _Single("the", PftTokenKind.Identifier);
            _Single("thea", PftTokenKind.Identifier);
            _Single("then", PftTokenKind.Then);
            _Single("thena", PftTokenKind.Identifier);
            _Single("thez", PftTokenKind.Identifier);

            _Single("то", "then", PftTokenKind.Then);
            _Single("того", PftTokenKind.Identifier);
            _Single("тогда", "then", PftTokenKind.Then);

            _Single("tru", PftTokenKind.Identifier);
            _Single("trua", PftTokenKind.Identifier);
            _Single("true", PftTokenKind.True);
            _Single("truea", PftTokenKind.Identifier);
            _Single("truz", PftTokenKind.Identifier);

            _Single("истина", "true", PftTokenKind.True);

            _Single("trun", PftTokenKind.Identifier);
            _Single("truna", PftTokenKind.Identifier);
            _Single("trunc", PftTokenKind.Trunc);
            _Single("trunca", PftTokenKind.Identifier);
            _Single("trunz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_V()
        {
            _Single("va", PftTokenKind.Identifier);
            _Single("vaa", PftTokenKind.Identifier);
            _Single("val", PftTokenKind.Val);
            _Single("vala", PftTokenKind.Identifier);
            _Single("vaz", PftTokenKind.Identifier);
        }

        [TestMethod]
        public void PftLexer_KnownIdentifiers_W()
        {
            _Single("wher", PftTokenKind.Identifier);
            _Single("whera", PftTokenKind.Identifier);
            _Single("where", PftTokenKind.Where);
            _Single("wherea", PftTokenKind.Identifier);
            _Single("wherz", PftTokenKind.Identifier);

            _Single("whil", PftTokenKind.Identifier);
            _Single("whila", PftTokenKind.Identifier);
            _Single("while", PftTokenKind.While);
            _Single("whilea", PftTokenKind.Identifier);
            _Single("whilz", PftTokenKind.Identifier);

            _Single("пока", "while", PftTokenKind.While);

            _Single("wit", PftTokenKind.Identifier);
            _Single("wita", PftTokenKind.Identifier);
            _Single("with", PftTokenKind.With);
            _Single("witha", PftTokenKind.Identifier);
            _Single("witz", PftTokenKind.Identifier);
        }
    }
}
