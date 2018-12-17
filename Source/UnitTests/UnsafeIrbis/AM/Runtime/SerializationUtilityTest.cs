using System.IO;

using UnsafeAM.Runtime;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UnsafeAM.Runtime
{
    [TestClass]
    public class SerializationUtilityTest
    {
        [NotNull]
        private CanaryClass _GetCanary()
        {
            return new CanaryClass
            {
                Name = "John",
                Age = 3
            };
        }

        [TestMethod]
        public void SerializationUtility_RestoreNullable_1()
        {
            // ReSharper disable InvokeAsExtensionMethod

            CanaryClass first = _GetCanary();
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            SerializationUtility.WriteNullable(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            CanaryClass second = SerializationUtility.RestoreNullable<CanaryClass>(reader);
            Assert.IsNotNull(second);
            first.EnsureSame(second);

            // ReSharper restore InvokeAsExtensionMethod
        }

        [TestMethod]
        public void SerializationUtility_RestoreNullable_2()
        {
            // ReSharper disable InvokeAsExtensionMethod
            // ReSharper disable ExpressionIsAlwaysNull

            CanaryClass first = null;
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            SerializationUtility.WriteNullable(writer, first);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            CanaryClass second = SerializationUtility.RestoreNullable<CanaryClass>(reader);
            Assert.IsNull(second);

            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper restore InvokeAsExtensionMethod
        }

        [TestMethod]
        public void SerializationUtility_SaveToFile_1()
        {
            string fileName = Path.GetTempFileName();
            CanaryClass first = _GetCanary();
            first.SaveToFile(fileName);
            CanaryClass second = SerializationUtility.RestoreObjectFromFile<CanaryClass>(fileName);
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToFile_2()
        {
            const int ArraySize = 10;
            string fileName = Path.GetTempFileName();
            CanaryClass[] first = new CanaryClass[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                first[i] = _GetCanary();
                first[i].Age = i;
            }
            first.SaveToFile(fileName);
            CanaryClass[] second = SerializationUtility.RestoreArrayFromFile<CanaryClass>(fileName);
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Length, second.Length);
            for (int i = 0; i < ArraySize; i++)
            {
                first[i].EnsureSame(second[i]);
            }
        }

        [TestMethod]
        public void SerializationUtility_SaveToMemory_1()
        {
            CanaryClass first = _GetCanary();
            byte[] memory = first.SaveToMemory();
            CanaryClass second = memory.RestoreObjectFromMemory<CanaryClass>();
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToMemory_2()
        {
            CanaryClass first = new CanaryClass();
            byte[] memory = first.SaveToMemory();
            CanaryClass second = memory.RestoreObjectFromMemory<CanaryClass>();
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToMemory_3()
        {
            const int ArraySize = 10;
            CanaryClass[] first = new CanaryClass[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                first[i] = _GetCanary();
                first[i].Age = i;
            }
            byte[] memory = first.SaveToMemory();
            CanaryClass[] second = memory.RestoreArrayFromMemory<CanaryClass>();
            Assert.AreEqual(first.Length, second.Length);
            for (int i = 0; i < ArraySize; i++)
            {
                first[i].EnsureSame(second[i]);
            }
        }

        [TestMethod]
        public void SerializationUtility_SaveToString_1()
        {
            CanaryClass first = _GetCanary();
            string text = first.SaveToString();
            CanaryClass second = text.RestoreObjectFromString<CanaryClass>();
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToString_2()
        {
            CanaryClass first = new CanaryClass();
            string text = first.SaveToString();
            CanaryClass second = text.RestoreObjectFromString<CanaryClass>();
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToZipFile_1()
        {
            string fileName = Path.GetTempFileName();
            CanaryClass first = _GetCanary();
            first.SaveToZipFile(fileName);
            CanaryClass second = SerializationUtility.RestoreObjectFromZipFile<CanaryClass>(fileName);
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToZipFile_2()
        {
            const int ArraySize = 10;
            string fileName = Path.GetTempFileName();
            CanaryClass[] first = new CanaryClass[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                first[i] = _GetCanary();
                first[i].Age = i;
            }
            first.SaveToZipFile(fileName);
            CanaryClass[] second = SerializationUtility.RestoreArrayFromZipFile<CanaryClass>(fileName);
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Length, second.Length);
            for (int i = 0; i < ArraySize; i++)
            {
                first[i].EnsureSame(second[i]);
            }
        }

        [TestMethod]
        public void SerializationUtility_SaveToZipMemory_1()
        {
            CanaryClass first = _GetCanary();
            byte[] memory = first.SaveToZipMemory();
            CanaryClass second = memory.RestoreObjectFromZipMemory<CanaryClass>();
            first.EnsureSame(second);
        }

        [TestMethod]
        public void SerializationUtility_SaveToZipMemory_2()
        {
            const int ArraySize = 10;
            CanaryClass[] first = new CanaryClass[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                first[i] = _GetCanary();
                first[i].Age = i;
            }
            byte[] memory = first.SaveToZipMemory();
            CanaryClass[] second = memory.RestoreArrayFromZipMemory<CanaryClass>();
            Assert.AreEqual(first.Length, second.Length);
            for (int i = 0; i < ArraySize; i++)
            {
                first[i].EnsureSame(second[i]);
            }
        }
    }
}
