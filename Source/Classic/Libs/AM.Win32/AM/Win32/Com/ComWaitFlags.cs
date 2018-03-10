// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ComWaitFlags.cs -- specify the behavior of the CoWaitForMultipleHandles function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// The values of the COWAIT_FLAGS enumeration specify the behavior
    /// of the CoWaitForMultipleHandles function. This enumeration is also
    /// used by the ISynchronize::Wait and ISynchronizeContainer::WaitMultiple
    /// methods, which typically call CoWaitForMultipleHandles.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum ComWaitFlags
    {
        /// <summary>
        /// If set, the call to CoWaitForMultipleHandles will return S_OK
        /// only when all handles associated with the synchronization object
        /// have been signaled. If not set, the call will return S_OK as soon
        /// as any handle associated with the synchronization object has been
        /// signaled.
        /// </summary>
        COWAIT_WAITALL = 0x00000001,

        /// <summary>
        /// If set, the call to CoWaitForMultipleHandles will return S_OK
        /// if an asynchronous procedure call (APC) has been queued to the
        /// calling thread with a call to the Win32 function QueueUserAPC,
        /// even if no handle has been signaled. 
        /// </summary>
        COWAIT_ALERTABLE = 0x00000002
    }
}
