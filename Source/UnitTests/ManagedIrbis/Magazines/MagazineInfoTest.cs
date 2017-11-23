using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class MagazineInfoTest
    {
        [NotNull]
        private MarcRecord _GetMagazine()
        {
            MarcRecord result = new MarcRecord();
            result.Fields.Add(RecordField.Parse(102, "SU"));
            result.Fields.Add(RecordField.Parse(101, "rus"));
            result.Fields.Add(RecordField.Parse(919, "^Arus^N0102^KPSBO"));
            result.Fields.Add(RecordField.Parse(920, "J"));
            result.Fields.Add(RecordField.Parse(200, "^AЗвезда Востока^Eлитературно-художественный и общественно-политический журнал"));
            result.Fields.Add(RecordField.Parse(210, "^CИздательство литературы и искусства имени Гафура Гуляма^AТашкент^D1939"));
            result.Fields.Add(RecordField.Parse(110, "^Ta^Ba^Df^X12^Z16+"));
            result.Fields.Add(RecordField.Parse(621, "С(Узб)"));
            result.Fields.Add(RecordField.Parse(906, "С(Узб)"));
            result.Fields.Add(RecordField.Parse(60, "10"));
            result.Fields.Add(RecordField.Parse(907, "^CКР^A20171115^BКобаковаЛА"));
            result.Fields.Add(RecordField.Parse(908, "З-43"));
            result.Fields.Add(RecordField.Parse(903, "З596747388"));
            result.Fields.Add(RecordField.Parse(934, "1981"));
            result.Fields.Add(RecordField.Parse(909, "^Q1981^DФП^H1^kЖ54482"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H9^kЖ54047"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H6^kЖ53859"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H5^kЖ53858"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H4^kЖ53857"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H3^kЖ53856"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H2^kЖ53855"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H12^kЖ54481"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H11^kЖ54480"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H10^kЖ57709"));
            result.Fields.Add(RecordField.Parse(909, "^Q1980^DФП^H1^kЖ53854"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H9^kЖ51539"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H8^kЖ51538"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H7^kЖ51537"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H6^kЖ51536"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H5^kЖ51419"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H4^kЖ51418"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H3^kЖ51233"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H2^kЖ51232"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H12^kЖ53449"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H11^kЖ51671"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H10^kЖ51670"));
            result.Fields.Add(RecordField.Parse(909, "^Q1979^DФП^H1^kЖ50887"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H9^kЖ50377"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H7^kЖ50215"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H6^kЖ50214"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H3^kЖ49588"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H2^kЖ48791"));
            result.Fields.Add(RecordField.Parse(909, "^Q1978^DФП^H10^kЖ50546"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^H5^kЖ48224"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H9^kЖ48492"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H8^kЖ48491"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H7^kЖ48490"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H6^kЖ48225"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H4^kЖ48223"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H3^kЖ48015"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H2^kЖ48014"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H12^kЖ48789"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H11^kЖ48612"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H10^kЖ48611"));
            result.Fields.Add(RecordField.Parse(909, "^Q1977^DФП^H1^k1"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H9-11^k1"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H8^kЖ45888"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H7^kЖ45887"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H6^kЖ45886"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H5^kЖ45460"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H4^kЖ45459"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H3^kЖ45458"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H2^kЖ44877"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H12^kЖ46188"));
            result.Fields.Add(RecordField.Parse(909, "^Q1976^DФП^H1^kЖ44876"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H9^k1"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H8^kЖ44441"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H7^kЖ44440"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H6^kЖ44275"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H5^kЖ44274"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H4^kЖ44207"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H3^kЖ43571"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H2^kЖ43131"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H12^kЖ44755"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H11^kЖ44532"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H10^kЖ44531"));
            result.Fields.Add(RecordField.Parse(909, "^Q1975^DФП^H1^kЖ43130"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H9^kЖ41274"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H7^kЖ49587"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H6^kЖ40996"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H4^kЖ40994"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H3^kЖ40993"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H2^kЖ40992"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H12^kЖ42033"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H11^kЖ42815"));
            result.Fields.Add(RecordField.Parse(909, "^Q1974^DФП^H10^kЖ41873"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H9^kЖ39254"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H8^kЖ39253"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H7^kЖ39252"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H6^kЖ39251"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H5^kЖ38934"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H4^kЖ38933"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H3^kЖ38932"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H2^kЖ37750"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H12^kЖ39429"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H11^kЖ39428"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H10^kЖ39466"));
            result.Fields.Add(RecordField.Parse(909, "^Q1973^DФП^H1^kЖ37749"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^H5^kЖ37003"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H9^kЖ37466"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H6^kЖ37004"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H4^kЖ36854"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H3^kЖ36853"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H2^kЖ36852"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H12^kЖ37748"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H11^kЖ37469"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H10^kЖ37467"));
            result.Fields.Add(RecordField.Parse(909, "^Q1972^DФП^H1^kЖ36851"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H9^kЖ36200"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H8^kЖ36087"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H7^kЖ36086"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H5^kЖ35250"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H4^kЖ35163"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H3^kЖ35018"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H2^kЖ35017"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H12^kЖ36203"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H11^kЖ36202"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H10^kЖ36201"));
            result.Fields.Add(RecordField.Parse(909, "^Q1971^DФП^H1^kЖ35016"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H9^kЖ34561"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H8^kЖ34560"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H7^kЖ34559"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H6^kЖ34113"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H5^kЖ33905"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H4^kЖ33904"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H3^kЖ33903"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H2^kЖ33902"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H12^kЖ35015"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H11^kЖ35014"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H10^kЖ35013"));
            result.Fields.Add(RecordField.Parse(909, "^Q1970^DФП^H1^kЖ33901"));
            result.Fields.Add(RecordField.Parse(905, "^D3^F2^S1^21"));

            return result;
        }

        [TestMethod]
        public void MagazineInfo_Construction_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            Assert.IsNull(magazine.Index);
            Assert.IsNull(magazine.Description);
            Assert.IsNull(magazine.Title);
            Assert.IsNull(magazine.SubTitle);
            Assert.IsNull(magazine.SeriesNumber);
            Assert.IsNull(magazine.SeriesTitle);
            Assert.IsNull(magazine.MagazineType);
            Assert.IsNull(magazine.MagazineKind);
            Assert.IsNull(magazine.Periodicity);
            Assert.IsNull(magazine.Cumulation);
            Assert.AreEqual(0, magazine.Mfn);
            Assert.IsNull(magazine.UserData);
        }

        public void MagazineInfo_Parse_1()
        {
            MagazineInfo magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("", magazine.Title);
            Assert.IsNotNull(magazine.Cumulation);
            Assert.AreEqual(0, magazine.Cumulation.Length);
        }

        private void _TestSerialization
            (
                [NotNull] MagazineInfo first
            )
        {
            byte[] bytes = first.SaveToMemory();
            MagazineInfo second = bytes.RestoreObjectFromMemory<MagazineInfo>();
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.SubTitle, second.SubTitle);
            Assert.AreEqual(first.SeriesNumber, second.SeriesNumber);
            Assert.AreEqual(first.SeriesTitle, second.SeriesTitle);
            Assert.AreEqual(first.MagazineType, second.MagazineType);
            Assert.AreEqual(first.MagazineKind, second.MagazineKind);
            Assert.AreEqual(first.Periodicity, second.Periodicity);
            Assert.AreEqual(first.Mfn, second.Mfn);
        }

        [TestMethod]
        public void MagazineInfo_Serialization_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            _TestSerialization(magazine);

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            _TestSerialization(magazine);
        }

        [TestMethod]
        public void MagazineInfo_ToXml_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            Assert.AreEqual("<magazine />", XmlUtility.SerializeShort(magazine));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("<magazine index=\"З596747388\" title=\"Звезда Востока\" sub-title=\"литературно-художественный и общественно-политический журнал\" magazine-type=\"a\" magazine-kind=\"a\" periodicity=\"12\"><cumulation year=\"1981\" place=\"ФП\" numbers=\"1\" set=\"Ж54482\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"9\" set=\"Ж54047\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"6\" set=\"Ж53859\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"5\" set=\"Ж53858\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"4\" set=\"Ж53857\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"3\" set=\"Ж53856\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"2\" set=\"Ж53855\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"12\" set=\"Ж54481\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"11\" set=\"Ж54480\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"10\" set=\"Ж57709\" /><cumulation year=\"1980\" place=\"ФП\" numbers=\"1\" set=\"Ж53854\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"9\" set=\"Ж51539\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"8\" set=\"Ж51538\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"7\" set=\"Ж51537\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"6\" set=\"Ж51536\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"5\" set=\"Ж51419\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"4\" set=\"Ж51418\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"3\" set=\"Ж51233\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"2\" set=\"Ж51232\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"12\" set=\"Ж53449\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"11\" set=\"Ж51671\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"10\" set=\"Ж51670\" /><cumulation year=\"1979\" place=\"ФП\" numbers=\"1\" set=\"Ж50887\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"9\" set=\"Ж50377\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"7\" set=\"Ж50215\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"6\" set=\"Ж50214\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"3\" set=\"Ж49588\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"2\" set=\"Ж48791\" /><cumulation year=\"1978\" place=\"ФП\" numbers=\"10\" set=\"Ж50546\" /><cumulation year=\"1977\" numbers=\"5\" set=\"Ж48224\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"9\" set=\"Ж48492\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"8\" set=\"Ж48491\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"7\" set=\"Ж48490\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"6\" set=\"Ж48225\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"4\" set=\"Ж48223\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"3\" set=\"Ж48015\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"2\" set=\"Ж48014\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"12\" set=\"Ж48789\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"11\" set=\"Ж48612\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"10\" set=\"Ж48611\" /><cumulation year=\"1977\" place=\"ФП\" numbers=\"1\" set=\"1\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"9-11\" set=\"1\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"8\" set=\"Ж45888\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"7\" set=\"Ж45887\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"6\" set=\"Ж45886\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"5\" set=\"Ж45460\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"4\" set=\"Ж45459\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"3\" set=\"Ж45458\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"2\" set=\"Ж44877\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"12\" set=\"Ж46188\" /><cumulation year=\"1976\" place=\"ФП\" numbers=\"1\" set=\"Ж44876\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"9\" set=\"1\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"8\" set=\"Ж44441\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"7\" set=\"Ж44440\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"6\" set=\"Ж44275\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"5\" set=\"Ж44274\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"4\" set=\"Ж44207\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"3\" set=\"Ж43571\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"2\" set=\"Ж43131\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"12\" set=\"Ж44755\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"11\" set=\"Ж44532\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"10\" set=\"Ж44531\" /><cumulation year=\"1975\" place=\"ФП\" numbers=\"1\" set=\"Ж43130\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"9\" set=\"Ж41274\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"7\" set=\"Ж49587\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"6\" set=\"Ж40996\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"4\" set=\"Ж40994\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"3\" set=\"Ж40993\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"2\" set=\"Ж40992\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"12\" set=\"Ж42033\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"11\" set=\"Ж42815\" /><cumulation year=\"1974\" place=\"ФП\" numbers=\"10\" set=\"Ж41873\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"9\" set=\"Ж39254\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"8\" set=\"Ж39253\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"7\" set=\"Ж39252\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"6\" set=\"Ж39251\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"5\" set=\"Ж38934\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"4\" set=\"Ж38933\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"3\" set=\"Ж38932\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"2\" set=\"Ж37750\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"12\" set=\"Ж39429\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"11\" set=\"Ж39428\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"10\" set=\"Ж39466\" /><cumulation year=\"1973\" place=\"ФП\" numbers=\"1\" set=\"Ж37749\" /><cumulation year=\"1972\" numbers=\"5\" set=\"Ж37003\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"9\" set=\"Ж37466\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"6\" set=\"Ж37004\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"4\" set=\"Ж36854\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"3\" set=\"Ж36853\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"2\" set=\"Ж36852\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"12\" set=\"Ж37748\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"11\" set=\"Ж37469\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"10\" set=\"Ж37467\" /><cumulation year=\"1972\" place=\"ФП\" numbers=\"1\" set=\"Ж36851\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"9\" set=\"Ж36200\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"8\" set=\"Ж36087\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"7\" set=\"Ж36086\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"5\" set=\"Ж35250\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"4\" set=\"Ж35163\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"3\" set=\"Ж35018\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"2\" set=\"Ж35017\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"12\" set=\"Ж36203\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"11\" set=\"Ж36202\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"10\" set=\"Ж36201\" /><cumulation year=\"1971\" place=\"ФП\" numbers=\"1\" set=\"Ж35016\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"9\" set=\"Ж34561\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"8\" set=\"Ж34560\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"7\" set=\"Ж34559\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"6\" set=\"Ж34113\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"5\" set=\"Ж33905\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"4\" set=\"Ж33904\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"3\" set=\"Ж33903\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"2\" set=\"Ж33902\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"12\" set=\"Ж35015\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"11\" set=\"Ж35014\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"10\" set=\"Ж35013\" /><cumulation year=\"1970\" place=\"ФП\" numbers=\"1\" set=\"Ж33901\" /></magazine>", XmlUtility.SerializeShort(magazine));
        }

        [TestMethod]
        public void MagazineInfo_ToJson_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(magazine));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("{'index':'З596747388','title':'Звезда Востока','sub-title':'литературно-художественный и общественно-политический журнал','magazine-type':'a','magazine-kind':'a','periodicity':'12','cumulation':[{'year':'1981','place':'ФП','numbers':'1','set':'Ж54482'},{'year':'1980','place':'ФП','numbers':'9','set':'Ж54047'},{'year':'1980','place':'ФП','numbers':'6','set':'Ж53859'},{'year':'1980','place':'ФП','numbers':'5','set':'Ж53858'},{'year':'1980','place':'ФП','numbers':'4','set':'Ж53857'},{'year':'1980','place':'ФП','numbers':'3','set':'Ж53856'},{'year':'1980','place':'ФП','numbers':'2','set':'Ж53855'},{'year':'1980','place':'ФП','numbers':'12','set':'Ж54481'},{'year':'1980','place':'ФП','numbers':'11','set':'Ж54480'},{'year':'1980','place':'ФП','numbers':'10','set':'Ж57709'},{'year':'1980','place':'ФП','numbers':'1','set':'Ж53854'},{'year':'1979','place':'ФП','numbers':'9','set':'Ж51539'},{'year':'1979','place':'ФП','numbers':'8','set':'Ж51538'},{'year':'1979','place':'ФП','numbers':'7','set':'Ж51537'},{'year':'1979','place':'ФП','numbers':'6','set':'Ж51536'},{'year':'1979','place':'ФП','numbers':'5','set':'Ж51419'},{'year':'1979','place':'ФП','numbers':'4','set':'Ж51418'},{'year':'1979','place':'ФП','numbers':'3','set':'Ж51233'},{'year':'1979','place':'ФП','numbers':'2','set':'Ж51232'},{'year':'1979','place':'ФП','numbers':'12','set':'Ж53449'},{'year':'1979','place':'ФП','numbers':'11','set':'Ж51671'},{'year':'1979','place':'ФП','numbers':'10','set':'Ж51670'},{'year':'1979','place':'ФП','numbers':'1','set':'Ж50887'},{'year':'1978','place':'ФП','numbers':'9','set':'Ж50377'},{'year':'1978','place':'ФП','numbers':'7','set':'Ж50215'},{'year':'1978','place':'ФП','numbers':'6','set':'Ж50214'},{'year':'1978','place':'ФП','numbers':'3','set':'Ж49588'},{'year':'1978','place':'ФП','numbers':'2','set':'Ж48791'},{'year':'1978','place':'ФП','numbers':'10','set':'Ж50546'},{'year':'1977','numbers':'5','set':'Ж48224'},{'year':'1977','place':'ФП','numbers':'9','set':'Ж48492'},{'year':'1977','place':'ФП','numbers':'8','set':'Ж48491'},{'year':'1977','place':'ФП','numbers':'7','set':'Ж48490'},{'year':'1977','place':'ФП','numbers':'6','set':'Ж48225'},{'year':'1977','place':'ФП','numbers':'4','set':'Ж48223'},{'year':'1977','place':'ФП','numbers':'3','set':'Ж48015'},{'year':'1977','place':'ФП','numbers':'2','set':'Ж48014'},{'year':'1977','place':'ФП','numbers':'12','set':'Ж48789'},{'year':'1977','place':'ФП','numbers':'11','set':'Ж48612'},{'year':'1977','place':'ФП','numbers':'10','set':'Ж48611'},{'year':'1977','place':'ФП','numbers':'1','set':'1'},{'year':'1976','place':'ФП','numbers':'9-11','set':'1'},{'year':'1976','place':'ФП','numbers':'8','set':'Ж45888'},{'year':'1976','place':'ФП','numbers':'7','set':'Ж45887'},{'year':'1976','place':'ФП','numbers':'6','set':'Ж45886'},{'year':'1976','place':'ФП','numbers':'5','set':'Ж45460'},{'year':'1976','place':'ФП','numbers':'4','set':'Ж45459'},{'year':'1976','place':'ФП','numbers':'3','set':'Ж45458'},{'year':'1976','place':'ФП','numbers':'2','set':'Ж44877'},{'year':'1976','place':'ФП','numbers':'12','set':'Ж46188'},{'year':'1976','place':'ФП','numbers':'1','set':'Ж44876'},{'year':'1975','place':'ФП','numbers':'9','set':'1'},{'year':'1975','place':'ФП','numbers':'8','set':'Ж44441'},{'year':'1975','place':'ФП','numbers':'7','set':'Ж44440'},{'year':'1975','place':'ФП','numbers':'6','set':'Ж44275'},{'year':'1975','place':'ФП','numbers':'5','set':'Ж44274'},{'year':'1975','place':'ФП','numbers':'4','set':'Ж44207'},{'year':'1975','place':'ФП','numbers':'3','set':'Ж43571'},{'year':'1975','place':'ФП','numbers':'2','set':'Ж43131'},{'year':'1975','place':'ФП','numbers':'12','set':'Ж44755'},{'year':'1975','place':'ФП','numbers':'11','set':'Ж44532'},{'year':'1975','place':'ФП','numbers':'10','set':'Ж44531'},{'year':'1975','place':'ФП','numbers':'1','set':'Ж43130'},{'year':'1974','place':'ФП','numbers':'9','set':'Ж41274'},{'year':'1974','place':'ФП','numbers':'7','set':'Ж49587'},{'year':'1974','place':'ФП','numbers':'6','set':'Ж40996'},{'year':'1974','place':'ФП','numbers':'4','set':'Ж40994'},{'year':'1974','place':'ФП','numbers':'3','set':'Ж40993'},{'year':'1974','place':'ФП','numbers':'2','set':'Ж40992'},{'year':'1974','place':'ФП','numbers':'12','set':'Ж42033'},{'year':'1974','place':'ФП','numbers':'11','set':'Ж42815'},{'year':'1974','place':'ФП','numbers':'10','set':'Ж41873'},{'year':'1973','place':'ФП','numbers':'9','set':'Ж39254'},{'year':'1973','place':'ФП','numbers':'8','set':'Ж39253'},{'year':'1973','place':'ФП','numbers':'7','set':'Ж39252'},{'year':'1973','place':'ФП','numbers':'6','set':'Ж39251'},{'year':'1973','place':'ФП','numbers':'5','set':'Ж38934'},{'year':'1973','place':'ФП','numbers':'4','set':'Ж38933'},{'year':'1973','place':'ФП','numbers':'3','set':'Ж38932'},{'year':'1973','place':'ФП','numbers':'2','set':'Ж37750'},{'year':'1973','place':'ФП','numbers':'12','set':'Ж39429'},{'year':'1973','place':'ФП','numbers':'11','set':'Ж39428'},{'year':'1973','place':'ФП','numbers':'10','set':'Ж39466'},{'year':'1973','place':'ФП','numbers':'1','set':'Ж37749'},{'year':'1972','numbers':'5','set':'Ж37003'},{'year':'1972','place':'ФП','numbers':'9','set':'Ж37466'},{'year':'1972','place':'ФП','numbers':'6','set':'Ж37004'},{'year':'1972','place':'ФП','numbers':'4','set':'Ж36854'},{'year':'1972','place':'ФП','numbers':'3','set':'Ж36853'},{'year':'1972','place':'ФП','numbers':'2','set':'Ж36852'},{'year':'1972','place':'ФП','numbers':'12','set':'Ж37748'},{'year':'1972','place':'ФП','numbers':'11','set':'Ж37469'},{'year':'1972','place':'ФП','numbers':'10','set':'Ж37467'},{'year':'1972','place':'ФП','numbers':'1','set':'Ж36851'},{'year':'1971','place':'ФП','numbers':'9','set':'Ж36200'},{'year':'1971','place':'ФП','numbers':'8','set':'Ж36087'},{'year':'1971','place':'ФП','numbers':'7','set':'Ж36086'},{'year':'1971','place':'ФП','numbers':'5','set':'Ж35250'},{'year':'1971','place':'ФП','numbers':'4','set':'Ж35163'},{'year':'1971','place':'ФП','numbers':'3','set':'Ж35018'},{'year':'1971','place':'ФП','numbers':'2','set':'Ж35017'},{'year':'1971','place':'ФП','numbers':'12','set':'Ж36203'},{'year':'1971','place':'ФП','numbers':'11','set':'Ж36202'},{'year':'1971','place':'ФП','numbers':'10','set':'Ж36201'},{'year':'1971','place':'ФП','numbers':'1','set':'Ж35016'},{'year':'1970','place':'ФП','numbers':'9','set':'Ж34561'},{'year':'1970','place':'ФП','numbers':'8','set':'Ж34560'},{'year':'1970','place':'ФП','numbers':'7','set':'Ж34559'},{'year':'1970','place':'ФП','numbers':'6','set':'Ж34113'},{'year':'1970','place':'ФП','numbers':'5','set':'Ж33905'},{'year':'1970','place':'ФП','numbers':'4','set':'Ж33904'},{'year':'1970','place':'ФП','numbers':'3','set':'Ж33903'},{'year':'1970','place':'ФП','numbers':'2','set':'Ж33902'},{'year':'1970','place':'ФП','numbers':'12','set':'Ж35015'},{'year':'1970','place':'ФП','numbers':'11','set':'Ж35014'},{'year':'1970','place':'ФП','numbers':'10','set':'Ж35013'},{'year':'1970','place':'ФП','numbers':'1','set':'Ж33901'}]}", JsonUtility.SerializeShort(magazine));
        }

        [TestMethod]
        public void MagazineInfo_ToString_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            Assert.AreEqual("", magazine.ToString());

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("Звезда Востока: литературно-художественный и общественно-политический журнал", magazine.ToString());
        }

        [TestMethod]
        public void MagazineInfo_Verify_1()
        {
            MagazineInfo magazine = new MagazineInfo();
            Assert.IsFalse(magazine.Verify(false));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.IsTrue(magazine.Verify(false));
        }
    }
}
