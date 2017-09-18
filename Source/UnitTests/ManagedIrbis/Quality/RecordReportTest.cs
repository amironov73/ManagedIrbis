using System;
using AM.Json;
using AM.Runtime;
using AM.Xml;
using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality
{
    [TestClass]
    public class RecordReportTest
    {
        [TestMethod]
        public void RecordReport_Construction_1()
        {
            RecordReport report = new RecordReport();
            Assert.IsNotNull(report.Defects);
            Assert.AreEqual(0, report.Defects.Count);
            Assert.AreEqual(0, report.Mfn);
            Assert.IsNull(report.Description);
            Assert.IsNull(report.Index);
            Assert.AreEqual(0, report.Quality);
            Assert.IsNull(report.UserData);
        }

        private void _TestSerialization
            (
                RecordReport first
            )
        {
            byte[] bytes = first.SaveToMemory();

            RecordReport second = bytes
                .RestoreObjectFromMemory<RecordReport>();

            Assert.AreEqual(first.Description, second.Description);
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
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void RecordReport_Serialization_1()
        {
            RecordReport report = new RecordReport();
            _TestSerialization(report);

            report.UserData = "User data";
            _TestSerialization(report);

            report.Mfn = 1;
            report.Quality = 100;
            report.Description = "Description";
            report.Index = "Index";
            report.Defects.Add(new FieldDefect{Damage = 100, Field = 200, Message = "Сообщение"});
            _TestSerialization(report);
        }

        [TestMethod]
        public void RecordReport_ToXml_1()
        {
            RecordReport report = new RecordReport();
            Assert.AreEqual("<report mfn=\"0\" quality=\"0\" />", XmlUtility.SerializeShort(report));

            report.Mfn = 1;
            report.Quality = 100;
            report.Description = "Description";
            report.Index = "Index";
            report.Defects.Add(new FieldDefect { Damage = 100, Field = 200, Message = "Сообщение" });
            Assert.AreEqual("<report mfn=\"1\" index=\"Index\" description=\"Description\" quality=\"100\"><defect field=\"200\" message=\"Сообщение\" damage=\"100\" /></report>", XmlUtility.SerializeShort(report));
        }

        [TestMethod]
        public void RecordReport_ToJson_1()
        {
            RecordReport report = new RecordReport();
            Assert.AreEqual("{'mfn':0,'quality':0}", JsonUtility.SerializeShort(report));

            report.Mfn = 1;
            report.Quality = 100;
            report.Description = "Description";
            report.Index = "Index";
            report.Defects.Add(new FieldDefect { Damage = 100, Field = 200, Message = "Сообщение" });
            Assert.AreEqual("{'mfn':1,'index':'Index','description':'Description','defects':[{'field':200,'message':'Сообщение','damage':100}],'quality':100}", JsonUtility.SerializeShort(report));
        }

        [TestMethod]
        public void RecordReport_ToString_1()
        {
            RecordReport report = new RecordReport();
            Assert.AreEqual("MFN: 0, Defects: 0, Quality: 0, Description: (null)", report.ToString());

            report.Mfn = 1;
            report.Quality = 100;
            report.Description = "Description";
            report.Index = "Index";
            report.Defects.Add(new FieldDefect { Damage = 100, Field = 200, Message = "Сообщение" });
            Assert.AreEqual("MFN: 1, Defects: 1, Quality: 100, Description: Description", report.ToString());
        }
    }
}
