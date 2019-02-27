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
    public class PriceMenuTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetFileName()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis64/Datai/Deposit",
                    PriceMenu.DefaultFileName
                );
        }

        [NotNull]
        private string _GetContent()
        {
            return File.ReadAllText(_GetFileName(), IrbisEncoding.Ansi);
        }

        private void _CheckMenu
            (
                [NotNull] PriceMenu menu
            )
        {
            Assert.AreEqual(99, menu.Items.Count);
            Assert.AreEqual("1980", menu.Items[0].Date);
        }

        [TestMethod]
        public void ReturnMnu_FromFile_1()
        {
            string fileName = _GetFileName();
            PriceMenu menu = PriceMenu.FromFile(fileName);
            _CheckMenu(menu);
        }

    }
}
