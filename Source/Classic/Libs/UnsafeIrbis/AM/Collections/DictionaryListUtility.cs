// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DictionaryListUtility.cs -- useful routines for DictionaryList
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using UnsafeAM.Logging;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Collections
{
    /// <summary>
    /// Useful routines for <see cref="DictionaryList{TKey,TValue}"/>.
    /// </summary>
    [PublicAPI]
    public static class DictionaryListUtility
    {
        #region Public methods

        /// <summary>
        ///
        /// </summary>
        public static void RestoreFromStream<TKey, TValue>
            (
                [NotNull] this DictionaryList<TKey, TValue> list,
                [NotNull] BinaryReader reader
            )
        {
            Code.NotNull(list, nameof(list));
            Code.NotNull(reader, nameof(reader));

            Log.Error
                (
                    "DictionaryListUtility::RestoreFromStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public static void SaveToStream<TKey, TValue>
            (
                [NotNull] this DictionaryList<TKey, TValue> list,
                [NotNull] BinaryWriter writer
            )
        {
            Code.NotNull(list, nameof(list));
            Code.NotNull(writer, nameof(writer));

            Log.Error
                (
                    "DictionaryListUtility::SaveToStream: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion
    }
}
