using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public class DoiTest
    {
        [TestMethod]
        public void Doi_Construction_1()
        {
            Doi doi = new Doi();
            Assert.IsNull(doi.Prefix);
            Assert.IsNull(doi.Suffix);
            Assert.IsNull(doi.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] Doi first
            )
        {
            byte[] bytes = first.SaveToMemory();
            Doi second = bytes.RestoreObjectFromMemory<Doi>();
            Assert.AreEqual(first.Prefix, second.Prefix);
            Assert.AreEqual(first.Suffix, second.Suffix);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void Doi_Serialization_1()
        {
            Doi doi = new Doi();
            _TestSerialization(doi);

            doi.UserData = "User data";
            _TestSerialization(doi);

            doi.Prefix = "10.1000";
            doi.Suffix = "182";
            _TestSerialization(doi);
        }

        [TestMethod]
        public void Doi_ToXml_1()
        {
            Doi doi = new Doi();
            Assert.AreEqual("<doi />", XmlUtility.SerializeShort(doi));

            doi.Prefix = "10.1000";
            doi.Suffix = "182";
            Assert.AreEqual("<doi prefix=\"10.1000\" suffix=\"182\" />", XmlUtility.SerializeShort(doi));
        }

        [TestMethod]
        public void Doi_ToJson_1()
        {
            Doi doi = new Doi();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(doi));

            doi.Prefix = "10.1000";
            doi.Suffix = "182";
            Assert.AreEqual("{'prefix':'10.1000','suffix':'182'}", JsonUtility.SerializeShort(doi));
        }

        [TestMethod]
        public void Doi_ToString()
        {
            Doi doi = new Doi();
            Assert.AreEqual("Prefix: (null), Suffix: (null)", doi.ToString());

            doi.Prefix = "10.1000";
            doi.Suffix = "182";
            Assert.AreEqual("Prefix: 10.1000, Suffix: 182", doi.ToString());
        }
    }
}
