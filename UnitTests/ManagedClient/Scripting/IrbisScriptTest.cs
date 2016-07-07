#if NOTDEF

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
            IrbisConnection client = new IrbisConnection();

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

        [TestMethod]
        public void TestIrbisScript2()
        {
            IrbisConnection client = new IrbisConnection();

            IrbisRecord record = new IrbisRecord();
            RecordField field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            record.Fields.Add(field);

            using (IrbisScript script = new IrbisScript(client))
            {
                script.SetRecord(record);

                DynValue result = script.DoString(@"
return v('v200^a')
");

                Assert.AreEqual("Заглавие", result.String);
            }
        }

        [TestMethod]
        public void TestIrbisScript3()
        {
            IrbisConnection client = new IrbisConnection();

            IrbisRecord record = new IrbisRecord();
            RecordField field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            record.Fields.Add(field);

            using (IrbisScript script = new IrbisScript(client))
            {
                script.SetRecord(record);

                DynValue result = script.DoString(@"
return v('v201^a')
");

                Assert.AreEqual(string.Empty, result.String);
            }
        }
    }
}

#endif
