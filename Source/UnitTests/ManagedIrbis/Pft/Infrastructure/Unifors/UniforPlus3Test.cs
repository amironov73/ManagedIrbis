using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus3Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus3_ConvertToAnsi_1()
        {
            Execute("+3W", "");
            Execute("+3WРџСЂРёРІРµС‚", "Привет");
        }

        [TestMethod]
        public void UniforPlus3_ConvertToUtf_1()
        {
            Execute("+3U", "");
            Execute("+3UПривет", "РџСЂРёРІРµС‚");
        }

        [TestMethod]
        public void UniforPlus3_ReplacePlus_1()
        {
            Execute("+3+", "");
            Execute("+3+Съешь ещё+этих+мягких французских булок", "Съешь ещё%2Bэтих%2Bмягких французских булок");
        }

        [TestMethod]
        public void UniforPlus3_UrlDecode_1()
        {
            Execute("+3D", "");
            Execute("+3D%D0%A2%D0%B8%D0%BB%D0%B8%2D%D1%82%D0%B8%D0%BB%D0%B8%2C%20%D1%82%D1%80%D0%B0%D0%BB%D0%B8%2D%D0%B2%D0%B0%D0%BB%D0%B8", "Тили-тили, трали-вали");
        }

        [TestMethod]
        public void UniforPlus3_UrlEncode_1()
        {
            Execute("+3E", "");
            Execute("+3EТили-тили, трали-вали", "%D0%A2%D0%B8%D0%BB%D0%B8-%D1%82%D0%B8%D0%BB%D0%B8%2C+%D1%82%D1%80%D0%B0%D0%BB%D0%B8-%D0%B2%D0%B0%D0%BB%D0%B8");
        }
    }
}
