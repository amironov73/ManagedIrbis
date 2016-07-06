/* RevisionInfo.cs -- данные о редактировании записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using BLToolkit.Mapping;

using CodeJam;

using JetBrains.Annotations;

using ManagedClient.Mapping;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Fields
{
    /// <summary>
    /// Данные о редактировании записи (поле 907).
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    [DebuggerDisplay("Stage={Stage} Date={Date} Name={Name}")]
    public sealed class RevisionInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const string Tag = "907";

        #endregion

        #region Properties

        /// <summary>
        /// Этап работы. Подполе c.
        /// </summary>
        [CanBeNull]
        [SubField('c')]
        [XmlAttribute("stage")]
        [JsonProperty("stage")]
        [MapField("stage")]
        public string Stage { get; set; }

        /// <summary>
        /// Дата. Подполе a.
        /// </summary>
        [CanBeNull]
        [SubField('a')]
        [XmlAttribute("date")]
        [JsonProperty("date")]
        [MapField("date")]
        public string Date { get; set; }

        /// <summary>
        /// ФИО оператора. Подполе b.
        /// </summary>
        [CanBeNull]
        [SubField('b')]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        [MapField("name")]
        public string Name { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
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

        [NonSerialized]
        private object _userData;

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public static RevisionInfo Parse
            (
                [JetBrains.Annotations.NotNull] RecordField field
            )
        {
            RevisionInfo result = new RevisionInfo
                {
                    Date = field.GetFirstSubFieldValue('a'),
                    Name = field.GetFirstSubFieldValue('b'),
                    Stage = field.GetFirstSubFieldValue('c')
                };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        [ItemNotNull]
        public static RevisionInfo[] Parse
            (
                [JetBrains.Annotations.NotNull] IrbisRecord record,
                string tag
            )
        {
            Code.NotNull(record, "record");
            Code.NotNullNorEmpty(tag, "tag");


            return record.Fields
                .GetField(tag)
                .Select(Parse)
                .ToArray();
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        [ItemNotNull]
        public static RevisionInfo[] Parse
            (
                [JetBrains.Annotations.NotNull] IrbisRecord record
            )
        {
            return Parse
                (
                    record,
                    Tag
                );
        }

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        [JetBrains.Annotations.NotNull]
        public RecordField ToField()
        {
            RecordField result = new RecordField("907")
                .AddNonEmptySubField('a', Date)
                .AddNonEmptySubField('b', Name)
                .AddNonEmptySubField('c', Stage);
            
            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Stage = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            Name = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Stage)
                .WriteNullable(Date)
                .WriteNullable(Name);
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
            return string.Format
                (
                    "Stage: {0}, Date: {1}, Name: {2}",
                    Stage,
                    Date,
                    Name
                );
        }

        #endregion
    }
}
