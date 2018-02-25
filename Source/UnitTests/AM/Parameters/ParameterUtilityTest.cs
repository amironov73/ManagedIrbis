using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Parameters;
using JetBrains.Annotations;

namespace UnitTests.AM.Parameters
{
    [TestClass]
    public class ParameterUtilityTest
    {
        [NotNull]
        private Parameter[] _GetParameters()
        {
            return new []
            {
                new Parameter("ordinary", "easy"),
                new Parameter("noValue", null),
                new Parameter("with space", "should work"),
                new Parameter("es=caped", "is; OK")
            };
        }

        [TestMethod]
        public void ParameterUtility_Encode_1()
        {
            Parameter[] parameters = _GetParameters();
            string expected = "ordinary=easy;noValue=;"
                + @"with space=should work;es\=caped=is\; OK;";
            string actual = ParameterUtility.Encode(parameters);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParameterUtility_GetParameter_1()
        {
            Parameter[] parameters = _GetParameters();
            string actual = parameters.GetParameter("ordinary", "default");
            Assert.AreEqual("easy", actual);

            actual = parameters.GetParameter("noSuchParameter", "default");
            Assert.AreEqual("default", actual);
        }

        [TestMethod]
        public void ParameterUtility_GetParameter_2()
        {
            Parameter[] parameters = _GetParameters();
            string actual = parameters.GetParameter<string>("ordinary", "default");
            Assert.AreEqual("easy", actual);

            actual = parameters.GetParameter<string>("noSuchParameter", "default");
            Assert.AreEqual("default", actual);
        }

        [TestMethod]
        public void ParameterUtility_GetParameter_3()
        {
            Parameter[] parameters =
            {
                new Parameter("numeric", "123"),
                new Parameter("boolean", "true"), 
            };
            int numeric = parameters.GetParameter("numeric", 111);
            Assert.AreEqual(123, numeric);

            numeric = parameters.GetParameter("noSuchParameter", 111);
            Assert.AreEqual(111, numeric);

            bool boolean = parameters.GetParameter("boolean", false);
            Assert.IsTrue(boolean);

            boolean = parameters.GetParameter("noSuchParameter", true);
            Assert.IsTrue(boolean);
        }

        [TestMethod]
        public void ParameterUtility_GetParameter_4()
        {
            Parameter[] parameters =
            {
                new Parameter("numeric", "123"),
                new Parameter("boolean", "true"), 
            };
            int numeric = parameters.GetParameter<int>("numeric");
            Assert.AreEqual(123, numeric);

            numeric = parameters.GetParameter<int>("noSuchParameter");
            Assert.AreEqual(0, numeric);

            bool boolean = parameters.GetParameter<bool>("boolean");
            Assert.IsTrue(boolean);

            boolean = parameters.GetParameter<bool>("noSuchParameter");
            Assert.IsFalse(boolean);
        }

        [TestMethod]
        public void ParameterUtility_ParseString_1()
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
