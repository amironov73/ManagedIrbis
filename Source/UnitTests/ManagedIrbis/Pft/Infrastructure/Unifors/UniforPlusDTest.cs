using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusDTest
    {
        [TestMethod]
        public void UniforPlusD_GetDatabaseName_1()
        {
            const string DatabaseName = "IBIS";

            PftContext context = new PftContext(null)
            {
                Provider =
                {
                    Database = DatabaseName
                }
            };
            Unifor unifor = new Unifor();
            string expression = "+D";
            unifor.Execute(context, null, expression);
            string actual = context.Text;
            Assert.AreEqual(DatabaseName, actual);
        }
    }
}
