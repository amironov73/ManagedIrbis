using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforLTest
        : CommonUniforTest
    {
        // [TestMethod]
        public void UniforL_ContinueTerm_1()
        {
            // TODO implement

            Execute("LJAZ=рус", "СКИЙ");
        }
    }
}
