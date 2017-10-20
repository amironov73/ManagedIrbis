using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforUTest
    {
        [TestMethod]
        public void UniforU_Cumulate_1()
        {
            PftContext context = new PftContext(null);
            Unifor unifor = new Unifor();
            string expression = "U10-15,16,17";
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual("10-17", actual);

        }
    }
}
