using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Menus
{
    [TestClass]
    public class ReturnMnuTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis64/Datai/RDR",
                    ReturnMnu.DefaultFileName
                );
        }

        [NotNull]
        private string _GetContent()
        {
            return File.ReadAllText(_GetFileName(), IrbisEncoding.Ansi);
        }

        private void _CheckMenu
            (
                [NotNull] ReturnMnu menu
            )
        {
            Assert.AreEqual(2, menu.Items.Count);
            Assert.AreEqual(new DateTime(2018, 3, 29), menu.Items[0].Date);
        }

        [TestMethod]
        public void ReturnMnu_FromFile_1()
        {
            string fileName = _GetFileName();
            ReturnMnu menu = ReturnMnu.FromFile(fileName);
            _CheckMenu(menu);
        }

        [TestMethod]
        public void ReturnMnu_FromConnection_1()
        {
            Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
            IIrbisConnection connection = mock.Object;
            mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
                .Returns(_GetContent());

            ReturnMnu menu = ReturnMnu.FromConnection(connection);
            _CheckMenu(menu);

            mock.Verify(c => c.ReadTextFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ReturnMnu_FromProvider_1()
        {
            Mock<IrbisProvider> mock = new Mock<IrbisProvider>();
            mock.Setup(c => c.ReadMenuFile(It.IsAny<FileSpecification>()))
                .Returns((FileSpecification specification) => MenuFile.ParseLocalFile(_GetFileName()));
            IrbisProvider provider = mock.Object;

            ReturnMnu menu = ReturnMnu.FromProvider(provider);
            _CheckMenu(menu);

            mock.Verify(c => c.ReadMenuFile(It.IsAny<FileSpecification>()));
        }

        [TestMethod]
        public void ReturnMnu_Item_1()
        {
            DateTime date = new DateTime(2018, 3, 29);
            string comment = "Some date";
            ReturnMnu.Item item = new ReturnMnu.Item
            {
                Date = date,
                Comment = comment
            };
            Assert.AreEqual(date, item.Date);
            Assert.AreEqual(comment, item.Comment);
            Assert.AreEqual("29.03.2018 Some date", item.ToString());
        }
    }
}
