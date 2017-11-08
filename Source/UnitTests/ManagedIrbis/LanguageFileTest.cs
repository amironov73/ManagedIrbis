using System;
using System.IO;
using AM.Runtime;
using AM.Text;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class LanguageFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void LanguageFile_Construction_1()
        {
            LanguageFile language = new LanguageFile();
            Assert.IsNull(language.Name);
        }

        [TestMethod]
        public void LanguageFile_Name_1()
        {
            const string name = "English";
            LanguageFile language = new LanguageFile
            {
                Name = name
            };
            Assert.AreSame(name, language.Name);
        }

        [TestMethod]
        public void LanguageFile_ReadFrom_1()
        {
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            Assert.AreEqual("Январь", language.GetTranslation("Январь"));
        }

        [TestMethod]
        public void LanguageFile_ReadFrom_1a()
        {
            // Повторяющиеся ключи
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            Assert.AreEqual("Январь", language.GetTranslation("Январь"));
        }

        [TestMethod]
        public void LanguageFile_ReadFrom_1b()
        {
            // Некомплект строк
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            Assert.AreEqual("Январь", language.GetTranslation("Январь"));
        }

        [TestMethod]
        public void LanguageFile_Clear_1()
        {
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            language.Clear();
            Assert.AreEqual("Август", language.GetTranslation("Август"));
        }

        [TestMethod]
        public void LanguageFile_GetTranslation_1()
        {
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            Assert.AreEqual("Серпень", language.GetTranslation("август"));
            Assert.AreEqual("Серпень", language.GetTranslation("АВГУСТ"));
        }

        [TestMethod]
        public void LanguageFile_ReadLocalFile_1()
        {
            string fileName = Path.Combine(TestDataPath, "uk.lng");
            LanguageFile language = LanguageFile.ReadLocalFile(fileName);
            Assert.AreEqual("Серпень", language.GetTranslation("Август"));
            Assert.AreEqual("ювелірні вироби", language.GetTranslation("ювелирные изделия"));
        }

        private void _TestSerialization_1
            (
                [NotNull] LanguageFile first
            )
        {
            byte[] bytes = first.SaveToMemory();
            LanguageFile second = bytes.RestoreObjectFromMemory<LanguageFile>();
            Assert.AreEqual(first.Name, second.Name);
            string expected = first.GetTranslation("Август");
            string actual = second.GetTranslation("Август");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LanguageFile_Serialization_1()
        {
            LanguageFile language = new LanguageFile
            {
                Name = "Украинский"
            };
            _TestSerialization_1(language);
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            language.ReadFrom(reader);
            _TestSerialization_1(language);
        }

        [TestMethod]
        public void LanguageFile_WriteTo_1()
        {
            string text = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(text);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            StringWriter writer = new StringWriter();
            language.WriteTo(writer);
            Assert.AreEqual(text, writer.ToString().DosToUnix());
        }

        [TestMethod]
        public void LanguageFile_WriteLocalFile_1()
        {
            string source = "Абхазский\nАбхазька\nАвгуст\nСерпень\nАвстралия\nАвстралія\nАвстрия\nАвстрія\nАвтограф\nАвтограф\n";
            StringReader reader = new StringReader(source);
            LanguageFile language = new LanguageFile();
            language.ReadFrom(reader);
            string fileName = Path.GetTempFileName();
            language.WriteLocalFile(fileName);
            string text = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual(source, text);
        }
    }
}
