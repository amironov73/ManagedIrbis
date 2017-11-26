using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Extensibility;
using ManagedIrbis.ImportExport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Extensibility
{
    [TestClass]
    public class ExtensionManagerTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description("Создание объекта")]
        public void ExtensionManager_Construction_1()
        {
            ExtensionManager manager = new ExtensionManager();
            Assert.IsNotNull(manager.Extensions);
            Assert.AreEqual(0, manager.Extensions.Count);
        }

        [TestMethod]
        [Description("Чтение из INI-файла")]
        public void ExtensionManager_FromIniFile_1()
        {
            string fileName = Path.Combine
                (
                    Irbis64RootPath,
                    "irbisc.ini"
                );
            using (IniFile iniFile
                = new IniFile(fileName, IrbisEncoding.Ansi, false))
            {
                ExtensionManager manager = ExtensionManager.FromIniFile(iniFile);
                Assert.AreEqual(2, manager.Extensions.Count);
            }
        }

        [TestMethod]
        [Description("Запись в INI-файл")]
        public void ExtensionManager_UpdateIniFile_1()
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

            string fileName = Path.GetTempFileName();
            using (IniFile iniFile = new IniFile(fileName, IrbisEncoding.Ansi, true))
            {
                ExtensionManager manager = new ExtensionManager();
                manager.Extensions.AddRange(extensions);
                manager.UpdateIniFile(iniFile);
            }

            string expected = "[USERMODE]\nUMNUMB=2\nUMDLL0=Rubricator.dll\n" +
               "UMFUNCTION0=ShowRubricator\nUMPFT0=Rubricator.pft\n" +
               "UMGROUP0=1\nUMNAME0=Рубрикатор\nUMICON0=RUB\n" +
               "UMDLL1=RecordChecker.dll\nUMFUNCTION1=CheckTheRecord\n" +
               "UMPFT1=RecordChecker.pft\nUMGROUP1=1\n" +
               "UMNAME1=Проверка качества библиографической записи\n" +
               "UMICON1=CHECK\n";
            string actual = File.ReadAllText(fileName, IrbisEncoding.Ansi).DosToUnix();
            Assert.AreEqual(expected, actual);
        }
    }
}
