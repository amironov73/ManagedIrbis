using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;
using ManagedIrbis.Morphology;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class SynonymEngineTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void SynonymEngine_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                SynonymEngine engine = new SynonymEngine(provider);
                Assert.AreSame(provider, engine.Provider);
                Assert.AreEqual(SynonymEngine.DefaultDatabase, engine.Database.Value);
                Assert.AreEqual(SynonymEngine.DefaultPrefix, engine.Prefix.Value);
            }
        }

        [TestMethod]
        public void SynonymEngine_GetSynonyms_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                SynonymEngine engine = new SynonymEngine(provider);
                string[] synonyms = engine.GetSynonyms("абориген");
                Assert.AreEqual(2, synonyms.Length);

                synonyms = engine.GetSynonyms("неттакогослова");
                Assert.AreEqual(0, synonyms.Length);
            }
        }
    }
}
