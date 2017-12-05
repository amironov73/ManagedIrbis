using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ConnectionAliasTest
    {
        [NotNull]
        private ConnectionAlias _GetAlias()
        {
            return new ConnectionAlias
            {
                Name = "Name",
                Value = "Value"
            };
        }

        [TestMethod]
        public void ConnectionAlias_Construction_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            Assert.IsNull(alias.Name);
            Assert.IsNull(alias.Value);
        }

        private void _TestSerialization
            (
                [NotNull] ConnectionAlias first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ConnectionAlias second = bytes.RestoreObjectFromMemory<ConnectionAlias>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void ConnectionAlias_Serialization_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            _TestSerialization(alias);

            alias = _GetAlias();
            _TestSerialization(alias);
        }

        [TestMethod]
        public void ConnectionAlias_Verify_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            Assert.IsFalse(alias.Verify(false));

            alias = _GetAlias();
            Assert.IsTrue(alias.Verify(false));
        }

        [TestMethod]
        public void ConnectionAlias_ToXml_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            Assert.AreEqual("<alias />", XmlUtility.SerializeShort(alias));

            alias = _GetAlias();
            Assert.AreEqual("<alias name=\"Name\" value=\"Value\" />", XmlUtility.SerializeShort(alias));
        }

        [TestMethod]
        public void ConnectionAlias_ToJson_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(alias));

            alias = _GetAlias();
            Assert.AreEqual("{'name':'Name','value':'Value'}", JsonUtility.SerializeShort(alias));
        }

        [TestMethod]
        public void ConnectionAlias_ToString_1()
        {
            ConnectionAlias alias = new ConnectionAlias();
            Assert.AreEqual("(null)=(null)", alias.ToString());

            alias = _GetAlias();
            Assert.AreEqual("Name=Value", alias.ToString());
        }
    }
}
