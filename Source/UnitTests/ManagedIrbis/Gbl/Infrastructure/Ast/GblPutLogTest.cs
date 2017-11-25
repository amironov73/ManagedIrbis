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
    public class GblPutLogTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblPutLog_Construction_1()
        {
            GblPutLog putLog = new GblPutLog();
        }

        [TestMethod]
        public void GblPutLog_Execute_1()
        {
            GblContext context = new GblContext();
            GblPutLog putLog = new GblPutLog();
            putLog.Execute(context);
        }

        [TestMethod]
        public void GblPutLog_Verify_1()
        {
            GblPutLog putLog = new GblPutLog();
            Assert.IsTrue(putLog.Verify(false));
        }
    }
}
