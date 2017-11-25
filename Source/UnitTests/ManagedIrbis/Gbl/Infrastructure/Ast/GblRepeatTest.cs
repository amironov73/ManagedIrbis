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
    public class GblRepeatTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblRepeat_Construction_1()
        {
            GblRepeat repeat = new GblRepeat();
        }

        [TestMethod]
        public void GblRepeat_Execute_1()
        {
            GblContext context = new GblContext();
            GblRepeat repeat = new GblRepeat();
            repeat.Execute(context);
        }

        [TestMethod]
        public void GblRepeat_Verify_1()
        {
            GblRepeat repeat = new GblRepeat();
            Assert.IsTrue(repeat.Verify(false));
        }
    }
}
