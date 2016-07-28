/* ExemplarInfo.cs -- информация об экземпляре (поле 910).
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;
using AM.Text;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using ManagedIrbis.Mapping;

#endregion

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация об экземпляре (поле 910).
    /// </summary>
    [PublicAPI]
    [TableName("exemplars")]
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
        public const string ExemplarTag = "910";

        #endregion

        #region Properties

        /// <summary>
        /// Статус. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("status")]
        [JsonProperty("status")]
        [MapField("status")]
        public string Status { get; set; }

        /// <summary>
        /// Инвентарный номер. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("number")]
        [JsonProperty("number")]
        [MapField("number")]
        public string Number { get; set; }

        /// <summary>
        /// Дата поступления. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        [MapField("date")]
        public string Date { get; set; }

        /// <summary>
        /// Место хранения. Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("place")]
        [JsonProperty("place")]
        [MapField("place")]
        public string Place { get; set; }

        /// <summary>
        /// Наименование коллекции. Подполе q.
        /// </summary>
        [SubField('q')]
        [XmlAttribute("collection")]
        [JsonProperty("collection")]
        [MapField("collection")]
        public string Collection { get; set; }

        /// <summary>
        /// Расстановочный шифр. Подполе r.
        /// </summary>
        [SubField('r')]
        [XmlAttribute("shelf-index")]
        [JsonProperty("shelf-index")]
        [MapField("shelfindex")]
        public string ShelfIndex { get; set; }

        /// <summary>
        /// Цена экземпляра. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonProperty("price")]
        [MapField("price")]
        public string Price { get; set; }

        /// <summary>
        /// Штрих-код/радиометка. Подполе h.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonProperty("barcode")]
        [MapField("barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Число экземпляров. Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("amount")]
        [JsonProperty("amount")]
        [MapField("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Специальное назначение фонда. Подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("purpose")]
        [JsonProperty("purpose")]
        [MapField("purpose")]
        public string Purpose { get; set; }

        /// <summary>
        /// Коэффициент многоразового использования. Подполе =.
        /// </summary>
        [SubField('=')]
        [XmlAttribute("coefficient")]
        [JsonProperty("coefficient")]
        [MapField("coefficient")]
        public string Coefficient { get; set; }

        /// <summary>
        /// Экземпляры не на баланс. Подполе 4.
        /// </summary>
        [SubField('4')]
        [XmlAttribute("off-balance")]
        [JsonProperty("off-balance")]
        [MapField("offbalance")]
        public string OffBalance { get; set; }

        /// <summary>
        /// Номер записи КСУ. Подполе u.
        /// </summary>
        [SubField('u')]
        [XmlAttribute("ksu-number1")]
        [JsonProperty("ksu-number1")]
        [MapField("ksunumber1")]
        public string KsuNumber1 { get; set; }

        /// <summary>
        /// Номер акта. Подполе y.
        /// </summary>
        [SubField('y')]
        [XmlAttribute("act-number1")]
        [JsonProperty("act-number1")]
        [MapField("actnumber1")]
        public string ActNumber1 { get; set; }

        /// <summary>
        /// Канал поступления. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("channel")]
        [JsonProperty("channel")]
        [MapField("channel")]
        public string Channel { get; set; }

        /// <summary>
        /// Число выданных экземпляров. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("on-hand")]
        [JsonProperty("on-hand")]
        [MapField("onhand")]
        public string OnHand { get; set; }

        /// <summary>
        /// Номер акта списания. Подполе v.
        /// </summary>
        [SubField('v')]
        [XmlAttribute("act-number2")]
        [JsonProperty("act-number2")]
        [MapField("actnumber2")]
        public string ActNumber2 { get; set; }

        /// <summary>
        /// Количество списываемых экземпляров. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("write-off")]
        [JsonProperty("write-off")]
        [MapField("writeoff")]
        public string WriteOff { get; set; }

        /// <summary>
        /// Количество экземпляров для докомплектования. Подполе k.
        /// </summary>
        [SubField('k')]
        [XmlAttribute("completion")]
        [JsonProperty("completion")]
        [MapField("completion")]
        public string Completion { get; set; }

        /// <summary>
        /// Номер акта передачи в другое подразделение. Подполе w.
        /// </summary>
        [SubField('w')]
        [XmlAttribute("act-number3")]
        [JsonProperty("act-number3")]
        [MapField("actnumber3")]
        public string ActNumber3 { get; set; }

        /// <summary>
        /// Количество передаваемых экземпляров. Подполе z.
        /// </summary>
        [SubField('z')]
        [XmlAttribute("moving")]
        [JsonProperty("moving")]
        [MapField("moving")]
        public string Moving { get; set; }

        /// <summary>
        /// Нове место хранения. Подполе m.
        /// </summary>
        [SubField('m')]
        [XmlAttribute("new-place")]
        [JsonProperty("new-place")]
        [MapField("newplace")]
        public string NewPlace { get; set; }

        /// <summary>
        /// Дата проверки фонда. Подполе s.
        /// </summary>
        [SubField('s')]
        [XmlAttribute("checked-date")]
        [JsonProperty("checked-date")]
        [MapField("checkeddate")]
        public string CheckedDate { get; set; }

        /// <summary>
        /// Число проверенных экземпляров. Подполе 0.
        /// </summary>
        [SubField('0')]
        [XmlAttribute("checked-amount")]
        [JsonProperty("checked-amount")]
        [MapField("checkedamount")]
        public string CheckedAmount { get; set; }

        /// <summary>
        /// Реальное место нахождения книги. Подполе !.
        /// </summary>
        [SubField('!')]
        [XmlAttribute("real-place")]
        [JsonProperty("real-place")]
        [MapField("realplace")]
        public string RealPlace { get; set; }

        /// <summary>
        /// Шифр подшивки. Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlAttribute("binding-index")]
        [JsonProperty("binding-index")]
        [MapField("bindingindex")]
        public string BindingIndex { get; set; }

        /// <summary>
        /// Инвентарный номер подшивки. Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlAttribute("binding-number")]
        [JsonProperty("binding-number")]
        [MapField("bindingnumber")]
        public string BindingNumber { get; set; }

        /// <summary>
        /// Год издания. Берётся не из подполя.
        /// </summary>
        [XmlAttribute("year")]
        [JsonProperty("year")]
        [MapField("year")]
        public string Year { get; set; }

        /// <summary>
        /// Прочие подполя, не попавшие в вышеперечисленные.
        /// </summary>
        [XmlElement("other-subfields")]
        [JsonProperty("other-subfields")]
        [MapIgnore]
        public SubField[] OtherSubFields { get; set; }

        /// <summary>
        /// MFN записи, из которой заимствован экземпляр.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        [MapField("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое библиографическое описание экземпляра.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description")]
        [MapField("description")]
        public string Description { get; set; }

        /// <summary>
        /// ББК.
        /// </summary>
        [XmlAttribute("bbk")]
        [JsonProperty("bbk")]
        [MapIgnore]
        public string Bbk { get; set; }

        /// <summary>
        /// Номер по порядку (для списков).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public int SequentialNumber { get; set; }

        /// <summary>
        /// Информация для упорядочения в списках.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        [CanBeNull]
        public string OrderingData { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [MapIgnore]
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Private members

        //[NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>ExemplarInfo.</returns>
        [JetBrains.Annotations.NotNull]
        public static ExemplarInfo Parse
            (
                [JetBrains.Annotations.NotNull] RecordField field
            )
        {
            if (ReferenceEquals(field, null))
            {
                throw new ArgumentNullException("field");
            }

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
                        .ToArray()
                };
            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        [ItemNotNull]
        [JetBrains.Annotations.NotNull]
        public static ExemplarInfo[] Parse
            (
                [JetBrains.Annotations.NotNull] MarcRecord record,
                [JetBrains.Annotations.NotNull] string tagNumber
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }
            if (string.IsNullOrEmpty(tagNumber))
            {
                throw new ArgumentNullException("tagNumber");
            }

            ExemplarInfo[] result = record.Fields
                .GetField(tagNumber)
                .Select(Parse)
                .ToArray();

            foreach (ExemplarInfo exemplar in result)
            {
                exemplar.Mfn = record.Mfn;
                exemplar.Description = record.Description;
            }

            return result;
        }

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        [ItemNotNull]
        [JetBrains.Annotations.NotNull]
        public static ExemplarInfo[] Parse
            (
                [JetBrains.Annotations.NotNull] MarcRecord record
            )
        {
            return Parse
                (
                    record,
                    ExemplarTag
                );
        }

        /// <summary>
        /// Преобразование экземпляра обратно в поле записи.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("910")
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
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>System.Int32.</returns>
        public static int CompareNumbers
            (
                [JetBrains.Annotations.NotNull] ExemplarInfo first,
                [JetBrains.Annotations.NotNull] ExemplarInfo second
            )
        {
            NumberText one = new NumberText(first.Number);
            NumberText two = new NumberText(second.Number);
            return one.CompareTo(two);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Read one instance from the given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
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
            OrderingData = reader.ReadNullableString();
            Mfn = reader.ReadInt32();
        }

        /// <summary>
        /// Save this instance to the given stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
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
                .WriteNullable(OrderingData);
            writer.Write(Mfn);
        }


        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
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
                result = result + " <подшивка " + BindingNumber + ">";
            }

            return result;
        }

        #endregion
    }
}
