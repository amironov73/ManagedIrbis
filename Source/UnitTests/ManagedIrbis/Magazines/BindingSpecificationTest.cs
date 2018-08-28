using ManagedIrbis;
using ManagedIrbis.Magazines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class BindingSpecificationTest
    {
        [TestMethod]
        public void BindingSpecification_Construction_1()
        {
            BindingSpecification specification = new BindingSpecification();
            Assert.IsNull(specification.MagazineIndex);
            Assert.IsNull(specification.Year);
            Assert.IsNull(specification.IssueNumbers);
            Assert.IsNull(specification.Description);
            Assert.IsNull(specification.BindingNumber);
            Assert.IsNull(specification.Inventory);
            Assert.IsNull(specification.Fond);
            Assert.IsNull(specification.Complect);
        }
    }
}
