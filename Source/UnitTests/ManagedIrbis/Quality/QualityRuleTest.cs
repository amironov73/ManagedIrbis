using System;

using ManagedIrbis;
using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality
{
    class TestRule : QualityRule
    {
        public override string FieldSpec
        {
            get { return "999"; }
        }

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();

            if (fields.Length == 0)
            {
                AddDefect(999, 10, "Отсутствует поле 999");
            }
            if (fields.Length != 0)
            {
                foreach (RecordField field in fields)
                {
                    MustNotContainSubfields(field);
                    MustNotContainWhitespace(field);
                }
            }
            if (fields.Length > 1)
            {
                AddDefect(999, 10, "Много повторений поля 999");
            }

            RuleReport result = EndCheck();
            if (fields.Length == 1)
            {
                result.Bonus = 5;
            }

            return result;
        }

        public RuleReport CheckRecord2
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            RecordField[] fields = GetFields();
            MustBeUniqueField(fields);

            return EndCheck();
        }
    }

    [TestClass]
    public class QualityRuleTest
    {
        [TestMethod]
        public void QualityRule_Construction_1()
        {
            QualityRule rule = new TestRule();
            Assert.IsNull(rule.UserData);

            rule.UserData = "User data";
            Assert.AreEqual("User data", rule.UserData);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_1()
        {
            QualityRule rule = new TestRule();
            MarcRecord record = new MarcRecord();
            RuleContext context = new RuleContext
            {
                Record = record
            };
            RuleReport report = rule.CheckRecord(context);
            Assert.AreEqual(10, report.Damage);
            Assert.AreEqual(1, report.Defects.Count);
            Assert.AreEqual(0, report.Bonus);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_2()
        {
            QualityRule rule = new TestRule();
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(999, "1000"));
            RuleContext context = new RuleContext
            {
                Record = record
            };
            RuleReport report = rule.CheckRecord(context);
            Assert.AreEqual(0, report.Damage);
            Assert.AreEqual(0, report.Defects.Count);
            Assert.AreEqual(5, report.Bonus);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_3()
        {
            TestRule rule = new TestRule();
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(999, "1000"));
            record.Fields.Add(new RecordField(999, "1001"));
            RuleContext context = new RuleContext
            {
                Record = record
            };
            RuleReport report = rule.CheckRecord2(context);
            Assert.AreEqual(0, report.Damage);
            Assert.AreEqual(0, report.Defects.Count);
            Assert.AreEqual(0, report.Bonus);
        }

    }
}
