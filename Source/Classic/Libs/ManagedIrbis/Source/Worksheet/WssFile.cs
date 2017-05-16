// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

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
        /// Read from server.
        /// </summary>
        [CanBeNull]
        public static WssFile ReadFromServer
            (
                [NotNull] IrbisConnection connection,
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(specification, "specification");

            string content = connection.ReadTextFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            using (StringReader reader = new StringReader(content))
            {
                return ParseStream(reader);
            }
        }

#if !WIN81 && !PORTABLE

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

            using (StreamReader reader = TextReaderUtility.OpenRead
                (
                    fileName,
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

#endif

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
