// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReaderRegistration.cs -- информация о регистрации/перерегистрации читателя
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о регистрации/перерегистрации читателя.
    /// </summary>
    [MoonSharpUserData]
    [XmlRoot("registration")]
    public sealed class ReaderRegistration
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Поле регистрация.
        /// </summary>
        public const string RegistrationTag = "51";

        /// <summary>
        /// Поле "перерегистрация".
        /// </summary>
        public const string ReregistrationTag = "52";

        #endregion

        #region Properties

        /// <summary>
        /// Дата. Подполе *.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        public string DateString { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime Date
        {
            get
            {
                return IrbisDate.ConvertStringToDate(DateString);
            }
            set
            {
                DateString = IrbisDate.ConvertDateToString(value);
            }
        }

        /// <summary>
        /// Место (кафедра обслуживания).
        /// Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("chair")]
        [JsonProperty("chair")]
        public string Chair { get; set; }

        /// <summary>
        /// Номер приказа. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("order-number")]
        [JsonProperty("order-number")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Причина. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("reason")]
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Ссылка на зарегистрированного читателя.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public ReaderInfo Reader { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [NotNull]
        public static ReaderRegistration Parse
            (
                [NotNull]RecordField field
            )
        {
            // TODO Support for unknown subfields

            Code.NotNull(field, "field");

            ReaderRegistration result = new ReaderRegistration
            {
                DateString = field.Value,
                Chair = field.GetFirstSubFieldValue('c'),
                OrderNumber = field.GetFirstSubFieldValue('a'),
                Reason = field.GetFirstSubFieldValue('b')
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ReaderRegistration[] Parse
            (
                [NotNull] MarcRecord record,
                [NotNull] string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");

            ReaderRegistration[] result = record.Fields
                .GetField(tag)

#if !WINMOBILE && !PocketPC

                .Select(Parse)

#else

                .Select(field => Parse(field))

#endif

                .ToArray();

            return result;
        }

        /// <summary>
        /// Преобразование в поле.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField
                {
                    Value = DateString
                }
                .AddNonEmptySubField('c', Chair)
                .AddNonEmptySubField('a', OrderNumber)
                .AddNonEmptySubField('b', Reason);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            DateString = reader.ReadNullableString();
            Chair = reader.ReadNullableString();
            OrderNumber = reader.ReadNullableString();
            Reason = reader.ReadNullableString();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(DateString);
            writer.WriteNullable(Chair);
            writer.WriteNullable(OrderNumber);
            writer.WriteNullable(Reason);
        }

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "{0} - {1}",
                    DateString.ToVisibleString(),
                    Chair.ToVisibleString()
                );
        }

        #endregion
    }
}
