using AM.Text;

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
    public class PftOrphanTest
    {
        private void _Execute
        (
            [NotNull] PftOrphan node,
            [NotNull] string expected
        )
        {
            PftContext context = new PftContext(null);
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftOrphan_Construction_1()
        {
            PftOrphan node = new PftOrphan();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftOrphan_Execute_1()
        {
            PftOrphan node = new PftOrphan();
            _Execute(node, "");
        }

        //[TestMethod]
        //public void PftOrphan_ToString_1()
        //{
        //    PftOrphan node = new PftOrphan();
        //    Assert.AreEqual("", node.ToString());
        //}
    }
}
