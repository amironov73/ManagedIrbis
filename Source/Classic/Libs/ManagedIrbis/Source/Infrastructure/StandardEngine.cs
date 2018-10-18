// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StandardEngine.cs -- standard execution engine
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Standard execution engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class StandardEngine
        : AbstractEngine
    {
        #region Properties

        /// <summary>
        /// Memory usage.
        /// </summary>
        public int MemoryUsage { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StandardEngine
            (
                [NotNull] IIrbisConnection connection,
                [CanBeNull] AbstractEngine nestedEngine
            )
            : base(connection, nestedEngine)
        {
            Log.Trace("StandardEngine::Constructor");

            MemoryUsage = 1024;
        }

        #endregion

        #region AbstractEngine members

        /// <inheritdoc cref="AbstractEngine.GetMemoryStream" />
        public override MemoryStream GetMemoryStream
            (
                Type consumer
            )
        {
            int memoryUsage = Math.Max(MemoryUsage, 1024);

            return MemoryManager.GetMemoryStream(memoryUsage);

        }

        /// <inheritdoc cref="AbstractEngine.RecycleMemoryStream" />
        public override void RecycleMemoryStream
            (
                MemoryStream stream
            )
        {
            MemoryManager.RecycleMemoryStream(stream);
        }

        /// <inheritdoc cref="AbstractEngine.ReportMemoryUsage" />
        public override void ReportMemoryUsage
            (
                Type consumer,
                int memoryUsage
            )
        {
            MemoryUsage = Math.Max(memoryUsage, MemoryUsage);
        }

        #endregion
    }
}
