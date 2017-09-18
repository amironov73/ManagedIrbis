using System;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Xml;
using ManagedIrbis;
using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality
{
    [TestClass]
    public class RuleReportTest
    {
        [TestMethod]
        public void RuleReport_Construction_1()
        {
            RuleReport report = new RuleReport();
            Assert.IsNotNull(report.Defects);
            Assert.AreEqual(0, report.Defects.Count);
            Assert.AreEqual(0, report.Damage);
            Assert.AreEqual(0, report.Bonus);
        }

        private void _TestSerialization
            (
                RuleReport first
            )
        {
            byte[] bytes = first.SaveToMemory();
            RuleReport second = bytes.RestoreObjectFromMemory<RuleReport>();
            Assert.AreEqual(first.Defects.Count, second.Defects.Count);
            for (int i = 0; i < first.Defects.Count; i++)
            {
                Assert.AreEqual(first.Defects[i].Damage, second.Defects[i].Damage);
                Assert.AreEqual(first.Defects[i].Field, second.Defects[i].Field);
                Assert.AreEqual(first.Defects[i].Repeat, second.Defects[i].Repeat);
                Assert.AreEqual(first.Defects[i].Message, second.Defects[i].Message);
                Assert.AreEqual(first.Defects[i].Subfield, second.Defects[i].Subfield);
                Assert.AreEqual(first.Defects[i].Value, second.Defects[i].Value);
            }
            Assert.AreEqual(first.Damage, second.Damage);
            Assert.AreEqual(first.Bonus, second.Bonus);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void RuleReport_Serialization_1()
        {
            RuleReport report = new RuleReport();
            _TestSerialization(report);

            report.UserData = "User data";
            _TestSerialization(report);

            report.Defects.Add(new FieldDefect{Damage = 10, Field = 100, Message = "Сообщение"});
            report.Damage = 10;
            _TestSerialization(report);
        }

        [TestMethod]
        public void RuleReport_Verify_1()
        {
            RuleReport report = new RuleReport();
            Assert.IsTrue(report.Verify(false));

            report.Damage = -10;
            Assert.IsFalse(report.Verify(false));

            report.Damage = 10;
            report.Defects.Add(new FieldDefect{Damage = 10, Field = 100, Message = "Сообщение"});
            Assert.IsTrue(report.Verify(false));
        }

        [TestMethod]
        public void RuleReport_ToXml_1()
        {
            RuleReport report = new RuleReport();
            Assert.AreEqual("<report />", XmlUtility.SerializeShort(report));

            report.Damage = 100;
            report.Bonus = 10;
            report.Defects.Add(new FieldDefect{Damage = 100, Field = 200, Message = "Сообщение"});
            Assert.AreEqual("<report damage=\"100\" bonus=\"10\"><defect field=\"200\" message=\"Сообщение\" damage=\"100\" /></report>", XmlUtility.SerializeShort(report));
        }

        [TestMethod]
        public void RuleReport_ToJson_1()
        {
            RuleReport report = new RuleReport();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(report));

            report.Damage = 100;
            report.Bonus = 10;
            report.Defects.Add(new FieldDefect { Damage = 100, Field = 200, Message = "Сообщение" });
            Assert.AreEqual("{'defects':[{'field':200,'message':'Сообщение','damage':100}],'damage':100,'bonus':10}", JsonUtility.SerializeShort(report));
        }

        [TestMethod]
        public void RuleReport_ToString_1()
        {
            RuleReport report = new RuleReport();
            Assert.AreEqual("Damage=0, Bonus=0, Defects=0", report.ToString());

            report.Damage = 100;
            report.Bonus = 5;
            report.Defects.Add(new FieldDefect());
            Assert.AreEqual("Damage=100, Bonus=5, Defects=1", report.ToString());
        }
    }
}
