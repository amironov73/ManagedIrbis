using System;

using AM.PlatformAbstraction;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftContextTest
    {
        [TestMethod]
        public void PftContext_Construction_1()
        {
            PftContext context = new PftContext(null);
            Assert.IsNull(context.Parent);
            Assert.IsNotNull(context.Provider);
            Assert.IsNotNull(context.Output);
            Assert.IsNotNull(context.Driver);
            Assert.IsNotNull(context.Globals);
            Assert.IsNotNull(context.Variables);
            Assert.IsNotNull(context.Procedures);
            Assert.IsNull(context.CurrentField);
            Assert.IsNull(context.CurrentGroup);
            Assert.IsNotNull(context.Record);
            Assert.IsNull(context.AlternativeRecord);
            Assert.IsNull(context.Debugger);
        }

        [TestMethod]
        public void PftContext_Construction_2()
        {
            PftContext parent = new PftContext(null);
            PftContext context = new PftContext(parent);
            Assert.AreSame(parent, context.Parent);
            Assert.IsNotNull(context.Provider);
            Assert.IsNotNull(context.Output);
            Assert.IsNotNull(context.Driver);
            Assert.IsNotNull(context.Globals);
            Assert.IsNotNull(context.Variables);
            Assert.IsNotNull(context.Procedures);
            Assert.IsNull(context.CurrentField);
            Assert.IsNull(context.CurrentGroup);
            Assert.IsNotNull(context.Record);
            Assert.IsNull(context.AlternativeRecord);
            Assert.IsNull(context.Debugger);
        }

        [TestMethod]
        public void PftContext_ClearAll_1()
        {
            string text = "Some text";
            PftContext context = new PftContext(null);
            context.Output.Write(text);
            context.Output.Error.Write(text);
            context.Output.Warning.Write(text);
            context.ClearAll();
            Assert.IsFalse(context.Output.HaveText);
            Assert.IsFalse(context.Output.HaveWarning);
            Assert.IsFalse(context.Output.HaveError);
        }

        [TestMethod]
        public void PftContext_ClearText_1()
        {
            PftContext context = new PftContext(null);
            context.Output.Write("some text");
            context.ClearText();
            Assert.IsFalse(context.Output.HaveText);
        }
    }
}
