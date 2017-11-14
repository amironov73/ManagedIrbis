using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable ConvertToLocalFunction

namespace UnitTests.AM
{
    [TestClass]
    public class EventUtilityTest
    {
        class MyClass
        {
            public bool Flag;

            public event EventHandler Event1;
            public event EventHandler Event1a; // остаётся неподписанным

            public event EventHandler<ExceptionEventArgs> Event2;
            public event EventHandler<ExceptionEventArgs> Event2a;

            public MyClass()
            {
                Event1 += Handler1;
                Event2 += Handler2;
            }

            void Handler1(object sender, EventArgs args)
            {
                Flag = true;
            }

            void Handler2(object sender, ExceptionEventArgs args)
            {
                if (ReferenceEquals(args, null))
                {
                    Flag = true;
                }
                else
                {
                    args.Handled = true;
                }
            }
        }

        //[TestMethod]
        //public void EventUtility_Raise_1()
        //{
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event1.Raise(this);
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event1a.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_2()
        //{
        //    EventArgs args = new EventArgs();
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event1.Raise(this, args);
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event1a.Raise(this, args);
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_3()
        //{
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event2.Raise();
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event2a.Raise();
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_4()
        //{
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event2.Raise(this);
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event2a.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_5()
        //{
        //    Exception exception = new Exception();
        //    ExceptionEventArgs args = new ExceptionEventArgs(exception);
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event2.Raise(this, args);
        //    Assert.IsFalse(obj.Flag);
        //    Assert.IsTrue(args.Handled);

        //    args.Handled = false;
        //    obj.Event2a.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //    Assert.IsFalse(args.Handled);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_6()
        //{
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event1.RaiseAsync(this).Wait();
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event1a.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_Raise_7()
        //{
        //    EventArgs args = new EventArgs();
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    obj.Event1.RaiseAsync(this, args).Wait();
        //    Assert.IsTrue(obj.Flag);

        //    obj.Flag = false;
        //    obj.Event1a.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //}

        //[TestMethod]
        //public void EventUtility_UnsubscribeAll_1()
        //{
        //    MyClass obj = new MyClass();
        //    Assert.IsFalse(obj.Flag);

        //    EventUtility.UnsubscribeAll(obj, "Event1");
        //    //obj.Event1.Raise(this);
        //    Assert.IsFalse(obj.Flag);
        //}
    }
}
