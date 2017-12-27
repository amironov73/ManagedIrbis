using System;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Walking
{
    [TestClass]
    public class VisitorContextTest
    {
        [TestMethod]
        public void VisitorContext_Construction_1()
        {
            PftVisitor visitor = new UniversalVisitor();
            PftNode node = new PftNode();
            VisitorContext context = new VisitorContext(visitor, node);
            Assert.AreSame(node, context.Node);
            Assert.IsTrue(context.Result);
        }

        [TestMethod]
        public void VisitorContext_Result_1()
        {
            PftVisitor visitor = new UniversalVisitor();
            PftNode node = new PftNode();
            VisitorContext context = new VisitorContext(visitor, node);
            context.Result = false;
            Assert.IsFalse(context.Result);
        }
    }
}
