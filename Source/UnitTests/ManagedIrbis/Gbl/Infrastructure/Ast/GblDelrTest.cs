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
    public class GblDelrTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblDelr_Construction_1()
        {
            GblDelr delr = new GblDelr();
        }

        [TestMethod]
        public void GblDelr_Execute_1()
        {
            GblContext context = new GblContext();
            GblDelr delr = new GblDelr();
            delr.Execute(context);
        }

        [TestMethod]
        public void GblDelr_Verify_1()
        {
            GblDelr delr = new GblDelr();
            Assert.IsTrue(delr.Verify(false));
        }
    }
}
