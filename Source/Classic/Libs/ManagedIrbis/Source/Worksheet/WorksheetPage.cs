/* WorksheetPage.cs -- 
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

namespace ManagedIrbis.Worksheet
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class WorksheetPage
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Имя страницы.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Элементы страницы.
        /// </summary>
        [NotNull]
        [XmlArray("items")]
        [XmlArrayItem("item")]
        [JsonProperty("items")]
        public NonNullCollection<WorksheetItem> Items { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public WorksheetPage()
        {
            Items = new NonNullCollection<WorksheetItem>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор потока.
        /// </summary>
        [NotNull]
        public static WorksheetPage ParseStream
            (
                [NotNull] TextReader reader,
                [NotNull] string name,
                int count
            )
        {
            Code.NotNull(reader, "reader");

            WorksheetPage result = new WorksheetPage
            {
                Name = name
            };

            for (int i = 0; i < count; i++)
            {
                WorksheetItem item = WorksheetItem.ParseStream(reader);
                result.Items.Add(item);
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Items = reader.ReadNonNullCollection<WorksheetItem>();
        }

        /// <inheritdoc/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.Write(Items);
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
