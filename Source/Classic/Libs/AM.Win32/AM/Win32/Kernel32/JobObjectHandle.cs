/* JobObjectHandle.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains handle for <see cref="WindowsJob"/>.
    /// </summary>
    public class JobObjectHandle
        : SafeHandle
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public JobObjectHandle()
            : base(IntPtr.Zero, true)
        {
        }

        #endregion


        #region SafeHandle members

        /// <inheritdoc />
        public override bool IsInvalid
        {
            [PrePrepareMethod]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get { return (handle == IntPtr.Zero); }
        }

        /// <inheritdoc />
        [PrePrepareMethod]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            return Kernel32.CloseHandle(handle);
        }

        #endregion
    }
}
