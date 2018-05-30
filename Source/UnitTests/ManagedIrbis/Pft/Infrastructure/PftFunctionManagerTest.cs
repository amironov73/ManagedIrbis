using JetBrains.Annotations;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftFunctionManagerTest
    {
        private void _MyFunction
            (
                [NotNull] PftContext context,
                [NotNull] PftNode node,
                [NotNull] PftNode[] arguments
            )
        {
            context.Index = 100;
        }

        [TestMethod]
        public void PftFunctionManager_RegisterFunction_1()
        {
            const string name = "myFunction";
            PftFunctionManager manager = new PftFunctionManager();
            Assert.IsFalse(manager.HaveFunction(name));
            manager.RegisterFunction(name, _MyFunction);
            Assert.IsTrue(manager.HaveFunction(name));
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftFunctionManager_RegisterFunction_2()
        {
            const string name = "eval";
            PftFunctionManager manager = new PftFunctionManager();
            Assert.IsFalse(manager.HaveFunction(name));
            manager.RegisterFunction(name, _MyFunction);
            Assert.IsTrue(manager.HaveFunction(name));
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftFunctionManager_RegisterFunction_3()
        {
            const string name = "myFunction";
            PftFunctionManager manager = new PftFunctionManager();
            Assert.IsFalse(manager.HaveFunction(name));
            manager.RegisterFunction(name, _MyFunction);
            Assert.IsTrue(manager.HaveFunction(name));
            manager.RegisterFunction(name, _MyFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void PftFunctionManager_ExecuteFunction_1()
        {
            const string name = "myFunction";
            PftContext context = new PftContext(null);
            PftNode node = new PftNode();
            PftNode[] arguments = new PftNode[0];
            PftFunctionManager.ExecuteFunction(name, context, node, arguments);
        }
    }
}
