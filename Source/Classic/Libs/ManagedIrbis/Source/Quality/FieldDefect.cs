// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldDefect.cs -- detected defect of the field/subfield
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Detected defect of the field/subfield.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Field={Field} Value={Value} Message={Message}")]
    public sealed class FieldDefect
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Поле.
        /// </summary>
        [JsonProperty("field")]
        public int Field { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [JsonProperty("field-repeat")]
        public int FieldRepeat { get; set; }

        /// <summary>
        /// Подполе (если есть).
        /// </summary>
        [CanBeNull]
        [JsonProperty("subfield")]
        public string Subfield { get; set; }

        /// <summary>
        /// Значение поля/подполя.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [CanBeNull]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Урон от дефекта.
        /// </summary>
        [JsonProperty("damage")]
        public int Damage { get; set; }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Field = reader.ReadPackedInt32();
            FieldRepeat = reader.ReadPackedInt32();
            Subfield = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Message = reader.ReadNullableString();
            Damage = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Field)
                .WritePackedInt32(FieldRepeat)
                .WriteNullable(Subfield)
                .WriteNullable(Value)
                .WriteNullable(Message)
                .WritePackedInt32(Damage);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<FieldDefect> verifier
                = new Verifier<FieldDefect>(this, throwOnError);

            verifier
                .Positive(Field, "Field")
                .Assert(FieldRepeat >= 0, "FieldRepeat")
                .NotNullNorEmpty(Message, "Message")
                .Assert(Damage >= 0, "Damage");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
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
