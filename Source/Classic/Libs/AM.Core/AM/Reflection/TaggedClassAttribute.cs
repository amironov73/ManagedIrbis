// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TaggedClassAttribute.cs -- marks class with given tag
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Reflection
{
    /// <summary>
    /// One can tag the class (e.g. for XML serialization).
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TaggedClassAttribute
        : Attribute
    {
        #region Properties

        /// <summary>
        /// Tag.
        /// </summary>
        public string Tag
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TaggedClassAttribute
            (
                [NotNull] string tag
            )
        {
            Tag = tag;
        }

        #endregion
    }
}
