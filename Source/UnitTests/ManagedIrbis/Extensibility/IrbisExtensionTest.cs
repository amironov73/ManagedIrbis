using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Extensibility;
using ManagedIrbis.ImportExport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.ManagedIrbis.Extensibility
{
    [TestClass]
    public class IrbisExtensionTest
        : Common.CommonUnitTest
    {
        class MyExtension
            : IrbisExtension
        {
            public override string HandleInput
                (
                    string input
                )
            {
                MarcRecord record = DecodeRecord(input);
                record.Fields.Add(new RecordField(1000, "Field1000"));
                string result = EncodeRecord(record);

                return result;
            }
        }

        [TestMethod]
        public void IrbisExtension_Construction_1()
        {
            MyExtension extension = new MyExtension();
            Assert.IsNotNull(extension);
        }

        [TestMethod]
        public void IrbisExtension_HandleInput_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                MyExtension extension = new MyExtension();
                MarcRecord source = provider.ReadRecord(1);
                Assert.IsNotNull(source);
                Assert.IsFalse(source.HaveField(1000));
                string input = PlainText.ToAllFormat(source);
                string output = extension.HandleInput(input);
                MarcRecord target = IrbisExtension.DecodeRecord(output);
                Assert.IsTrue(target.HaveField(1000));
            }
        }

        [TestMethod]
        public void IrbisExtension_EntryPoint_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                MyExtension extension = new MyExtension();
                MarcRecord source = provider.ReadRecord(1);
                Assert.IsNotNull(source);
                Assert.IsFalse(source.HaveField(1000));
                string input = PlainText.ToAllFormat(source);
                IntPtr inputBuffer = InteropUtility.AllocateMemory(32000);
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                Marshal.Copy(bytes, 0, inputBuffer, bytes.Length);
                IntPtr outputBuffer = InteropUtility.AllocateMemory(32000);
                int retCode = extension.EntryPoint(inputBuffer, outputBuffer, 32000);
                Assert.AreEqual(1, retCode);
                string output = InteropUtility.GetZeroTerminatedString
                    (
                        outputBuffer,
                        Encoding.UTF8,
                        32000
                    );
                MarcRecord target = IrbisExtension.DecodeRecord(output);
                Assert.IsTrue(target.HaveField(1000));
            }
        }
    }
}
