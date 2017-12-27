using System.IO;

using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftParserTest
    {
        [NotNull]
        private PftProgram _Parse
            (
                [NotNull] string text
            )
        {
            PftLexer lexer = new PftLexer();
            PftTokenList list = lexer.Tokenize(text);
            PftParser parser = new PftParser(list);
            PftProgram result = parser.Parse();

            return result;
        }

        private void _Compare
            (
                [NotNull] PftProgram expected,
                [NotNull] PftProgram actual
            )
        {
            PftSerializationUtility.VerifyDeserializedProgram(expected, actual);
        }

        [TestMethod]
        public void PftParser_Construction_1()
        {
            PftToken[] tokens = new PftToken[0];
            PftTokenList list = new PftTokenList(tokens);
            PftParser parser = new PftParser(list);
            Assert.AreSame(list, parser.Tokens);
        }

        [TestMethod]
        public void PftParser_EmptyProgram_1()
        {
            PftProgram program = _Parse(string.Empty);
            Assert.AreEqual(0, program.Children.Count);
        }

        [TestMethod]
        public void PftParser_ParseA_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftA
                        {
                            Field = new PftV("v200^a")
                        }
                    }
                }
            };
            PftProgram actual = _Parse("if a(v200^a) then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_ParseA_2()
        {
            _Parse("if a(d200^a) then fi");
        }

        [TestMethod]
        public void PftParser_ParseAbs_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftAbs
                        {
                            Children =
                            {
                                new PftMinus
                                {
                                    Children = { new PftNumericLiteral(123) }
                                }
                            }
                        },
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                }
            };
            PftProgram actual = _Parse("f(abs(-123),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_All_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftConditionalStatement
                    {
                        Condition = new PftAll
                            (
                                new PftComparison
                                {
                                    LeftOperand = new PftV("v910^a"),
                                    Operation = "=",
                                    RightOperand = new PftUnconditionalLiteral("0")
                                }
                            )
                    }
                );
            PftProgram actual = _Parse("if all(v910^a='0') then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Any_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftConditionalStatement
                    {
                        Condition = new PftAny
                            (
                                new PftComparison
                                {
                                    LeftOperand = new PftV("v910^a"),
                                    Operation = "=",
                                    RightOperand = new PftUnconditionalLiteral("0")
                                }
                            )
                    }
                );
            PftProgram actual = _Parse("if any(v910^a='0') then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_At_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Before"),
                    new PftInclude("inclusion"),
                    new PftUnconditionalLiteral("After")
                }
            };
            PftProgram actual = _Parse("'Before' @inclusion 'After'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Assignment_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftAssignment
                        (
                            "x",
                            new PftUnconditionalLiteral("Hello"),
                            new PftComma(),
                            new PftUnconditionalLiteral("world")
                        )
                );
            PftProgram actual = _Parse("$x='Hello','world';");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_Assignment_1a()
        {
            _Parse("$x='Hello','world'");
        }

        [TestMethod]
        public void PftParser_Assignment_2()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftAssignment
                    {
                        IsNumeric = true,
                        Name = "x",
                        Children =
                        {
                            new PftNumericExpression
                            {
                                LeftOperand = new PftNumericLiteral(1),
                                Operation = "+",
                                RightOperand = new PftNumericLiteral(2)
                            }
                        }
                    }
                }
            };
            PftProgram actual = _Parse("$x=1+2;");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_Assignment_2a()
        {
            _Parse("$x=1+");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_Assignment_2b()
        {
            _Parse("$x=1+;");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_Assignment_2c()
        {
            _Parse("$x=1+2 3;");
        }

        [TestMethod]
        public void PftParser_Bang_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("The Big "),
                    new PftBang(),
                    new PftUnconditionalLiteral("Bang")
                }
            };
            PftProgram actual = _Parse("'The Big ' ! 'Bang'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Blank_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftBlank
                            {
                                Children =
                                {
                                    new PftV("v910^b")
                                }
                            }
                    }
                }
            };
            PftProgram actual = _Parse("if blank(v910^b) then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Break_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Spring"),
                    new PftBreak()
                }
            };
            PftProgram actual = _Parse("'Spring' break");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_C_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftC(1),
                    new PftC(2),
                    new PftC(3)
                }
            };
            PftProgram actual = _Parse("c1 c2 c3");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Ceil_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftCeil
                        {
                            Children = { new PftNumericLiteral(3.14) }
                        },
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                }
            };
            PftProgram actual = _Parse("f(ceil(3.14),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_CodeBlock_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftCodeBlock("Console.WriteLine(\"Hello\");")
                }
            };
            PftProgram actual = _Parse("{{{Console.WriteLine(\"Hello\");}}}");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Comma_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftComma(),
                    new PftUnconditionalLiteral("world")
                }
            };
            PftProgram actual = _Parse("'Hello' , 'world'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Comment_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftComment("Comment"),
                    new PftUnconditionalLiteral("world")
                }
            };
            PftProgram actual = _Parse("'Hello' /*Comment\n 'world'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Eat_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Hello"),
                    new PftEat
                    {
                        Children =
                        {
                            new PftUnconditionalLiteral("new")
                        }
                    },
                    new PftUnconditionalLiteral("world")
                }
            };
            PftProgram actual = _Parse("'Hello' [[[ 'new' ]]] 'world'");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_Eat_2()
        {
            _Parse("'Hello' [[[ 'new' 'world'");
        }

        [TestMethod]
        public void PftParser_Empty_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftEmpty
                        {
                            Children =
                            {
                                new PftV("v910^b")
                            }
                        }
                    }
                }
            };
            PftProgram actual = _Parse("if empty(v910^b) then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_F_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(3),
                            Operation = "+",
                            RightOperand = new PftNumericLiteral(0.14)
                        },
                        Argument2 = new PftNumericLiteral(10),
                        Argument3 = new PftNumericLiteral(5)
                    }
                }
            };
            PftProgram actual = _Parse("f(3+0.14,10,5)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_F_2()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(3),
                            Operation = "+",
                            RightOperand = new PftNumericLiteral(0.14)
                        },
                        Argument2 = new PftNumericLiteral(10)
                    }
                }
            };
            PftProgram actual = _Parse("f(3+0.14,10)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_F_3()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(3),
                            Operation = "+",
                            RightOperand = new PftNumericLiteral(0.14)
                        }
                    }
                }
            };
            PftProgram actual = _Parse("f(3+0.14)");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_F_3a()
        {
            _Parse("f(3+0.14");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_F_3b()
        {
            _Parse("f(3+0.14,");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_F_3c()
        {
            _Parse("f(3+0.14,10");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_F_3d()
        {
            _Parse("f(3+0.14,10,");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_F_3e()
        {
            _Parse("f(3+0.14,10,5");
        }

        [TestMethod]
        public void PftParser_False_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftConditionalStatement
                    {
                        Condition = new PftFalse()
                    }
                }
            };
            PftProgram actual = _Parse("if false then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_FieldAssignment_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftFieldAssignment
                        (
                            "v300",
                            new PftUnconditionalLiteral("First"),
                            new PftSlash(),
                            new PftUnconditionalLiteral("Second"),
                            new PftSlash(),
                            new PftUnconditionalLiteral("Third")
                        )
                );
            PftProgram actual = _Parse("v300='First'/'Second'/'Third';");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_First_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftFirst
                            (
                                new PftComparison
                                {
                                    LeftOperand = new PftV("v910^a"),
                                    Operation = "=",
                                    RightOperand = new PftUnconditionalLiteral("0")
                                }
                            )
                    }
                );
            PftProgram actual = _Parse("f(first(v910^a='0'))");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Floor_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftFloor
                        {
                            Children = { new PftNumericLiteral(3.14) }
                        },
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                );
            PftProgram actual = _Parse("f(floor(3.14),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Fmt_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftFmt
                    {
                        Number = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(3),
                            Operation = "+",
                            RightOperand = new PftNumericLiteral(0.14)
                        },
                        Format =
                        {
                            new PftUnconditionalLiteral("F2")
                        }
                    }
                }
            };
            PftProgram actual = _Parse("fmt(3+0.14,'F2')");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_For_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftFor
                    {
                        Initialization =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                Name = "x",
                                Children = { new PftNumericLiteral(1) }
                            }
                        },
                        Condition = new PftComparison
                        {
                            LeftOperand = new PftVariableReference("x"),
                            Operation = "<",
                            RightOperand = new PftNumericLiteral(10)
                        },
                        Loop =
                        {
                            new PftAssignment
                            {
                                IsNumeric = true,
                                Name = "x",
                                Children =
                                {
                                    new PftNumericExpression
                                    {
                                        LeftOperand = new PftVariableReference("x"),
                                        Operation = "+",
                                        RightOperand = new PftNumericLiteral(1)
                                    }
                                }
                            }
                        },
                        Body =
                        {
                            new PftUnconditionalLiteral("Hello"),
                            new PftSlash()
                        }
                    }
                );
            PftProgram actual = _Parse("for $x=1; $x < 10; $x = $x + 1; do 'Hello' / end");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_ForEach_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftForEach
                    {
                        Variable = new PftVariableReference("x"),
                        Sequence =
                        {
                            new PftV(200, 'a'),
                            new PftV(200, 'e'),
                            new PftUnconditionalLiteral("Hello")
                        },
                        Body =
                        {
                            new PftVariableReference("x"),
                            new PftHash()
                        }
                    }
                );
            PftProgram actual = _Parse("foreach $x in v200^a v200^e 'Hello'\ndo\n\t$x\n\t#\nend");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Frac_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftF
                    {
                        Argument1 = new PftFrac
                        {
                            Children = { new PftNumericLiteral(3.14) }
                        },
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                }
            };
            PftProgram actual = _Parse("f(frac(3.14),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_From_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftFrom
                    {
                        Variable = new PftVariableReference("x"),
                        Source =
                        {
                            new PftGroup
                                (
                                    new PftV(692, 'b')
                                    {
                                        RightHand = { new PftSlash() }
                                    }
                                )
                        },
                        Where = new PftComparison
                        {
                            LeftOperand = new PftVariableReference("x"),
                            Operation = ":",
                            RightOperand = new PftUnconditionalLiteral("2008")
                        },
                        Select =
                        {
                            new PftUnconditionalLiteral("Item: "),
                            new PftComma(),
                            new PftVariableReference("x"),
                            new PftComma()
                        },
                        Order =
                        {
                            new PftVariableReference("x"),
                            new PftComma()
                        }
                    }
                );
            PftProgram actual = _Parse("from $x in (v692^b/)\nwhere $x:'2008'\n"
                + "select 'Item: ', $x,\norder $x,\nend");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Group_1()
        {
            PftProgram expected = new PftProgram
            {
                Children =
                {
                    new PftGroup
                    {
                        Children =
                        {
                            new PftV("v910^b"),
                            new PftComma(),
                            new PftSlash()
                        }
                    }
                }
            };
            PftProgram actual = _Parse("( v910^b , / )");
            _Compare(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftParser_Group_2()
        {
            _Parse("( v910^b /");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftParser_Group_3()
        {
            _Parse("( v910^b ( v910^d / ) / )");
        }

        [TestMethod]
        public void PftParser_Hash_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftHash(),
                    new PftHash(),
                    new PftHash()
                );
            PftProgram actual = _Parse("###");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Have_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftConditionalStatement
                    {
                        Condition = new PftHave("x", true)
                    }
                );
            PftProgram actual = _Parse("if have($x) then fi");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_L_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftL
                            (
                                new PftUnconditionalLiteral("K=ATLAS")
                            ),
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                );
            PftProgram actual = _Parse("f(l('K=ATLAS'),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Last_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftLast
                            (
                                new PftComparison
                                {
                                    LeftOperand = new PftV("v910^a"),
                                    Operation = "=",
                                    RightOperand = new PftUnconditionalLiteral("0")
                                }
                            )
                    }
                );
            PftProgram actual = _Parse("f(last(v910^a='0'))");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Local_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftAssignment("x", new PftUnconditionalLiteral("outer")),
                    new PftLocal
                    {
                        Names = { "x", "y" },
                        Children =
                        {
                            new PftAssignment("x", new PftUnconditionalLiteral("inner")),
                            new PftAssignment("y", new PftUnconditionalLiteral("another")),
                            new PftVariableReference("x"),
                            new PftHash(),
                            new PftVariableReference("y"),
                            new PftHash(),
                        }
                    },
                    new PftVariableReference("x")
                );
            PftProgram actual = _Parse("$x='outer';\nlocal $x, $y,\ndo\n$x='inner';\n$y ='another';\n"
                    + "$x#$y#\nend\n$x");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Mfn_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftMfn(),
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(2),
                    }
                );
            PftProgram actual = _Parse("f(mfn,0,2)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Mfn_2()
        {
            PftProgram expected = new PftProgram
                (
                    new PftMfn(10),
                    new PftV(200, 'a')
                );
            PftProgram actual = _Parse("mfn(10) v200^a");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Mpl_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'a'),
                    new PftMode("mpl")
                );
            PftProgram actual = _Parse("v200^ampl");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Pow_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftF
                    {
                        Argument1 = new PftPow
                            (
                                new PftNumericLiteral(2),
                                new PftNumericLiteral(8)
                            ),
                        Argument2 = new PftNumericLiteral(0),
                        Argument3 = new PftNumericLiteral(0)
                    }
                );
            PftProgram actual = _Parse("f(pow(2,8),0,0)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_Ref_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftRef
                        (
                            new PftNumericLiteral(3),
                            new PftV(200, 'a'),
                            new PftComma(),
                            new PftV(200, 'e')
                            {
                                LeftHand = { new PftConditionalLiteral(" : ", false) }
                            },
                            new PftComma(),
                            new PftV(200, 'f')
                            {
                                LeftHand = { new PftConditionalLiteral(" / ", false) }
                            }
                        )
                );
            PftProgram actual = _Parse("ref(3,v200^a, \" : \"v200^e, \" / \"v200^f)");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_UnconditionalLiteral_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftUnconditionalLiteral("Hello")
                );
            PftProgram actual = _Parse("'Hello'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_UnconditionalLiteral_2()
        {
            PftProgram expected = new PftProgram
                (
                    new PftUnconditionalLiteral("Hello, "),
                    new PftUnconditionalLiteral("world!")
                );
            PftProgram actual = _Parse("'Hello, ''world!'");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_V_1()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'a'),
                    new PftV(200, 'e')
                );
            PftProgram actual = _Parse("v200^av200^e");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_V_2()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'e')
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" : ", false)
                        }
                    }
                );
            PftProgram actual = _Parse("\" : \"v200^e");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_V_3()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'e')
                    {
                        RightHand =
                        {
                            new PftConditionalLiteral(" : ", true)
                        }
                    }
                );
            PftProgram actual = _Parse("v200^e\" : \"");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_V_4()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'e')
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" : ", false)
                        }
                    },
                    new PftComma(),
                    new PftV(200, 'f')
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" / ", false)
                        }
                    }
                );
            PftProgram actual = _Parse("\" : \"v200^e,\" / \"v200^f");
            _Compare(expected, actual);
        }

        [TestMethod]
        public void PftParser_V_4a()
        {
            PftProgram expected = new PftProgram
                (
                    new PftV(200, 'e')
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" : ", false)
                        },
                        RightHand =
                        {
                            new PftConditionalLiteral(" / ", true)
                        }
                    },
                    new PftV(200, 'f')
                );
            PftProgram actual = _Parse("\" : \"v200^e\" / \"v200^f");
            _Compare(expected, actual);
        }
    }
}
