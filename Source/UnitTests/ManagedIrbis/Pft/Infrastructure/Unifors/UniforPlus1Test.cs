using AM.Collections;
using AM.Text;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus1Test
        : CommonUniforTest
    {
        private void Execute
            (
                [NotNull] string expression,
                [NotNull] Pair<int, string>[] input,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);

                foreach (Pair<int, string> pair in input)
                {
                    context.Globals.Add(pair.First, pair.Second);
                }

                Unifor unifor = new Unifor();
                unifor.Execute(context, null, expression);
                string actual = context.Text.DosToUnix();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void UniforPlus1_AddGlobals_1()
        {
            Execute
                (
                    "+1A100,3#200,3",
                    new []
                    {
                        new Pair<int, string>(100, "Value100"),
                        new Pair<int, string>(101, "Value101"),
                        new Pair<int, string>(102, "Value102"),
                        new Pair<int, string>(200, "Value99"),
                        new Pair<int, string>(201, "Value100"),
                        new Pair<int, string>(202, "Value101"),
                    },
                    "Value99\nValue100\nValue101\nValue102"
                );
        }
        
        [TestMethod]
        public void UniforPlus1_ClearGlobals_1()
        {
            Execute
                (
                    "+1",
                    new[]
                    {
                        new Pair<int, string>(100, "Value100"),
                        new Pair<int, string>(101, "Value101"),
                        new Pair<int, string>(102, "Value102"),
                        new Pair<int, string>(200, "Value99"),
                        new Pair<int, string>(201, "Value100"),
                        new Pair<int, string>(202, "Value101"),
                    },
                    ""
                );
        }

        [TestMethod]
        public void UniforPlus1_DecodeList_1()
        {
            Execute
                (
                    "+1Omhr.mnu|КХ\r\nЦОР\r\nНЕТ\r\nЦНИ\r\n",
                    "Книгохранилище\nЦентр образовательных ресурсов\n\nЦентр научной информации"
                );
        }
        
        [TestMethod]
        public void UniforPlus1_DecodeGlobals_1()
        {
            Execute
                (
                    "+1Kmhr.mnu|100,5",
                    new[]
                    {
                        new Pair<int, string>(100, "КХ"),
                        new Pair<int, string>(101, "ЦОР"),
                        new Pair<int, string>(102, "НЕТ"),
                        new Pair<int, string>(103, "ЦНИ"),
                        new Pair<int, string>(104, ""),
                    },
                    "Книгохранилище\nЦентр образовательных ресурсов\n\nЦентр научной информации"
                );
        }

        [TestMethod]
        public void UniforPlus1_DistinctGlobals_1()
        {
            Execute
                (
                    "+1G100,6",
                    new[]
                    {
                        new Pair<int, string>(100, "First"),
                        new Pair<int, string>(101, "Second"),
                        new Pair<int, string>(102, "First"),
                        new Pair<int, string>(103, ""),
                        new Pair<int, string>(104, "Third"),
                        new Pair<int, string>(105, "Second"),
                    },
                    "First\nSecond\n\nThird\n"
                );
        }

        [TestMethod]
        public void UniforPlus1_DistinctList_1()
        {
            Execute
                (
                    "+1IFirst\r\nSecond\r\nFirst\r\n\r\nThird\r\nSecond",
                    "First\nSecond\n\nThird\n"
                );
        }
        
        [TestMethod]
        public void UniforPlus1_MultiplyGlobals_1()
        {
            Execute
                (
                    "+1M100,3#200,3",
                    new[]
                    {
                        new Pair<int, string>(100, "Value100"),
                        new Pair<int, string>(101, "Value101"),
                        new Pair<int, string>(102, "Value102"),
                        new Pair<int, string>(200, "Value99"),
                        new Pair<int, string>(201, "Value100"),
                        new Pair<int, string>(202, "Value101"),
                    },
                    "Value100\nValue101"
                );
        }

        [TestMethod]
        public void UniforPlus1_ReadGlobal_1()
        {
            Execute
                (
                    "+1R100,3",
                    new[]
                        {
                            new Pair<int, string>(100, "Value100"),
                            new Pair<int, string>(101, ""),
                            new Pair<int, string>(102, "Value102"),
                        },
                    "Value100\n\nValue102"
                );
        }

        [TestMethod]
        public void UniforPlus1_ReadGlobal_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                context.Globals
                    .Add(1, "Value1")
                    .Add(2, "Value2")
                    .Add(3, "Value3")
                    .Add(4, "Value4")
                    .Add(5, "Value5");

                Unifor unifor = new Unifor();
                for (int index = 0; index < 5; index++)
                {
                    context.Index = index;
                    unifor.Execute(context, null, "+1R*+1");
                    context.WriteLine(null, "|");
                }
                string actual = context.Text.DosToUnix();
                Assert.AreEqual("Value2|\nValue3|\nValue4|\nValue5|\n|\n", actual);
            }
        }

        [TestMethod]
        public void UniforPlus1_SortGlobals_1()
        {
            Execute
                (
                    "+1T1,4",
                    new[]
                    {
                        new Pair<int, string>(1, "First"),
                        new Pair<int, string>(2, "second"),
                        new Pair<int, string>(3, "Third"),
                        new Pair<int, string>(4, "Fourth"),
                    },
                    "First\nFourth\nsecond\nThird\n"
                );
        }

        [TestMethod]
        public void UniforPlus1_SortList_1()
        {
            Execute
                (
                    "+1VThird\r\nFirst\r\nSecond\r\n\r\nfourth",
                    "\nFirst\nfourth\nSecond\nThird\n"
                );
        }
        
        [TestMethod]
        public void UniforPlus1_SubstractGlobals_1()
        {
            Execute
                (
                    "+1S100,3#200,3",
                    new[]
                    {
                        new Pair<int, string>(100, "Value100"),
                        new Pair<int, string>(101, "Value101"),
                        new Pair<int, string>(102, "Value102"),
                        new Pair<int, string>(200, "Value99"),
                        new Pair<int, string>(201, "Value100"),
                        new Pair<int, string>(202, "Value101"),
                    },
                    "Value102"
                );
        }

        [TestMethod]
        public void UniforPlus1_WriteGlobal_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);

                Unifor unifor = new Unifor();
                for (int index = 1; index <= 5; index++)
                {
                    unifor.Execute(context, null, "+1W" + index + "#Value" + index);
                }
                unifor.Execute(context, null, "+1R1,5");

                string actual = context.Text.DosToUnix();
                Assert.AreEqual("Value1\nValue2\nValue3\nValue4\nValue5", actual);
            }
        }
    }
}
