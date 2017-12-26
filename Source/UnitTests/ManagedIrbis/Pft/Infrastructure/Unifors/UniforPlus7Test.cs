using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus7Test
        : CommonUniforTest
    {
        private void _Plus7
            (
                [NotNull][ItemNotNull] string[] commands,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null);
            foreach (string command in commands)
            {
                Unifor unifor = new Unifor();
                unifor.Execute(context, null, command);
            }
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforPlus7_WriteGlobal_1()
        {
            // Простая запись переменных
            string[] commands =
            {
                "+7",
                "+7W111#|Value1",
                "+7W222#|Value2",
                "+7R111",
                "+7R222"
            };
            _Plus7(commands, "|Value1|Value2");
        }

        [TestMethod]
        public void UniforPlus7_AppendGlobal_1()
        {
            // Добавление строк к переменной
            string[] commands =
            {
                "+7",
                "+7W111#|Value1",
                "+7U111#|Value2",
                "+7U111#|Value3",
                "+7R111"
            };
            _Plus7(commands, "|Value1\n|Value2\n|Value3");
        }

        [TestMethod]
        public void UniforPlus7_ClearGlobals_1()
        {
            // Очистка переменных
            string[] commands =
            {
                "+7",
                "+7W111#|Value1",
                "+7",
                "+7R111"
            };
            _Plus7(commands, "");
        }

        [TestMethod]
        public void UniforPlus7_MultiplyGlobals_1()
        {
            // Логическое умножение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7M1#2",
                "+7R1"
            };
            _Plus7(commands, "222\n333");
        }

        [TestMethod]
        public void UniforPlus7_MultiplyGlobals_2()
        {
            // Логическое умножение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7M1",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_MultiplyGlobals_3()
        {
            // Логическое умножение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7Mq#w",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_SubstractGlobals_1()
        {
            // Логическое вычитание переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7S1#2",
                "+7R1"
            };
            _Plus7(commands, "111");
        }

        [TestMethod]
        public void UniforPlus7_SubstractGlobals_2()
        {
            // Логическое вычитание переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7S1",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_SubstractGlobals_3()
        {
            // Логическое вычитание переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7Sq#w",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_UnionGlobals_1()
        {
            // Логическое сложение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7A1#2",
                "+7R1"
            };
            _Plus7(commands, "222\n333\n444\n111");
        }

        [TestMethod]
        public void UniforPlus7_UnionGlobals_2()
        {
            // Логическое сложение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7A1",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_UnionGlobals_3()
        {
            // Логическое сложение переменных
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7W2#222",
                "+7U2#333",
                "+7U2#444",
                "+7Aq#w",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_DistinctGlobal_1()
        {
            // Уникальные значения переменной
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7U1#111",
                "+7G1",
                "+7R1"
            };
            _Plus7(commands, "111\n222\n333");
        }

        [TestMethod]
        public void UniforPlus7_SortGlobal_1()
        {
            // Уникальные значения переменной
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7U1#111",
                "+7T1",
                "+7R1"
            };
            _Plus7(commands, "111\n111\n222\n333");
        }
        
        [TestMethod]
        public void UniforPlus7_ReadGlobal_1()
        {
            // Чтение значений переменной
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7U1#111",
                "+7R1#1"
            };
            _Plus7(commands, "111");
        }

        [TestMethod]
        public void UniforPlus7_ReadGlobal_2()
        {
            // Чтение значений переменной
            string[] commands =
            {
                "+7",
                "+7W1#111",
                "+7U1#222",
                "+7U1#333",
                "+7U1#111",
                "+7R1#q"
            };
            _Plus7(commands, "");
        }
    }
}
