/* FieldDefect.cs -- дефект в поле/подполе.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient.Quality
{
    /// <summary>
    /// Дефект в поле/подполе.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("defect")]
    [MoonSharpUserData]
    [DebuggerDisplay("Field={Field} Value={Value} Message={Message}")]
    public sealed class FieldDefect
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Поле.
        /// </summary>
        [CanBeNull]
        [JsonProperty("field")]
        [XmlAttribute("field")]
        public string Field { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [JsonProperty("field-repeat")]
        [XmlAttribute("field-repeat")]
        public int FieldRepeat { get; set; }

        /// <summary>
        /// Подполе (если есть).
        /// </summary>
        [CanBeNull]
        [JsonProperty("subfield")]
        [XmlAttribute("subfield")]
        public string Subfield { get; set; }

        /// <summary>
        /// Значение поля/подполя.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        [XmlAttribute("value")]
        public string Value { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [CanBeNull]
        [JsonProperty("message")]
        [XmlAttribute("message")]
        public string Message { get; set; }

        /// <summary>
        /// Урон от дефекта.
        /// </summary>
        [JsonProperty("damage")]
        [XmlAttribute("damage")]
        public int Damage { get; set; }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Field = reader.ReadNullableString();
            FieldRepeat = reader.ReadPackedInt32();
            Subfield = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Message = reader.ReadNullableString();
            Damage = reader.ReadPackedInt32();
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
                .WriteNullable(Field)
                .WritePackedInt32(FieldRepeat)
                .WriteNullable(Subfield)
                .WriteNullable(Value)
                .WriteNullable(Message)
                .WritePackedInt32(Damage);
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
                    "Field: {0}, Value: {1}, Message: {2}",
                    Field,
                    Value,
                    Message
               );
        }

        #endregion
    }
}
