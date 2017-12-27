using System;
using System.Collections.Generic;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Walking
{
    [TestClass]
    public class UniversalVisitorTest
    {
        [TestMethod]
        public void UniversalVisitor_Construction_1()
        {
            UniversalVisitor visitor = new UniversalVisitor();
            Assert.IsNull(visitor.UserData);
        }

        [TestMethod]
        public void UniversalVisitor_UserData_1()
        {
            string data = "user data";
            UniversalVisitor visitor = new UniversalVisitor();
            visitor.UserData = data;
            Assert.AreSame(data, visitor.UserData);
        }

        [TestMethod]
        public void UniversalVisitor_VisitNode_1()
        {
            PftProgram program = new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("hello"),
                    new PftV(200, 'a')
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral("=>", false)
                        }
                    }
                }
            };

            List<PftNode> visitedNodes = new List<PftNode>();
            Action<VisitorContext> action = context =>
            {
                visitedNodes.Add(context.Node);
            };

            UniversalVisitor visitor = new UniversalVisitor();
            visitor.Visitor += action;

            program.AcceptVisitor(visitor);

            Assert.AreEqual(4, visitedNodes.Count);
        }
    }
}
