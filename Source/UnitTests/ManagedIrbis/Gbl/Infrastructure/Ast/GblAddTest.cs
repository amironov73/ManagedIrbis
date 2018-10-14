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
    public class GblAddTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblAdd_Construction_1()
        {
            GblAdd add = new GblAdd();
        }

        [TestMethod]
        public void GblAdd_Execute_1()
        {
            GblContext context = new GblContext()
            {
                CurrentRecord = new MarcRecord()
            };
            GblAdd add = new GblAdd();
            add.Execute(context);
        }

        [TestMethod]
        public void GblAdd_Verify_1()
        {
            GblAdd add = new GblAdd();
            Assert.IsTrue(add.Verify(false));
        }
    }
}
