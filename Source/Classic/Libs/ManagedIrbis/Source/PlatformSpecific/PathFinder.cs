// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PathFinder.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Finds a file on abstract path.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [ExcludeFromCodeCoverage]
    public abstract class PathFinder
    {
        #region Public methods

        /// <summary>
        /// Find file.
        /// </summary>
        [CanBeNull]
        public virtual string FindFile
            (
                [NotNull] string path,
                [NotNull] string fileName
            )
        {
            Code.NotNull(path, "path");
            Code.NotNull(fileName, "fileName");

            return null;
        }

        #endregion
    }
}
