using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ManagementInfoTest
    {
        private ManagementInfo _GetInfo()
        {
            ManagementInfo result = new ManagementInfo
            {
                FullTitle = "Директор Иркутской областной государственной универсальной научной библиотеки",
                ShortTitle = "Директор ИОГУНБ",
                DirectorName = "Стасюлевич О. К.",
                Accountant = "Нелюбова Ю. Н."
            };

            return result;
        }

        private RecordField _GetField()
        {
            RecordField result = new RecordField(ManagementInfo.Tag)
                .AddSubField('a', "Директор Иркутской областной государственной универсальной научной библиотеки")
                .AddSubField('d', "Директор ИОГУНБ")
                .AddSubField('b', "Стасюлевич О. К.")
                .AddSubField('c', "Нелюбова Ю. Н.");

            return result;
        }

        [TestMethod]
        public void ManagementInfo_Construction_1()
        {
            ManagementInfo info = new ManagementInfo();
            Assert.IsNull(info.FullTitle);
            Assert.IsNull(info.ShortTitle);
            Assert.IsNull(info.DirectorName);
            Assert.IsNull(info.Accountant);
            Assert.IsNull(info.ContactPerson);
            Assert.IsNull(info.ContactPhone);
            Assert.IsNull(info.DepartmentHead);
            Assert.IsNull(info.Registry1);
            Assert.IsNull(info.Registry2);
            Assert.IsNull(info.Registry3);
            Assert.IsNull(info.UnknownSubFields);
            Assert.IsNull(info.Field);
            Assert.IsNull(info.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] ManagementInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ManagementInfo second = bytes.RestoreObjectFromMemory<ManagementInfo>();
            Assert.AreEqual(first.FullTitle, second.FullTitle);
            Assert.AreEqual(first.ShortTitle, second.ShortTitle);
            Assert.AreEqual(first.DirectorName, second.DirectorName);
            Assert.AreEqual(first.Accountant, second.Accountant);
            Assert.AreEqual(first.ContactPerson, second.ContactPerson);
            Assert.AreEqual(first.ContactPhone, second.ContactPhone);
            Assert.AreEqual(first.DepartmentHead, second.DepartmentHead);
            Assert.AreEqual(first.Registry1, second.Registry1);
            Assert.AreEqual(first.Registry2, second.Registry2);
            Assert.AreEqual(first.Registry3, second.Registry3);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void ManagementInfo_Serialization_1()
        {
            ManagementInfo info = new ManagementInfo();
            _TestSerialization(info);

            info.Field = new RecordField();
            info.UserData = "User data";
            _TestSerialization(info);

            info = _GetInfo();
            _TestSerialization(info);
        }

        [TestMethod]
        public void ManagementInfo_ApplyToField_1()
        {
            RecordField field = new RecordField(ManagementInfo.Tag)
                .AddSubField('a', "?")
                .AddSubField('b', "?");
            ManagementInfo info = _GetInfo();
            info.ApplyToField(field);
            Assert.AreEqual(4, field.SubFields.Count);
            Assert.AreEqual(info.FullTitle, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(info.ShortTitle, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(info.DirectorName, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(info.Accountant, field.GetFirstSubFieldValue('c'));
        }

        [TestMethod]
        public void ManagementInfo_ParseField_1()
        {
            RecordField field = _GetField();
            ManagementInfo info = ManagementInfo.ParseField(field);
            Assert.AreSame(field, info.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), info.FullTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), info.ShortTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), info.DirectorName);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), info.Accountant);
            Assert.IsNull(info.ContactPerson);
            Assert.IsNull(info.ContactPhone);
            Assert.IsNull(info.DepartmentHead);
            Assert.IsNull(info.Registry1);
            Assert.IsNull(info.Registry2);
            Assert.IsNull(info.Registry3);
            Assert.IsNotNull(info.UnknownSubFields);
        }

        [TestMethod]
        public void ManagementInfo_ParseRecord_1()
        {
            MarcRecord record = new MarcRecord();
            RecordField field = _GetField();
            record.Fields.Add(field);
            ManagementInfo[] info = ManagementInfo.ParseRecord(record);
            Assert.AreEqual(1, info.Length);
            Assert.AreSame(field, info[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('a'), info[0].FullTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('d'), info[0].ShortTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('b'), info[0].DirectorName);
            Assert.AreEqual(field.GetFirstSubFieldValue('c'), info[0].Accountant);
            Assert.IsNull(info[0].ContactPerson);
            Assert.IsNull(info[0].ContactPhone);
            Assert.IsNull(info[0].DepartmentHead);
            Assert.IsNull(info[0].Registry1);
            Assert.IsNull(info[0].Registry2);
            Assert.IsNull(info[0].Registry3);
            Assert.IsNotNull(info[0].UnknownSubFields);
        }

        [TestMethod]
        public void ManagementInfo_ToField_1()
        {
            ManagementInfo info = _GetInfo();
            RecordField field = info.ToField();
            Assert.AreEqual(ManagementInfo.Tag, field.Tag);
            Assert.AreEqual(4, field.SubFields.Count);
            Assert.AreEqual(info.FullTitle, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(info.ShortTitle, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(info.DirectorName, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(info.Accountant, field.GetFirstSubFieldValue('c'));
        }

        [TestMethod]
        public void ManagementInfo_Verify_1()
        {
            ManagementInfo info = new ManagementInfo();
            Assert.IsFalse(info.Verify(false));

            info = _GetInfo();
            Assert.IsTrue(info.Verify(false));
        }

        [TestMethod]
        public void ManagementInfo_ToXml_1()
        {
            ManagementInfo info = new ManagementInfo();
            Assert.AreEqual("<management />", XmlUtility.SerializeShort(info));

            info = _GetInfo();
            Assert.AreEqual("<management><fullTitle>Директор Иркутской областной государственной универсальной научной библиотеки</fullTitle><shortTitle>Директор ИОГУНБ</shortTitle><director>Стасюлевич О. К.</director><accountant>Нелюбова Ю. Н.</accountant></management>", XmlUtility.SerializeShort(info));
        }

        [TestMethod]
        public void ManagementInfo_ToJson_1()
        {
            ManagementInfo info = new ManagementInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(info));

            info = _GetInfo();
            Assert.AreEqual("{'fullTitle':'Директор Иркутской областной государственной универсальной научной библиотеки','shortTitle':'Директор ИОГУНБ','director':'Стасюлевич О. К.','accountant':'Нелюбова Ю. Н.'}", JsonUtility.SerializeShort(info));
        }

        [TestMethod]
        public void ManagementInfo_ToString_1()
        {
            ManagementInfo info = new ManagementInfo();
            Assert.AreEqual
                (
                    "Title: (null)\nName: (null)",
                    info.ToString().DosToUnix()
                );

            info = _GetInfo();
            Assert.AreEqual
                (
                    "Title: Директор Иркутской областной государственной универсальной научной библиотеки\nName: Стасюлевич О. К.",
                    info.ToString().DosToUnix()
                );
        }
    }
}
