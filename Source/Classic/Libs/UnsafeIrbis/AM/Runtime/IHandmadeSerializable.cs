// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IHandmadeSerializable.cs -- объект умеет сохраняться в поток
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Runtime
{
    /// <summary>
    /// Объект умеет сохраняться в поток и восстанавливаться из него.
    /// </summary>
    public interface IHandmadeSerializable
    {
        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        void RestoreFromStream
            (
                [NotNull] BinaryReader reader
            );

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        void SaveToStream
            (
                [NotNull] BinaryWriter writer
            );
    }
}
