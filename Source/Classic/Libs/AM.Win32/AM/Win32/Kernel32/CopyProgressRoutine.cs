// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CopyProgressRoutine.cs -- application-defined callback function used with the CopyFileEx and MoveFileWithProgress functions
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The CopyProgressRoutine function is an application-defined 
    /// callback function used with the CopyFileEx and 
    /// MoveFileWithProgress functions. It is called when a portion 
    /// of a copy or move operation is completed. The 
    /// LPPROGRESS_ROUTINE type defines a pointer to this callback 
    /// function. CopyProgressRoutine is a placeholder for the 
    /// application-defined function name.
    /// </summary>
    /// <param name="totalFileSize">Total size of the file, in bytes.</param>
    /// <param name="totalBytesTransferred">Total number of bytes 
    /// transferred from the source file to the destination file since 
    /// the copy operation began.</param>
    /// <param name="streamSize">Total size of the current file stream, 
    /// in bytes.</param>
    /// <param name="streamBytesTransferred">Total number of bytes 
    /// in the current stream that have been transferred from the 
    /// source file to the destination file since the copy operation 
    /// began.</param>
    /// <param name="dwStreamNumber">Handle to the current stream. 
    /// The first time CopyProgressRoutine is called, the stream 
    /// number is 1.</param>
    /// <param name="dwCallbackReason">Reason that CopyProgressRoutine 
    /// was called.</param>
    /// <param name="hSourceFile">Handle to the source file.</param>
    /// <param name="hDestinationFile">Handle to the destination file.</param>
    /// <param name="lpData">Argument passed to CopyProgressRoutine 
    /// by the CopyFileEx or MoveFileWithProgress function.</param>
    /// <returns></returns>
    [PublicAPI]
    public delegate CopyProgressCode CopyProgressRoutine
        (
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            int dwStreamNumber,
            CopyCallbackReason dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData
    );
}
