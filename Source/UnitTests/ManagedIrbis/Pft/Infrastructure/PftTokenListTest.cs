using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftTokenListTest
    {
        [NotNull]
        private PftTokenList _GetList()
        {
            PftToken[] tokens =
            {
                new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "("),
                new PftToken(PftTokenKind.V, 1, 2, "v200^a"),
                new PftToken(PftTokenKind.Comma, 1, 8, ","),
                new PftToken(PftTokenKind.Slash, 1, 10, "/"),
                new PftToken(PftTokenKind.RightParenthesis, 1, 11, ")"),
            };
            PftTokenList result = new PftTokenList(tokens);

            return result;
        }

        [TestMethod]
        public void PftTokenList_Construction_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            Assert.AreEqual(0, list.Length);
            Assert.IsTrue(list.IsEof);
            Assert.IsFalse(list.MoveNext());
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftTokenList_Construction_1a()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            PftToken current = list.Current;
        }

        [TestMethod]
        public void PftTokenList_Construction_2()
        {
            PftToken[] tokens =
            {
                new PftToken(PftTokenKind.Comma, 1, 1, ",")
            };
            PftTokenList list = new PftTokenList(tokens);
            Assert.AreEqual(1, list.Length);
            Assert.AreEqual(PftTokenKind.Comma, list.Current.Kind);
            Assert.IsFalse(list.IsEof);
            Assert.IsFalse(list.MoveNext());
            Assert.IsTrue(list.IsEof);
        }

        [TestMethod]
        public void PftTokenList_Add_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            list.Add(PftTokenKind.Comma);
            Assert.AreEqual(1, list.Length);
            Assert.AreEqual(PftTokenKind.Comma, list.Current.Kind);
            Assert.IsFalse(list.IsEof);
            Assert.IsFalse(list.MoveNext());
            Assert.IsTrue(list.IsEof);
        }

        [TestMethod]
        public void PftTokenList_CountRemainingTokens_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            Assert.AreEqual(0, list.CountRemainingTokens());
            Assert.IsFalse(list.MoveNext());
            Assert.AreEqual(0, list.CountRemainingTokens());
        }

        [TestMethod]
        public void PftTokenList_CountRemainingTokens_2()
        {
            PftTokenList list = _GetList();
            Assert.AreEqual(4, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(3, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(2, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(1, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(0, list.CountRemainingTokens());
        }

        [TestMethod]
        public void PftTokenList_Dump_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            StringWriter writer = new StringWriter();
            list.Dump(writer);
            Assert.AreEqual("Total tokens: 0\n", writer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftTokenList_Dump_2()
        {
            PftTokenList list = _GetList();
            StringWriter writer = new StringWriter();
            list.Dump(writer);
            Assert.AreEqual("Total tokens: 5\nLeftParenthesis (1,1): (\nV (1,2): v200^a\nComma (1,8): ,\nSlash (1,10): /\nRightParenthesis (1,11): )\n", writer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftTokenList_MoveNext_1()
        {
            PftTokenList list = _GetList();
            Assert.AreEqual(4, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(3, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(2, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(1, list.CountRemainingTokens());
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual(0, list.CountRemainingTokens());
        }

        [TestMethod]
        public void PftTokenList_Peek_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            Assert.AreEqual(PftTokenKind.None, list.Peek());
        }

        [TestMethod]
        public void PftTokenList_Peek_2()
        {
            PftTokenList list = _GetList();
            Assert.AreEqual(PftTokenKind.V, list.Peek());
        }

        [TestMethod]
        public void PftTokenList_Peek_3()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            Assert.AreEqual(PftTokenKind.None, list.Peek(2));
        }

        [TestMethod]
        public void PftTokenList_Peek_4()
        {
            PftTokenList list = _GetList();
            Assert.AreEqual(PftTokenKind.Comma, list.Peek(2));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftTokenList_RequireNext_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            list.RequireNext();
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftTokenList_RequireNext_2()
        {
            PftTokenList list = _GetList();
            list.RequireNext(PftTokenKind.All);
        }

        [TestMethod]
        public void PftTokenList_RequireNext_3()
        {
            PftTokenList first = _GetList();
            PftTokenList second = first.RequireNext();
            Assert.AreSame(first, second);
        }

        [TestMethod]
        public void PftTokenList_RequireNext_4()
        {
            PftTokenList first = _GetList();
            PftTokenList second = first.RequireNext(PftTokenKind.V);
            Assert.AreSame(first, second);
        }

        [TestMethod]
        public void PftTokenList_Reset_1()
        {
            PftTokenList first = _GetList();
            first.MoveNext();
            first.MoveNext();
            PftTokenList second = first.Reset();
            Assert.AreSame(first, second);
        }

        [TestMethod]
        public void PftTokenList_SavePosition_1()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            int position = list.SavePosition();
            Assert.AreEqual(1, position);
        }

        [TestMethod]
        public void PftTokenList_RestorePosition_1()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            int position = list.SavePosition();
            PftTokenKind saved = list.Current.Kind;
            list.MoveNext();
            Assert.AreNotEqual(saved, list.Current.Kind);
            list.RestorePosition(position);
            Assert.AreEqual(saved, list.Current.Kind);
        }

        [TestMethod]
        public void PftTokenList_Segment_1()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftTokenKind[] stop = { PftTokenKind.RightParenthesis };
            PftTokenList segment = list.Segment(stop);
            Assert.IsNotNull(segment);
            Assert.AreEqual(3, segment.Length);
        }

        [TestMethod]
        public void PftTokenList_Segment_2()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftTokenKind[] stop = { PftTokenKind.LeftParenthesis };
            PftTokenList segment = list.Segment(stop);
            Assert.IsNull(segment);
            Assert.AreEqual(PftTokenKind.V, list.Current.Kind);
        }

        [TestMethod]
        public void PftTokenList_Segment_3()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftTokenKind[] open = { PftTokenKind.V };
            PftTokenKind[] close = { PftTokenKind.Comma };
            PftTokenKind[] stop = { PftTokenKind.RightParenthesis };
            PftTokenList segment = list.Segment(open, close, stop);
            Assert.IsNotNull(segment);
            Assert.AreEqual(3, segment.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftTokenList_Segment_4()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftTokenKind[] open = { PftTokenKind.V };
            PftTokenKind[] close = { PftTokenKind.Semicolon };
            PftTokenKind[] stop = { PftTokenKind.RightParenthesis };
            PftTokenList segment = list.Segment(open, close, stop);
            Assert.IsNotNull(segment);
            Assert.AreEqual(3, segment.Length);
        }

        [TestMethod]
        public void PftTokenList_Segment_5()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftTokenKind[] open = { PftTokenKind.V };
            PftTokenKind[] close = { PftTokenKind.Comma };
            PftTokenKind[] stop = { PftTokenKind.LeftParenthesis };
            PftTokenList segment = list.Segment(open, close, stop);
            Assert.IsNull(segment);
        }

        [TestMethod]
        public void PftTokenList_Segment_6()
        {
            PftToken[] tokens =
            {
                new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "("),
                new PftToken(PftTokenKind.V, 1, 2, "v200^a"),
                new PftToken(PftTokenKind.Comma, 1, 8, ","),
                new PftToken(PftTokenKind.Slash, 1, 10, "/"),
                new PftToken(PftTokenKind.RightParenthesis, 1, 11, ")"),
                new PftToken(PftTokenKind.RightParenthesis, 1, 12, ")"),
            };
            PftTokenList list = new PftTokenList(tokens);
            list.MoveNext();
            PftTokenKind[] open = { PftTokenKind.V };
            PftTokenKind[] close = { PftTokenKind.RightParenthesis };
            PftTokenKind[] stop = { PftTokenKind.RightParenthesis };
            PftTokenList segment = list.Segment(open, close, stop);
            Assert.IsNotNull(segment);
        }

        [TestMethod]
        public void PftTokenList_ShowLastTokens_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            string actual = list.ShowLastTokens(3);
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void PftTokenList_ShowLastTokens_2()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            string actual = list.ShowLastTokens(3);
            Assert.AreEqual("LeftParenthesis (1,1): ( V (1,2): v200^a Comma (1,8): , Slash (1,10): / RightParenthesis (1,11): )", actual);
        }

        [TestMethod]
        public void PftTokenList_ToArray_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            list.MoveNext();
            PftToken[] array = list.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void PftTokenList_ToArray_2()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            PftToken[] array = list.ToArray();
            // All the tokens!
            Assert.AreEqual(5, array.Length);
        }

        [TestMethod]
        public void PftTokenList_ToText_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            list.MoveNext();
            string actual = list.ToText();
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void PftTokenList_ToText_2()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            string actual = list.ToText();
            Assert.AreEqual("(v200^a,/)", actual);
        }

        [TestMethod]
        public void PftTokenList_ToString_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            list.MoveNext();
            string actual = list.ToString();
            Assert.AreEqual("(EOF)", actual);
        }

        [TestMethod]
        public void PftTokenList_ToString_2()
        {
            PftTokenList list = _GetList();
            list.MoveNext();
            string actual = list.ToString();
            Assert.AreEqual("1 of 5: V (1,2): v200^a", actual);
        }
    }
}
