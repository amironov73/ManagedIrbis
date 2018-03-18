// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblParameter.cs -- parameter for GBL file
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    //
    // Первая строка файла задания – это число, задающее
    // количество параметров, используемых в операторах корректировки.
    // 
    // Последующие пары строк, число пар должно быть равно
    // количеству параметров, используются программой
    // глобальной корректировки.
    // 
    // Первая строка пары - значение параметра или пусто,
    // если пользователю предлагается задать его значение
    // перед выполнением корректировки. В этой строке можно
    // задать имя файла меню (с расширением MNU)
    // или имя рабочего листа подполей (с расширением Wss),
    // которые будут поданы для выбора значения параметра.
    // Вторая строка пары – наименование параметра,
    // которое появится в названии столбца, задающего параметр.
    //

    /// <summary>
    /// Parameter for GBL file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("gbl-parameter")]
    public sealed class GblParameter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Parameter name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Parameter value.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        [XmlAttribute("value")]
        public string Value { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse specifiedStream.
        /// </summary>
        [NotNull]
        public static GblParameter ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            GblParameter result = new GblParameter
            {
                Value = reader.RequireLine(),
                Name = reader.RequireLine()
            };

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
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(Name);
            writer.WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<GblParameter> verifier = new Verifier<GblParameter>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNullNorEmpty(Value, "Value");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Name: {0}, Value: {1}",
                    Name,
                    Value
                );
        }

        #endregion
    }
}
