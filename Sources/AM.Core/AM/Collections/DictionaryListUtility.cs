/* DictionaryListUtility.cs -- useful routines for DictionaryList
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Useful routines for <see cref="DictionaryList{TKey,TValue}"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
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
            Code.NotNull(list, "list");
            Code.NotNull(reader, "reader");

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
            Code.NotNull(list, "list");
            Code.NotNull(writer, "writer");

            throw new NotImplementedException();
        }

        #endregion
    }
}
