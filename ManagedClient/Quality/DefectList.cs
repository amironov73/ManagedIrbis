/* DefectList.cs --
 * Ars Magna project, http://arsmagna.ru
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

namespace ManagedClient.Quality
{
    /// <summary>
    /// Список дефектов.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DefectList
        : NonNullCollection<FieldDefect>,
        IHandmadeSerializable
    {
        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public DefectList AddRange
            (
                [NotNull] IEnumerable<FieldDefect> defects
            )
        {
            Code.NotNull(() => defects);

            foreach (FieldDefect defect in defects)
            {
                Add(defect);
            }

            return this;
        }

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
            Code.NotNull(() => reader);

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
            Code.NotNull(() => writer);

            writer.WriteArray(ToArray());
        }

        #endregion
    }
}
