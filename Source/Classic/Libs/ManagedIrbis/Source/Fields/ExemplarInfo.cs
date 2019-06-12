// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExemplarInfo.cs -- информация об экземпляре (поле 910).
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using ManagedIrbis.Mapping;

using Newtonsoft.Json;

#endregion

// ReSharper disable ConvertClosureToMethodGroup

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация об экземпляре (поле 910).
    /// </summary>
    [PublicAPI]
    [XmlRoot("exemplar")]
    [MoonSharpUserData]
    public sealed class ExemplarInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "!=0124abcdefhiknpqrstuvwxyz";

        /// <summary>
        /// Тег полей, содержащих сведения об экземплярах.
        /// </summary>
        public const int ExemplarTag = 910;

        #endregion

        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Статус. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Инвентарный номер. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дата поступления. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string Date { get; set; }

        /// <summary>
        /// Место хранения. Подполе d.
        /// </summary>
        [CanBeNull]
        [SubField('d')]
        [XmlAttribute("place")]
        [JsonProperty("place")]
        public string Place { get; set; }

        /// <summary>
        /// Наименование коллекции. Подполе q.
        /// </summary>
        [CanBeNull]
        [SubField('q')]
        [XmlAttribute("collection")]
        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        /// Расстановочный шифр. Подполе r.
        /// </summary>
        [CanBeNull]
        [SubField('r')]
        [XmlAttribute("shelf-index")]
        [JsonProperty("shelf-index")]
        public string ShelfIndex { get; set; }

        /// <summary>
        /// Цена экземпляра. Подполе e.
        /// </summary>
        [CanBeNull]
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonProperty("price")]
        public string Price { get; set; }

        /// <summary>
        /// Штрих-код/радиометка. Подполе h.
        /// </summary>
        [CanBeNull]
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Число экземпляров. Подполе 1.
        /// </summary>
        [CanBeNull]
        [SubField('1')]
        [XmlAttribute("amount")]
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Специальное назначение фонда. Подполе t.
        /// </summary>
        [CanBeNull]
        [SubField('t')]
        [XmlAttribute("purpose")]
        [JsonProperty("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// Коэффициент многоразового использования. Подполе =.
        /// </summary>
        [CanBeNull]
        [SubField('=')]
        [XmlAttribute("coefficient")]
        [JsonProperty("coefficient")]
        public string Coefficient { get; set; }

        /// <summary>
        /// Экземпляры не на баланс. Подполе 4.
        /// </summary>
        [CanBeNull]
        [SubField('4')]
        [XmlAttribute("off-balance")]
        [JsonProperty("off-balance")]
        public string OffBalance { get; set; }

        /// <summary>
        /// Номер записи КСУ. Подполе u.
        /// </summary>
        [CanBeNull]
        [SubField('u')]
        [XmlAttribute("ksu-number1")]
        [JsonProperty("ksu-number1")]
        public string KsuNumber1 { get; set; }

        /// <summary>
        /// Номер акта. Подполе y.
        /// </summary>
        [CanBeNull]
        [SubField('y')]
        [XmlAttribute("act-number1")]
        [JsonProperty("act-number1")]
        public string ActNumber1 { get; set; }

        /// <summary>
        /// Канал поступления. Подполе f.
        /// </summary>
        [CanBeNull]
        [SubField('f')]
        [XmlAttribute("channel")]
        [JsonProperty("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Число выданных экземпляров. Подполе 2.
        /// </summary>
        [CanBeNull]
        [SubField('2')]
        [XmlAttribute("on-hand")]
        [JsonProperty("on-hand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Номер акта списания. Подполе v.
        /// </summary>
        [CanBeNull]
        [SubField('v')]
        [XmlAttribute("act-number2")]
        [JsonProperty("act-number2")]
        public string ActNumber2 { get; set; }

        /// <summary>
        /// Количество списываемых экземпляров. Подполе x.
        /// </summary>
        [CanBeNull]
        [SubField('x')]
        [XmlAttribute("write-off")]
        [JsonProperty("write-off")]
        public string WriteOff { get; set; }

        /// <summary>
        /// Количество экземпляров для докомплектования. Подполе k.
        /// </summary>
        [CanBeNull]
        [SubField('k')]
        [XmlAttribute("completion")]
        [JsonProperty("completion")]
        public string Completion { get; set; }

        /// <summary>
        /// Номер акта передачи в другое подразделение. Подполе w.
        /// </summary>
        [CanBeNull]
        [SubField('w')]
        [XmlAttribute("act-number3")]
        [JsonProperty("act-number3")]
        public string ActNumber3 { get; set; }

        /// <summary>
        /// Количество передаваемых экземпляров. Подполе z.
        /// </summary>
        [CanBeNull]
        [SubField('z')]
        [XmlAttribute("moving")]
        [JsonProperty("moving")]
        public string Moving { get; set; }

        /// <summary>
        /// Новое место хранения. Подполе m.
        /// </summary>
        [CanBeNull]
        [SubField('m')]
        [XmlAttribute("new-place")]
        [JsonProperty("new-place")]
        public string NewPlace { get; set; }

        /// <summary>
        /// Дата проверки фонда. Подполе s.
        /// </summary>
        [CanBeNull]
        [SubField('s')]
        [XmlAttribute("checked-date")]
        [JsonProperty("checked-date")]
        public string CheckedDate { get; set; }

        /// <summary>
        /// Число проверенных экземпляров. Подполе 0.
        /// </summary>
        [CanBeNull]
        [SubField('0')]
        [XmlAttribute("checked-amount")]
        [JsonProperty("checked-amount")]
        public string CheckedAmount { get; set; }

        /// <summary>
        /// Реальное место нахождения книги. Подполе !.
        /// </summary>
        [CanBeNull]
        [SubField('!')]
        [XmlAttribute("real-place")]
        [JsonProperty("real-place")]
        public string RealPlace { get; set; }

        /// <summary>
        /// Шифр подшивки. Подполе p.
        /// </summary>
        [CanBeNull]
        [SubField('p')]
        [XmlAttribute("binding-index")]
        [JsonProperty("binding-index")]
        public string BindingIndex { get; set; }

        /// <summary>
        /// Инвентарный номер подшивки. Подполе i.
        /// </summary>
        [CanBeNull]
        [SubField('i')]
        [XmlAttribute("binding-number")]
        [JsonProperty("binding-number")]
        public string BindingNumber { get; set; }

        /// <summary>
        /// Год издания. Берётся не из подполя.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("year")]
        [JsonProperty("year")]
        public string Year { get; set; }

        /// <summary>
        /// Прочие подполя, не попавшие в вышеперечисленные.
        /// </summary>
        [CanBeNull]
        [XmlElement("other-subfields")]
        [JsonProperty("other-subfields")]
        public SubField[] OtherSubFields { get; set; }

        /// <summary>
        /// MFN записи, из которой заимствован экземпляр.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое библиографическое описание экземпляра.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// ББК.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("bbk")]
        [JsonProperty("bbk")]
        public string Bbk { get; set; }

        /// <summary>
        /// Номер выпуска (для журналов).
        /// </summary>
        [CanBeNull]
        [XmlAttribute("issue")]
        [JsonProperty("issue")]
        public string Issue { get; set; }

        /// <summary>
        /// Номер по порядку (для списков).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int SequentialNumber { get; set; }

        /// <summary>
        /// Информация для упорядочения в списках.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string OrderingData { get; set; }

        /// <summary>
        /// Флаг.
        /// </summary>
        [XmlAttribute("marked")]
        [JsonProperty("marked")]
        public bool Marked { get; set; }

        /// <summary>
        /// Record for just in case.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Associated <see cref="RecordField"/>.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public RecordField Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public object UserData { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="RecordField"/>.
        /// </summary>
        /// <param name="field"></param>
        public void ApplyToField
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            field
                .ApplySubField('a', Status)
                .ApplySubField('b', Number)
                .ApplySubField('c', Date)
                .ApplySubField('d', Place)
                .ApplySubField('q', Collection)
                .ApplySubField('r', ShelfIndex)
                .ApplySubField('e', Price)
                .ApplySubField('h', Barcode)
                .ApplySubField('1', Amount)
                .ApplySubField('t', Purpose)
                .ApplySubField('=', Coefficient)
                .ApplySubField('4', OffBalance)
                .ApplySubField('u', KsuNumber1)
                .ApplySubField('y', ActNumber1)
                .ApplySubField('f', Channel)
                .ApplySubField('2', OnHand)
                .ApplySubField('v', ActNumber2)
                .ApplySubField('x', WriteOff)
                .ApplySubField('k', Completion)
                .ApplySubField('w', ActNumber3)
                .ApplySubField('z', Moving)
                .ApplySubField('m', NewPlace)
                .ApplySubField('s', CheckedDate)
                .ApplySubField('0', CheckedAmount)
                .ApplySubField('!', RealPlace)
                .ApplySubField('p', BindingIndex)
                .ApplySubField('i', BindingNumber);
        }

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        [NotNull]
        public static ExemplarInfo Parse
            (
                [NotNull] RecordField field
            )
        {
            Code.NotNull(field, "field");

            ExemplarInfo result = new ExemplarInfo
                {
                    Status = field.GetFirstSubFieldValue('a'),
                    Number = field.GetFirstSubFieldValue('b'),
                    Date = field.GetFirstSubFieldValue('c'),
                    Place = field.GetFirstSubFieldValue('d'),
                    Collection = field.GetFirstSubFieldValue('q'),
                    ShelfIndex = field.GetFirstSubFieldValue('r'),
                    Price = field.GetFirstSubFieldValue('e'),
                    Barcode = field.GetFirstSubFieldValue('h'),
                    Amount = field.GetFirstSubFieldValue('1'),
                    Purpose = field.GetFirstSubFieldValue('t'),
                    Coefficient = field.GetFirstSubFieldValue('='),
                    OffBalance = field.GetFirstSubFieldValue('4'),
                    KsuNumber1 = field.GetFirstSubFieldValue('u'),
                    ActNumber1 = field.GetFirstSubFieldValue('y'),
                    Channel = field.GetFirstSubFieldValue('f'),
                    OnHand = field.GetFirstSubFieldValue('2'),
                    ActNumber2 = field.GetFirstSubFieldValue('v'),
                    WriteOff = field.GetFirstSubFieldValue('x'),
                    Completion = field.GetFirstSubFieldValue('k'),
                    ActNumber3 = field.GetFirstSubFieldValue('w'),
                    Moving = field.GetFirstSubFieldValue('z'),
                    NewPlace = field.GetFirstSubFieldValue('m'),
                    CheckedDate = field.GetFirstSubFieldValue('s'),
                    CheckedAmount = field.GetFirstSubFieldValue('0'),
                    RealPlace = field.GetFirstSubFieldValue('!'),
                    BindingIndex = field.GetFirstSubFieldValue('p'),
                    BindingNumber = field.GetFirstSubFieldValue('i'),
                    OtherSubFields = field.SubFields
                        .Where(sub => KnownCodes
                            .IndexOf(char.ToLower(sub.Code)) < 0)
                        .ToArray(),
                    Field = field
                };

            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ExemplarInfo[] Parse
            (
                [NotNull] MarcRecord record,
                int tagNumber
            )
        {
            Code.NotNull(record, "record");

            ExemplarInfo[] result = record.Fields
                .GetField(tagNumber)
                .Select(field => Parse(field))
                .ToArray();

            foreach (ExemplarInfo exemplar in result)
            {
                exemplar.Mfn = record.Mfn;
                exemplar.Description = record.Description;
                exemplar.Record = record;
            }

            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ExemplarInfo[] Parse
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            return Parse
                (
                    record,
                    ExemplarTag
                );
        }

        /// <summary>
        /// Should serialize <see cref="OtherSubFields"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOtherSubFields()
        {
            return !OtherSubFields.IsNullOrEmpty();
        }

        /// <summary>
        /// Преобразование экземпляра обратно в поле записи.
        /// </summary>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField(910)
                .AddNonEmptySubField('a', Status)
                .AddNonEmptySubField('b', Number)
                .AddNonEmptySubField('c', Date)
                .AddNonEmptySubField('d', Place)
                .AddNonEmptySubField('q', Collection)
                .AddNonEmptySubField('r', ShelfIndex)
                .AddNonEmptySubField('e', Price)
                .AddNonEmptySubField('h', Barcode)
                .AddNonEmptySubField('1', Amount)
                .AddNonEmptySubField('t', Purpose)
                .AddNonEmptySubField('=', Coefficient)
                .AddNonEmptySubField('4', OffBalance)
                .AddNonEmptySubField('u', KsuNumber1)
                .AddNonEmptySubField('y', ActNumber1)
                .AddNonEmptySubField('f', Channel)
                .AddNonEmptySubField('2', OnHand)
                .AddNonEmptySubField('v', ActNumber2)
                .AddNonEmptySubField('x', WriteOff)
                .AddNonEmptySubField('k', Completion)
                .AddNonEmptySubField('w', ActNumber3)
                .AddNonEmptySubField('z', Moving)
                .AddNonEmptySubField('m', NewPlace)
                .AddNonEmptySubField('s', CheckedDate)
                .AddNonEmptySubField('0', CheckedAmount)
                .AddNonEmptySubField('!', RealPlace)
                .AddNonEmptySubField('p', BindingIndex)
                .AddNonEmptySubField('i', BindingNumber);

            if (OtherSubFields != null)
            {
                foreach (SubField subField in OtherSubFields)
                {
                    result.AddSubField(subField.Code, subField.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Compares two specified numbers.
        /// </summary>
        public static int CompareNumbers
            (
                [NotNull] ExemplarInfo first,
                [NotNull] ExemplarInfo second
            )
        {
            Code.NotNull(first, "first");
            Code.NotNull(second, "second");

            NumberText one = new NumberText(first.Number);
            NumberText two = new NumberText(second.Number);

            int result = one.CompareTo(two);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Id = reader.ReadPackedInt32();
            Status = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            Collection = reader.ReadNullableString();
            ShelfIndex = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            Barcode = reader.ReadNullableString();
            Amount = reader.ReadNullableString();
            Purpose = reader.ReadNullableString();
            Coefficient = reader.ReadNullableString();
            OffBalance = reader.ReadNullableString();
            KsuNumber1 = reader.ReadNullableString();
            ActNumber1 = reader.ReadNullableString();
            Channel = reader.ReadNullableString();
            OnHand = reader.ReadNullableString();
            ActNumber2 = reader.ReadNullableString();
            WriteOff = reader.ReadNullableString();
            Completion = reader.ReadNullableString();
            ActNumber3 = reader.ReadNullableString();
            Moving = reader.ReadNullableString();
            NewPlace = reader.ReadNullableString();
            CheckedDate = reader.ReadNullableString();
            CheckedAmount = reader.ReadNullableString();
            RealPlace = reader.ReadNullableString();
            BindingIndex = reader.ReadNullableString();
            BindingNumber = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Bbk = reader.ReadNullableString();
            Issue = reader.ReadNullableString();
            OrderingData = reader.ReadNullableString();
            Mfn = reader.ReadInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Id)
                .WriteNullable(Status)
                .WriteNullable(Number)
                .WriteNullable(Date)
                .WriteNullable(Place)
                .WriteNullable(Collection)
                .WriteNullable(ShelfIndex)
                .WriteNullable(Price)
                .WriteNullable(Barcode)
                .WriteNullable(Amount)
                .WriteNullable(Purpose)
                .WriteNullable(Coefficient)
                .WriteNullable(OffBalance)
                .WriteNullable(KsuNumber1)
                .WriteNullable(ActNumber1)
                .WriteNullable(Channel)
                .WriteNullable(OnHand)
                .WriteNullable(ActNumber2)
                .WriteNullable(WriteOff)
                .WriteNullable(Completion)
                .WriteNullable(ActNumber3)
                .WriteNullable(Moving)
                .WriteNullable(NewPlace)
                .WriteNullable(CheckedDate)
                .WriteNullable(CheckedAmount)
                .WriteNullable(RealPlace)
                .WriteNullable(BindingIndex)
                .WriteNullable(BindingNumber)
                .WriteNullable(Year)
                .WriteNullable(Description)
                .WriteNullable(Bbk)
                .WriteNullable(Issue)
                .WriteNullable(OrderingData);
            writer.Write(Mfn);
        }


        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            string result = string.Format
                (
                    "{0} ({1}) [{2}]",
                    Number,
                    Place,
                    Status
                );

            if (!string.IsNullOrEmpty(BindingNumber))
            {
                result = result + " <binding " + BindingNumber + ">";
            }

            return result;
        }

        #endregion
    }
}
