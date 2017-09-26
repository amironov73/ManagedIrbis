using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable HeapView.CanAvoidClosure

namespace UnitTests.AM
{
    [TestClass]
    public class ActionUtilityTest
    {
        private bool _flag1;
        private string _flag2;
        private int _flag3;

        [TestMethod]
        public void ActionUtility_SafeCall_1()
        {
            _flag1 = false;

            Action action = null;
            action.SafeCall();
            Assert.IsFalse(_flag1);

            action = () => { _flag1 = true; };
            action.SafeCall();
            Assert.IsTrue(_flag1);
        }

        [TestMethod]
        public void ActionUtility_SafeCall_2()
        {
            _flag2 = null;

            Action<string> action = null;
            action.SafeCall("Hello");
            Assert.IsNull(_flag2);

            action = text => { _flag2 = text; };
            action.SafeCall("Hello");
            Assert.AreEqual("Hello", _flag2);
        }

        [TestMethod]
        public void ActionUtility_SafeCall_3()
        {
            _flag2 = null;
            _flag3 = 0;

            Action<string, int> action = null;
            action.SafeCall("Hello", 123);
            Assert.IsNull(_flag2);
            Assert.AreEqual(0, _flag3);

            action = (text, number) => { _flag2 = text; _flag3 = number; };
            action.SafeCall("Hello", 123);
            Assert.AreEqual("Hello", _flag2);
            Assert.AreEqual(123, _flag3);
        }

        [TestMethod]
        public void ActionUtility_SafeCall_4()
        {
            _flag1 = false;
            _flag2 = null;
            _flag3 = 0;

            Action<bool, string, int> action = null;
            action.SafeCall(true, "Hello", 123);
            Assert.IsFalse(_flag1);
            Assert.IsNull(_flag2);
            Assert.AreEqual(0, _flag3);

            action = (flag, text, number) =>
            {
                _flag1 = flag;
                _flag2 = text;
                _flag3 = number;
            };
            action.SafeCall(true, "Hello", 123);
            Assert.IsTrue(_flag1);
            Assert.AreEqual("Hello", _flag2);
            Assert.AreEqual(123, _flag3);
        }

        [TestMethod]
        public void ActionUtilty_SafeCall_5()
        {
            Func<int> func = null;
            Assert.AreEqual(123, func.SafeCall(123));

            func = () => 321;
            Assert.AreEqual(321, func.SafeCall(123));
        }

        [TestMethod]
        public void ActionUtility_SafeCall_6()
        {
            _flag1 = false;

            Func<bool, int> func = null;
            Assert.AreEqual(123, func.SafeCall(true, 123));
            Assert.IsFalse(_flag1);

            func = flag => { _flag1 = flag; return 321; };
            Assert.AreEqual(321, func.SafeCall(true, 123));
            Assert.IsTrue(_flag1);
        }

        [TestMethod]
        public void ActionUtility_SafeCall_7()
        {
            _flag2 = null;

            Func<string, int, int> func = null;
            Assert.AreEqual(123, func.SafeCall("Hello", 321, 123));
            Assert.IsNull(_flag2);

            func = (text, number) => { _flag2 = text; return number; };
            Assert.AreEqual(321, func.SafeCall("Hello", 321, 123));
            Assert.AreEqual("Hello", _flag2);
        }

        [TestMethod]
        public void ActionUtility_SafeCall_8()
        {
            _flag1 = false;
            _flag2 = null;

            Func<bool, string, int, int> func = null;
            Assert.AreEqual(123, func.SafeCall(true, "Hello", 321, 123));
            Assert.IsFalse(_flag1);
            Assert.IsNull(_flag2);

            func = (flag, text, number) =>
            {
                _flag1 = flag;
                _flag2 = text;
                return number;
            };
            Assert.AreEqual(321, func.SafeCall(true, "Hello", 321, 123));
            Assert.IsTrue(_flag1);
            Assert.AreEqual("Hello", _flag2);
        }
    }
}

