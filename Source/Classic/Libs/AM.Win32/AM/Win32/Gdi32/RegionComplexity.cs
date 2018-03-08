// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RegionComplexity.cs -- specifies the complexity of region
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Specifies the complexity of region.
    /// </summary>
    [PublicAPI]
    public enum RegionComplexity
    {
        /// <summary>
        /// Error. No region created.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// The region is empty.
        /// </summary>
        NULLREGION = 1,

        /// <summary>
        /// The region is a single rectangle.
        /// </summary>
        SIMPLEREGION = 2,

        /// <summary>
        /// The region is more than a single rectangle.
        /// </summary>
        COMPLEXREGION = 3
    }
}
