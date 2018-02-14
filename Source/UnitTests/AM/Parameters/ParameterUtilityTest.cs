using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Parameters;

namespace UnitTests.AM.Parameters
{
    [TestClass]
    public class ParameterUtilityTest
    {
        [TestMethod]
        public void TestParameterUtility_Encode_1()
        {
            Parameter[] parameters =
            {
                new Parameter("ordinary", "easy"),
                new Parameter("noValue", null),
                new Parameter("with space", "should work"),
                new Parameter("es=caped", "is; OK")
            };

            const string expected = "ordinary=easy;noValue=;"
                + @"with space=should work;es\=caped=is\; OK;";
            string actual = ParameterUtility.Encode(parameters);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestParameterUtility_ParseString_1()
        {
            string text = "";
            Parameter[] parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(0, parameters.Length);

            text = ";;;";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(0, parameters.Length);

            text = " ";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(0, parameters.Length);

            text = " ; ; ";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(0, parameters.Length);

            text = "ordinary=easy";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = "ordinary=easy;";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = ";ordinary=easy";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = "ordinary = easy";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = " ordinary = easy";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = " ; ; ordinary = easy; ";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ordinary", parameters[0].Name);
            Assert.AreEqual("easy", parameters[0].Value);

            text = "ordinary=easy;noValue=";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(2, parameters.Length);
            Assert.AreEqual("noValue", parameters[1].Name);
            Assert.AreEqual(string.Empty, parameters[1].Value);

            text = "ordinary=easy;noValue=;";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(2, parameters.Length);
            Assert.AreEqual("noValue", parameters[1].Name);
            Assert.AreEqual(string.Empty, parameters[1].Value);

            text = "ordinary=easy;noValue=;with space=should work";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(3, parameters.Length);
            Assert.AreEqual("with space", parameters[2].Name);
            Assert.AreEqual("should work", parameters[2].Value);

            text = "ordinary=easy;noValue=;"
                + @"with space=should work;es\=caped=is\; OK;";
            parameters = ParameterUtility.ParseString(text);
            Assert.AreEqual(4, parameters.Length);
            Assert.AreEqual("es=caped", parameters[3].Name);
            Assert.AreEqual("is; OK", parameters[3].Value);
        }
    }
}
