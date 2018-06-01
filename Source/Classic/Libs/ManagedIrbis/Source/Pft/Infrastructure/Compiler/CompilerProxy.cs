// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CompilerProxy.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC || NETCORE

#region Using directives

using System;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    /// Хак для загрузки сборки в другом домене.
    /// </summary>
    /// <remarks>
    /// Borrowed from https://stackoverflow.com/questions/658498/how-to-load-an-assembly-to-appdomain-with-all-references-recursively
    /// </remarks>
    public sealed class CompilerProxy
        : MarshalByRefObject
    {
        #region Public methods

        /// <summary>
        /// Load the assembly.
        /// </summary>
        [NotNull]
        public Assembly LoadAssembly
            (
                [NotNull] string assemblyPath
            )
        {
            Assembly result = Assembly.LoadFile(assemblyPath);

            return result;
        }

        #endregion
    }
}

#endif
