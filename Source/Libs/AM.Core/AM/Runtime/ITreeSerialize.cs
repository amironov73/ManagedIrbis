/* IHandmadeSerializable.cs -- объект умеет сохраняться в поток
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public interface ITreeSerialize
    {
        /// <summary>
        /// Deserialize.
        /// </summary>
        void DeserializeTree
            (
                [NotNull] BinaryReader reader
            );

        /// <summary>
        /// Serialize.
        /// </summary>
        void SerializeTree
            (
                [NotNull] BinaryWriter writer
            );
    }
}
