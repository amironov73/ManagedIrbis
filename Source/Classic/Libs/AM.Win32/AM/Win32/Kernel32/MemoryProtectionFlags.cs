// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MemoryProtectionFlags.cs -- memory protection options provided by Windows
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

// ReSharper disable InconsistentNaming

namespace AM.Win32
{
    /// <summary>
    /// Memory protection options provided by Windows.
    /// </summary>
    [Flags]
    [PublicAPI]
    public enum MemoryProtectionFlags
    {
        /// <summary>
        /// Disables all access to the committed region of pages. 
        /// An attempt to read from, write to, or execute the 
        /// committed region results in an access violation exception, 
        /// called a general protection (GP) fault.
        /// </summary>
        PAGE_NOACCESS = 0x01,

        /// <summary>
        /// Enables read access to the committed region of pages. An 
        /// attempt to write to the committed region results in an 
        /// access violation. If the system differentiates between 
        /// read-only access and execute access, an attempt to execute 
        /// code in the committed region results in an access violation.
        /// </summary>
        PAGE_READONLY = 0x02,

        /// <summary>
        /// Enables both read and write access to the committed region 
        /// of pages.
        /// </summary>
        PAGE_READWRITE = 0x04,

        /// <summary>
        /// <para>Gives copy-on-write protection to the committed region 
        /// of pages.</para>
        /// <para>Windows Me/98/95:  This flag is not supported.</para>
        /// </summary>
        PAGE_WRITECOPY = 0x08,

        /// <summary>
        /// Enables execute access to the committed region of pages. 
        /// An attempt to read or write to the committed region results 
        /// in an access violation.
        /// </summary>
        PAGE_EXECUTE = 0x10,

        /// <summary>
        /// Enables execute and read access to the committed region 
        /// of pages. An attempt to write to the committed region 
        /// results in an access violation.
        /// </summary>
        PAGE_EXECUTE_READ = 0x20,

        /// <summary>
        /// Enables execute, read, and write access to the committed 
        /// region of pages.
        /// </summary>
        PAGE_EXECUTE_READWRITE = 0x40,

        /// <summary>
        /// Enables execute, read, and write access to the committed 
        /// region of pages. The pages are shared read-on-write and 
        /// copy-on-write.
        /// </summary>
        PAGE_EXECUTE_WRITECOPY = 0x80,

        /// <summary>
        /// <para>Pages in the region become guard pages. Any attempt 
        /// to access a guard page causes the system to raise a 
        /// STATUS_GUARD_PAGE exception and turn off the guard page 
        /// status. Guard pages thus act as a one-time access alarm. 
        /// </para>
        /// <para>When an access attempt leads the system to turn off 
        /// guard page status, the underlying page protection takes 
        /// over.</para>
        /// <para>If a guard page exception occurs during a system 
        /// service, the service typically returns a failure status 
        /// indicator.</para>
        /// <para>This value cannot be used with PAGE_NOACCESS.</para>
        /// <para>Windows Me/98/95:  This flag is not supported. 
        /// To simulate this behavior, use PAGE_NOACCESS.</para>
        /// </summary>
        PAGE_GUARD = 0x100,

        /// <summary>
        /// <para>Does not allow caching of the committed regions of 
        /// pages in the CPU cache. The hardware attributes for the 
        /// physical memory should be specified as "no cache." This is 
        /// not recommended for general usage. It is useful for device 
        /// drivers; for example, mapping a video frame buffer with no 
        /// caching.</para>
        /// <para>This value cannot be used with PAGE_NOACCESS.</para>
        /// </summary>
        PAGE_NOCACHE = 0x200,

        /// <summary>
        /// ???
        /// </summary>
        PAGE_WRITECOMBINE = 0x400,
    }
}
