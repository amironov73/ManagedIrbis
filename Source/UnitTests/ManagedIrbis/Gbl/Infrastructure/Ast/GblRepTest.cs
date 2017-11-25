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
    public class GblRepTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblRep_Construction_1()
        {
            GblRep rep = new GblRep();
        }

        [TestMethod]
        public void GblRep_Execute_1()
        {
            GblContext context = new GblContext();
            GblRep rep = new GblRep();
            rep.Execute(context);
        }

        [TestMethod]
        public void GblRep_Verify_1()
        {
            GblRep rep = new GblRep();
            Assert.IsTrue(rep.Verify(false));
        }
    }
}
