using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Flc;

namespace UnitTests.ManagedIrbis.Flc
{
    [TestClass]
    public class FlcProcessorTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void FlcProcessor_CheckRecord_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                string format = "if a(v200) then '1 Missing title data' else '0' fi";
                FlcProcessor processor = new FlcProcessor();
                MarcRecord record = new MarcRecord();
                FlcResult flc = processor.CheckRecord
                    (
                        provider,
                        record,
                        format
                    );
                Assert.IsFalse(flc.CanContinue);
                Assert.AreEqual(1, flc.Messages.Count);
                Assert.AreEqual("Missing title data", flc.Messages[0]);
            }
        }

        [TestMethod]
        public void FlcProcessor_CheckRecord_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                string format = "if a(v200) then '1 Missing title data' else '0' fi";
                FlcProcessor processor = new FlcProcessor();
                MarcRecord record = provider.ReadRecord(1);
                Assert.IsNotNull(record);
                FlcResult flc = processor.CheckRecord
                    (
                        provider,
                        record,
                        format
                    );
                Assert.IsTrue(flc.CanContinue);
                Assert.AreEqual(0, flc.Messages.Count);
            }
        }
    }
}
