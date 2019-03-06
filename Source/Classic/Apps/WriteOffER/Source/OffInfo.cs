// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* OffInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure.Commands;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

using CM = System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable UseNameofExpression

namespace WriteOffER
{
    /// <summary>
    /// Информация о списываемых экземплярах.
    /// </summary>
    public class OffInfo
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Инвентарный номер экземпляра или номер карточки комплектования.
        /// </summary>
        [XmlAttribute("number")]
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [XmlAttribute("description")]
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Год издания.
        /// </summary>
        [XmlAttribute("year")]
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        [XmlAttribute("price")]
        [JsonProperty("price", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Price { get; set; }

        /// <summary>
        /// Коэффициент переоценки.
        /// </summary>
        [XmlAttribute("coefficient")]
        [JsonProperty("coefficient", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Coefficient { get; set; }

        /// <summary>
        /// Количество экземпляров.
        /// </summary>
        [XmlAttribute("amount")]
        [JsonProperty("amount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Amount { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        [XmlAttribute("summa")]
        [JsonProperty("summa", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal Summa { get; set; }

        /// <summary>
        /// MFN.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Mfn { get; set; }

        /// <summary>
        /// Запись.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public MarcRecord Record { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        [XmlAttribute("error")]
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверяет возможность списания данного экземпляра.
        /// </summary>
        public bool Check()
        {
            return false;
        }

        /// <summary>
        /// Осуществляет списание.
        /// </summary>
        public void Execute()
        {
            // TODO
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream(BinaryReader reader)
        {
            Code.NotNull(reader, "reader");

            Number = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Price = reader.ReadDecimal();
            Coefficient = reader.ReadDecimal();
            Amount = reader.ReadPackedInt32();
            Summa = reader.ReadDecimal();
            Mfn = reader.ReadPackedInt32();
            Error = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Number)
                .WriteNullable(Description)
                .WriteNullable(Year)
                .Write(Price);
            writer.Write(Coefficient);
            writer.WritePackedInt32(Amount);
            writer.Write(Summa);
            writer.WritePackedInt32(Mfn)
                .WriteNullable(Error);
        }

        #endregion
    }
}
