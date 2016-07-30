/* GblFile.cs -- GBL file
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
    /// <summary>
    /// GBL file
    /// </summary>
    [PublicAPI]
    [XmlRoot("gbl")]
    [MoonSharpUserData]
    public sealed class GblFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// File name.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        public string FileName { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        [NotNull]
        [XmlElement("item")]
        [JsonProperty("items")]
        public NonNullCollection<GblItem> Items
        {
            get { return _items; }
        }

            /// <summary>
        /// Parameters.
        /// </summary>
        [NotNull]
        [XmlElement("parameter")]
        [JsonProperty("parameters")]
        public NonNullCollection<GblParameter> Parameters
        {
            get { return _parameters; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        public GblFile()
        {
            _parameters = new NonNullCollection<GblParameter>();
            _items = new NonNullCollection<GblItem>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<GblParameter> _parameters;

        private readonly NonNullCollection<GblItem> _items;

        #endregion

        #region Public methods

        /// <summary>
        /// Parse local file.
        /// </summary>
        [NotNull]
        public static GblFile ParseLocalFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNull(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            using (StreamReader reader = new StreamReader
                (
                    File.OpenRead(fileName),
                    encoding
                ))
            {
                GblFile result = ParseStream(reader);

                return result;
            }
        }

        /// <summary>
        /// Parse specified stream.
        /// </summary>
        [NotNull]
        public static GblFile ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            GblFile result = new GblFile();

            string line = reader.RequireLine();
            int count = int.Parse(line);
            for (int i = 0; i < count; i++)
            {
                GblParameter parameter = GblParameter.ParseStream(reader);
                result.Parameters.Add(parameter);
            }

            while (true)
            {
                GblItem item = GblItem.ParseStream(reader);
                if (item == null)
                {
                    break;
                }
                result.Items.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Should JSON serialize <see cref="Parameters"/>.
        /// </summary>
        public bool ShouldSerializeParameters()
        {
            return Parameters.Count != 0;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from specified stream.
        /// </summary>
        /// <param name="reader"></param>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            FileName = reader.ReadNullableString();
            reader.ReadCollection(Parameters);
            reader.ReadCollection(Items);
        }

        /// <summary>
        /// Save object state to specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(FileName);
            writer.WriteCollection(Parameters);
            writer.WriteCollection(Items);
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = Items.Count != 0;

            if (result)
            {
                result = Items.All
                    (
                        item => item.Verify(throwOnError)
                    );
            }

            if (result
                && (Parameters.Count != 0))
            {
                result = Parameters.All
                    (
                        parameter => parameter.Verify(throwOnError)
                    );
            }

            if (!result && throwOnError)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
