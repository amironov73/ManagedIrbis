// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BookInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;
using System.Text.RegularExpressions;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure.Unifors;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Общая информация о книге.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BookInfo
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [NotNull]
        public IrbisProvider Provider { get; private set; }

        /// <summary>
        /// Record.
        /// </summary>
        [NotNull]
        public MarcRecord Record { get; private set; }

        /// <summary>
        /// Количество экземпляров.
        /// </summary>
        public int Amount
        {
            get { return _ExecuteScript(_amountScript).SafeToInt32(); }
        }

        /// <summary>
        /// Первый автор.
        /// </summary>
        [CanBeNull]
        public AuthorInfo FirstAuthor
        {
            get
            {
                AuthorInfo result = null;
                RecordField field700 = Record.Fields.GetFirstField(700);
                if (!ReferenceEquals(field700, null))
                {
                    result = AuthorInfo.ParseField700(field700);
                }

                return result;
            }
        }

        /// <summary>
        /// Авторы.
        /// </summary>
        [NotNull]
        public AuthorInfo[] Authors
        {
            get { return AuthorInfo.ParseRecord(Record, AuthorInfo.AllKnownTags); }
        }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        public string Description
        {
            get
            {
                string result = Record.Description
                                ?? _ExecuteScript(_descriptionScript);
                result = UniforPlusS.DecodeTitle("1" + result);

                return result;
            }
        }

        /// <summary>
        /// Характер документа (первый из).
        /// </summary>
        [CanBeNull]
        public string DocumentCharacter
        {
            get { return Record.FM(900, 'c'); }
        }

        /// <summary>
        /// Тип документа.
        /// </summary>
        [CanBeNull]
        public string DocumentType
        {
            get { return Record.FM(900, 't'); }
        }

        /// <summary>
        /// Электронный ресурс?
        /// </summary>
        public bool Electronic
        {
            get
            {
                // Электронными считаются:
                // 1. Те, у которых проставлен тип документа L
                // 2. Те, у которых единица измерения r, j или o
                // 3. Те, к которым прикреплен файл, и это не обложка

                string documentType = DocumentType;
                if (documentType.OneOf("l", "m"))
                {
                    return true;
                }

                char measureUnit = Record.FM(215, '1').FirstChar();
                if (measureUnit.OneOf('r', 'j', 'o'))
                {
                    return true;
                }

                RecordField[] all951 = Record.Fields.GetField(951);
                foreach (RecordField v951 in all951)
                {
                    string v951h = v951.GetFirstSubFieldValue('h');
                    if (!v951h.OneOf("02", "02a", "02b"))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Экземпляры.
        /// </summary>
        [NotNull]
        public ExemplarInfo[] Exemplars
        {
            get { return ExemplarInfo.Parse(Record); }
        }

        /// <summary>
        /// Число экземпляров.
        /// </summary>
        public int ExemplarCount
        {
            get
            {
                int result = 0;
                foreach (ExemplarInfo exemplar in Exemplars)
                {
                    string status = exemplar.Status;
                    if (status != "0"
                        && status != "1"
                        && status != "5"
                        && status != "9")
                        continue;

                    int amount = exemplar.Amount.SafeToInt32();
                    if (amount == 0)
                    {
                        amount = 1;
                    }

                    result += amount;
                }

                return result;
            }
        }

        /// <summary>
        /// Документ на иностранном языке?
        /// </summary>
        public bool Foreign
        {
            get
            {
                string[] languages = Languages;
                if (languages.Length == 0)
                {
                    return false;
                }

                return !languages[0].SameString("rus");
            }
        }

        /// <summary>
        /// Языки документа
        /// </summary>
        [NotNull]
        public string[] Languages
        {
            get { return Record.FMA(101); }
        }

        /// <summary>
        /// Первая ссылка на внешний ресурс.
        /// </summary>
        [CanBeNull]
        public string Link
        {
            get { return _ExecuteScript(_linkScript).EmptyToNull(); }
        }

        /// <summary>
        /// Количество страниц.
        /// </summary>
        public int Pages
        {
            get { return CountPages(Volume); }
        }

        /// <summary>
        /// Цена, общая для всех экземпляров.
        /// </summary>
        public decimal Price
        {
            get { return Record.FM(10, 'd').SafeToDecimal(0); }
        }

        /// <summary>
        /// Издательства.
        /// </summary>
        [NotNull]
        public string[] Publishers
        {
            get
            {
                return Record.FMA(210, 'c')
                    .Union(Record.FMA(461, 'g'))
                    .ToArray();
            }
        }

        /// <summary>
        /// Область заглавия.
        /// </summary>
        [NotNull]
        public TitleInfo Title
        {
            get
            {
                TitleInfo[] result = TitleInfo.Parse(Record);

                return result[0];
            }
        }

        /// <summary>
        /// Заголовок.
        /// </summary>
        [NotNull]
        public string TitleText
        {
            get { return _ExecuteScript(_titleScript); }
        }

        /// <summary>
        /// Счётчик выдач.
        /// </summary>
        public int UsageCount
        {
            get { return Record.FM(999).SafeToInt32(); }
        }

        /// <summary>
        /// Объем издания (цифры).
        /// </summary>
        [CanBeNull]
        public string Volume
        {
            get { return Record.FM(215, 'a'); }
        }

        /// <summary>
        /// Рабочий лист.
        /// </summary>
        [CanBeNull]
        public string Worksheet
        {
            get { return Record.FM(920); }
        }

        /// <summary>
        /// Год издания.
        /// </summary>
        public int Year
        {
            get
            {
                MarcRecord record = Record;
                string result = record.FM(210, 'd');
                if (string.IsNullOrEmpty(result))
                {
                    result = record.FM(461, 'h');
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = record.FM(461, 'z');
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = record.FM(463, 'j');
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = record.FM(934);
                }
                if (string.IsNullOrEmpty(result))
                {
                    return 0;
                }

                Match match = Regex.Match(result, @"\d{4}");
                if (match.Success)
                {
                    result = match.Value;
                }
                return result.SafeToInt32();

            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BookInfo
            (
                [NotNull] IrbisProvider provider,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNull(record, "record");

            Provider = provider;
            Record = record;
        }

        #endregion

        #region Private members

        private static string _amountScript = "f(rsum((if p(v910) then if '0159': v910^a then '1' fi, if 'CU': v910^a then v910^1 fi, ';' fi)),0,0)";

        private static string _descriptionScript = "@brief";

        private static string _linkScript = "(if p(v951^i) then if not v951^h:'02' then v951^i, break, fi, fi)";

        private static string _titleScript = "if p(v461) then v461^c, v461^2, \" : \"v461^e, '. ' fi, v200^v\". \", v200^a, v200^b, \" : \"v200^e, &uf('!')";

        [NotNull]
        private string _ExecuteScript
            (
                [NotNull] string script
            )
        {
            PftFormatter formatter = new PftFormatter();
            formatter.SetProvider(Provider);
            formatter.ParseProgram(script);
            string result = formatter.FormatRecord(Record);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Count pages.
        /// </summary>
        public static int CountPages
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int result = 0;

            MatchCollection matches = Regex.Matches
                (
                    text,
                    @"[ivxlcm]+",
                    RegexOptions.IgnoreCase
                );
            foreach (Match match in matches)
            {
                int value = UniforPlus9.ToArabicNumber(match.Value);
                result += value;
            }

            matches = Regex.Matches(text, @"[0-9]+");
            foreach (Match match in matches)
            {
                int value = FastNumber.ParseInt32(match.Value);
                result += value;
            }

            return result;
        }

        #endregion
    }
}
