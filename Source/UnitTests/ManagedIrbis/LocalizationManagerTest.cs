using System;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class LocalizationManagerTest
    {
        private void _TestTranslate
            (
                [NotNull] LocalizationManager locMan,
                [NotNull] string source,
                [NotNull] string expected
            )
        {
            string actual = locMan.Translate(source);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LocalizationManager_Translate_1()
        {
            LocalizationManager locMan
                = new LocalizationManager();

            _TestTranslate(locMan, "", "");
            _TestTranslate(locMan, " ", " ");
            _TestTranslate(locMan, "Привет", "Привет");
            _TestTranslate(locMan, "~Привет~", "~Привет~");
            _TestTranslate(locMan, "~~Привет~", "~~Привет~");
            _TestTranslate(locMan, "~~Привет~~", "Привет");
            _TestTranslate(locMan, "~~Привет~~, ~~мир", "Привет, ~~мир");
            _TestTranslate(locMan, "~~Привет~~, ~~мир~~", "Привет, мир");
        }
    }
}
