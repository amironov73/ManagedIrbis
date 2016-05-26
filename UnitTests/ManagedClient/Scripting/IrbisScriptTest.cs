using System;
using ManagedClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedClient.Scripting;

using MoonSharp.Interpreter;

namespace UnitTests.ManagedClient.Scripting
{
    [TestClass]
    public class IrbisScriptTest
    {
        [TestMethod]
        public void TestIrbisScript1()
        {
            ManagedClient64 client = new ManagedClient64();

            using (IrbisScript script = new IrbisScript(client))
            {
                DynValue result = script.DoString(@"
function fact(n)
   if (n==0) then return 1
   else return n * fact(n-1)
   end
end
 
return fact (5)");

                Assert.AreEqual(120.0, result.Number);
            }
        }
    }
}
