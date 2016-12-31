// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RecordDiffResult.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Collections;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Difference result for two records.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RecordDifferenceResult
    {
        #region Properties

        /// <summary>
        /// Fields present in both records.
        /// </summary>
        [NotNull]
        public NonNullCollection<RecordField> Both
        {
            get;
            private set;
        }

        /// <summary>
        /// Fields present in first record only.
        /// </summary>
        [NotNull]
        public NonNullCollection<RecordField> FirstOnly
        {
            get;
            private set;
        }

        /// <summary>
        /// Fields present in second record only.
        /// </summary>
        [NotNull]
        public NonNullCollection<RecordField> SecondOnly
        {
            get;
            private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constuctor
        /// </summary>
        public RecordDifferenceResult()
        {
            Both = new NonNullCollection<RecordField>();
            FirstOnly = new NonNullCollection<RecordField>();
            SecondOnly = new NonNullCollection<RecordField>();
        }

        #endregion
    }
}
