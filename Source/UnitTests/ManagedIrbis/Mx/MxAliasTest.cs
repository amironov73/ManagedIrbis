using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable MustUseReturnValue
// ReSharper disable EqualExpressionComparison
// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Mx
{
    [TestClass]
    public class MxAliasTest
    {
        [NotNull]
        private MxAlias _GetAlias()
        {
            return new MxAlias("name", "value");
        }

        [TestMethod]
        public void MxAlias_Construction_1()
        {
            MxAlias alias = new MxAlias();
            Assert.IsNull(alias.Name);
            Assert.IsNull(alias.Value);
        }

        [TestMethod]
        public void MxAlias_Construction_2()
        {
            string name = "name", value = "value";
            MxAlias alias = new MxAlias(name, value);
            Assert.AreSame(name, alias.Name);
            Assert.AreSame(value, alias.Value);
        }

        [TestMethod]
        public void MxAlias_Parse_1()
        {
            MxAlias alias = MxAlias.Parse("name=value");
            Assert.AreEqual("name", alias.Name);
            Assert.AreEqual("value", alias.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MxAlias_Parse_2()
        {
            MxAlias.Parse("=value");
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MxAlias_Parse_3()
        {
            MxAlias.Parse(" =value");
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MxAlias_Parse_4()
        {
            MxAlias.Parse("name value");
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MxAlias_Parse_5()
        {
            MxAlias.Parse("name=");
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void MxAlias_Parse_6()
        {
            MxAlias.Parse("name= ");
        }

        private void _TestSerialization
            (
                [NotNull] MxAlias first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MxAlias second = bytes.RestoreObjectFromMemory<MxAlias>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void MxAlias_Serialization_1()
        {
            MxAlias alias = new MxAlias();
            _TestSerialization(alias);

            alias = _GetAlias();
            _TestSerialization(alias);
        }

        [TestMethod]
        public void MxAlias_Equals_1()
        {
            MxAlias first = new MxAlias("name1", "value1");
            MxAlias second = new MxAlias("name1", "value1");
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second = new MxAlias("name1", "value2");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = new MxAlias("name2", "value2");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void MxAlias_Equals_2()
        {
            object first = new MxAlias("name1", "value1");
            object second = new MxAlias("name1", "value1");
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
            Assert.IsTrue(first.Equals(first));
            Assert.IsTrue(second.Equals(second));

            second = new MxAlias("name2", "value2");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = "second";
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void MxAlias_ToXml_1()
        {
            MxAlias alias = new MxAlias();
            Assert.AreEqual("<alias />", XmlUtility.SerializeShort(alias));

            alias = _GetAlias();
            Assert.AreEqual("<alias name=\"name\" value=\"value\" />", XmlUtility.SerializeShort(alias));
        }

        [TestMethod]
        public void MxAlias_ToJson_1()
        {
            MxAlias alias = new MxAlias();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(alias));

            alias = _GetAlias();
            Assert.AreEqual("{'name':'name','value':'value'}", JsonUtility.SerializeShort(alias));
        }

        [TestMethod]
        public void MxAlias_Verify_1()
        {
            MxAlias alias = new MxAlias();
            Assert.IsFalse(alias.Verify(false));

            alias = _GetAlias();
            Assert.IsTrue(alias.Verify(false));
        }

        [TestMethod]
        public void MxAlias_ToString_1()
        {
            MxAlias alias = new MxAlias();
            Assert.AreEqual("(null)=(null)", alias.ToString());

            alias = _GetAlias();
            Assert.AreEqual("name=value", alias.ToString());
        }
    }
}
