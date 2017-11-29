﻿using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftWhileTest
    {
        private void _Execute
        (
            [NotNull] PftWhile node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftWhile_Construction_1()
        {
            PftWhile node = new PftWhile();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
        }

        [TestMethod]
        public void PftWhile_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.While, 1, 1, "while");
            PftWhile node = new PftWhile(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        // Вечный цикл. Зависает
        //[TestMethod]
        //public void PftWhile_Execute_1()
        //{
        //    PftWhile node = new PftWhile();
        //    _Execute(node, "");
        //}

        [TestMethod]
        public void PftWhile_ToString_1()
        {
            PftWhile node = new PftWhile();
            Assert.AreEqual("while  do end", node.ToString());
        }
    }
}
