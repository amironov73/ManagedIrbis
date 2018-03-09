// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CopyCallbackReason.cs -- reason that CopyProgressRoutine was called
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Reason that CopyProgressRoutine was called.
    /// </summary>
    [PublicAPI]
    public enum CopyCallbackReason
    {
        /// <summary>
        /// Another part of the data file was copied.
        /// </summary>
        CALLBACK_CHUNK_FINISHED = 0x00000000,

        /// <summary>
        /// Another stream was created and is about to be copied. 
        /// This is the callback reason given when the callback 
        /// routine is first invoked.
        /// </summary>
        CALLBACK_STREAM_SWITCH = 0x00000001
    }
}
