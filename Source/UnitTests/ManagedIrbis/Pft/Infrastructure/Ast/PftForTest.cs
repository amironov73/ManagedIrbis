using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftForTest
    {
        private void _Execute
            (
                [NotNull] MarcRecord record,
                [NotNull] PftFor node,
                [NotNull] string expected
            )
        {
            PftContext context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            string actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
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

        [NotNull]
        private PftFor _GetNode()
        {
            string name = "i";
            return new PftFor
            {
                Initialization =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = name,
                        Children =
                        {
                            new PftNumericLiteral(1)
                        }
                    }
                },
                Condition = new PftComparison
                {
                    LeftOperand = new PftVariableReference(name),
                    Operation = "<=",
                    RightOperand = new PftNumericLiteral(10)
                },
                Loop =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = name,
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftVariableReference(name),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(1)
                            }
                        }
                    }
                },
                Body =
                {
                    new PftUnconditionalLiteral("Строка "),
                    new PftVariableReference(name),
                    new PftSlash()
                }
            };
        }

        [TestMethod]
        public void PftFor_Construction_1()
        {
            PftFor node = new PftFor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Initialization);
            Assert.AreEqual(0, node.Initialization.Count);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Loop);
            Assert.AreEqual(0, node.Loop.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
        }

        [TestMethod]
        public void PftFor_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.For, 1, 1, "for");
            PftFor node = new PftFor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNotNull(node.Initialization);
            Assert.AreEqual(0, node.Initialization.Count);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.Loop);
            Assert.AreEqual(0, node.Loop.Count);
            Assert.IsNotNull(node.Body);
            Assert.AreEqual(0, node.Body.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFor_Clone_1()
        {
            PftFor first = new PftFor();
            PftFor second = (PftFor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Clone_2()
        {
            PftFor first = _GetNode();
            PftFor second = (PftFor)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Execute_1()
        {
            MarcRecord record = _GetRecord();
            PftFor node = _GetNode();
            _Execute
                (
                    record,
                    node,
                    "Строка 1\nСтрока 2\nСтрока 3\nСтрока 4\nСтрока 5\nСтрока 6\nСтрока 7\nСтрока 8\nСтрока 9\nСтрока 10\n"
                );
        }

        [TestMethod]
        public void PftFor_Execute_2()
        {
            MarcRecord record = _GetRecord();
            PftFor node = _GetNode();
            node.Body.Add(new PftBreak());
            _Execute
                (
                    record,
                    node,
                    "Строка 1\n"
                );
        }

        [TestMethod]
        public void PftFor_Execute_3()
        {
            // Вложенные циклы

            string outer = "i", inner = "j";
            MarcRecord record = _GetRecord();
            PftFor node = new PftFor
            {
                Initialization =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = outer,
                        Children =
                        {
                            new PftNumericLiteral(1)
                        }
                    }
                },
                Condition = new PftComparison
                {
                    LeftOperand = new PftVariableReference(outer),
                    Operation = "<=",
                    RightOperand = new PftNumericLiteral(3)
                },
                Loop =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = outer,
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftVariableReference(outer),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(1)
                            }
                        }
                    }
                },
                Body =
                {
                    new PftFor
                    {
                        Initialization =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                Name = inner,
                                Children =
                                {
                                    new PftNumericLiteral(1)
                                }
                            }
                        },
                        Condition = new PftComparison
                        {
                            LeftOperand = new PftVariableReference(inner),
                            Operation = "<=",
                            RightOperand = new PftNumericLiteral(3)
                        },
                        Loop =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                Name = inner,
                                Children =
                                {
                                    new PftNumericExpression
                                    {
                                        LeftOperand = new PftVariableReference(inner),
                                        Operation = "+",
                                        RightOperand = new PftNumericLiteral(1)
                                    }
                                }
                            }
                        },
                        Body =
                        {
                            new PftVariableReference(outer),
                            new PftUnconditionalLiteral("=>"),
                            new PftVariableReference(inner),
                            new PftSlash()
                        }
                    },
                    new PftUnconditionalLiteral("=================="),
                    new PftSlash()
                }
            };
            _Execute
                (
                    record,
                    node,
                    "1=>1\n1=>2\n1=>3\n==================\n" +
                    "2=>1\n2=>2\n2=>3\n==================\n" +
                    "3=>1\n3=>2\n3=>3\n==================\n"
                );
        }

        [TestMethod]
        public void PftFor_GetNodeInfo_1()
        {
            PftFor node = _GetNode();
            PftNodeInfo info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("For", info.Name);
        }

        [TestMethod]
        public void PftFor_PrettyPrint_1()
        {
            PftFor node = _GetNode();
            PftPrettyPrinter printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nfor $i=1;; $i<=10; $i=$i + 1;;\ndo\n  \'Строка \'$i /\nend\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftFor first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftFor second = (PftFor)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFor_Serialization_1()
        {
            PftFor node = new PftFor();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFor_ToString_1()
        {
            PftFor node = new PftFor();
            Assert.AreEqual("for ;; do  end", node.ToString());
        }

        [TestMethod]
        public void PftFor_ToString_2()
        {
            PftFor node = _GetNode();

            // TODO FIX THIS!
            Assert.AreEqual("for $i=1;;$i<=10;$i=$i+1; do 'Строка ' $i / end", node.ToString());
        }
    }
}
