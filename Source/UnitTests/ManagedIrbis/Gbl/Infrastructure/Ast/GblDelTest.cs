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
    public class GblDelTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblDel_Construction_1()
        {
            GblDel del = new GblDel();
        }

        [TestMethod]
        public void GblDel_Execute_1()
        {
            GblContext context = new GblContext();
            GblDel del = new GblDel();
            del.Execute(context);
        }

        [TestMethod]
        public void GblDel_Verify_1()
        {
            GblDel del = new GblDel();
            Assert.IsTrue(del.Verify(false));
        }
    }
}
