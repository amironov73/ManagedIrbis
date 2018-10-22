// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BbkRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Запись в базе данных RSBBK.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class BbkRecord
    {
        #region Properties

        /// <summary>
        /// Вид записи.
        /// Поле 1.
        /// </summary>
        [CanBeNull]
        [Field("1")]
        public string RecordKind { get; set; }

        /// <summary>
        /// Надрубрика.
        /// Поле 2.
        /// </summary>
        [CanBeNull]
        [Field("2")]
        public string SuperHeading { get; set; }

        /// <summary>
        /// Заглавный индекс.
        /// Поле 3.
        /// </summary>
        [CanBeNull]
        [Field("3")]
        public string MainIndex { get; set; }

        /// <summary>
        /// Заглавная рубрика.
        /// Поле 4.
        /// </summary>
        [CanBeNull]
        [Field("4")]
        public string MainHeading { get; set; }

        /// <summary>
        /// Расширение заглавной рубрики.
        /// Поле 5.
        /// </summary>
        [CanBeNull]
        [Field("5")]
        public string MainExtension { get; set; }

        /// <summary>
        /// Поисковая форма заглавного индекса.
        /// Поле 6.
        /// </summary>
        [CanBeNull]
        [Field("6")]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Дата и основание исключения.
        /// Поле 7.
        /// </summary>
        [CanBeNull]
        [Field("7")]
        public string ExclusionDate { get; set; }

        /// <summary>
        /// Методические указания.
        /// Поле 8.
        /// </summary>
        [CanBeNull]
        [Field("8")]
        public string[] MethodicalInstructions { get; set; }

        /// <summary>
        /// Фасетно-методические указания.
        /// Поле 9.
        /// </summary>
        [CanBeNull]
        [Field("9")]
        public string[] FacetedInstructions { get; set; }

        /// <summary>
        /// Отсылки "Смотри".
        /// Поле 10.
        /// </summary>
        [CanBeNull]
        [Field("10")]
        public BbkReference[] SeeReferences { get; set; }

        /// <summary>
        /// Ссылки "Смотри также".
        /// Поле 11.
        /// </summary>
        [CanBeNull]
        [Field("11")]
        public BbkReference[] SeeAlsoReferences { get; set; }

        /// <summary>
        /// Раскрытие.
        /// Поле 12.
        /// </summary>
        [CanBeNull]
        [Field("12")]
        public string[] Expansion { get; set; }

        /// <summary>
        /// Смежные области.
        /// Поле 13.
        /// </summary>
        [CanBeNull]
        [Field("13")]
        public string[] AdjacentAreas { get; set; }

        /// <summary>
        /// Области применения.
        /// Поле 14.
        /// </summary>
        [CanBeNull]
        [Field("14")]
        public string[] ApplicationAreas { get; set; }

        /// <summary>
        /// Заменяющий индекс.
        /// Поле 15.
        /// </summary>
        [CanBeNull]
        [Field("15")]
        public string[] SubstituteIndex { get; set; }

        /// <summary>
        /// Номер продолжающей записи.
        /// Поле 16.
        /// </summary>
        [CanBeNull]
        [Field("16")]
        public string ContinuingRecordNumber { get; set; }

        /// <summary>
        /// Поисковый образ на ИЯ ГРНТИ.
        /// Поле 17.
        /// </summary>
        [CanBeNull]
        [Field("17")]
        public string[] Grnti { get; set; }

        /// <summary>
        /// Поисковый образ на ИЯ Дьюи.
        /// Поле 18.
        /// </summary>
        [CanBeNull]
        [Field("18")]
        public string[] Dewey { get; set; }

        /// <summary>
        /// Поисковый образ на ИЯ УДК.
        /// Поле 19.
        /// </summary>
        [CanBeNull]
        [Field("19")]
        public string[] Udc { get; set; }

        /// <summary>
        /// Поисковый образ на ИЯ МТ.
        /// Поле 20.
        /// </summary>
        [CanBeNull]
        [Field("20")]
        public string[] MT { get; set; }

        /// <summary>
        /// Дата и составитель записи.
        /// Поле 21.
        /// </summary>
        [CanBeNull]
        [Field("21")]
        public string Composition { get; set; }

        /// <summary>
        /// Дата, оператор, вид корректуры.
        /// Поле 22.
        /// </summary>
        [CanBeNull]
        [Field("22")]
        public string[] Correcture { get; set; }

        /// <summary>
        /// Рабочая схема.
        /// Поле 23.
        /// </summary>
        [CanBeNull]
        [Field("23")]
        public string[] WorkingScheme { get; set; }

        /// <summary>
        /// Обратные отсылки.
        /// Поле 24.
        /// </summary>
        [CanBeNull]
        [Field("24")]
        public BbkReference[] BackReferences { get; set; }

        /// <summary>
        /// Дефисные конструкции.
        /// Поле 505.
        /// </summary>
        [CanBeNull]
        [Field("505")]
        public string Hyphens { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the record.
        /// </summary>
        [NotNull]
        public static BbkRecord Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            BbkRecord result = new BbkRecord
            {
                RecordKind = record.FM(1),
                SuperHeading = record.FM(2),
                MainIndex = record.FM(3),
                MainHeading = record.FM(4),
                MainExtension = record.FM(5),
                SearchQuery = record.FM(6),
                ExclusionDate = record.FM(7),
                MethodicalInstructions = record.FMA(8),
                FacetedInstructions = record.FMA(9),
                SeeReferences = record.Fields
                    .GetField(10)
                    .Select(field => BbkReference.Parse(field))
                    .ToArray(),
                SeeAlsoReferences = record.Fields
                    .GetField(11)
                    .Select(field => BbkReference.Parse(field))
                    .ToArray(),
                Expansion = record.FMA(12),
                AdjacentAreas = record.FMA(13),
                ApplicationAreas = record.FMA(14),
                SubstituteIndex = record.FMA(15),
                ContinuingRecordNumber = record.FM(16),
                Grnti = record.FMA(17),
                Dewey = record.FMA(18),
                Udc = record.FMA(19),
                MT = record.FMA(20),
                Composition = record.FM(21),
                Correcture = record.FMA(22),
                WorkingScheme = record.FMA(23),
                BackReferences = record.Fields
                    .GetField(24)
                    .Select(field => BbkReference.Parse(field))
                    .ToArray(),
                Hyphens = record.FM(505)
            };

            return result;
        }

        #endregion
    }
}