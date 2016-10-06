/* DisposableCollection.cs -- collection of disposable objects.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Collections
{
    /// <summary>
    /// Collection of disposable objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("Count = {Count}")]
    public class DisposableCollection<T>
        : Collection<T>,
        IDisposable
        where T : IDisposable
    {
        #region Construction/destruction

        /// <summary>
        /// Finalize.
        /// </summary>
        ~DisposableCollection()
        {
            Dispose();
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Dispose all items.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < Count; i++)
            {
                IDisposable item = this[i];
                if (item != null)
                {
                    item.Dispose();
                    //GC.SuppressFinalize(item);
                }
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
