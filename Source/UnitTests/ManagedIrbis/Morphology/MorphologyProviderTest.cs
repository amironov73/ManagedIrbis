using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Morphology;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class MorphologyProviderTest
    {
        [TestMethod]
        public void MorphologyProvider_Construction_1()
        {
            MorphologyProvider provider = new MorphologyProvider();
            Assert.IsNotNull(provider);
        }

        [TestMethod]
        public void MorphologyProvider_Flatten_1()
        {
            MorphologyProvider provider = new MorphologyProvider();
            MorphologyEntry[] entries = new MorphologyEntry[0];
            string[] flatten = provider.Flatten("новость", entries);
            Assert.AreEqual(1, flatten.Length);
        }

        [TestMethod]
        public void MorphologyProvider_FindWord_1()
        {
            MorphologyProvider provider = new MorphologyProvider();
            MorphologyEntry[] entries = provider.FindWord("новость");
            Assert.AreEqual(0, entries.Length);
        }

        [TestMethod]
        public void MorphologyProvider_RewriteQuery_1()
        {
            const string query = "K=новость";
            MorphologyProvider provider = new MorphologyProvider();
            Assert.AreEqual(query, provider.RewriteQuery(query));
        }
    }
}
