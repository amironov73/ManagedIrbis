using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;
using ManagedIrbis.Morphology;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class IrbisMorphologyProviderTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void IrbisMorphologyProvider_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IrbisMorphologyProvider morphology
                    = new IrbisMorphologyProvider(provider);
                Assert.AreSame(provider, morphology.Provider);
                Assert.AreEqual(IrbisMorphologyProvider.DefaultDatabase, morphology.Database);
                Assert.AreEqual(IrbisMorphologyProvider.DefaultPrefix, morphology.Prefix);
            }
        }

        [TestMethod]
        public void IrbisMorphologyProvider_FindWord_1()
        {
            const string word = "новость";
            using (IrbisProvider provider = GetProvider())
            {
                IrbisMorphologyProvider morphology
                    = new IrbisMorphologyProvider(provider);
                MorphologyEntry[] entries = morphology.FindWord(word);
                Assert.AreEqual(1, entries.Length);
                Assert.AreEqual(word, entries[0].MainTerm);

                entries = morphology.FindWord("неттакогослова");
                Assert.AreEqual(0, entries.Length);
            }
        }

        [TestMethod]
        public void IrbisMorphologyProvider_RewriteQuery_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                IrbisMorphologyProvider morphology
                    = new IrbisMorphologyProvider(provider);
                string actual = morphology.RewriteQuery("K=новость");
                Assert.AreEqual("( \"K=НОВОСТЬ\" + \"K=НОВОСТИ\" + \"K=НОВОСТЬЮ\" + \"K=НОВОСТЕЙ\" + \"K=НОВОСТЯМ\" + \"K=НОВОСТЯМИ\" + \"K=НОВОСТЯХ\" )", actual);

                actual = morphology.RewriteQuery("K=новость * K=старость");
                Assert.AreEqual("(  ( \"K=НОВОСТЬ\" + \"K=НОВОСТИ\" + \"K=НОВОСТЬЮ\" + \"K=НОВОСТЕЙ\" + \"K=НОВОСТЯМ\" + \"K=НОВОСТЯМИ\" + \"K=НОВОСТЯХ\" )  *  ( \"K=СТАРОСТЬ\" + \"K=СТАРОСТИ\" + \"K=СТАРОСТЬЮ\" )  )", actual);

                actual = morphology.RewriteQuery("K=неттакогослова");
                Assert.AreEqual("\"K=неттакогослова\"", actual);
            }
        }
    }
}
