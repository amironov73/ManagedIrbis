using System;
using System.Reflection;

using AM;
using AM.Text;
using AM.Text.Output;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Compiler
{
    [TestClass]
    public class PftPacketTest
        : Common.CommonUnitTest
    {
        [NotNull]
        private PftPacket _GetPacket
            (
                [NotNull] IrbisProvider provider,
                [NotNull] PftNode[] nodes
            )
        {
            PftContext context = new PftContext(null);
            context.SetProvider(provider);
            PftProgram program = new PftProgram();
            foreach (PftNode node in nodes)
            {
                program.Children.Add(node);
            }
            PftFormatter formatter = new PftFormatter(context)
            {
                Program = program
            };
            PftCompiler compiler = new PftCompiler()
            {
                KeepSource = true,
                Debug = true
            };
            compiler.SetProvider(provider);
            MarcRecord record = new MarcRecord();
            string className = compiler.CompileProgram(formatter.Program);
            AbstractOutput output = AbstractOutput.Console;
            string dllPath = compiler.CompileToDll(output, className);
            Assert.IsNotNull(dllPath, "dllPath");
            Assembly assembly = Assembly.LoadFile(dllPath);
            Func<PftContext, PftPacket> creator
                = CompilerUtility.GetEntryPoint(assembly);
            PftPacket result = creator(context);

            return result;
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
        public void PftPacket_Construction_1()
        {
            PftContext context = new PftContext(null);
            Mock<PftPacket> mock = new Mock<PftPacket>(context);
            PftPacket packet = mock.Object;
            Assert.IsNotNull(packet.Context);
            Assert.IsNull(packet.CurrentField);
            Assert.IsFalse(packet.InGroup);
            Assert.IsNotNull(packet.Breakpoints);
            Assert.AreEqual(0, packet.Breakpoints.Count);
        }

        [TestMethod]
        public void PftPacket_CallDebugger_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftUnconditionalLiteral("Hello"),
                        new PftBang(),
                    };
                    PftPacket packet = _GetPacket(provider, nodes);

                    PftContext context = packet.Context;
                    Mock<PftDebugger> debuggerMock = new Mock<PftDebugger>(context);
                    PftDebugger debugger = debuggerMock.Object;
                    context.Debugger = debugger;

                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("Hello", text);

                    debuggerMock.Verify
                    (
                        d => d.Activate(It.IsAny<PftDebugEventArgs>()),
                        Times.Once
                    );
                }
            }
        }

        [TestMethod]
        public void PftPacket_DoFieldV_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftV(200, 'a'),
                        new PftV(200, 'e')
                        {
                            LeftHand =
                            {
                                new PftConditionalLiteral(" : ", false)
                            }
                        },
                        new PftV(200, 'f')
                        {
                            LeftHand =
                            {
                                new PftConditionalLiteral(" / ", false)
                            }
                        }
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("Заглавие : подзаголовочное / И. И. Иванов, П. П. Петров", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_DoFieldV_2()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftV(300)
                        {
                            LeftHand =
                            {
                                new PftRepeatableLiteral(" => ", true, true)
                            }
                        }
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("Первое примечание => Второе примечание => Третье примечание", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_DoGroup_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftGroup
                        {
                            Children =
                            {
                                new PftV(300),
                                new PftSlash()
                            }
                        },
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("Первое примечание\nВторое примечание\nТретье примечание\n", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_DoGlobal_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftGroup
                        {
                            Children =
                            {
                                new PftG(100),
                                new PftSlash()
                            }
                        },
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    PftContext context = packet.Context;
                    context.Globals.Add(100, "First");
                    context.Globals.Append(100, "Second");
                    context.Globals.Append(100, "Third");
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("First\nSecond\nThird\n", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_DoGlobal_2()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftG(100)
                        {
                            LeftHand =
                            {
                                new PftRepeatableLiteral(" => ", true, true)
                            }
                        }
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    PftContext context = packet.Context;
                    context.Globals.Add(100, "First");
                    context.Globals.Append(100, "Second");
                    context.Globals.Append(100, "Third");
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("First => Second => Third", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_Evaluate_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftUnifor("uf")
                        {
                            Children =
                            {
                                new PftUnconditionalLiteral("+9V")
                            }
                        }
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("64", text);
                }
            }
        }

        [TestMethod]
        public void PftPacket_DebuggerHook_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftUnconditionalLiteral("Hello")
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    packet.Breakpoints.Add(2, true);

                    PftContext context = packet.Context;
                    Mock<PftDebugger> debuggerMock = new Mock<PftDebugger>(context);
                    PftDebugger debugger = debuggerMock.Object;
                    context.Debugger = debugger;

                    string text = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual("Hello", text);

                    debuggerMock.Verify
                        (
                            d => d.Activate(It.IsAny<PftDebugEventArgs>()),
                            Times.Once
                        );
                }
            }
        }

        [TestMethod]
        public void PftPacket_ToString_1()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!ContinuousIntegrationUtility.DetectAppVeyor())
            {
                string expected = "Hello";
                using (IrbisProvider provider = new NullProvider())
                {
                    PftNode[] nodes =
                    {
                        new PftUnconditionalLiteral(expected)
                    };
                    PftPacket packet = _GetPacket(provider, nodes);
                    string actual = packet.Execute(_GetRecord()).DosToUnix();
                    Assert.AreEqual(expected, actual);
                    actual = packet.ToString();
                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }
}
