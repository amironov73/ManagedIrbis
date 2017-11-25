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
    public class GblNewMfnTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblNewMfn_Construction_1()
        {
            GblNewMfn newMfn = new GblNewMfn();
        }

        [TestMethod]
        public void GblNewMfn_Execute_1()
        {
            GblContext context = new GblContext();
            GblNewMfn newMfn = new GblNewMfn();
            newMfn.Execute(context);
        }

        [TestMethod]
        public void GblNewMfn_Verify_1()
        {
            GblNewMfn newMfn = new GblNewMfn();
            Assert.IsTrue(newMfn.Verify(false));
        }
    }
}
