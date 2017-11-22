using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Morphology;

using Newtonsoft.Json.Linq;

namespace UnitTests.ManagedIrbis.Morphology
{
    [TestClass]
    public class MorphologyEngineTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void MorphologyEngine_Construction_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                MorphologyEngine engine = new MorphologyEngine(provider);
                Assert.AreSame(provider, engine.Connection);
            }
        }
    }
}
