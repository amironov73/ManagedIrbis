﻿/* StatDefinition.cs -- parameters for stat command
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Signature for Stat command.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class StatDefinition
    {
        #region Nested classes

        /// <summary>
        /// Sort method.
        /// </summary>
        public enum SortMethod
        {
            /// <summary>
            /// Don't sort.
            /// </summary>
            None = 0,

            /// <summary>
            /// Ascending sort.
            /// </summary>
            Ascending = 1,

            /// <summary>
            /// Descending sort.
            /// </summary>
            Descending = 2
        }

        /// <summary>
        /// Stat item.
        /// </summary>
        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Field (possibly with subfield) specification.
            /// </summary>
            [CanBeNull]
            public string Field { get; set; }

            /// <summary>
            /// Maximum length of the value (truncation).
            /// </summary>
            public int Length { get; set; }

            /// <summary>
            /// Count of items to take.
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// How to sort result.
            /// </summary>
            public SortMethod Sort { get; set; }

            #endregion

            #region Object members

            /// <summary>
            /// Returns a <see cref="System.String" />
            /// that represents this instance.
            /// </summary>
            /// <returns>A <see cref="System.String" />
            /// that represents this instance.</returns>
            public override string ToString()
            {
                return string.Format
                    (
                        "{0},{1},{2},{3}",
                        Field,
                        Length,
                        Count,
                        (int) Sort
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string DatabaseName { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        [NotNull]
        // ReSharper disable ConvertToAutoProperty
        public List<Item> Items
        {
            get { return _items; }
        }
        // ReSharper restore ConvertToAutoProperty

        /// <summary>
        /// Search query specification.
        /// </summary>
        [CanBeNull]
        public string SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional query for sequential search.
        /// </summary>
        [CanBeNull]
        public string SequentialQuery { get; set; }
        
        /// <summary>
        /// List of MFN.
        /// </summary>
        [NotNull]
        // ReSharper disable ConvertToAutoProperty
        public List<int> MfnList
        {
            get { return _mfnList; }
        }
        // ReSharper restore ConvertToAutoProperty

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<Item> _items
            = new List<Item>();

        private readonly List<int> _mfnList
            = new List<int>();

        #endregion

        #region Public methods

        #endregion

        #region Object members
        
        #endregion
    }
}
