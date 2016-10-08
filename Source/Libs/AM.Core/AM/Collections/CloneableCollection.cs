/* CloneableCollection.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("Count={Count}")]
#endif
    public class CloneableCollection<T>
        : Collection<T>
#if !NETCORE && !SILVERLIGHT && !UAP
        , ICloneable
#endif
    {
        #region ICloneable members

        /// <summary>
        /// Creates a new object that is a copy
        /// of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy
        /// of this instance.</returns>
        public object Clone()
        {
            CloneableCollection<T> result 
                = new CloneableCollection<T>();

            foreach (T item in this)
            {
                result.Add(item);
            }

            return result;
        }

        #endregion
    }
}
