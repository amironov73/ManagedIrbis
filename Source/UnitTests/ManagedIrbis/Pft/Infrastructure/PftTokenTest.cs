using System;
using System.Collections;
using System.Collections.Generic;
using AM.Runtime;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftTokenTest
    {
        [NotNull]
        private PftToken _GetToken()
        {
            return new PftToken(PftTokenKind.Abs, 1, 2, "abs");
        }

        [TestMethod]
        public void PftToken_Construction_1()
        {
            PftToken token = new PftToken();
            Assert.AreEqual(0, token.Column);
            Assert.AreEqual(0, token.Line);
            Assert.AreEqual((PftTokenKind)0, token.Kind);
            Assert.IsNull(token.Text);
            Assert.IsNull(token.UserData);
        }

        [TestMethod]
        public void PftToken_Construction_2()
        {
            PftTokenKind kind = PftTokenKind.Abs;
            int column = 1, line = 2;
            string text = "abs";
            PftToken token = new PftToken(kind, line, column, text);
            Assert.AreEqual(kind, token.Kind);
            Assert.AreEqual(column, token.Column);
            Assert.AreEqual(line, token.Line);
            Assert.AreSame(text, token.Text);
        }

        private void _TestClone
            (
                [NotNull] PftToken first
            )
        {
            PftToken second = (PftToken) first.Clone();
            Assert.AreEqual(first.Kind, second.Kind);
            Assert.AreEqual(first.Column, second.Column);
            Assert.AreEqual(first.Line, second.Line);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void PftToken_Clone_1()
        {
            PftToken token = new PftToken();
            _TestClone(token);

            token = _GetToken();
            _TestClone(token);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftToken_MustBe_1()
        {
            PftToken token = _GetToken();
            token.MustBe(PftTokenKind.All);
        }

        private void _TestSerialization
            (
                [NotNull] PftToken first
            )
        {
            byte[] bytes = first.SaveToMemory();
            PftToken second = bytes.RestoreObjectFromMemory<PftToken>();
            Assert.AreEqual(first.Kind, second.Kind);
            Assert.AreEqual(first.Column, second.Column);
            Assert.AreEqual(first.Line, second.Line);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void PftToken_Serialization_1()
        {
            PftToken token = new PftToken();
            _TestSerialization(token);

            token = _GetToken();
            _TestSerialization(token);
        }

        [TestMethod]
        public void PftToken_ToString_1()
        {
            PftToken token = new PftToken();
            Assert.AreEqual("None (0,0): (null)", token.ToString());

            token = _GetToken();
            Assert.AreEqual("Abs (1,2): abs", token.ToString());
        }
    }
}
