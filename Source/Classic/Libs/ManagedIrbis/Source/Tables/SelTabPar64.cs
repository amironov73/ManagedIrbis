// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SelTabPar64.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using AM;
using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Tables
{
    //
    // Official documentation:
    // http://sntnarciss.ru/irbis/spravka/px000020.htm
    //

    /// <summary>
    /// Файл описания таблиц для ИРБИС64 в директории БД CMPL.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("selTabPar")]
    public sealed class SelTabPar64
    {
        #region Properties

        /// <summary>
        /// Tables.
        /// </summary>
        [NotNull]
        [XmlElement("table")]
        [JsonProperty("tables")]
        public NonNullCollection<AcquisitionTable> Tables
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SelTabPar64()
        {
            Tables = new NonNullCollection<AcquisitionTable>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the specified stream.
        /// </summary>
        [NotNull]
        public static SelTabPar64 ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            Code.NotNull(reader, "reader");

            SelTabPar64 result = new SelTabPar64();

            while (true)
            {
                string name = reader.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    break;
                }

                AcquisitionTable table = new AcquisitionTable
                {
                    TableName = name,
                    SelectionMethod = reader.ReadLine().SafeToInt32(),
                    Worksheet = reader.ReadLine().EmptyToNull(),
                    Format = reader.ReadLine().EmptyToNull(),
                    Filter = reader.ReadLine().EmptyToNull(),
                    ModelField = reader.ReadLine().EmptyToNull()
                };
                result.Tables.Add(table);

                // абор строк, описывающих таблицу,
                // заканчивается строкой ‘*****’.
                reader.ReadLine(); 
            }

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return "Tables: " + Tables.Count.ToInvariantString();
        }

        #endregion
    }
}
