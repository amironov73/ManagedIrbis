using System.Linq;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class AbstractIniSectionTest
    {
        class MyIniSection: AbstractIniSection
        {
            public const string SectionName = "MySection";

            public MyIniSection()
                : base(SectionName)
            {
            }
        }

        [TestMethod]
        public void AbstractIniSection_Construction_1()
        {
            MyIniSection section = new MyIniSection();
            Assert.AreEqual(MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual(0, section.Section.Count);
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_Clear_1()
        {
            MyIniSection section = new MyIniSection();
            Assert.AreEqual(0, section.Section.Count);
            section.Section.Add("key", "value");
            Assert.AreEqual(1, section.Section.Count);
            section.Clear();
            Assert.AreEqual(0, section.Section.Count);
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_Boolean_1()
        {
            string name = "Name";
            MyIniSection section = new MyIniSection();
            Assert.IsFalse(section.GetBoolean(name, "0"));
            section.SetBoolean(name, true);
            Assert.IsTrue(section.GetBoolean(name, "0"));
            section.SetBoolean(name, false);
            Assert.IsFalse(section.GetBoolean(name, "0"));
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_ToString_1()
        {
            MyIniSection section = new MyIniSection();
            section.Section.Add("name1", "value1");
            section.Section.Add("name2", "value2");
            Assert.AreEqual("[MySection]\nname1=value1\nname2=value2\n", section.ToString().DosToUnix());
        }
    }
}
