using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable ConvertToLocalFunction

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FunctionDescriptorTest
    {
        [NotNull]
        private FunctionDescriptor _GetDescriptor()
        {
            PftFunction function = (context, node, arguments) =>
            {
                context.WriteLine(node, "Do something");
            };
            FunctionDescriptor result = new FunctionDescriptor
            {
                Name = "SuperFunction",
                Description = "Typical God function",
                Signature = new[]
                {
                    FunctionParameter.String,
                    FunctionParameter.Boolean,
                    FunctionParameter.Numeric
                },
                Function = function
            };

            return result;
        }

        [TestMethod]
        public void FunctionDescriptor_Construction_1()
        {
            FunctionDescriptor descriptor = new FunctionDescriptor();
            Assert.IsNull(descriptor.Name);
            Assert.IsNull(descriptor.Description);
            Assert.IsNull(descriptor.Signature);
            Assert.IsNull(descriptor.Function);
        }

        [TestMethod]
        public void FunctionDescriptor_ToString_1()
        {
            FunctionDescriptor descriptor = _GetDescriptor();
            Assert.AreEqual("SuperFunction", descriptor.ToString());
        }
    }
}
