using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable EqualExpressionComparison

#pragma warning disable CS1718 // Comparison made to same variable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void Set_Construction_1()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            Assert.IsTrue(set.IsEmpty);
            Assert.IsFalse(set.IsReadOnly);
        }

        [TestMethod]
        public void Set_Construction_2()
        {
            Set<int> set = new Set<int>(100);
            Assert.AreEqual(0, set.Count);
            Assert.IsTrue(set.IsEmpty);
            Assert.IsFalse(set.IsReadOnly);
        }

        [TestMethod]
        public void Set_Construction_3()
        {
            Set<int> first = new Set<int> { 1, 2 };
            Set<int> second = new Set<int>(first);
            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.IsEmpty, second.IsEmpty);
        }

        [TestMethod]
        public void Set_Construction_4()
        {
            int[] array = { 1, 2, 3 };
            Set<int> set = new Set<int>(array);
            Assert.AreEqual(array.Length, set.Count);
            Assert.IsFalse(set.IsEmpty);
            Assert.IsFalse(set.IsReadOnly);
        }

        [TestMethod]
        public void Set_Add_1()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            set.Add(1);
            Assert.AreEqual(1, set.Count);
            set.Add(1);
            Assert.AreEqual(1, set.Count);
        }

        [TestMethod]
        public void Set_Add_2()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            set.Add(1, 2);
            Assert.AreEqual(2, set.Count);
            set.Add(2, 3);
            Assert.AreEqual(3, set.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Set_Add_3()
        {
            // ReSharper disable UnusedVariable

            Set<string> set = new Set<string>
            {
                (string) null
            };

            // ReSharper restore UnusedVariable
        }

        [TestMethod]
        public void Set_AddRange_1()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.Count);
            set.AddRange(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            set.AddRange(new[] { 3, 4, 5 });
            Assert.AreEqual(5, set.Count);
        }

        [TestMethod]
        public void Set_ConvertAll_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<string> second = first.ConvertAll
                (
                    item => item.ToString()
                );
            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void Set_TrueForAll_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.IsTrue(set.TrueForAll(item => item > 0));
            Assert.IsFalse(set.TrueForAll(item => item > 2));
        }

        [TestMethod]
        public void Set_FindAdd_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = first.FindAll
                (
                    item => item > 1
                );
            Assert.AreEqual(2, second.Count);

            second = first.FindAll
                (
                    item => item > 3
                );
            Assert.AreEqual(true, second.IsEmpty);
        }

        [TestMethod]
        public void Set_ForEach_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int counter = 0;
            set.ForEach(item => counter = counter + item);
            Assert.AreEqual(6, counter);
        }

        [TestMethod]
        public void Set_Clear_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(false, set.IsEmpty);

            set.Clear();
            Assert.AreEqual(0, set.Count);
            Assert.IsTrue(set.IsEmpty);
        }

        [TestMethod]
        public void Set_Contains_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.IsTrue(set.Contains(1));
            Assert.IsTrue(set.Contains(2));
            Assert.IsTrue(set.Contains(3));
            Assert.IsFalse(set.Contains(4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Set_Contains_2()
        {
            Set<string> set = new Set<string>();
            set.Contains(null);
        }

        [TestMethod]
        public void Set_CopyTo_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] array = new int[6];
            set.CopyTo(array, 0);
            Assert.IsTrue(array.Contains(1));
            Assert.IsTrue(array.Contains(2));
            Assert.IsTrue(array.Contains(3));
            Assert.IsFalse(array.Contains(4));
        }

        [TestMethod]
        public void Set_CopyTo_2()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] array = new int[6];
            ((ICollection)set).CopyTo(array, 0);
            Assert.IsTrue(array.Contains(1));
            Assert.IsTrue(array.Contains(2));
            Assert.IsTrue(array.Contains(3));
            Assert.IsFalse(array.Contains(4));
        }

        [TestMethod]
        public void Set_Remove_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(true, set.Contains(2));

            Assert.IsTrue(set.Remove(2));
            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains(1));
            Assert.IsFalse(set.Contains(2));
            Assert.IsTrue(set.Contains(3));

            Assert.IsFalse(set.Remove(10));
        }

        [TestMethod]
        public void Set_Remove_2()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            Assert.AreEqual(3, set.Count);
            Assert.IsTrue(set.Contains(1));
            Assert.IsTrue(set.Contains(2));
            Assert.IsTrue(set.Contains(3));

            set.Remove(2, 3);
            Assert.AreEqual(1, set.Count);
            Assert.IsTrue(set.Contains(1));
            Assert.IsFalse(set.Contains(2));
            Assert.IsFalse(set.Contains(3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Set_Remove_3()
        {
            Set<string> set = new Set<string>();
            set.Remove((string)null);
        }

        [TestMethod]
        public void Set_GetEnumerator_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] items = set.ToArray();
            Assert.AreEqual(set.Count, items.Length);
            Assert.IsTrue(items.Contains(1));
            Assert.IsTrue(items.Contains(2));
            Assert.IsTrue(items.Contains(3));
        }

        [TestMethod]
        public void Set_GetEnumerator_2()
        {
            // ReSharper disable PossibleNullReferenceException

            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            IEnumerator enumerator = ((IEnumerable)set).GetEnumerator();
            List<int> list = new List<int>();
            Assert.IsTrue(enumerator.MoveNext());
            list.Add((int)enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            list.Add((int)enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            list.Add((int)enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
            list.Sort();
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);

            // ReSharper restore PossibleNullReferenceException
        }

        [TestMethod]
        public void Set_Clone_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = (Set<int>)first.Clone();
            Assert.AreEqual(first.Count, second.Count);
        }

        [TestMethod]
        public void Set_Empty_1()
        {
            Set<int> set = Set<int>.Empty;
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(true, set.IsEmpty);
        }

        [TestMethod]
        public void Set_Items_1()
        {
            Set<int> set = new Set<int>(new[] { 1, 2, 3 });
            int[] items = set.Items;
            Assert.AreEqual(3, items.Length);
            Assert.IsTrue(items.Contains(1));
            Assert.IsTrue(items.Contains(2));
            Assert.IsTrue(items.Contains(3));
        }

        [TestMethod]
        public void Set_Difference_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            int[] result = first.Difference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            result = first.Difference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(0, result.Length);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            result = first.Difference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
        }

        [TestMethod]
        public void Set_Intersection_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            int[] result = first.Intersection(second).Items;
            Array.Sort(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(3, result[1]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            result = first.Intersection(second).Items;
            Array.Sort(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            result = first.Intersection(second).Items;
            Array.Sort(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Set_Union_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            int[] result = first.Union(second).Items;
            Array.Sort(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            result = first.Union(second).Items;
            Array.Sort(result);
            Assert.AreEqual(4, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            result = first.Union(second).Items;
            Array.Sort(result);
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(6, result[5]);
        }

        [TestMethod]
        public void Set_ExclusiveOr_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            int[] result = (first ^ second).Items;
            Array.Sort(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(4, result[1]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            result = (first ^ second).Items;
            Array.Sort(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(4, result[0]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            result = (first ^ second).Items;
            Array.Sort(result);
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(6, result[5]);
        }

        [TestMethod]
        public void Set_SymmetricDifference_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            int[] second = { 2, 3, 4 };
            int[] result = first.SymmetricDifference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(4, result[1]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new[] { 1, 2, 3, 4 };
            result = first.SymmetricDifference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(4, result[0]);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new[] { 4, 5, 6 };
            result = first.SymmetricDifference(second).Items;
            Array.Sort(result);
            Assert.AreEqual(6, result.Length);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
            Assert.AreEqual(3, result[2]);
            Assert.AreEqual(4, result[3]);
            Assert.AreEqual(5, result[4]);
            Assert.AreEqual(6, result[5]);
        }

        [TestMethod]
        public void Set_Equals_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = new Set<int>(new[] { 1, 2, 3 });
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));

            second = null;
            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void Set_Equals_2()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            object second = new Set<int>(new[] { 2, 3, 4 });
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));

            second = new Set<int>(new[] { 1, 2, 3 });
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void Set_Equals_3()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            object second = "Hello, world";
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void Set_GetHashCode_1()
        {
            Set<int> set = new Set<int>();
            Assert.AreEqual(0, set.GetHashCode());

            set.Add(1);
            Assert.AreEqual(1, set.GetHashCode());

            set.Add(2);
            Assert.AreEqual(19, set.GetHashCode());

            set.Add(3);
            Assert.AreEqual(326, set.GetHashCode());
        }

        [TestMethod]
        public void Set_GreaterThan_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            Assert.IsFalse(first > second);
            Assert.IsFalse(second > first);
            Assert.IsFalse(first > first);
            Assert.IsFalse(second > second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            Assert.IsFalse(first > second);
            Assert.IsTrue(second > first);
            Assert.IsFalse(first > first);
            Assert.IsFalse(second > second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            Assert.IsFalse(first > second);
            Assert.IsFalse(second > first);
            Assert.IsFalse(first > first);
            Assert.IsFalse(second > second);
        }

        [TestMethod]
        public void Set_GreaterThanOrEqual_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            Assert.IsFalse(first >= second);
            Assert.IsFalse(second >= first);
            Assert.IsTrue(first >= first);
            Assert.IsTrue(second >= second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3, 4 });
            Assert.IsFalse(first >= second);
            Assert.IsTrue(second >= first);
            Assert.IsTrue(first >= first);
            Assert.IsTrue(second >= second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            Assert.IsFalse(first >= second);
            Assert.IsFalse(second >= first);
            Assert.IsTrue(first >= first);
            Assert.IsTrue(second >= second);
        }

        [TestMethod]
        public void Set_Inequality_1()
        {
            Set<int> first = new Set<int>(new[] { 1, 2, 3 });
            Set<int> second = new Set<int>(new[] { 2, 3, 4 });
            Assert.IsTrue(first != second);
            Assert.IsTrue(second != first);
            Assert.IsFalse(first != first);
            Assert.IsFalse(second != second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 1, 2, 3 });
            Assert.IsFalse(first != second);
            Assert.IsFalse(second != first);
            Assert.IsFalse(first != first);
            Assert.IsFalse(second != second);

            first = new Set<int>(new[] { 1, 2, 3 });
            second = new Set<int>(new[] { 4, 5, 6 });
            Assert.IsTrue(first != second);
            Assert.IsTrue(second != first);
            Assert.IsFalse(first != first);
            Assert.IsFalse(second != second);
        }

    }
}
