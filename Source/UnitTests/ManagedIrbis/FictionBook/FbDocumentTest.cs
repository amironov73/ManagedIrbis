using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.FictionBook;

namespace UnitTests.ManagedIrbis.FictionBook
{
    [TestClass]
    public class FbDocumentTest
        : Common.CommonUnitTest
    {
        private string GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "sample.fb2"
                );
        }

        [TestMethod]
        public void FbDocument_LoadBook_1()
        {
            string fileName = GetFileName();
            FbDocument doc = FbDocument.LoadBook(fileName);
            Assert.IsNotNull(doc);

            Assert.IsNotNull(doc.Stylesheet);

            Assert.IsNotNull(doc.Description);
            Assert.IsNotNull(doc.Description.Title);
            Assert.IsNotNull(doc.Description.Title.Author);
            Assert.AreEqual(1, doc.Description.Title.Author.Length);
            Assert.AreEqual("Лев", doc.Description.Title.Author[0].FirstName);
            Assert.AreEqual("Николаевич", doc.Description.Title.Author[0].MiddleName);
            Assert.AreEqual("Толстой", doc.Description.Title.Author[0].LastName);
            Assert.IsNotNull(doc.Description.Title.Genres);
            Assert.AreEqual(7, doc.Description.Title.Genres.Length);
            Assert.AreEqual("history_russia", doc.Description.Title.Genres[0]);
            Assert.AreEqual("romance_historical", doc.Description.Title.Genres[1]);
            Assert.AreEqual("literature_classics", doc.Description.Title.Genres[2]);
            Assert.AreEqual("literature_history", doc.Description.Title.Genres[3]);
            Assert.AreEqual("literature_war", doc.Description.Title.Genres[4]);
            Assert.AreEqual("literature_rus_classic", doc.Description.Title.Genres[5]);
            Assert.AreEqual("computers", doc.Description.Title.Genres[6]);
            Assert.AreEqual("ru", doc.Description.Title.Language);
            Assert.AreEqual("Война и мир", doc.Description.Title.Title);
            //Assert.IsNotNull(doc.Description.Title.Annotation);
            Assert.IsNotNull(doc.Description.Title.Keywords);

            Assert.IsNotNull(doc.Body);
            Assert.AreEqual(2, doc.Body.Length);

            Assert.IsNotNull(doc.Binary);
            Assert.AreEqual(2, doc.Binary.Length);
            Assert.AreEqual("image/jpeg", doc.Binary[0].ContentType);
            Assert.AreEqual("tolstoy_port.png", doc.Binary[0].Id);
            Assert.AreEqual("image/jpeg", doc.Binary[1].ContentType);
            Assert.AreEqual("cover.jpg", doc.Binary[1].Id);
        }

        [TestMethod]
        public void FbDocument_ToString_1()
        {
            FbDocument document = new FbDocument();
            Assert.AreEqual("(null)", document.ToString());
        }

        [TestMethod]
        public void FbDocument_ToString_2()
        {
            FbDocument document = new FbDocument
            {
                Description = new FbDescription()
            };
            Assert.AreEqual("(null)", document.ToString());
        }

        [TestMethod]
        public void FbDocument_ToString_3()
        {
            FbDocument document = new FbDocument
            {
                Description = new FbDescription
                {
                    Title = new FbTitle()
                }
            };
            Assert.AreEqual("(null)", document.ToString());
        }

        [TestMethod]
        public void FbDocument_ToString_4()
        {
            FbDocument document = new FbDocument
            {
                Description = new FbDescription
                {
                    Title = new FbTitle
                    {
                        Title = "Война и мир"
                    }
                }
            };
            Assert.AreEqual("Война и мир", document.ToString());
        }
    }
}
