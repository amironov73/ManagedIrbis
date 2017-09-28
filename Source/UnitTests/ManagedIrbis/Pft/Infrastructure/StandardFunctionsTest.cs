using System;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
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
            PftFormatter formatter = new PftFormatter(result);
            formatter.ParseProgram(source);
            formatter.Program.Execute(result);

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
    }
}
