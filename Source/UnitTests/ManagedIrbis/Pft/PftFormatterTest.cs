using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftFormatterTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField("700");
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField("701");
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField("200");
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField("300", "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField("300", "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [NotNull]
        private PftProgram _GetProgram()
        {
            return new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Title is: "),
                    new PftV("v200^a"),
                    new PftComma(),
                    new PftV("v200^e")
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" : ", false)
                        }
                    },
                    new PftComma(),
                    new PftV("v200^f")
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" / ", false)
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftFormatter_Construction_1()
        {
            PftFormatter formatter = new PftFormatter();
            Assert.IsTrue(formatter.SupportsExtendedSyntax);
            Assert.IsNotNull(formatter.Context);
            Assert.IsNull(formatter.Program);
            Assert.IsNotNull(formatter.Output);
            Assert.IsNotNull(formatter.Error);
            Assert.IsNotNull(formatter.Warning);
            Assert.IsFalse(formatter.HaveError);
            Assert.IsFalse(formatter.HaveWarning);
            Assert.AreEqual(0L, formatter.Elapsed.Ticks);
        }

        [TestMethod]
        public void PftFormatter_Construction_2()
        {
            PftContext context = new PftContext(null);
            PftFormatter formatter = new PftFormatter(context);
            Assert.IsTrue(formatter.SupportsExtendedSyntax);
            Assert.AreSame(context, formatter.Context);
            Assert.IsNull(formatter.Program);
            Assert.IsNotNull(formatter.Output);
            Assert.IsNotNull(formatter.Error);
            Assert.IsNotNull(formatter.Warning);
            Assert.IsFalse(formatter.HaveError);
            Assert.IsFalse(formatter.HaveWarning);
            Assert.AreEqual(0L, formatter.Elapsed.Ticks);
        }

        [TestMethod]
        public void PftFormatter_FormatRecord_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                using (PftFormatter formatter = new PftFormatter(context))
                {
                    formatter.Program = _GetProgram();
                    MarcRecord record = _GetRecord();
                    string expected = "Title is: Заглавие : подзаголовочное / И. И. Иванов, П. П. Петров";
                    string actual = formatter.FormatRecord(record);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftFormatter_FormatRecord_1a()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                using (PftFormatter formatter = new PftFormatter(context))
                {
                    MarcRecord record = _GetRecord();
                    formatter.FormatRecord(record);
                }
            }
        }

        [TestMethod]
        public void PftFormatter_FormatRecord_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                using (PftFormatter formatter = new PftFormatter(context))
                {
                    formatter.Program = _GetProgram();
                    string expected = "Title is: Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                    string actual = formatter.FormatRecord(1);
                    Assert.AreEqual(expected, actual);
                };
            }
        }

        [TestMethod]
        public void PftFormatter_FormatRecords_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                using (PftFormatter formatter = new PftFormatter(context))
                {
                    formatter.Program = _GetProgram();
                    string expected = "Title is: Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                    int[] mfns = { 1 };
                    string[] actual = formatter.FormatRecords(mfns);
                    Assert.AreEqual(1, actual.Length);
                    Assert.AreEqual(expected, actual[0]);
                };
            }
        }

        [TestMethod]
        public void PftFormatter_ParseProgram_1()
        {
            using (PftFormatter formatter = new PftFormatter())
            {
                string source = "'Title is: ' v200^a, \" : \" v200^e, \" / \" v200^f";
                formatter.ParseProgram(source);
                PftProgram expected = _GetProgram();
                PftProgram actual = formatter.Program;
                PftSerializationUtility.VerifyDeserializedProgram(expected, actual);
            }
        }

        [TestMethod]
        public void PftFormatter_SetProvider_1()
        {
            using (PftFormatter formatter = new PftFormatter())
            {
                IrbisProvider provider = new NullProvider();
                formatter.SetProvider(provider);
                Assert.AreSame(provider, formatter.Context.Provider);
            }
        }
    }
}
