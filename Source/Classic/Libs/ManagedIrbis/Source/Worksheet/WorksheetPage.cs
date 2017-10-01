// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* WorksheetPage.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
    [XmlRoot("page")]
    [MoonSharpUserData]
    public sealed class WorksheetPage
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя страницы.
        /// </summary>
        [CanBeNull]
        [XmlElement("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Элементы страницы.
        /// </summary>
        [NotNull]
        [XmlElement("item")]
        [JsonProperty("items")]
        public NonNullCollection<WorksheetItem> Items { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

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

        #region Public methods

        /// <summary>
        /// Encode the page.
        /// </summary>
        public void Encode
            (
                [NotNull] TextWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            foreach (WorksheetItem item in Items)
            {
                item.Encode(writer);
            }
        }

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

        /// <summary>
        /// Should serialize the <see cref="Items"/> collection?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeItems()
        {
            return Items.Count != 0;
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
            Items = reader.ReadNonNullCollection<WorksheetItem>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteNullable(Name);
            writer.Write(Items);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<WorksheetPage> verifier
                = new Verifier<WorksheetPage>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, "Name")
                .Assert
                    (
                        Items.Count != 0,
                        "Items.Count != 0"
                    );

            foreach (WorksheetItem item in Items)
            {
                verifier.VerifySubObject
                    (
                        item,
                        "Item"
                    );
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
