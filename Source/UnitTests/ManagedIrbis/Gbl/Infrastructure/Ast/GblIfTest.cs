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
    public class GblIfTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblIf_Construction_1()
        {
            GblIf gblIf = new GblIf();
        }

        [TestMethod]
        public void GblAdd_Execute_1()
        {
            GblContext context = new GblContext();
            GblIf gblIf = new GblIf();
            gblIf.Execute(context);
        }

        [TestMethod]
        public void GblIf_Verify_1()
        {
            GblIf gblIf = new GblIf();
            Assert.IsTrue(gblIf.Verify(false));
        }
    }
}
