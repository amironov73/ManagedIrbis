using System;
using System.Linq;

using AM.Runtime;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftGlobalManagerTest
    {
        [NotNull]
        private PftGlobalManager _GetManager()
        {
            PftGlobalManager result = new PftGlobalManager();
            result.Add(100, "Global100");
            result.Add(101, "Global101\nAnother101");
            result.Add(102, "");

            return result;
        }

        [TestMethod]
        public void PftGlobalManager_Construction_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            Assert.IsNotNull(manager.Registry);
            Assert.AreEqual(0, manager.Registry.Count);
        }

        [TestMethod]
        public void PftGlobalManager_Indexer_1()
        {
            PftGlobalManager manager = _GetManager();
            Assert.AreEqual("Global100", manager[100].DosToUnix());
            Assert.AreEqual("Global101\nAnother101", manager[101].DosToUnix());
            Assert.AreEqual("", manager[1111].DosToUnix());
        }

        [TestMethod]
        public void PftGlobalManager_Indexer_2()
        {
            int index = 111;
            string value = "value";
            PftGlobalManager manager = _GetManager();
            manager[index] = value;
            Assert.AreEqual(value, manager[index]);
        }

        [TestMethod]
        public void PftGlobalManager_Indexer_3()
        {
            int index = 100;
            PftGlobalManager manager = _GetManager();
            Assert.AreNotEqual("", manager[index]);
            manager[index] = null;
            Assert.AreEqual("", manager[index]);
        }

        [TestMethod]
        public void PftGlobalManager_Add_1()
        {
            int index = 111;
            string value = "value";
            PftGlobalManager manager = new PftGlobalManager();
            Assert.AreEqual("", manager[index]);
            manager.Add(index, value);
            Assert.AreEqual(value, manager[index]);
            manager.Add(index, value);
            Assert.AreEqual(value, manager[index]);
        }

        [TestMethod]
        public void PftGlobalManager_Append_1()
        {
            int index = 111;
            string value = "value";
            PftGlobalManager manager = new PftGlobalManager();
            Assert.AreEqual("", manager[index].DosToUnix());
            manager.Append(index, value);
            Assert.AreEqual(value, manager[index].DosToUnix());
            manager.Append(index, value);
            Assert.AreEqual(value + "\n" + value, manager[index].DosToUnix());
        }

        [TestMethod]
        public void PftGlobalManager_Clear_1()
        {
            PftGlobalManager manager = _GetManager();
            Assert.AreNotEqual(0, manager.Registry.Count);
            manager.Clear();
            Assert.AreEqual(0, manager.Registry.Count);
        }

        [TestMethod]
        public void PftGlobalManager_Delete_1()
        {
            int index = 100;
            PftGlobalManager manager = _GetManager();
            Assert.AreNotEqual("", manager[index]);
            manager.Delete(index);
            Assert.AreEqual("", manager[index]);
        }

        [TestMethod]
        public void PftGlobalManager_Get_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            RecordField[] fields = manager.Get(100);
            Assert.AreEqual(0, fields.Length);

            manager = _GetManager();
            fields = manager.Get(100);
            Assert.AreEqual(1, fields.Length);

            fields = manager.Get(101);
            Assert.AreEqual(2, fields.Length);
        }

        [TestMethod]
        public void PftGlobalManager_GetAllVariables_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            PftGlobal[] globals = manager.GetAllVariables();
            Assert.AreEqual(0, globals.Length);

            manager = _GetManager();
            globals = manager.GetAllVariables();
            Assert.AreEqual(3, globals.Length);
        }

        [TestMethod]
        public void PftGlobalManager_HaveVariable_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            Assert.IsFalse(manager.HaveVariable(100));

            manager = _GetManager();
            Assert.IsTrue(manager.HaveVariable(100));
        }

        private void _TestSerialization
            (
                [NotNull] PftGlobalManager first
            )
        {
            byte[] bytes = first.SaveToMemory();
            PftGlobalManager second
                = bytes.RestoreObjectFromMemory<PftGlobalManager>();
            Assert.AreEqual(first.Registry.Count, second.Registry.Count);
            int[] keys = first.Registry.Keys.ToArray();
            foreach (int key in keys)
            {
                string expected = first[key];
                string actual = second[key];
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void PftGlobalManager_Serialization_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            _TestSerialization(manager);

            manager = _GetManager();
            _TestSerialization(manager);
        }

        [TestMethod]
        public void PftGlobalManager_Set_1()
        {
            PftGlobalManager manager = new PftGlobalManager();
            RecordField[] fields =
            {
                new RecordField(100, "Line1"),
                new RecordField(100, "Line2"),
            };
            manager.Set(100, fields);
            Assert.AreEqual("Line1\nLine2", manager[100].DosToUnix());
        }

        [TestMethod]
        public void PftGlobalManager_Set_2()
        {
            PftGlobalManager manager = _GetManager();
            Assert.AreEqual("Global100", manager[100].DosToUnix());
            RecordField[] fields =
            {
                new RecordField(100, "Line1"),
                new RecordField(100, "Line2"),
            };
            manager.Set(100, fields);
            Assert.AreEqual("Line1\nLine2", manager[100].DosToUnix());
        }

        [TestMethod]
        public void PftGlobalManager_Set_3()
        {
            PftGlobalManager manager = _GetManager();
            Assert.AreEqual("Global100", manager[100].DosToUnix());
            RecordField[] fields = new RecordField[0];
            manager.Set(100, fields);
            Assert.IsFalse(manager.HaveVariable(100));
        }

        [TestMethod]
        public void PftGlobalManager_Set_4()
        {
            PftGlobalManager manager = _GetManager();
            Assert.AreEqual("Global100", manager[100].DosToUnix());
            RecordField[] fields = null;
            manager.Set(100, fields);
            Assert.IsFalse(manager.HaveVariable(100));
        }
    }
}
