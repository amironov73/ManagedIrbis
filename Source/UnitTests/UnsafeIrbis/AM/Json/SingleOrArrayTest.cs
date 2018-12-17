using System.Collections.Generic;

using UnsafeAM;
using UnsafeAM.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

namespace UnitTests.UnsafeAM.Json
{
    [TestClass]
    public class SingleOrArrayTest
    {
        [TestMethod]
        public void SingleOrArray_Construction_1()
        {
            SingleOrArray<int> obj = new SingleOrArray<int>();
            Assert.IsTrue(obj.IsEmpty);
            Assert.IsTrue(obj.IsSingle);
            Assert.AreEqual(0, obj.Value);
            Assert.AreEqual(0, obj.Values.Length);
        }

        [TestMethod]
        public void SingleOrArray_Construction_2()
        {
            int value = 42;
            SingleOrArray<int> obj = new SingleOrArray<int>(value);
            Assert.IsFalse(obj.IsEmpty);
            Assert.IsTrue(obj.IsSingle);
            Assert.AreEqual(value, obj.Value);
            Assert.AreEqual(1, obj.Values.Length);
            Assert.AreEqual(value, obj.Values[0]);
        }

        [TestMethod]
        public void SingleOrArray_Construction_3()
        {
            int[] values = {42, 1973};
            SingleOrArray<int> obj = new SingleOrArray<int>(values);
            Assert.IsFalse(obj.IsEmpty);
            Assert.IsFalse(obj.IsSingle);
            Assert.AreEqual(values[0], obj.Value);
            Assert.AreEqual(values.Length, obj.Values.Length);
            Assert.AreEqual(values[0], obj.Values[0]);
            Assert.AreEqual(values[1], obj.Values[1]);
        }

        [TestMethod]
        public void SingleOrArray_Construction_4()
        {
            SingleOrArray<int> obj = new SingleOrArray<int>(EmptyArray<int>.Value);
            Assert.IsTrue(obj.IsEmpty);
            Assert.IsTrue(obj.IsSingle);
            Assert.AreEqual(0, obj.Value);
            Assert.AreEqual(0, obj.Values.Length);
        }

        [TestMethod]
        public void SingleOrArray_Construction_5()
        {
            int[] value = {42};
            SingleOrArray<int> obj = new SingleOrArray<int>(value);
            Assert.IsFalse(obj.IsEmpty);
            Assert.IsTrue(obj.IsSingle);
            Assert.AreEqual(value[0], obj.Value);
            Assert.AreEqual(1, obj.Values.Length);
            Assert.AreEqual(value[0], obj.Values[0]);
        }

        [TestMethod]
        public void SingleOrArray_Operator_1()
        {
            int value = 42;
            SingleOrArray<int> obj = value;
            Assert.IsFalse(obj.IsEmpty);
            Assert.AreEqual(value, obj.Value);
            Assert.AreEqual(1, obj.Values.Length);
            Assert.AreEqual(value, obj.Values[0]);
        }

        [TestMethod]
        public void SingleOrArray_Operator_2()
        {
            int[] values = {42, 1973};
            SingleOrArray<int> obj = values;
            Assert.IsFalse(obj.IsEmpty);
            Assert.AreEqual(values[0], obj.Value);
            Assert.AreEqual(values.Length, obj.Values.Length);
            Assert.AreEqual(values[0], obj.Values[0]);
            Assert.AreEqual(values[1], obj.Values[1]);
        }

        [TestMethod]
        public void SingleOrArray_Operator_3()
        {
            int source = 42;
            SingleOrArray<int> obj = source;
            int target = obj;
            Assert.AreEqual(source, target);
        }

        [TestMethod]
        public void SingleOrArray_Operator_4()
        {
            int[] source = {42, 1973};
            SingleOrArray<int> obj = source;
            int[] target = obj;
            CollectionAssert.AreEqual(source, target);
        }

        [TestMethod]
        public void SingleOrArray_FromJson_1()
        {
            int value = 42;
            JProperty prop = new JProperty("prop", value);
            SingleOrArray<int> obj = SingleOrArray<int>.FromJson(prop);
            Assert.IsFalse(obj.IsEmpty);
            Assert.AreEqual(value, obj.Value);
            Assert.AreEqual(1, obj.Values.Length);
            Assert.AreEqual(value, obj.Values[0]);
        }

        [TestMethod]
        public void SingleOrArray_FromJson_2()
        {
            int[] values = {42, 1973};
            JProperty prop = new JProperty("prop", values);
            SingleOrArray<int> obj = SingleOrArray<int>.FromJson(prop);
            Assert.IsFalse(obj.IsEmpty);
            Assert.AreEqual(values[0], obj.Value);
            Assert.AreEqual(values.Length, obj.Values.Length);
            Assert.AreEqual(values[0], obj.Values[0]);
            Assert.AreEqual(values[1], obj.Values[1]);
        }

        [TestMethod]
        public void SingleOrArray_FromJson_3()
        {
            SingleOrArray<int> obj = SingleOrArray<int>.FromJson(null);
            Assert.IsTrue(obj.IsEmpty);
            Assert.AreEqual(0, obj.Value);
            Assert.AreEqual(0, obj.Values.Length);
        }

        [TestMethod]
        public void SingleOrArray_FromSequence_1()
        {
            int[] values = {42, 1973};
            SingleOrArray<int> obj = SingleOrArray<int>.FromSequence(values);
            Assert.IsFalse(obj.IsEmpty);
            Assert.AreEqual(values[0], obj.Value);
            Assert.AreEqual(values.Length, obj.Values.Length);
            Assert.AreEqual(values[0], obj.Values[0]);
            Assert.AreEqual(values[1], obj.Values[1]);
        }

        [TestMethod]
        public void SingleOrArray_GetEnumerator_1()
        {
            SingleOrArray<int> obj = new SingleOrArray<int>();
            IEnumerator<int> enumerator = obj.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(0, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SingleOrArray_GetEnumerator_2()
        {
            int value = 42;
            SingleOrArray<int> obj = new SingleOrArray<int>(value);
            IEnumerator<int> enumerator = obj.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(value, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SingleOrArray_GetEnumerator_3()
        {
            int[] values = {42, 1973};
            SingleOrArray<int> obj = new SingleOrArray<int>(values);
            IEnumerator<int> enumerator = obj.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(values[0], enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(values[1], enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SingleOrArray_ToString_1()
        {
            SingleOrArray<int> obj = new SingleOrArray<int>();
            Assert.AreEqual("0", obj.ToString());
        }

        [TestMethod]
        public void SingleOrArray_ToString_2()
        {
            SingleOrArray<int> obj = new SingleOrArray<int>(42);
            Assert.AreEqual("42", obj.ToString());
        }

        [TestMethod]
        public void SingleOrArray_ToString_3()
        {
            int[] values = {42, 1973};
            SingleOrArray<int> obj = new SingleOrArray<int>(values);
            Assert.AreEqual("42", obj.ToString());
        }
    }
}
