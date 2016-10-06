/* CodePageEnumFlags.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    ///  Specifies the code pages to enumerate.
    /// </summary>
    [Flags]
    public enum CodePageEnumFlags
    {
        /// <summary>
        /// Enumerate only installed code pages.
        /// </summary>
        CP_INSTALLED = 0x00000001,

        /// <summary>
        /// Enumerate all supported code pages.
        /// </summary>
        CP_SUPPORTED = 0x00000002
    }
}
