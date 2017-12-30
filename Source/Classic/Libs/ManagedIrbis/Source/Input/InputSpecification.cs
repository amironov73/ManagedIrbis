// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* InputSpecification.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Json;
using AM.IO;
using AM.Runtime;
using AM.Xml;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Input
{
    /// <summary>
    /// Input specification.
    /// </summary>
    [PublicAPI]
    [XmlRoot("input")]
    [MoonSharpUserData]
    public sealed class InputSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Input lines.
        /// </summary>
        [CanBeNull]
        [XmlElement("line")]
        [JsonProperty("lines")]
        public InputLine[] Lines { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Load specification from JSON file.
        /// </summary>
        [NotNull]
        public static InputSpecification LoadJsonFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            InputSpecification result 
                = JsonUtility.ReadObjectFromFile<InputSpecification>
                (
                    fileName
                );

            return result;
        }

        /// <summary>
        /// Load specification from XML file.
        /// </summary>
        [NotNull]
        public static InputSpecification LoadXmlFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            InputSpecification result
                = XmlUtility.Deserialize<InputSpecification>
                (
                    fileName
                );

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Name = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            reader.RestoreArray<InputLine>();
        }

        /// <inheritdoc />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(Name)
                .WriteNullable(Title);
            Lines.SaveToStream(writer);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<InputSpecification> verifier 
                = new Verifier<InputSpecification>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Name, "Name")
                .NotNullNorEmpty(Title, "Title")
                .NotNull(Lines, "Lines")
                // ReSharper disable once PossibleNullReferenceException
                .Assert(Lines.Length != 0, "Lines.Count");

            return verifier.Result;
        }

        #endregion
    }
}

