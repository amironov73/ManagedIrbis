using System;
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
            using (IrbisScript script = new IrbisScript())
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
