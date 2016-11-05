using System;
using System.Linq;
using AM.Collections;
using AM.Runtime;
using ManagedIrbis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblSettingsTest
    {
        [TestMethod]
        public void GblSettingsTest_Construction1()
        {
            GblSettings settings = new GblSettings();
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(null, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
        }

        [TestMethod]
        public void GblSettingsTest_Construction2()
        {
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = new GblSettings(connection);
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(connection.Database, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
        }

        [TestMethod]
        public void GblSettingsTest_Construction3()
        {
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = new GblSettings
                (
                    connection,
                    statements
                );
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(connection.Database, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
            Assert.AreEqual(statements.Length, settings.Statements.Count);
        }

        [TestMethod]
        public void GblSettings_ForInterval1()
        {
            const int minMfn = 100, maxMfn = 200;
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForInterval
                (
                    connection,
                    minMfn,
                    maxMfn,
                    statements
                );
            Assert.AreEqual(minMfn, settings.MinMfn);
            Assert.AreEqual(maxMfn, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
        }

        [TestMethod]
        public void GblSettings_ForInterval2()
        {
            const int minMfn = 100, maxMfn = 200;
            const string database = "ISTU";
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForInterval
                (
                    connection,
                    database,
                    minMfn,
                    maxMfn,
                    statements
                );
            Assert.AreEqual(database, settings.Database);
            Assert.AreEqual(minMfn, settings.MinMfn);
            Assert.AreEqual(maxMfn, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
        }

        [TestMethod]
        public void GblSettings_ForList1()
        {
            int[] mfnList = new XRange(100, 200).ToArray();
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForList
                (
                    connection,
                    mfnList,
                    statements
                );
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.IsNotNull(settings.MfnList);
            Assert.AreEqual(mfnList.Length, settings.MfnList.Length);
        }

        [TestMethod]
        public void GblSettings_ForList2()
        {
            int[] mfnList = new XRange(100, 200).ToArray();
            const string database = "ISTU";
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForList
                (
                    connection,
                    database,
                    mfnList,
                    statements
                );
            Assert.AreEqual(database, settings.Database);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.IsNotNull(settings.MfnList);
            Assert.AreEqual(mfnList.Length, settings.MfnList.Length);
        }

        [TestMethod]
        public void GblSettings_ForSearchExpression1()
        {
            const string searchExpression = "A=AUTHOR$";
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForSearchExpression
                (
                    connection,
                    searchExpression,
                    statements
                );
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        [TestMethod]
        public void GblSettings_ForSearchExpression2()
        {
            const string searchExpression = "A=AUTHOR$";
            const string database = "ISTU";
            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            GblSettings settings = GblSettings.ForSearchExpression
                (
                    connection,
                    database,
                    searchExpression,
                    statements
                );
            Assert.AreEqual(searchExpression, settings.SearchExpression);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        [TestMethod]
        public void GblSettings_SetFileName()
        {
            const string fileName = "fileName";
            GblSettings settings = new GblSettings();
            settings.SetFileName(fileName);
            Assert.AreEqual(fileName, settings.FileName);
        }

        [TestMethod]
        public void GblSettings_SetRange()
        {
            const int firstRecord = 100;
            const int numberOfRecords = 123;
            GblSettings settings = new GblSettings();
            settings.SetRange(firstRecord, numberOfRecords);
            Assert.AreEqual(firstRecord, settings.FirstRecord);
            Assert.AreEqual(numberOfRecords, settings.NumberOfRecords);
        }

        [TestMethod]
        public void GblSettings_SetSearchExpression()
        {
            const string searchExpression = "A=AUTHOR$";
            GblSettings settings = new GblSettings();
            settings.SetSearchExpression(searchExpression);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        private void _TestSerialize
            (
                GblSettings first
            )
        {
            byte[] bytes = first.SaveToMemory();

            GblSettings second = bytes
                .RestoreObjectFromMemory<GblSettings>();

            Assert.AreEqual(first.Actualize, second.Actualize);
            Assert.AreEqual(first.Autoin, second.Autoin);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.FirstRecord, second.FirstRecord);
            Assert.AreEqual(first.FormalControl, second.FormalControl);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            Assert.AreEqual(first.MinMfn, second.MinMfn);
            Assert.AreEqual(first.NumberOfRecords, second.NumberOfRecords);
            Assert.AreEqual(first.SearchExpression, second.SearchExpression);
            Assert.AreEqual(first.Statements.Count, second.Statements.Count);
        }

        [TestMethod]
        public void GblSettings_Serialize()
        {
            GblSettings settings = new GblSettings();
            _TestSerialize(settings);

            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            settings = new GblSettings
                (
                    connection,
                    statements
                );
            settings
                .SetSearchExpression("A=AUTHOR$")
                .SetRange(100, 200)
                .SetFileName("hello.gbl");
            _TestSerialize(settings);
        }

        [TestMethod]
        public void GblSettings_Verify()
        {
            GblSettings settings = new GblSettings();
            Assert.AreEqual(false, settings.Verify(false));

            GblStatement[] statements =
            {
                new GblStatement(),
                new GblStatement(),
                new GblStatement()
            };
            IrbisConnection connection = new IrbisConnection();
            settings = new GblSettings
                (
                    connection,
                    statements
                );
            settings
                .SetSearchExpression("A=AUTHOR$")
                .SetRange(100, 200)
                .SetFileName("hello.gbl");
            Assert.AreEqual(true, settings.Verify(false));
        }
    }
}
