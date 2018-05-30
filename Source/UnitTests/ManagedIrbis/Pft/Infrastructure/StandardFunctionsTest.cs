using AM.PlatformAbstraction;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class StandardFunctionsTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private PftContext _Run
            (
                [NotNull] string source
            )
        {
            PftContext result = new PftContext(null);
            result.Provider.PlatformAbstraction
                = new TestingPlatformAbstraction();
            PftFormatter formatter = new PftFormatter(result);
            formatter.ParseProgram(source);
            formatter.Program.Execute(result);

            return result;
        }

        [CanBeNull]
        private string _Test
            (
                [NotNull] string source
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                provider.PlatformAbstraction = new TestingPlatformAbstraction();
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                PftFormatter formatter = new PftFormatter(context);
                formatter.ParseProgram(source);
                formatter.Program.Execute(context);
                string result = context.Text.DosToUnix();

                return result;
            }
        }

        private void _Test
            (
                [NotNull] string source,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                TestingPlatformAbstraction abstraction
                    = new TestingPlatformAbstraction();
                abstraction.Variables.Add("COMSPEC", @"c:\windows\cmd.exe");
                provider.PlatformAbstraction = abstraction;
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                PftFormatter formatter = new PftFormatter(context);
                formatter.ParseProgram(source);
                formatter.Program.Execute(context);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [CanBeNull]
        private string _Test
            (
                [NotNull] MarcRecord record,
                [NotNull] string source
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null)
                {
                    Record = record
                };
                context.SetProvider(provider);
                PftFormatter formatter = new PftFormatter(context);
                formatter.ParseProgram(source);
                formatter.Program.Execute(context);
                string result = context.Text.DosToUnix();

                return result;
            }
        }

        private void _Test
            (
                [NotNull] MarcRecord record,
                [NotNull] string source,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null)
                {
                    Record = record
                };
                context.SetProvider(provider);
                PftFormatter formatter = new PftFormatter(context);
                formatter.ParseProgram(source);
                formatter.Program.Execute(context);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [NotNull]
        private MarcRecord _GetRecord()
        {
            MarcRecord result = new MarcRecord();

            RecordField field = new RecordField(700);
            field.AddSubField('a', "Иванов");
            field.AddSubField('b', "И. И.");
            result.Fields.Add(field);

            field = new RecordField(701);
            field.AddSubField('a', "Петров");
            field.AddSubField('b', "П. П.");
            result.Fields.Add(field);

            field = new RecordField(200);
            field.AddSubField('a', "Заглавие");
            field.AddSubField('e', "подзаголовочное");
            field.AddSubField('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new RecordField(300, "Первое примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Второе примечание");
            result.Fields.Add(field);
            field = new RecordField(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void StandardFunctions_Bold_1()
        {
            Assert.AreEqual
                (
                    "<b>Hello</b>",
                    _Run("bold('Hello')").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("bold()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Chr_1()
        {
            Assert.AreEqual
                (
                    "A",
                    _Run("chr(65)").Text
                );

            Assert.AreEqual
                (
                    "B",
                    _Run("chr('66')").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("chr()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Error_1()
        {
            Assert.AreEqual
                (
                    "message\n",
                    _Run("error('message')").Output.ErrorText.DosToUnix()
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("error()").Output.ErrorText
                );
        }

        [TestMethod]
        public void StandardFunctions_Insert_1()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Run("insert('HelloWorld!'; 5; ', ')").Text
                );
            Assert.AreEqual
                (
                    string.Empty,
                    _Run("insert()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Insert_2()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Run("insert('World!'; 0; 'Hello, ')").Text
                );

            Assert.AreEqual
                (
                    "Hello, World!",
                    _Run("insert('Hello'; 100000; ', World!')").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Italic_1()
        {
            Assert.AreEqual
                (
                    "<i>Hello</i>",
                    _Run("italic('Hello')").Text
                );

            Assert.AreEqual
            (
                string.Empty,
                _Run("italic()").Text
            );
        }

        [TestMethod]
        public void StandardFunctions_Len_1()
        {
            Assert.AreEqual
                (
                    "5",
                    _Run("len('Hello')").Text
                );

            Assert.AreEqual
                (
                    "0",
                    _Run("len()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_NOcc_1()
        {
            MarcRecord record = _GetRecord();
            _Test
                (
                    record,
                    "nocc()",
                    "0"
                );
            _Test
                (
                    record,
                    "nocc(910)",
                    "0"
                );
            _Test
                (
                    record,
                    "nocc(300)",
                    "3"
                );
        }

        [TestMethod]
        public void StandardFunctions_NOcc_2()
        {
            MarcRecord record = _GetRecord();
            _Test
                (
                    record,
                    "(if p(v300) then iocc(), '-',nocc(),'|',fi)",
                    "1-3|2-3|3-3|"
                );
            _Test
                (
                    record,
                    "(if p(v910) then iocc(), '-',nocc(),'|',fi)",
                    string.Empty
                );
            _Test
                (
                    record,
                    "(iocc(), '-',nocc(),'|')",
                    "1-0|"
                );
        }

        [TestMethod]
        public void StandardFunctions_PadLeft_1()
        {
            Assert.AreEqual
                (
                    "     Hello",
                    _Run("padLeft('Hello'; 10)").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("padLeft()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_PadLeft_2()
        {
            Assert.AreEqual
                (
                    "!!!!!Hello",
                    _Run("padLeft('Hello'; 10; '!')").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_PadRight_1()
        {
            Assert.AreEqual
                (
                    "Hello     ",
                    _Run("padRight('Hello'; 10)").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("padRight()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_PadRight_2()
        {
            Assert.AreEqual
            (
                "Hello!!!!!",
                _Run("padRight('Hello'; 10; '!')").Text
            );
        }

        [TestMethod]
        public void StandardFunctions_Remove_1()
        {
            Assert.AreEqual
                (
                    "HelloWorld!",
                    _Run("remove('Hello, World!'; 5; 2)").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("remove()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Replace_1()
        {
            Assert.AreEqual
                (
                    "Hello, Miron!",
                    _Run("replace('Hello, World!'; 'World'; 'Miron')").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("replace()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Size_1()
        {
            Assert.AreEqual
                (
                    "1",
                    _Run("size('Hello')").Text
                );

            Assert.AreEqual
                (
                    "2",
                    _Run("size('Hello' # 'World')").Text
                );

            Assert.AreEqual
                (
                    "0",
                    _Run("size()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Sort_1()
        {
            Assert.AreEqual
                (
                    "1\n2\n3",
                    _Run("sort('3'#'2'#'1')").Text.DosToUnix()
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("sort()").Text.DosToUnix()
                );
        }

        [TestMethod]
        public void StandardFunctions_SubString_1()
        {
            Assert.AreEqual
                (
                    "Hello",
                    _Run("subString('Hello, World!'; 0; 5)").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("subString()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_ToLower_1()
        {
            Assert.AreEqual
            (
                "hello, world!",
                _Run("toLower('Hello, World!')").Text
            );

            Assert.AreEqual
            (
                string.Empty,
                _Run("toLower()").Text
            );
        }

        [TestMethod]
        public void StandardFunctions_ToUpper_1()
        {
            Assert.AreEqual
                (
                    "HELLO, WORLD!",
                    _Run("toUpper('Hello, World!')").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("toUpper()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Trim_1()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Run("trim(' Hello, World! ')").Text
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("trim()").Text
                );
        }

        [TestMethod]
        public void StandardFunctions_Warn_1()
        {
            Assert.AreEqual
                (
                    "message\n",
                    _Run("warn('message')").Output.WarningText.DosToUnix()
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("warn()").Output.WarningText.DosToUnix()
                );
        }

        // ===================================================================

        [TestMethod]
        public void StandardFunctions_AddField_1()
        {
            MarcRecord record = new MarcRecord();
            _Test(record, "addField('100#Field100')", "");
            string[] field = record.FMA(100);
            Assert.AreEqual(1, field.Length);
            Assert.AreEqual("Field100", field[0]);
        }

        [TestMethod]
        public void StandardFunctions_AddField_2()
        {
            MarcRecord record = new MarcRecord();
            _Test(record, "addField('100#Line1'/'Line2')", "");
            string[] field = record.FMA(100);
            Assert.AreEqual(2, field.Length);
            Assert.AreEqual("Line1", field[0]);
            Assert.AreEqual("Line2", field[1]);
        }

        [TestMethod]
        public void StandardFunctions_Cat_1()
        {
            _Test("cat('dumb.fst')", "201 0 (v200 /)\n");
        }

        [TestMethod]
        public void StandardFunctions_CommandLine_1()
        {
            string commandLine = _Test("commandLine()");
            Assert.IsNotNull(commandLine);
        }

        [TestMethod]
        public void StandardFunctions_COut_1()
        {
            string output = _Test("cout('Hello')");
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void StandardFunctions_Debug_1()
        {
            string output = _Test("debug('Hello')");
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void StandardFunctions_DelField_1()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "Field100"));
            _Test(record, "delField('100')");
            Assert.IsFalse(record.HaveField(100));
        }

        [TestMethod]
        public void StandardFunctions_DelField_2()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "Field100-1"));
            record.Fields.Add(new RecordField(100, "Field100-2"));
            _Test(record, "delField('100#2')");
            string[] fields = record.FMA(100);
            Assert.AreEqual(1, fields.Length);
            Assert.AreEqual("Field100-1", fields[0]);
        }

        [TestMethod]
        public void StandardFunctions_DelField_3()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "Field100-1"));
            record.Fields.Add(new RecordField(100, "Field100-2"));
            _Test(record, "delField('100#*')");
            string[] fields = record.FMA(100);
            Assert.AreEqual(1, fields.Length);
            Assert.AreEqual("Field100-1", fields[0]);
        }

        [TestMethod]
        public void StandardFunctions_DelField_4()
        {
            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(100, "Field100-1"));
            record.Fields.Add(new RecordField(100, "Field100-2"));
            _Test(record, "delField('100#?')");
            string[] fields = record.FMA(100);
            Assert.AreEqual(2, fields.Length);
            Assert.AreEqual("Field100-1", fields[0]);
            Assert.AreEqual("Field100-2", fields[1]);
        }

        [TestMethod]
        public void StandardFunctions_Fatal_1()
        {
            PftContext context = _Run("fatal('message')");
            TestingPlatformAbstraction platform
                = (TestingPlatformAbstraction) context.Provider.PlatformAbstraction;
            Assert.IsTrue(platform.FailFastFlag);
        }

        [TestMethod]
        public void StandardFunctions_GetEnv_1()
        {
            _Test("getEnv('COMSPEC')", @"c:\windows\cmd.exe");
        }

        [TestMethod]
        public void StandardFunctions_Include_1()
        {
            _Test
                (
                    "include('_test_hello')",
                    "Hello"
                );
        }

        [TestMethod]
        public void StandardFunctions_MachineName_1()
        {
            _Test("machineName()", "MACHINE");
        }

        [TestMethod]
        public void StandardFunctions_Now_1()
        {
            string format = IrbisDate.DefaultFormat;
            string now =
                new TestingPlatformAbstraction().NowValue.ToString(format);
            string source = string.Format("now('{0}')", format);
            _Test(source, now);
        }

        [TestMethod]
        public void StandardFunctions_NPost_1()
        {
            MarcRecord record = new MarcRecord();
            record
                .AddField(100, "Line1")
                .AddField(100, "Line2");
            _Test(record, "npost('v100')", "2");
        }

        [TestMethod]
        public void StandardFunctions_OsVersion_1()
        {
            _Test
                (
                    "osVersion()",
                    "Microsoft Windows NT 6.1.7601.65536"
                );
        }

        [TestMethod]
        public void StandardFunctions_Search_1()
        {
            _Test("search('K=ATLAS')", "27");
        }

        [TestMethod]
        public void StandardFunctions_Search_2()
        {
            _Test("search('K=A$')", "197\n19\n97\n203\n20\n151\n328\n136\n204\n111\n27");
        }

        [TestMethod]
        public void StandardFunctions_Split_1()
        {
            _Test("split('First,Second,Third';',')", "First\nSecond\nThird");
        }

        [TestMethod]
        public void StandardFunctions_Split_2()
        {
            _Test("split('First,Second,Third')", "");
        }

        [TestMethod]
        public void StandardFunctions_Tags_1()
        {
            MarcRecord record = new MarcRecord()
                .AddField(100, "Field100")
                .AddField(200, "Field200")
                .AddField(210, "Field210")
                .AddField(215, "Field215")
                .AddField(300, "Field300");
            _Test(record, "tags()", "100\n200\n210\n215\n300");
        }

        [TestMethod]
        public void StandardFunctions_Tags_2()
        {
            MarcRecord record = new MarcRecord()
                .AddField(100, "Field100")
                .AddField(200, "Field200")
                .AddField(210, "Field210")
                .AddField(215, "Field215")
                .AddField(300, "Field300");
            _Test(record, "tags('^2')", "200\n210\n215");
        }

        [TestMethod]
        public void StandardFunctions_Today_1()
        {
            string format = "yyyyMMdd";
            string today = new TestingPlatformAbstraction().NowValue.Date.ToString(format);
            string source = string.Format("today('{0}')", format);
            _Test(source, today);
        }

        [TestMethod]
        public void StandardFunctions_Trace_1()
        {
            string output = _Test("trace('Hello')");
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void StandardFunctions_LoadRecord_1()
        {
            string source = "loadRecord(1)";
            _Test(source, "1");
        }

        [TestMethod]
        public void StandardFunctions_LoadRecord_2()
        {
            string source = "loadRecord(1111111)";
            _Test(source, "0");
        }

        [TestMethod]
        public void StandardFunctions_LoadRecord_3()
        {
            string source = "{ loadRecord(1;2) / v200^a }";
            _Test(source, "1\nКуда пойти учиться?");
        }

        [TestMethod]
        public void StandardFunctions_LoadRecord_4()
        {
            string source = "{ loadRecord(1) } / v200^a";
            _Test(source, "1\n");
        }

    }
}
