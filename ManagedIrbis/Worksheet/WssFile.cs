/* WssFile.cs -- вложенный рабочий лист
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
    /// Вложенный рабочий лист.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Name}")]
    [XmlRoot("wss-file")]
    public sealed class WssFile
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Имя рабочего листа.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Элементы рабочего листа.
        /// </summary>
        [NotNull]
        [XmlArray("items")]
        [XmlArrayItem("item")]
        [JsonProperty("items")]
        public NonNullCollection<WorksheetItem> Items { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public WssFile()
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
        public static WssFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            WssFile result = new WssFile();

            int count = int.Parse(reader.RequireLine());
            
            for (int i = 0; i < count; i++)
            {
                WorksheetItem item = WorksheetItem.ParseStream(reader);
                result.Items.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        [NotNull]
        public static WssFile ReadLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamReader reader = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                ))
            {
                WssFile result = ParseStream(reader);

                result.Name = Path.GetFileName(fileName);

                return result;
            }
        }

        /// <summary>
        /// Считывание из локального файла.
        /// </summary>
        [NotNull]
        public static WssFile ReadLocalFile
            (
                [NotNull] string fileName
            )
        {
            return ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
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
            Name = reader.ReadNullableString();
            Items = reader.ReadNonNullCollection<WorksheetItem>();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
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

        #endregion
    }
}
