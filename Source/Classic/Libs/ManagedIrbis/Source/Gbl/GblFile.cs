// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
        public NonNullCollection<GblStatement> Statements
        {
            get { return _statements; }
        }

            /// <summary>
        /// Signature.
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
            _statements = new NonNullCollection<GblStatement>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<GblParameter> _parameters;

        private readonly NonNullCollection<GblStatement> _statements;

        #endregion

        #region Public methods

#if !WIN81

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

#endif

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
                GblStatement statement = GblStatement.ParseStream(reader);
                if (statement == null)
                {
                    break;
                }
                result.Statements.Add(statement);
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
            reader.ReadCollection(Statements);
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
            writer.WriteCollection(Statements);
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
            bool result = Statements.Count != 0;

            if (result)
            {
                result = Statements.All
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

