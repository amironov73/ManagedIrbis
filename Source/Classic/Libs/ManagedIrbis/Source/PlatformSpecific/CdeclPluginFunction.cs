/* CdeclPluginFunction.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#if CLASSIC

#region Using directives

using System.Runtime.InteropServices;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Plugin function with cdecl call convention.
    /// </summary>
    [PublicAPI]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int CdeclPluginFunction
        (
            string buf1,
            StringBuilder buf2,
            int bufsize
        );
}

#endif
