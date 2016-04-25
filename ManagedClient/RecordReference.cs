/* RecordReference.cs -- ссылка на запись.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Xml.Serialization;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Ссылка на запись (например, для сохранения в "кармане").
    /// </summary>
    [PublicAPI]
    [Serializable]
    [XmlRoot("record")]
    [MoonSharpUserData]
    [DebuggerDisplay("MFN={Mfn}, Index={Index}")]
    public sealed class RecordReference
    {
        #region Properties

        /// <summary>
        /// Сервер ИРБИС64. Например, "127.0.0.1".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("host")]
        [JsonProperty("host")]
        public string HostName { get; set; }

        /// <summary>
        /// База данных. Например, "IBIS".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("db")]
        [JsonProperty("db")]
        public string Database { get; set; }

        /// <summary>
        /// MFN. Чаще всего = 0, т. к. используется Index.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonProperty("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи в базе данных, например "81.432.1-42/P41-012833".
        /// </summary>
        [CanBeNull]
        [XmlAttribute("index")]
        [JsonProperty("index")]
        public string Index { get; set; }

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
                    "Mfn: {0}, Index: {1}",
                    Mfn,
                    Index
                );
        }

        #endregion
    }
}
