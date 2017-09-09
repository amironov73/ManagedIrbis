using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftNodeTest1
    {
        private void _TestAffectedFields
            (
                string text,
                int[] expectedTags
            )
        {
            PftFormatter formatter = new PftFormatter();
            formatter.ParseProgram(text);
            int[] actualTags
                = formatter.Program.GetAffectedFields();

            Assert.AreEqual
                (
                    expectedTags.Length,
                    actualTags.Length
                );
            for (int i = 0; i < expectedTags.Length; i++)
            {
                Assert.AreEqual
                    (
                        expectedTags[i],
                        actualTags[i]
                    );
            }
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_1()
        {
            _TestAffectedFields("", new int[0]);
            _TestAffectedFields(" ", new int[0]);
            _TestAffectedFields("'Hello'", new int[0]);
            _TestAffectedFields("v200^a", new[] { 200 });
            _TestAffectedFields("v200^a, v200^e", new[] { 200 });
            _TestAffectedFields("v200^a, v300", new[] { 200, 300 });
            _TestAffectedFields("if p(v200) then 'OK' fi", new[] { 200 });
            _TestAffectedFields("if p(v200) then v300 fi", new[] { 200, 300 });
            _TestAffectedFields("(if p(v300) then v300 / fi)", new[] { 300 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_2()
        {
            _TestAffectedFields("\"Заглавие\" d200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" d200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_3()
        {
            _TestAffectedFields("\"Заглавие\" n200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" n200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_4()
        {
            _TestAffectedFields("g1", new int[0]);
            _TestAffectedFields("g1, v200", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_5()
        {
            _TestAffectedFields("\"no\"", new int[0]);
            _TestAffectedFields("\"no\", \"yes\"v200^a", new[] { 200 });
        }
    }
}
