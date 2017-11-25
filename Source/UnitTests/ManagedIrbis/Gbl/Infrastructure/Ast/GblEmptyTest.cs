using System;
using System.Collections.Generic;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Gbl;
using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Gbl.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure.Ast
{
    [TestClass]
    public class GblEmptyTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblEmpty_Construction_1()
        {
            GblEmpty empty = new GblEmpty();
        }

        [TestMethod]
        public void GblEmpty_Execute_1()
        {
            GblContext context = new GblContext();
            GblEmpty empty = new GblEmpty();
            empty.Execute(context);
        }

        [TestMethod]
        public void GblEmpty_Verify_1()
        {
            GblEmpty empty = new GblEmpty();
            Assert.IsTrue(empty.Verify(false));
        }
    }
}
