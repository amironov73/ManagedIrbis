// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisEncoder.cs -- encodes/decodes records from Irbis64
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using AM;

using ManagedIrbis;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    /// Кодирование/декодирование записей в клиентском
    /// представлении Ирбис 64.
    /// </summary>
    public class IrbisEncoder
    {
        #region Public methods

        /// <summary>
        /// Декодирование записи из клиентского представления.
        /// </summary>
        /// <param name="text">Клиентское представление записи.
        /// </param>
        /// <returns>Декодированная запись.</returns>
        public MarcRecord DecodeRecord
            (
                string text
            )
        {
            // MarcLabel.AllowExceptionsOnValidate = false;

            var result = new MarcRecord();
            string[] lines = text.SplitLines();

            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                RecordField field = ParseLine(line);
                result.Fields.Add(field);
            }

            return result;
        }

        /// <summary>
        /// Decodes the record info.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public Irbis64RecordInfo DecodeRecordInfo(string text)
        {
            Irbis64RecordInfo result = new Irbis64RecordInfo();
            Regex regex = new Regex
                (
                    @"^(?<zero1>\d+)#(?<retcode>\d+)
(?<mfn>\d+)#(?<status>\d+)
(?<zero2>\d+)#(?<version>\d+)",
                              RegexOptions.IgnoreCase
                );
            Match match = regex.Match(text);
            if (match.Success)
            {
                result.ReturnCode = int.Parse
                    (
                        match.Groups["retcode"].Value
                    );
                result.Mfn = int.Parse
                    (
                        match.Groups["mfn"].Value
                    );
                result.Status = int.Parse
                    (
                        match.Groups["status"].Value
                    );
                result.Version = int.Parse
                    (
                        match.Groups["version"].Value
                    );
            }

            return result;
        }

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        /// <param name="record">Запись для кодирования.</param>
        /// <param name="returnCode">Код возврата (чаще всего 0).</param>
        /// <param name="mfn">MFN записи (м. б. несуществующий).</param>
        /// <param name="status">The status.</param>
        /// <param name="version">Версия записи (чаще всего 1).</param>
        /// <returns>
        /// Закодированная запись.
        /// </returns>
        public string EncodeRecord
            (
            MarcRecord record,
            int returnCode,
            int mfn,
            int status,
            int version
            )
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine
                (
                    string.Format
                        (
                            "0#{0}",
                            returnCode
                        )
                );
            result.AppendLine
                (
                    string.Format
                        (
                            "{0}#{1}",
                            mfn,
                            status
                        ));
            result.AppendLine
                (
                    string.Format
                        (
                            "0#{0}",
                            version
                        )
                );

            foreach (RecordField field in record.Fields)
            {
                EncodeField
                    (
                        result,
                        field
                    );
            }

            return result.ToString();
        }

        #endregion

        #region Utilities

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="subField"></param>
        public static void EncodeSubField
            (
                StringBuilder builder,
                SubField subField
            )
        {
            builder.AppendFormat
                (
                    "^{0}{1}",
                    subField.Code,
                    subField.Value
                );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="field"></param>
        public static void EncodeField
            (
                StringBuilder builder,
                RecordField field
            )
        {
            builder.AppendFormat
                (
                    "{0}#",
                    field.Tag
                );

            if (!string.IsNullOrEmpty(field.Value))
            {
                builder.Append(field.Value);
            }

            foreach (SubField subField in field.SubFields)
            {
                EncodeSubField
                    (
                        builder,
                        subField
                    );
            }
            builder.AppendLine();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="field"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public static void AddSubField
            (
                RecordField field,
                char code,
                StringBuilder value
            )
        {
            if (code != 0)
            {
                SubField subField;
                if (value.Length == 0)
                {
                    subField = new SubField(code);
                }
                else
                {
                    subField = new SubField
                        (
                            code,
                            value.ToString()
                        );
                }
                field.SubFields.Add(subField);
            }
            value.Length = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static RecordField ParseLine(string line)
        {
            string[] parts = line.Split
                (
                    new[] { '#' },
                    2
                );
            string tag = parts[0];
            string body = parts[1];

            var result = new RecordField(tag);

            int first = body.IndexOf('^');
            if (first != 0)
            {
                if (first < 0)
                {
                    result.Value = body;
                    body = string.Empty;
                }
                else
                {
                    result.Value = body.Substring
                        (
                            0,
                            first
                        );
                    body = body.Substring(first);
                }
            }

            var code = (char)0;
            var value = new StringBuilder();
            foreach (char c in body)
            {
                if (c == '^')
                {
                    AddSubField
                        (
                            result,
                            code,
                            value
                        );
                    code = (char)0;
                }
                else
                {
                    if (code == 0)
                    {
                        code = c;
                    }
                    else
                    {
                        value.Append(c);
                    }
                }
            }

            AddSubField
                (
                    result,
                    code,
                    value
                );

            return result;
        }

        /// <summary>
        /// Encodes the date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static string EncodeDate(DateTime date)
        {
            string result = date.ToString
                (
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture
                );
            return result;
        }

        /// <summary>
        /// Encodes the currency.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        public static string EncodeCurrency(Decimal amount)
        {
            string result = amount.ToString
                (
                    "0.00",
                    CultureInfo.InvariantCulture
                );
            return result;
        }

        #endregion

        /*

         Формат клиентского представления записи:

         0#код возврата
         MFN#статус записи
         0#номер версии записи
         далее следуют строки вида:
         TAG#значение поля
         где TAG – числовая метка поля

         */

        /*

        Пример записи в клиентском представлении Ирбис 64:

0#0
1#0
0#5
692#^B2007/2008^CV^D17^X!COM^G20080501
692#^B2007/2008^CV^X!NOFOND^D42^S10.50^G20080501
692#^B2007/2008^CO^D42^E4^N10.50^G20080107
692#^B2007/2008^CO^D42^E4^Z10.50^G20080107
692#^B2008/2009^CO^D17^X!COM^G20081218
692#^B2008/2009^CO^X!NOFOND^D42^E3^N14.00^G20081218
692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218
692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107
692#^B2008/2009^CV^AЗИ^D25^E0^F0^S0^G20090830
692#^B2008/2009^CV^D17^X!COM^K21^E0^M0^G20090830
692#^B2008/2009^CV^X!NOFOND^D42^K46^E0^N0^S14.00^G20090830
20#^0 ^! ^b93-1141
102#RU
10#^a5-7110-0177-9^d300
675#37
675#37(470.311)(03)
964#14
999#0000002
920#PAZK
210#^aМ.^cСП "Вся Москва"^d1993
215#^A240^Cил^D12^YДА^Z3
225#^u2^aВся Москва
101#rus
908#К88
903#37/К88-602720
690#^L1.12
700#^AАкулова^BЗ.М.^PНИИ ВК
900#^B05^Cg^227
702#^4340 ред.^AПавловский^BА.С.
702#^4340 ред.^AПанасенко^BВ.А.
702#^4340 ред.^AПанков^BИ.
702#^4340 ред.^AПетрова^BН.Б.
941#^A0^B32^H107206G^C19930907^DБИНТ^U2004/7^E300
941#^A0^B33^H107216G^C19930907^DБИНТ^U2004/7^E2400
907#^A20020530^BДСМ
907#^CПРФ^A20060601^BДСМ
907#^C^A20060601^BДСМ
907#^C^A20070109^B
907#^C^A20020530^B
907#^C^A20030129^B
907#^C^A20050524^B
907#^C^A20050525^B
907#^C^A20051110^B
907#^C^A20070207^B
907#^A20071108^BОЛН^C
907#^A20071226^BОЛН^C
907#^A20080107^B^C
907#^A20081101^BОЛН^C
907#^A20080501^BОЛН^C
907#^A20081218^BОЛН^C
907#^CКТ^A20090108^B
907#^A20090830^BОЛН^C
907#^A20090909^BОЛН^C
692#^B2007/2008^CV^AЗИ^D25^S0.00^G20080501
692#^B2009/2010^CO^AЗИ^D25^E4^F6.25^G20100511
692#^B2009/2010^CO^D17^X!COM^K21^E0^M0^G20100511
692#^B2009/2010^CO^X!NOFOND^D42^K46^E4^N11.50^G20100511
907#^A20100511^BОЛН^C
701#^AБабич^BА.М.^U2
200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]
907#^CКТ^A20100527^B1
910#^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60
910#^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7
910#^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7
910#^A0^B559^C19990924^DЧЗ^H107256G^=2^U2004/7
910#^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР
910#^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР
910#^AU^BЗИ-1^C20071226^DЖГ^125^TЗИ
905#^F2^21
693#^B2010/2011^CO^AЗИ^D25^E4^F6.25
693#^B2010/2011^CO^D17^X!COM^K21^E0^M0
693#^B2010/2011^CO^X!NOFOND^D42^K46^E4^N11.50
691#^! 3^IАКТ^DАктинометрия^SОПД^BФЭиОЭП^KУМО^AЗИ^Vспц^Oд/о^C310700^F3^WАКТ/3^GОсн^0ЗИ310700спцд/о-S3
907#^A20100908^B1^C

         */
    }
}
