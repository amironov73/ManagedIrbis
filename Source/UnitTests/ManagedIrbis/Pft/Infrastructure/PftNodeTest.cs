using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AM.Collections;
using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftNodeTest
    {
        class MyNode
            : PftNode
        {
            public void SetChildren(PftNodeCollection children)
            {
                Children = children;
            }
        }

        class MyVisitor
            : PftVisitor
        {
            public List<PftNode> Visited = new List<PftNode>();
            public bool Result = true;

            public override bool VisitNode
                (
                    PftNode node
                )
            {
                Visited.Add(node);

                return Result;
            }
        }

        class MyVisitor2
            : PftVisitor
        {
            public List<PftNode> Visited = new List<PftNode>();
            public int HowMany = 2;

            public override bool VisitNode
                (
                    PftNode node
                )
            {
                Visited.Add(node);

                return --HowMany > 0;
            }
        }

        class MyDebugger
            : PftDebugger
        {
            public bool Flag;

            public MyDebugger([NotNull] PftContext context)
                : base(context)
            {
                Flag = false;
            }

            public override void Activate(PftDebugEventArgs eventArgs)
            {
                base.Activate(eventArgs);
                Flag = true;
            }
        }

        [TestMethod]
        public void PftNode_Construction_1()
        {
            PftNode node = new PftNode();
            Assert.IsNull(node.Parent);
            Assert.IsFalse(node.Breakpoint);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Help);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(PftNodeKind.None, node.Kind);
            Assert.AreEqual(0, node.Column);
            Assert.AreEqual(0, node.LineNumber);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftNode_Construction_2()
        {
            PftToken token = new PftToken(PftTokenKind.A, 1, 1, "a");
            PftNode node = new PftNode(token);
            Assert.IsNull(node.Parent);
            Assert.IsFalse(node.Breakpoint);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Help);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(PftNodeKind.None, node.Kind);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNode_Children_1()
        {
            MyNode node = new MyNode();
            PftNodeCollection children = new PftNodeCollection(node)
            {
                new PftA(),
                new PftAbs(),
                new PftAll()
            };
            node.SetChildren(children);
            Assert.AreSame(children, node.Children);
            foreach (PftNode child in children)
            {
                Assert.AreSame(node, child.Parent);
            }
        }

        [TestMethod]
        public void PftNode_CompareNode_1()
        {
            string text = "text";
            PftNode left = new PftNode
            {
                Text = text
            };
            PftNode right = new PftNode
            {
                Text = text
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_2()
        {
            string text = "text";
            PftNode left = new PftNode
            {
                Text = text,
                Children = { new PftComma() }
            };
            PftNode right = new PftNode
            {
                Text = text,
                Children = { new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_3()
        {
            string text = "text";
            PftNode left = new PftNode
            {
                Text = text,
                Children = { new PftA(), new PftComma() }
            };
            PftNode right = new PftNode
            {
                Text = text,
                Children = { new PftA(), new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_4()
        {
            PftNode left = new PftNode
            {
                Text = "1",
                Children = { new PftComma() }
            };
            PftNode right = new PftNode
            {
                Text = "2",
                Children = { new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        private void _TestSerialization
            (
                [NotNull] PftNode first
            )
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);
            byte[] bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            PftNode second = PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftNode_Serialization_1()
        {
            PftNode node = new PftNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftNode_Properties_1()
        {
            PftNode node = new PftNode();
            node.Breakpoint = true;
            Assert.IsTrue(node.Breakpoint);
            node.Column = 123;
            Assert.AreEqual(123, node.Column);
            node.LineNumber = 234;
            Assert.AreEqual(234, node.LineNumber);
            string text = "text";
            node.Text = text;
            Assert.AreSame(text, node.Text);
        }

        private void _TestAffectedFields
            (
                [NotNull] string text,
                [NotNull] int[] expectedTags
            )
        {
            PftFormatter formatter = new PftFormatter();
            formatter.ParseProgram(text);
            int[] actualTags
                = formatter.Program.GetAffectedFields();

            Assert.AreEqual
                (
                    expectedTags.Length,
                    actualTags.Length
                );
            for (int i = 0; i < expectedTags.Length; i++)
            {
                Assert.AreEqual
                    (
                        expectedTags[i],
                        actualTags[i]
                    );
            }
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_1()
        {
            _TestAffectedFields("", new int[0]);
            _TestAffectedFields(" ", new int[0]);
            _TestAffectedFields("'Hello'", new int[0]);
            _TestAffectedFields("v200^a", new[] { 200 });
            _TestAffectedFields("v200^a, v200^e", new[] { 200 });
            _TestAffectedFields("v200^a, v300", new[] { 200, 300 });
            _TestAffectedFields("if p(v200) then 'OK' fi", new[] { 200 });
            _TestAffectedFields("if p(v200) then v300 fi", new[] { 200, 300 });
            _TestAffectedFields("(if p(v300) then v300 / fi)", new[] { 300 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_2()
        {
            _TestAffectedFields("\"Заглавие\" d200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" d200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_3()
        {
            _TestAffectedFields("\"Заглавие\" n200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" n200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_4()
        {
            _TestAffectedFields("g1", new int[0]);
            _TestAffectedFields("g1, v200", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_5()
        {
            _TestAffectedFields("\"no\"", new int[0]);
            _TestAffectedFields("\"no\", \"yes\"v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_1()
        {
            MyVisitor visitor = new MyVisitor();
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsTrue(node.AcceptVisitor(visitor));
            Assert.AreEqual(4, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_2()
        {
            MyVisitor visitor = new MyVisitor
            {
                Result = false
            };
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsFalse(node.AcceptVisitor(visitor));
            Assert.AreEqual(1, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_3()
        {
            MyVisitor2 visitor = new MyVisitor2();
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsFalse(node.AcceptVisitor(visitor));
            Assert.AreEqual(2, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_Execute_1()
        {
            bool flagBefore = false, flagAfter = false;
            PftNode node = new PftNode();
            node.BeforeExecution += (sender, args) => { flagBefore = true; };
            node.AfterExecution += (sender, args) => { flagAfter = true; };
            PftContext context = new PftContext(null);
            node.Execute(context);
            Assert.IsTrue(flagBefore);
            Assert.IsTrue(flagAfter);
        }

        [TestMethod]
        public void PftNode_Execute_2()
        {
            PftNode node = new PftNode
            {
                Breakpoint = true
            };
            PftContext context = new PftContext(null);
            MyDebugger debugger = new MyDebugger(context);
            context.Debugger = debugger;
            node.Execute(context);
            Assert.IsTrue(debugger.Flag);
        }

        [TestMethod]
        public void PftNode_GetLeafs_1()
        {
            PftNode node = new PftNode();
            PftNode[] leafs = node.GetLeafs();
            Assert.AreEqual(1, leafs.Length);
            Assert.AreSame(node, leafs[0]);
        }

        [TestMethod]
        public void PftNode_GetLeafs_2()
        {
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftNode { Text = "Leaf1" },
                    new PftNode { Text = "Leaf2" },
                    new PftNode { Text = "Leaf3" }
                }
            };
            PftNode[] leafs = node.GetLeafs();
            Assert.AreEqual(3, leafs.Length);
            Assert.AreEqual("Leaf1", leafs[0].Text);
            Assert.AreEqual("Leaf2", leafs[1].Text);
            Assert.AreEqual("Leaf3", leafs[2].Text);
        }

        [TestMethod]
        public void PftNode_GetLeafs_3()
        {
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf1" } }
                    },
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf2" } }
                    },
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf3" } }
                    },
                }
            };
            PftNode[] leafs = node.GetLeafs();
            Assert.AreEqual(3, leafs.Length);
            Assert.AreEqual("Leaf1", leafs[0].Text);
            Assert.AreEqual("Leaf2", leafs[1].Text);
            Assert.AreEqual("Leaf3", leafs[2].Text);
        }

        [TestMethod]
        public void PftNode_GetDescendants_1()
        {
            PftNode node = new PftNode();
            NonNullCollection<PftComma> descendants = node.GetDescendants<PftComma>();
            Assert.AreEqual(0, descendants.Count);
        }

        [TestMethod]
        public void PftNode_GetDescendants_2()
        {
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftComma { Text = "Comma1" },
                    new PftBreak(),
                    new PftComma { Text = "Comma2" },
                }
            };
            NonNullCollection<PftComma> descendants = node.GetDescendants<PftComma>();
            Assert.AreEqual(2, descendants.Count);
            Assert.AreEqual("Comma1", descendants[0].Text);
            Assert.AreEqual("Comma2", descendants[1].Text);
        }

        [TestMethod]
        public void PftNode_SupportsMultithreading_1()
        {
            PftNode node = new PftNode();
            Assert.IsFalse(node.SupportsMultithreading());
        }

        [TestMethod]
        public void PftNode_SupportsMultithreading_2()
        {
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftBreak(),
                    new PftC()
                }
            };
            Assert.IsFalse(node.SupportsMultithreading());
        }

        [TestMethod]
        public void PftNode_Verify_1()
        {
            PftNode node = new PftNode();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void PftNode_Verify_2()
        {
            PftNode node = new PftNode
            {
                Children =
                {
                    new PftNode(),
                    new PftNode(),
                    new PftNode()
                }
            };
            Assert.IsTrue(node.Verify(false));
        }
    }
}
