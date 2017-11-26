using System;
using System.IO;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Extensibility;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Extensibility
{
    [TestClass]
    public class ExtensionInfoTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private ExtensionInfo _GetExtension()
        {
            return new ExtensionInfo
            {
                Index = 1,
                DllName = "RecordChecker.dll",
                FunctionName = "CheckTheRecord",
                GroupNumber = 1,
                IconName = "CHECK",
                Name = "Проверка качества библиографической записи",
                PftName = "RecordChecker.pft"
            };
        }

        [TestMethod]
        [Description("Состояние объекта сразу после создания")]
        public void ExtensionInfo_Construction_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            Assert.AreEqual(0, extension.Index);
            Assert.IsNull(extension.DllName);
            Assert.IsNull(extension.FunctionName);
            Assert.IsNull(extension.PftName);
            Assert.AreEqual(1, extension.GroupNumber);
            Assert.IsNull(extension.Name);
            Assert.IsNull(extension.IconName);
            Assert.IsNull(extension.UserData);
        }

        [TestMethod]
        [Description("Чтение из INI-файла")]
        public void ExtensionInfo_FromIniFile_1()
        {
            string fileName = Path.Combine
                (
                    Irbis64RootPath,
                    "irbisc.ini"
                );
            using (IniFile iniFile
                = new IniFile(fileName, IrbisEncoding.Ansi, false))
            {
                ExtensionInfo[] extensions = ExtensionInfo.FromIniFile(iniFile);
                Assert.AreEqual(2, extensions.Length);
            }
        }

        [TestMethod]
        public void ExtensionInfo_UpdateIniFile_1()
        {
            ExtensionInfo[] extensions =
            {
                new ExtensionInfo
                {
                    Index = 0,
                    DllName = "Rubricator.dll",
                    FunctionName = "ShowRubricator",
                    GroupNumber = 1,
                    IconName = "RUB",
                    Name = "Рубрикатор",
                    PftName = "Rubricator.pft"
                },
                new ExtensionInfo
                {
                    Index = 1,
                    DllName = "RecordChecker.dll",
                    FunctionName = "CheckTheRecord",
                    GroupNumber = 1,
                    IconName = "CHECK",
                    Name = "Проверка качества библиографической записи",
                    PftName = "RecordChecker.pft"
                }
            };
            IniFile iniFile = new IniFile();
            ExtensionInfo.UpdateIniFile(iniFile, extensions);
            StringWriter writer = new StringWriter();
            iniFile.Save(writer);
            string actual = writer.ToString().DosToUnix();
            Assert.AreEqual
                (
                    "[USERMODE]\n" +
                    "UMNUMB=2\n" +
                    "UMDLL0=Rubricator.dll\n" +
                    "UMFUNCTION0=ShowRubricator\n" +
                    "UMPFT0=Rubricator.pft\n" +
                    "UMGROUP0=1\n" +
                    "UMNAME0=Рубрикатор\n" +
                    "UMICON0=RUB\n" +
                    "UMDLL1=RecordChecker.dll\n" +
                    "UMFUNCTION1=CheckTheRecord\n" +
                    "UMPFT1=RecordChecker.pft\n" +
                    "UMGROUP1=1\n" +
                    "UMNAME1=Проверка качества библиографической записи\n" +
                    "UMICON1=CHECK\n",
                    actual
                );
        }

        private void _TestSerialization
            (
                [NotNull] ExtensionInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            ExtensionInfo second = bytes.RestoreObjectFromMemory<ExtensionInfo>();
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.DllName, second.DllName);
            Assert.AreEqual(first.FunctionName, second.FunctionName);
            Assert.AreEqual(first.PftName, second.PftName);
            Assert.AreEqual(first.GroupNumber, second.GroupNumber);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.IconName, second.IconName);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        [Description("Сериализация")]
        public void ExtensionInfo_Serialization_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            _TestSerialization(extension);

            extension = _GetExtension();
            extension.UserData = "User data";
            _TestSerialization(extension);
        }

        [TestMethod]
        [Description("Преобразование в XML")]
        public void ExtensionInfo_ToXml_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            Assert.AreEqual("<extension group=\"1\" />", XmlUtility.SerializeShort(extension));

            extension = _GetExtension();
            Assert.AreEqual("<extension dll=\"RecordChecker.dll\" function=\"CheckTheRecord\" pft=\"RecordChecker.pft\" group=\"1\" name=\"Проверка качества библиографической записи\" icon=\"CHECK\" />", XmlUtility.SerializeShort(extension));
        }

        [TestMethod]
        [Description("Преобразование в JSON")]
        public void ExtensionInfo_ToJson_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            Assert.AreEqual("{'group':1}", JsonUtility.SerializeShort(extension));

            extension = _GetExtension();
            Assert.AreEqual("{'dll':'RecordChecker.dll','function':'CheckTheRecord','pft':'RecordChecker.pft','group':1,'name':'Проверка качества библиографической записи','icon':'CHECK'}", JsonUtility.SerializeShort(extension));
        }

        [TestMethod]
        [Description("Верификация")]
        public void ExtensionInfo_Verify_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            Assert.IsFalse(extension.Verify(false));

            extension = _GetExtension();
            Assert.IsTrue(extension.Verify(false));
        }

        [TestMethod]
        [Description("Преобразование в строку")]
        public void ExtensionInfo_ToString_1()
        {
            ExtensionInfo extension = new ExtensionInfo();
            Assert.AreEqual("(null)", extension.ToString());

            extension = _GetExtension();
            Assert.AreEqual("Проверка качества библиографической записи", extension.ToString());
        }
    }
}
