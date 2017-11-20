using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Text;
using AM.Xml;

using ManagedIrbis;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldDifferenceTest
    {
        [TestMethod]
        public void FieldDifference_Construction_1()
        {
            FieldDifference difference = new FieldDifference();
            Assert.AreEqual(0, difference.Tag);
            Assert.AreEqual(0, difference.Repeat);
            Assert.AreEqual(FieldState.Unchanged, difference.State);
            Assert.IsNull(difference.NewValue);
            Assert.IsNull(difference.OldValue);
        }

        [TestMethod]
        public void FieldDifference_ToXml_1()
        {
            FieldDifference difference = new FieldDifference();
            Assert.AreEqual("<field tag=\"0\" repeat=\"0\" state=\"Unchanged\" />", XmlUtility.SerializeShort(difference));

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Edited,
                OldValue = "Old value",
                NewValue = "New value"
            };
            Assert.AreEqual("<field tag=\"200\" repeat=\"1\" state=\"Edited\"><newValue>New value</newValue><oldValue>Old value</oldValue></field>", XmlUtility.SerializeShort(difference));

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Added,
                NewValue = "New value"
            };
            Assert.AreEqual("<field tag=\"200\" repeat=\"1\" state=\"Added\"><newValue>New value</newValue></field>", XmlUtility.SerializeShort(difference));

        }

        [TestMethod]
        public void FieldDifference_ToJson_1()
        {
            FieldDifference difference = new FieldDifference();
            Assert.AreEqual("{'tag':0,'repeat':0,'state':0}", JsonUtility.SerializeShort(difference));

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Edited,
                OldValue = "Old value",
                NewValue = "New value"
            };
            Assert.AreEqual("{'tag':200,'repeat':1,'state':1,'newValue':'New value','oldValue':'Old value'}", JsonUtility.SerializeShort(difference));

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Added,
                NewValue = "New value"
            };
            Assert.AreEqual("{'tag':200,'repeat':1,'state':2,'newValue':'New value'}", JsonUtility.SerializeShort(difference));

        }

        [TestMethod]
        public void FieldDifference_ToString_1()
        {
            FieldDifference difference = new FieldDifference();
            Assert.AreEqual("= 0/0     (null)", difference.ToString().DosToUnix());

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Edited,
                OldValue = "Old value",
                NewValue = "New value"
            };
            Assert.AreEqual("~ 200/1   New value\n          Old value", difference.ToString().DosToUnix());

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Unchanged,
                OldValue = "Old value",
                NewValue = "New value"
            };
            Assert.AreEqual("= 200/1   New value", difference.ToString());

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = FieldState.Added,
                NewValue = "New value"
            };
            Assert.AreEqual("+ 200/1   New value", difference.ToString());

            difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 0,
                State = FieldState.Removed,
                OldValue = "Old value"
            };
            Assert.AreEqual("- 200/0   (null)\n          Old value", difference.ToString().DosToUnix());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void FieldDifference_ToString_2()
        {
            FieldDifference difference = new FieldDifference
            {
                Tag = 200,
                Repeat = 1,
                State = (FieldState)100,
                OldValue = "Old value",
                NewValue = "New value"
            };
            difference.ToString();
        }
    }
}
