using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftOutputTest
    {
        [TestMethod]
        public void PftOutput_Construction_1()
        {
            PftOutput output = new PftOutput();
            Assert.IsNull(output.Parent);
            Assert.IsNotNull(output.Normal);
            Assert.IsNotNull(output.Warning);
            Assert.IsNotNull(output.Error);
            Assert.IsFalse(output.HaveText);
            Assert.IsFalse(output.HaveWarning);
            Assert.IsFalse(output.HaveError);
            Assert.AreEqual(string.Empty, output.Text);
            Assert.AreEqual(string.Empty, output.WarningText);
            Assert.AreEqual(string.Empty, output.ErrorText);
        }

        [TestMethod]
        public void PftOutput_Construction_2()
        {
            PftOutput parent = new PftOutput();
            PftOutput output = new PftOutput(parent);
            Assert.AreSame(parent, output.Parent);
            Assert.IsNotNull(output.Normal);
            Assert.IsNotNull(output.Warning);
            Assert.IsNotNull(output.Error);
            Assert.IsFalse(output.HaveText);
            Assert.IsFalse(output.HaveWarning);
            Assert.IsFalse(output.HaveError);
            Assert.AreEqual(string.Empty, output.Text);
            Assert.AreEqual(string.Empty, output.WarningText);
            Assert.AreEqual(string.Empty, output.ErrorText);
        }

        [TestMethod]
        public void PftOutput_Warning_1()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Warning.Write(text);
            Assert.IsTrue(output.HaveWarning);
            Assert.AreEqual(text, output.WarningText);
        }

        [TestMethod]
        public void PftOutput_Warning_2()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Warning.WriteLine(text);
            Assert.IsTrue(output.HaveWarning);
            Assert.AreEqual("text\n", output.WarningText.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_Error_1()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Error.Write(text);
            Assert.IsTrue(output.HaveError);
            Assert.AreEqual(text, output.ErrorText);
        }

        [TestMethod]
        public void PftOutput_Error_2()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Error.WriteLine(text);
            Assert.IsTrue(output.HaveError);
            Assert.AreEqual("text\n", output.ErrorText.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_ClearError_1()
        {
            PftOutput output = new PftOutput();
            output.Error.Write("text");
            output.ClearError();
            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void PftOutput_ClearText_1()
        {
            PftOutput output = new PftOutput();
            output.Normal.Write("text");
            output.ClearText();
            Assert.IsFalse(output.HaveText);
        }

        [TestMethod]
        public void PftOutput_ClearText_2()
        {
            PftOutput output = new PftOutput();
            output.Write("text");
            output.ClearText();
            Assert.IsFalse(output.HaveText);
        }

        [TestMethod]
        public void PftOutput_ClearWarning_1()
        {
            PftOutput output = new PftOutput();
            output.Warning.Write("text");
            output.ClearWarning();
            Assert.IsFalse(output.HaveWarning);
        }

        [TestMethod]
        public void PftOutput_GetCaretPosition_1()
        {
            PftOutput output = new PftOutput();
            Assert.AreEqual(1, output.GetCaretPosition());

            output.Write("text");
            Assert.AreEqual(5, output.GetCaretPosition());

            output.WriteLine("text");
            Assert.AreEqual(1, output.GetCaretPosition());

            output.WriteLine();
            Assert.AreEqual(1, output.GetCaretPosition());
        }

        [TestMethod]
        public void PftOutput_HaveEmptyLine_1()
        {
            PftOutput output = new PftOutput();
            Assert.IsTrue(output.HaveEmptyLine());
        }

        [TestMethod]
        public void PftOutput_HaveEmptyLine_2()
        {
            PftOutput output = new PftOutput();
            output.Write("text");
            Assert.IsFalse(output.HaveEmptyLine());
        }

        [TestMethod]
        public void PftOutput_HaveEmptyLine_3()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text");
            Assert.IsTrue(output.HaveEmptyLine());
        }

        [TestMethod]
        public void PftOutput_HaveEmptyLine_4()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text").WriteLine();
            Assert.IsTrue(output.HaveEmptyLine());
        }

        [TestMethod]
        public void PftOutput_HaveEmptyLine_5()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text").WriteLine().Write("another text");
            Assert.IsFalse(output.HaveEmptyLine());
        }

        [TestMethod]
        public void PftOutput_PrecededByEmptyLine_1()
        {
            PftOutput output = new PftOutput();
            Assert.IsFalse(output.PrecededByEmptyLine());
        }

        [TestMethod]
        public void PftOutput_PrecededByEmptyLine_2()
        {
            PftOutput output = new PftOutput();
            output.Write("text");
            Assert.IsFalse(output.PrecededByEmptyLine());
        }

        [TestMethod]
        public void PftOutput_PrecededByEmptyLine_3()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text");
            Assert.IsTrue(output.PrecededByEmptyLine());
        }

        [TestMethod]
        public void PftOutput_PrecededByEmptyLine_4()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text").WriteLine();
            Assert.IsTrue(output.PrecededByEmptyLine());
        }

        [TestMethod]
        public void PftOutput_PrecededByEmptyLine_5()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("text").WriteLine().Write("another text");
            Assert.IsFalse(output.PrecededByEmptyLine());
        }

        [TestMethod]
        public void PftOutput_Pop_1()
        {
            string normal = "normal";
            string warning = "warning";
            string error = "error";
            PftOutput parent = new PftOutput();
            PftOutput child = new PftOutput(parent);
            child.Write(normal);
            child.Warning.Write(warning);
            child.Error.Write(error);
            Assert.IsTrue(child.HaveText);
            Assert.IsTrue(child.HaveWarning);
            Assert.IsTrue(child.HaveError);
            child.Pop();
            Assert.IsFalse(parent.HaveText);
            Assert.IsTrue(parent.HaveWarning);
            Assert.IsTrue(parent.HaveError);
            Assert.AreEqual(string.Empty, parent.Text);
            Assert.AreEqual(warning, parent.WarningText);
            Assert.AreEqual(error, parent.ErrorText);
        }

        [TestMethod]
        public void PftOutput_Push_1()
        {
            string normal = "normal";
            string warning = "warning";
            string error = "error";
            PftOutput parent = new PftOutput();
            PftOutput child = parent.Push();
            child.Write(normal);
            child.Warning.Write(warning);
            child.Error.Write(error);
            Assert.IsTrue(child.HaveText);
            Assert.IsTrue(child.HaveWarning);
            Assert.IsTrue(child.HaveError);
            child.Pop();
            Assert.IsFalse(parent.HaveText);
            Assert.IsTrue(parent.HaveWarning);
            Assert.IsTrue(parent.HaveError);
            Assert.AreEqual(string.Empty, parent.Text);
            Assert.AreEqual(warning, parent.WarningText);
            Assert.AreEqual(error, parent.ErrorText);
        }

        [TestMethod]
        public void PftOutput_RemoveEmptyLines_1()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("first line").WriteLine();
            output.RemoveEmptyLines();
            Assert.AreEqual("first line", output.Text);
        }

        [TestMethod]
        public void PftOutput_RemoveEmptyLines_2()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("first line").WriteLine().Write("second line");
            output.RemoveEmptyLines();
            Assert.AreEqual("first line\n\nsecond line", output.Text.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_Write_1()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Write(text);
            Assert.AreEqual(text, output.Text);
        }

        [TestMethod]
        public void PftOutput_Write_2()
        {
            PftOutput output = new PftOutput();
            output.Write("format: {0}", 1);
            Assert.AreEqual("format: 1", output.Text);
        }

        [TestMethod]
        public void PftOutput_WriteLine_1()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("line");
            Assert.AreEqual("line\n", output.Text.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_WriteLine_2()
        {
            PftOutput output = new PftOutput();
            output.WriteLine();
            Assert.AreEqual("\n", output.Text.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_WriteLine_3()
        {
            PftOutput output = new PftOutput();
            output.WriteLine("format: {0}", 1);
            Assert.AreEqual("format: 1\n", output.Text.DosToUnix());
        }

        [TestMethod]
        public void PftOutput_ToString_1()
        {
            string text = "text";
            PftOutput output = new PftOutput();
            output.Write(text);
            Assert.AreEqual(text, output.ToString());
        }
    }
}
