/* FoundLine.cs -- line in list of found documents
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Search
{
    /// <summary>
    /// Line in list of found documents.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class FoundLine
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Icon.
        /// </summary>
        [CanBeNull]
        public object Icon { get; set; }

        /// <summary>
        /// Selected by user.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        [CanBeNull]
        public string Description { get; set; }

        /// <summary>
        /// For list sorting.
        /// </summary>
        [CanBeNull]
        public string Sort { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }

        #endregion
    }
}
