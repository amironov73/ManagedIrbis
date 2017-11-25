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
    public class GblCorrecTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblCorrec_Construction_1()
        {
            GblCorrec correc = new GblCorrec();
        }

        [TestMethod]
        public void GblCorrec_Execute_1()
        {
            GblContext context = new GblContext();
            GblCorrec correc = new GblCorrec();
            correc.Execute(context);
        }

        [TestMethod]
        public void GblCorrec_Verify_1()
        {
            GblCorrec correc = new GblCorrec();
            Assert.IsTrue(correc.Verify(false));
        }
    }
}
