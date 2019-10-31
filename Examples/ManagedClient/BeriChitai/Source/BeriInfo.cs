// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BeriInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

#endregion

public class BeriInfo
{
    #region Constants


    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcde";

    /// <summary>
    /// Тег полей.
    /// </summary>
    public const int BeriTag = 9190;

    #endregion

    #region Properties

    /// <summary>
    /// Дата бронирования, подполе a.
    /// </summary>
    [CanBeNull]
    [SubField('a')]
    [XmlAttribute("booking-date")]
    [JsonProperty("bookingDate")]
    public string BookingDate { get; set; }

    /// <summary>
    /// Читательский билет, подполе b.
    /// </summary>
    [CanBeNull]
    [SubField('b')]
    [XmlAttribute("ticket")]
    [JsonProperty("ticket")]
    public string Ticket { get; set; }

    /// <summary>
    /// Дата выдачи, подполе c.
    /// </summary>
    [CanBeNull]
    [SubField('c')]
    [XmlAttribute("issue-date")]
    [JsonProperty("issueDate")]
    public string IssueDate { get; set; }

    /// <summary>
    /// Примечания в произвольной форме, подполе d.
    /// </summary>
    [CanBeNull]
    [SubField('d')]
    [XmlAttribute("notes")]
    [JsonProperty("notes")]
    public string Notes { get; set; }

    /// <summary>
    /// Населенный пункт, подполе e.
    /// </summary>
    [CanBeNull]
    [SubField('e')]
    [XmlAttribute("locality")]
    [JsonProperty("locality")]
    public string Locality { get; set; }

    /// <summary>
    /// Шифр в базе. Поле 903.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public string Index { get; set; }

    /// <summary>
    /// Биб. описание.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public string Description { get; set; }

    public string ShortDescription
    {
        get
        {
            string date = IrbisDate.ConvertStringToDate(BookingDate)
                .ToString("dd MMM yyyy");
            string result = $"№ {Index}: {date}";

            return result;
        }
    }

    /// <summary>
    /// Соответствующая запись.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public MarcRecord Record { get; set; }

    /// <summary>
    /// Соответствующее поле записи.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public RecordField Field { get; set; }

    /// <summary>
    /// Читатель, заказавший книгу.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public ReaderInfo Reader { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [CanBeNull]
    [XmlIgnore]
    [JsonIgnore]
    public object UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение к указанному полю.
    /// </summary>
    public void ApplyToField
        (
            [NotNull] RecordField field
        )
    {
        Code.NotNull(field, "field");

        field
            .ApplySubField('a', BookingDate)
            .ApplySubField('b', Ticket)
            .ApplySubField('c', IssueDate)
            .ApplySubField('d', Notes)
            .ApplySubField('e', Locality);
    }

    /// <summary>
    /// Разбор поля.
    /// </summary>
    [NotNull]
    public static BeriInfo Parse
    (
        [NotNull] RecordField field
    )
    {
        Code.NotNull(field, "field");

        BeriInfo result = new BeriInfo
        {
            BookingDate = field.GetFirstSubFieldValue('a'),
            Ticket = field.GetFirstSubFieldValue('b'),
            IssueDate = field.GetFirstSubFieldValue('c'),
            Notes = field.GetFirstSubFieldValue('d'),
            Locality = field.GetFirstSubFieldValue('e'),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор записи.
    /// </summary>
    [NotNull]
    [ItemNotNull]
    public static BeriInfo[] Parse
        (
            [NotNull] MarcRecord record
        )
    {
        Code.NotNull(record, "record");

        LocalList<BeriInfo> result = new LocalList<BeriInfo>();
        foreach (RecordField field in record.Fields.GetField(BeriTag))
        {
            BeriInfo info = Parse(field);
            info.Record = record;
            info.Index = record.FM(903);
            result.Add(info);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Превращение обратно в поле.
    /// </summary>
    [NotNull]
    public RecordField ToField()
    {
        RecordField result = new RecordField(BeriTag)
            .AddNonEmptySubField('a', BookingDate)
            .AddNonEmptySubField('b', Ticket)
            .AddNonEmptySubField('c', IssueDate)
            .AddNonEmptySubField('d', Notes)
            .AddNonEmptySubField('e', Locality);

        return result;
    }

    #endregion
}
