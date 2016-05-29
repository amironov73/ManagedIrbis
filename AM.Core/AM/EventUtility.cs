/* EventUtility.cs -- Useful routines for event manipulations
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM
{
    /// <summary>
    /// Useful routines for event manipulations.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class EventUtility
    {
        #region Public methods

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise <T>
            (
                [CanBeNull] this EventHandler <T> handler,
                [CanBeNull] object sender,
                [CanBeNull] T args
            )
            where T : EventArgs
        {
            if ( handler != null )
            {
                handler ( sender, args );
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise <T> 
            (
                [CanBeNull] this EventHandler <T> handler,
                [CanBeNull] object sender
            )
            where T : EventArgs
        {
            if ( handler != null )
            {
                handler ( sender, null );
            }
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise <T> 
            (
                [CanBeNull] this EventHandler <T> handler
            )
            where T : EventArgs
        {
            if ( handler != null )
            {
                handler ( null, null );
            }
        }

        #endregion
    }
}