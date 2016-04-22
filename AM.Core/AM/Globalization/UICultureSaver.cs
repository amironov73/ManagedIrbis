/* UICultureSaver.cs -- saves and restores current thread UI culture
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Globalization
{
    /// <summary>
    /// Saves and restores current thread UI culture.
    /// </summary>
    /// <example>
    /// <para>This example changes current thread UI culture to
    /// for a while.
    /// </para>
    /// <code>
    /// using System.Globalization;
    /// using AM.Globalization;
    /// 
    /// using ( new UICultureSaver ( "ru-RU" ) )
    /// {
    ///		// do something
    /// }
    /// // here old culture is restored.
    /// </code>
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UICultureSaver : IDisposable
    {
        #region Properties

        private readonly CultureInfo _previousCulture;

        /// <summary>
        /// Gets the previous UI culture.
        /// </summary>
        /// <value>The previous culture.</value>
        public CultureInfo PreviousCulture
        {
            [DebuggerStepThrough]
            get
            {
                return _previousCulture;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Saves current thread UI culture for a while.
        /// </summary>
        public UICultureSaver ( )
        {
            _previousCulture = Thread.CurrentThread.CurrentUICulture;
        }

        /// <summary>
        /// Sets new current thread UI culture to the given 
        /// <see cref="T:System.Globalization.CultureInfo"/>.
        /// </summary>
        /// <param name="newCulture">The new culture.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="newCulture"/> is <c>null</c>.
        /// </exception>
        public UICultureSaver ( CultureInfo newCulture )
            : this ()
        {
            Code.NotNull
                (
                    newCulture,
                    "newCulture"
                 );
            Thread.CurrentThread.CurrentUICulture = newCulture;
        }

        /// <summary>
        /// Sets current thread UI culture to based on the given name.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="cultureName"/> is <c>null</c>.
        /// </exception>
        public UICultureSaver ( string cultureName )
            : this ( new CultureInfo ( cultureName ) )
        {
        }

        /// <summary>
        /// Sets current thread UI culture to based on the given identifier.
        /// </summary>
        /// <param name="cultureIdentifier">The culture identifier.</param>
        public UICultureSaver ( int cultureIdentifier )
            : this ( new CultureInfo ( cultureIdentifier ) )
        {
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Restores old current thread culture.
        /// </summary>
        public void Dispose ( )
        {
            Thread.CurrentThread.CurrentUICulture = _previousCulture;
        }

        #endregion
    }
}
