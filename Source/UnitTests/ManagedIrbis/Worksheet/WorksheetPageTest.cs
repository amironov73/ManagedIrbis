using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis.Worksheet;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class WorksheetPageTest
    {
        [NotNull]
        private WorksheetPage _GetPage()
        {
            return new WorksheetPage
            {
                Name = "Основное БО",
                Items =
                {
                    new WorksheetItem
                    {
                        Tag = "A",
                        Title = "НАЗВАНИЕ",
                        Repeatable = false,
                        Help = "0",
                        EditMode = "2",
                        InputInfo = ",T=,@!tmovzh"
                    },
                    new WorksheetItem
                    {
                        Tag = "U",
                        Title = "Роль",
                        Repeatable = false,
                        Help = "0",
                        EditMode = "1",
                        InputInfo = "rolzm.mnu"
                    },
                    new WorksheetItem
                    {
                        Tag = "B",
                        Title = "Общее обозначение материала",
                        Repeatable = false,
                        Help = "0",
                        EditMode = "1",
                        InputInfo = "200bm.mnu"
                    },
                    new WorksheetItem
                    {
                        Tag = "E",
                        Title = "Сведения, относящиеся к названию",
                        Repeatable = false,
                        Help = "0",
                        EditMode = "1",
                        InputInfo = "200em.mnu"
                    }
                }
            };
        }

        [TestMethod]
        public void WorksheetPage_Construction_1()
        {
            WorksheetPage page = new WorksheetPage();
            Assert.IsNull(page.Name);
            Assert.IsNotNull(page.Items);
            Assert.AreEqual(0, page.Items.Count);
            Assert.IsNull(page.UserData);
        }

        private void _TestSerialization
            (
                [NotNull] WorksheetPage first
            )
        {
            byte[] bytes = first.SaveToMemory();
            WorksheetPage second = bytes.RestoreObjectFromMemory<WorksheetPage>();
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Items.Count, second.Items.Count);
            Assert.IsNull(second.UserData);
            for (int i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].DefaultValue, second.Items[i].DefaultValue);
                Assert.AreEqual(first.Items[i].FormalVerification, second.Items[i].FormalVerification);
                Assert.AreEqual(first.Items[i].Help, second.Items[i].Help);
                Assert.AreEqual(first.Items[i].Hint, second.Items[i].Hint);
                Assert.AreEqual(first.Items[i].InputInfo, second.Items[i].InputInfo);
                Assert.AreEqual(first.Items[i].EditMode, second.Items[i].EditMode);
                Assert.AreEqual(first.Items[i].Repeatable, second.Items[i].Repeatable);
                Assert.AreEqual(first.Items[i].Reserved, second.Items[i].Reserved);
                Assert.AreEqual(first.Items[i].Tag, second.Items[i].Tag);
                Assert.AreEqual(first.Items[i].Title, second.Items[i].Title);
                Assert.IsNull(second.Items[i].UserData);
            }
        }

        [TestMethod]
        public void WorksheetPage_Serialization_1()
        {
            WorksheetPage page = new WorksheetPage();
            _TestSerialization(page);

            page.UserData = "User data";
            _TestSerialization(page);

            page = _GetPage();
            _TestSerialization(page);
        }

        [TestMethod]
        public void WorksheetPage_Encode_1()
        {
            WorksheetPage page = _GetPage();
            using (StringWriter writer = new StringWriter())
            {
                page.Encode(writer);
                Assert.AreEqual
                (
                    "A\nНАЗВАНИЕ\n0\n0\n2\n,T=,@!tmovzh\n\n\n\n\nU\nРоль\n0\n0\n1\nrolzm.mnu\n\n\n\n\nB\nОбщее обозначение материала\n0\n0\n1\n200bm.mnu\n\n\n\n\nE\nСведения, относящиеся к названию\n0\n0\n1\n200em.mnu\n\n\n\n\n",
                    writer.ToString().DosToUnix()
                );
            }
        }

        [TestMethod]
        public void WorksheetPage_ParseStream_1()
        {
            const string text = "A\nНАЗВАНИЕ\n0\n0\n2\n,T =,@!tmovzh\n\n\n\n\nU\nРоль\n0\n0\n1\nrolzm.mnu\n\n\n\n\nB\nОбщее обозначение материала\n0\n0\n1\n200bm.mnu\n\n\n\n\nE\nСведения, относящиеся к названию\n0\n0\n1\n200em.mnu\n\n\n\n\n";
            using (StringReader reader = new StringReader(text))
            {
                WorksheetPage page = WorksheetPage.ParseStream(reader, "Name", 4);
                Assert.AreEqual("Name", page.Name);
                Assert.AreEqual(4, page.Items.Count);
                Assert.AreEqual("A", page.Items[0].Tag);
                Assert.AreEqual("U", page.Items[1].Tag);
                Assert.AreEqual("B", page.Items[2].Tag);
                Assert.AreEqual("E", page.Items[3].Tag);
            }
        }

        [TestMethod]
        public void WorksheetPage_Verify_1()
        {
            WorksheetPage page = new WorksheetPage();
            Assert.IsFalse(page.Verify(false));

            page = _GetPage();
            Assert.IsTrue(page.Verify(false));
        }

        [TestMethod]
        public void WorksheetPage_ToXml_1()
        {
            WorksheetPage page = new WorksheetPage();
            Assert.AreEqual("<page />", XmlUtility.SerializeShort(page));

            page = _GetPage();
            Assert.AreEqual("<page><name>Основное БО</name><item><tag>A</tag><title>НАЗВАНИЕ</title><help>0</help><input-mode>2</input-mode><input-info>,T=,@!tmovzh</input-info></item><item><tag>U</tag><title>Роль</title><help>0</help><input-mode>1</input-mode><input-info>rolzm.mnu</input-info></item><item><tag>B</tag><title>Общее обозначение материала</title><help>0</help><input-mode>1</input-mode><input-info>200bm.mnu</input-info></item><item><tag>E</tag><title>Сведения, относящиеся к названию</title><help>0</help><input-mode>1</input-mode><input-info>200em.mnu</input-info></item></page>", XmlUtility.SerializeShort(page));
        }

        [TestMethod]
        public void WorksheetPage_ToJson_1()
        {
            WorksheetPage page = new WorksheetPage();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(page));

            page = _GetPage();
            Assert.AreEqual("{'name':'Основное БО','items':[{'tag':'A','title':'НАЗВАНИЕ','help':'0','input-mode':'2','input-info':',T=,@!tmovzh'},{'tag':'U','title':'Роль','help':'0','input-mode':'1','input-info':'rolzm.mnu'},{'tag':'B','title':'Общее обозначение материала','help':'0','input-mode':'1','input-info':'200bm.mnu'},{'tag':'E','title':'Сведения, относящиеся к названию','help':'0','input-mode':'1','input-info':'200em.mnu'}]}", JsonUtility.SerializeShort(page));
        }

        [TestMethod]
        public void WorksheetPage_ToString_1()
        {
            WorksheetPage page = new WorksheetPage();
            Assert.AreEqual("(null)", page.ToString());

            page = _GetPage();
            Assert.AreEqual("Основное БО", page.ToString());
        }
    }
}
