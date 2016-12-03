// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DefectList.cs -- список дефектов для поля записи
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Список дефектов для записи.
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
                IEnumerable<FieldDefect> defects
            )
        {
            AddRange(defects);
        }

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
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

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
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
