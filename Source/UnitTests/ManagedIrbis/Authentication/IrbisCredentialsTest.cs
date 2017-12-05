using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Authentication;

namespace UnitTests.ManagedIrbis.Authentication
{
    [TestClass]
    public class IrbisCredentialsTest
    {
        [NotNull]
        private IrbisCredentials _GetCredentials()
        {
            return new IrbisCredentials
            {
                Hostname = "localhost",
                Username = "theReader",
                Password = "thePassword",
                Resource = "IBIS",
                Role = "Reader"
            };
        }

        [TestMethod]
        public void IrbisCredentials_Construction_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            Assert.IsNull(credentials.Hostname);
            Assert.IsNull(credentials.Username);
            Assert.IsNull(credentials.Password);
            Assert.IsNull(credentials.Resource);
            Assert.IsNull(credentials.Role);
        }

        private void _TestSerialization
            (
                [NotNull] IrbisCredentials first
            )
        {
            byte[] bytes = first.SaveToMemory();
            IrbisCredentials second = bytes.RestoreObjectFromMemory<IrbisCredentials>();
            Assert.AreEqual(first.Hostname, second.Hostname);
            Assert.AreEqual(first.Username, second.Username);
            Assert.AreEqual(first.Password, second.Password);
            Assert.AreEqual(first.Resource, second.Resource);
            Assert.AreEqual(first.Role, second.Role);
        }

        [TestMethod]
        public void IrbisCredentials_Serialization_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            _TestSerialization(credentials);

            credentials = _GetCredentials();
            _TestSerialization(credentials);
        }

        [TestMethod]
        public void IrbisCredentials_Verify_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            Assert.IsFalse(credentials.Verify(false));

            credentials = _GetCredentials();
            Assert.IsTrue(credentials.Verify(false));
        }

        [TestMethod]
        public void IrbisCredentials_ToXml_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            Assert.AreEqual("<credentials />", XmlUtility.SerializeShort(credentials));

            credentials = _GetCredentials();
            Assert.AreEqual("<credentials hostname=\"localhost\" username=\"theReader\" password=\"thePassword\" resource=\"IBIS\" role=\"Reader\" />", XmlUtility.SerializeShort(credentials));
        }

        [TestMethod]
        public void IrbisCredentials_ToJson_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(credentials));

            credentials = _GetCredentials();
            Assert.AreEqual("{'hostname':'localhost','username':'theReader','password':'thePassword','resource':'IBIS','role':'Reader'}", JsonUtility.SerializeShort(credentials));
        }

        [TestMethod]
        public void IrbisCredentials_ToString_1()
        {
            IrbisCredentials credentials = new IrbisCredentials();
            Assert.AreEqual("(null)", credentials.ToString());

            credentials = _GetCredentials();
            Assert.AreEqual("theReader", credentials.ToString());
        }
    }
}
