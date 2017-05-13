// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DefectList.cs -- defect list for the field of the record
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Defect list for the field of the record.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DefectList
        : NonNullCollection<FieldDefect>,
        IHandmadeSerializable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DefectList()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DefectList
            (
                [NotNull] IEnumerable<FieldDefect> defects
            )
        {
            AddRange(defects);
        }

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            ClearItems();
            FieldDefect[] array = reader.ReadArray<FieldDefect>();
            AddRange(array);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer.WriteArray(ToArray());
        }

        #endregion
    }
}
