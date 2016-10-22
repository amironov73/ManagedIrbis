/* CodePageEnumProc.cs -- 
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// 
    /// </summary>
    public delegate bool CodePageEnumProc 
    (
        string codePage
    );
}
