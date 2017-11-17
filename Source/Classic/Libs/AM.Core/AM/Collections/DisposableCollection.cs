// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DisposableCollection.cs -- collection of disposable objects.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

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
        [ExcludeFromCodeCoverage]
        ~DisposableCollection()
        {
            // TODO ???
            Dispose();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            for (int i = 0; i < Count; i++)
            {
                IDisposable item = this[i];
                if (!ReferenceEquals(item, null))
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
