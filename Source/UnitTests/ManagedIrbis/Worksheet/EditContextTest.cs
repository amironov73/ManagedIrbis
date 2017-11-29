using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Worksheet;

// ReSharper disable UseObjectOrCollectionInitializer

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class EditContextTest
    {
        [TestMethod]
        public void EditContext_Construction_1()
        {
            EditContext context = new EditContext();
            Assert.IsNull(context.Provider);
            Assert.IsNull(context.Record);
            Assert.IsNull(context.Tag);
            Assert.IsNull(context.Code);
            Assert.AreEqual(0, context.Repeat);
            Assert.IsNull(context.Item);
            Assert.IsNull(context.Source);
            Assert.IsNull(context.Result);
            Assert.IsFalse(context.Accept);
            Assert.IsNull(context.UserData);
        }

        [TestMethod]
        public void EditContext_Properties_1()
        {
            EditContext context = new EditContext();
            IrbisProvider provider = new LocalProvider();
            context.Provider = provider;
            Assert.AreSame(provider, context.Provider);
            MarcRecord record = new MarcRecord();
            context.Record = record;
            Assert.AreSame(record, context.Record);
            string tag = "Tag";
            context.Tag = tag;
            Assert.AreSame(tag, context.Tag);
            string code = "c";
            context.Code = code;
            Assert.AreSame(code, context.Code);
            int repeat = 5;
            context.Repeat = repeat;
            Assert.AreEqual(repeat, context.Repeat);
            WorksheetItem item = new WorksheetItem();
            context.Item = item;
            Assert.AreSame(item, context.Item);
            string[] source = {"source"};
            context.Source = source;
            Assert.AreSame(source, context.Source);
            string[] result = {"result"};
            context.Result = result;
            Assert.AreSame(result, context.Result);
            context.Accept = true;
            Assert.IsTrue(context.Accept);
            string userData = "User data";
            context.UserData = userData;
            Assert.AreSame(userData, context.UserData);
        }
    }
}
