// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AcquisitionTable.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Tables
{
    /// <summary>
    /// Описание таблицы для комплектования в ИРБИС64.
    /// </summary>
    [PublicAPI]
    [XmlRoot("table")]
    [MoonSharpUserData]
    public sealed class AcquisitionTable
    {
        #region Properties

        /// <summary>
        /// 1-я строка - имя таблицы.
        /// </summary>
        [CanBeNull]
        [XmlElement("name")]
        [JsonProperty("name")]
        public string TableName { get; set; }

        /// <summary>
        /// 2-я строка - способ отбора записей.
        /// </summary>
        [XmlElement("selectionMethod")]
        [JsonProperty("selectionMethod")]
        public int SelectionMethod { get; set; }

        /// <summary>
        /// 3-я строка - имя опросного рабочего листа,
        /// в котором задаются параметры для отбора записей
        /// и для построения значения модельного поля.
        /// </summary>
        [CanBeNull]
        [XmlElement("worksheet")]
        [JsonProperty("worksheet")]
        public string Worksheet { get; set; }

        /// <summary>
        /// 4-я строка - формат.
        /// </summary>
        [CanBeNull]
        [XmlElement("format")]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// 5-я строка – формат, который «фильтрует» отобранные записи.
        /// </summary>
        [CanBeNull]
        [XmlElement("filter")]
        [JsonProperty("filter")]
        public string Filter { get; set; }

        /// <summary>
        /// 6-я – формат для определения значения
        /// модельного поля с меткой 991.
        /// </summary>
        [CanBeNull]
        [XmlElement("modelField")]
        [JsonProperty("modelField")]
        public string ModelField { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return TableName.ToVisibleString();
        }

        #endregion
    }
}
