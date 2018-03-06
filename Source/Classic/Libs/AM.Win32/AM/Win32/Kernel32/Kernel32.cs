// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Kernel32.cs --  
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;
using System.Text;

using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Kernel32.dll interop.
    /// </summary>
    public static class Kernel32
    {
        #region Constants

        /// <summary>
        /// DLL name.
        /// </summary>
        public const string DllName = "Kernel32.dll";

        /// <summary>
        /// Invalid handle value.
        /// </summary>
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <summary>
        /// Null handle.
        /// </summary>
        public static readonly IntPtr NULL_HANDLE = IntPtr.Zero;

        /// <summary>
        /// NULL.
        /// </summary>
        public static readonly IntPtr NULL = IntPtr.Zero;

        /// <summary>
        /// Attach parent console.
        /// </summary>
        [CLSCompliant(false)]
        public const uint ATTACH_PARENT_CONSOLE = 0xFFFFFFFF;

        /// <summary>
        /// Unlimited number of pipe instances.
        /// </summary>
        [CLSCompliant(false)]
        public const uint PIPE_UNLIMITED_INSTANCES = 255;

        #endregion

        #region Interop

        /// <summary>
        /// Defines a console alias for the specified executable.
        /// </summary>
        /// <param name="Source">Pointer to a null-terminated string 
        /// that specifies the console alias to be mapped to the text 
        /// specified by Target.</param>
        /// <param name="Target">Pointer to a null-terminated string 
        /// that specifies the text to be substituted for Source. If 
        /// this parameter is NULL, then the console alias is removed.
        /// </param>
        /// <param name="ExeName">Pointer to a null-terminated string 
        /// that specifies the name of the executable file for which 
        /// the console alias is to be defined.</param>
        /// <returns>If the function succeeds, the return value 
        /// is TRUE.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool AddConsoleAlias
            (
                string Source,
                string Target,
                string ExeName
            );

        /// <summary>
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool AllocConsole();

        /// <summary>
        /// Assigns a process to an existing job object.
        /// </summary>
        /// <param name="job">A handle to the job object
        /// to which the process will be associated.
        /// The CreateJobObject or OpenJobObject function
        /// returns this handle. The handle must have
        /// the JOB_OBJECT_ASSIGN_PROCESS access right.
        /// </param>
        /// <param name="process">A handle to the process
        /// to associate with the job object.
        /// The handle must have the PROCESS_SET_QUOTA
        /// and PROCESS_TERMINATE access rights.</param>
        /// <returns>If the function succeeds, the return
        /// value is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool AssignProcessToJobObject
            (
                JobObjectHandle job,
                IntPtr process
            );

        /// <summary>
        /// Attaches the calling process to the console of the 
        /// specified process.
        /// </summary>
        /// <param name="dwProcessId">Identifier of the process.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool AttachConsole
            (
                uint dwProcessId
            );

        /// <summary>
        /// Generates simple tones on the speaker. The function is 
        /// synchronous; it does not return control to its caller 
        /// until the sound finishes.
        /// </summary>
        /// <param name="dwFreq"><para>Frequency of the sound, in hertz. 
        /// This parameter must be in the range 37 through 32,767 
        /// (0x25 through 0x7FFF).</para>
        /// <para>Windows Me/98/95: The Beep function ignores 
        /// this parameter.</para></param>
        /// <param name="dwDuration"><para>Duration of the sound, in 
        /// milliseconds.</para>
        /// <para>Windows Me/98/95: The Beep function ignores this 
        /// parameter.</para></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool Beep
            (
                uint dwFreq,
                uint dwDuration
            );

        /// <summary>
        /// Connects to a message-type pipe (and waits if an instance 
        /// of the pipe is not available), writes to and reads from 
        /// the pipe, and then closes the pipe.
        /// </summary>
        /// <param name="lpNamedPipeName">Pointer to a null-terminated 
        /// string specifying the pipe name.</param>
        /// <param name="lpInBuffer">Pointer to the buffer containing 
        /// the data written to the pipe.</param>
        /// <param name="nInBufferSize">Size of the write buffer, 
        /// in bytes.</param>
        /// <param name="lpOutBuffer">Pointer to the buffer that receives 
        /// the data read from the pipe.</param>
        /// <param name="nOutBufferSize">Size of the read buffer, 
        /// in bytes.</param>
        /// <param name="lpBytesRead">Pointer to a variable that receives 
        /// the number of bytes read from the pipe.</param>
        /// <param name="nTimeOut"></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool CallNamedPipe
            (
                string lpNamedPipeName,
                byte[] lpInBuffer,
                uint nInBufferSize,
                byte[] lpOutBuffer,
                uint nOutBufferSize,
                ref uint lpBytesRead,
                PipeWaitFlags nTimeOut
            );

        /// <summary>
        /// determines whether the specified name can be used to 
        /// create a file on the FAT file system.
        /// </summary>
        /// <param name="lpName">Pointer to a null-terminated string 
        /// that specifies the file name, in 8.3 format.</param>
        /// <param name="lpOemName">Pointer to a buffer that receives 
        /// the OEM string corresponding to Name. This parameter can 
        /// be NULL.</param>
        /// <param name="OemNameSize">Size of the lpOemName buffer, 
        /// in characters. If lpOemName is NULL, this parameter must 
        /// be zero.</param>
        /// <param name="pbNameContainsSpaces">Indicates whether the 
        /// name contains spaces. This parameter can be NULL. If the 
        /// name is not a valid 8.3 FAT file name, this parameter is 
        /// undefined.</param>
        /// <param name="pbNameLegal">If the function succeeds, this 
        /// parameter indicates whether the file name is a valid 8.3 
        /// FAT file name when upcased to the current OEM code page.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks>Included in Windows XP SP1.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CheckNameLegalDOS8Dot3
            (
                string lpName,
                StringBuilder lpOemName,
                int OemNameSize,
                out bool pbNameContainsSpaces,
                out bool pbNameLegal
            );

        /// <summary>
        /// Determines whether the specified process is being debugged.
        /// </summary>
        /// <param name="hProcess">Handle to the process.</param>
        /// <param name="pbDebuggerPresent">Pointer to a variable that 
        /// the function sets to TRUE if the specified process is being 
        /// debugged, or FALSE otherwise.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        /// <remarks>Included in: Windows XP SP1.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CheckRemoteDebuggerPresent
            (
                IntPtr hProcess,
                ref bool pbDebuggerPresent
            );

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">Handle to an open object.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Closing an invalid handle raises an exception 
        /// when the application is running under a debugger. This 
        /// includes closing a handle twice, and using CloseHandle 
        /// on a handle returned by the FindFirstFile function.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CloseHandle
            (
                IntPtr hObject
            );

        /// <summary>
        /// The ConnectNamedPipe function enables a named pipe server 
        /// process to wait for a client process to connect to an 
        /// instance of a named pipe. A client process connects by 
        /// calling either the CreateFile or CallNamedPipe function.
        /// </summary>
        /// <param name="hNamedPipe">Handle to the server end of a named 
        /// pipe instance. This handle is returned by the CreateNamedPipe 
        /// function.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED structure.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool ConnectNamedPipe
            (
                IntPtr hNamedPipe,
                ref OVERLAPPED lpOverlapped
            );

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="lpExistingFileName"><para>Pointer to a 
        /// null-terminated string that specifies the name of 
        /// an existing file.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="lpNewFileName"><para>Pointer to a 
        /// null-terminated string that specifies the name 
        /// of the new file.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="bFailIfExists">If this parameter is TRUE 
        /// and the new file specified by lpNewFileName already exists, 
        /// the function fails. If this parameter is FALSE and the 
        /// new file already exists, the function overwrites the 
        /// existing file and succeeds.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CopyFile
            (
                string lpExistingFileName,
                string lpNewFileName,
                bool bFailIfExists
            );

        /// <summary>
        /// Copies an existing file to a new file.
        /// </summary>
        /// <param name="lpExistingFileName"><para>Pointer to 
        /// a null-terminated string that specifies the name 
        /// of an existing file.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="lpNewFileName"><para>Pointer to 
        /// a null-terminated string that specifies the name 
        /// of the new file.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version 
        /// of the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="lpProgressRoutine">Address of a callback 
        /// function of type LPPROGRESS_ROUTINE that is called each 
        /// time another portion of the file has been copied. 
        /// This parameter can be NULL.</param>
        /// <param name="lpData">Argument to be passed to the 
        /// callback function. This parameter can be NULL.</param>
        /// <param name="pbCancel"> If this flag is set to TRUE during 
        /// the copy operation, the operation is canceled. Otherwise, 
        /// the copy operation will continue to completion.</param>
        /// <param name="dwCopyFlags">Flags that specify how the file 
        /// is to be copied.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks><para>This function preserves extended attributes, 
        /// OLE structured storage, NTFS alternate data streams, 
        /// and file attributes. Security attributes for the existing 
        /// file are not copied to the new file. To copy security attributes, 
        /// use the SHFileOperation function.</para>
        /// <para>This function fails with ERROR_ACCESS_DENIED if the 
        /// destination file already exists and has the FILE_ATTRIBUTE_HIDDEN 
        /// or FILE_ATTRIBUTE_READONLY attribute set.</para></remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CopyFileEx
            (
                string lpExistingFileName,
                string lpNewFileName,
                CopyProgressRoutine lpProgressRoutine,
                IntPtr lpData,
                out bool pbCancel,
                CreateFileFlags dwCopyFlags
            );

        /// <summary>
        /// Creates a console screen buffer.
        /// </summary>
        /// <param name="dwDesiredAccess">Access to the console screen buffer.
        /// </param>
        /// <param name="dwShareMode">This parameter can be zero, 
        /// indicating that the buffer cannot be shared, or it 
        /// can be one or more of the following values:
        /// FILE_SHARE_READ, FILE_SHARE_WRITE.</param>
        /// <param name="lpSecurityAttributes">Pointer to a SECURITY_ATTRIBUTES 
        /// structure that determines whether the returned handle can be inherited 
        /// by child processes. If lpSecurityAttributes is NULL, the handle cannot 
        /// be inherited. The lpSecurityDescriptor member of the structure specifies 
        /// a security descriptor for the new console screen buffer. If 
        /// lpSecurityAttributes is NULL, the console screen buffer gets a default 
        /// security descriptor. The ACLs in the default security descriptor for a 
        /// console screen buffer come from the primary or impersonation token of 
        /// the creator.</param>
        /// <param name="dwFlags">Type of console screen buffer to create. 
        /// The only supported screen buffer type is CONSOLE_TEXTMODE_BUFFER.</param>
        /// <param name="lpScreenBufferData">Reserved; should be NULL.</param>
        /// <returns>If the function succeeds, the return value is a handle to 
        /// the new console screen buffer. If the function fails, the return value 
        /// is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr CreateConsoleScreenBuffer
            (
                FileAccessFlags dwDesiredAccess,
                FileShareFlags dwShareMode,
                IntPtr lpSecurityAttributes,
                int dwFlags,
                IntPtr lpScreenBufferData
            );

        /// <summary>
        /// Creates or opens a named or unnamed event object.
        /// </summary>
        /// <param name="lpEventAttributes">Pointer to a 
        /// SECURITY_ATTRIBUTES structure.</param>
        /// <param name="bManualReset">If this parameter is TRUE, 
        /// the function creates a manual-reset event object which 
        /// requires use of the ResetEvent function set the state to 
        /// nonsignaled. If this parameter is FALSE, the function 
        /// creates an auto-reset event object, and system automatically 
        /// resets the state to nonsignaled after a single waiting 
        /// thread has been released.</param>
        /// <param name="bInitialState">If this parameter is TRUE, 
        /// the initial state of the event object is signaled; 
        /// otherwise, it is nonsignaled.</param>
        /// <param name="lpName">Pointer to a null-terminated string 
        /// specifying the name of the event object. The name is limited 
        /// to MAX_PATH characters. Name comparison is case sensitive.
        /// If lpName is NULL, the event object is created without a name.
        /// </param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the event object. If the named event object existed 
        /// before the function call, the function returns a handle to 
        /// the existing object and GetLastError returns 
        /// ERROR_ALREADY_EXISTS. If the function fails, the return 
        /// value is NULL.</returns>
        /// <remarks>Terminal Services: The name can have a "Global\" 
        /// or "Local\" prefix to explicitly create the object in the 
        /// global or session name space. The remainder of the name 
        /// can contain any character except the backslash character 
        /// (\).</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr CreateEvent
            (
                IntPtr lpEventAttributes,
                bool bManualReset,
                bool bInitialState,
                string lpName
            );

        /// <summary>
        /// The CreateFile function creates or opens a file, directory, physical disk, 
        /// volume, console buffer, tape drive, communications resource, mailslot, 
        /// or pipe. The function returns a handle that can be used to access the 
        /// object.
        /// </summary>
        /// <param name="lpFileName">string that specifies the name of the object to 
        /// create or open.</param>
        /// <param name="dwDesiredAccess">Access to the object. You cannot request 
        /// an access mode that conflicts with the sharing mode specified in a 
        /// previous open request whose handle is still open.</param>
        /// <param name="dwShareMode">Sharing mode of the object. You cannot request 
        /// a sharing mode that conflicts with the access mode specified in a 
        /// previous open request whose handle is still open.</param>
        /// <param name="lpSecurityAttributes">Pointer to a SECURITY_ATTRIBUTES 
        /// structure that determines whether the returned handle can be inherited 
        /// by child processes. If lpSecurityAttributes is NULL, the handle cannot 
        /// be inherited.</param>
        /// <param name="dwCreationDisposition">Action to take on files that exist, 
        /// and which action to take when files do not exist.</param>
        /// <param name="dwFlagsAndAttributes">File attributes and flags.</param>
        /// <param name="hTemplateFile">Handle to a template file, with 
        /// the GENERIC_READ access right. The template file supplies file 
        /// attributes and extended attributes for the file being created. 
        /// This parameter can be NULL.</param>
        /// <returns>If the function succeeds, the return value is an open handle 
        /// to the specified file. If the specified file exists before the function 
        /// call and dwCreationDisposition is CREATE_ALWAYS or OPEN_ALWAYS, a call 
        /// to GetLastError returns ERROR_ALREADY_EXISTS (even though the function 
        /// has succeeded). If the file does not exist before the call, GetLastError 
        /// returns zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr CreateFile
            (
                string lpFileName,
                FileAccessFlags dwDesiredAccess,
                FileShareFlags dwShareMode,
                IntPtr lpSecurityAttributes,
                CreationDisposition dwCreationDisposition,
                CreateFileFlags dwFlagsAndAttributes,
                IntPtr hTemplateFile
            );

        /// <summary>
        /// Creates or opens a named or unnamed file mapping object 
        /// for the specified file.
        /// </summary>
        /// <param name="hFile"><para>Handle to the file from which to 
        /// create a mapping object. The file must be opened with access 
        /// rights compatible with the protection flags specified by the 
        /// flProtect parameter. It is recommended, though not required, 
        /// that files you intend to map be opened for exclusive access. 
        /// </para>
        /// <para>If hFile is INVALID_HANDLE_VALUE, the calling process 
        /// must also specify a mapping object size in the 
        /// dwMaximumSizeHigh and dwMaximumSizeLow parameters. 
        /// In this case, CreateFileMapping creates a file mapping object 
        /// of the specified size backed by the operating-system paging 
        /// file rather than by a named file in the file system. The 
        /// file mapping object can be shared through duplication, 
        /// through inheritance, or by name. The initial contents of 
        /// the pages in the file mapping object are zero.</para></param>
        /// <param name="lpAttributes">Pointer to a SECURITY_ATTRIBUTES 
        /// structure that determines whether the returned handle can be 
        /// inherited by child processes.</param>
        /// <param name="flProtect"><para>Protection desired for the file 
        /// view, when the file is mapped. This parameter can be one of 
        /// the following values:</para>
        /// <list type="bullet">
        /// <item>PAGE_READONLY</item>
        /// <item>PAGE_READWRITE</item>
        /// <item>PAGE_WRITECOPY</item>
        /// <item>SEC_COMMIT</item>
        /// <item>SEC_IMAGE</item>
        /// <item>SEC_NOCACHE</item>
        /// <item>SEC_RESERVE</item>
        /// </list></param>
        /// <param name="dwMaximumSizeLow">High-order DWORD of the 
        /// maximum size of the file mapping object.</param>
        /// <param name="dwMaximumSizeHigh">Low-order DWORD of the 
        /// maximum size of the file mapping object. If this parameter 
        /// and dwMaximumSizeHigh are zero, the maximum size of the file 
        /// mapping object is equal to the current size of the file 
        /// identified by hFile.</param>
        /// <param name="lpName">Pointer to a null-terminated string 
        /// specifying the name of the mapping object.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the file mapping object. If the object existed 
        /// before the function call, the function returns a handle to 
        /// the existing object (with its current size, not the 
        /// specified size) and GetLastError returns ERROR_ALREADY_EXISTS. 
        /// If the function fails, the return value is NULL.</returns>
        /// <remarks>The name can have a "Global\" or "Local\" prefix to 
        /// explicitly create the object in the global or session name 
        /// space. The remainder of the name can contain any character 
        /// except the backslash character (\).</remarks>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr CreateFileMapping
            (
                IntPtr hFile,
                IntPtr lpAttributes,
                MemoryProtectionFlags flProtect,
                uint dwMaximumSizeLow,
                int dwMaximumSizeHigh,
                string lpName
            );

        /// <summary>
        /// The CreateHardLink function establishes a hard link between 
        /// an existing file and a new file. Currently, this function 
        /// is only supported on NTFS.
        /// </summary>
        /// <param name="lpFileName">Pointer to the name of the new file.
        /// </param>
        /// <param name="lpExistingFileName">Pointer to the name of the 
        /// existing file to which the link will point.</param>
        /// <param name="lpSecurityAttributes">Reserved; must be NULL.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool CreateHardLink
            (
                string lpFileName,
                string lpExistingFileName,
                IntPtr lpSecurityAttributes
            );

        /// <summary>
        /// Creates or opens a job object.
        /// </summary>
        /// <param name="lpJobAttributes">A pointer to
        /// a SECURITY_ATTRIBUTES structure that specifies
        /// the security descriptor for the job object
        /// and determines whether child processes can
        /// inherit the returned handle.
        /// If lpJobAttributes is NULL, the job object
        /// gets a default security descriptor and
        /// the handle cannot be inherited. The ACLs
        /// in the default security descriptor for
        /// a job object come from the primary
        /// or impersonation token of the creator.</param>
        /// <param name="lpName">The name of the job.
        /// The name is limited to MAX_PATH characters.
        /// Name comparison is case-sensitive.
        /// If lpName is NULL, the job is created
        /// without a name.</param>
        /// <returns>If the function succeeds, the return
        /// value is a handle to the job object.</returns>
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern JobObjectHandle CreateJobObject
            (
                IntPtr lpJobAttributes,
                string lpName
            );

        /// <summary>
        /// <para>The CreateNamedPipe function creates an instance of a 
        /// named pipe and returns a handle for subsequent pipe 
        /// operations. A named pipe server process uses this function 
        /// either to create the first instance of a specific named pipe 
        /// and establish its basic attributes or to create a new 
        /// instance of an existing named pipe.</para>
        /// <para>Windows Me/98/95: Named pipes cannot be created.
        /// </para></summary>
        /// <param name="lpName"><para>Pointer to the null-terminated 
        /// string that uniquely identifies the pipe. The string must 
        /// have the following form:</para>
        /// <para>\\.\pipe\pipename</para>
        /// <para>The pipename part of the name can include any character 
        /// other than a backslash, including numbers and special 
        /// characters. The entire pipe name string can be up to 256 
        /// characters long. Pipe names are not case sensitive.</para>
        /// <para>Windows Me/98/95: Pipe names cannot include a colon. 
        /// Therefore, if this pipe will be used from a Windows Me/98/95 
        /// client, do not include a colon in the name.</para></param>
        /// <param name="dwOpenMode">Pipe access mode, the overlapped 
        /// mode, the write-through mode, and the security access mode 
        /// of the pipe handle.</param>
        /// <param name="dwPipeMode"><para>Type, read, and wait modes 
        /// of the pipe handle.</para>
        /// <para>Type mode: PIPE_TYPE_BYTE or PIPE_TYPE_MESSAGE.</para>
        /// <para>Read mode: PIPE_READMODE_BYTE or PIPE_READMODE_MESSAGE.
        /// </para>
        /// <para>Wait mode: PIPE_WAIT or PIPE_NOWAIT.</para>
        /// </param>
        /// <param name="nMaxInstances">Maximum number of instances that 
        /// can be created for this pipe. The same number must be 
        /// specified for all instances. Acceptable values are in the 
        /// range 1 through PIPE_UNLIMITED_INSTANCES. If this parameter 
        /// is PIPE_UNLIMITED_INSTANCES, the number of pipe instances 
        /// that can be created is limited only by the availability of 
        /// system resources. If nMaxInstances is greater than 
        /// PIPE_UNLIMITED_INSTANCES, the return value is 
        /// ERROR_INVALID_PARAMETER.</param>
        /// <param name="nOutBufferSize">Number of bytes to reserve for 
        /// the output buffer.</param>
        /// <param name="nInBufferSize">Number of bytes to reserve for 
        /// the input buffer.</param>
        /// <param name="nDefaultTimeOut">Default time-out value, in 
        /// milliseconds, if the WaitNamedPipe function specifies 
        /// NMPWAIT_USE_DEFAULT_WAIT. Each instance of a named pipe 
        /// must specify the same value.</param>
        /// <param name="lpSecurityAttributes">Pointer to a 
        /// SECURITY_ATTRIBUTES structure that specifies a security 
        /// descriptor for the new named pipe and determines whether 
        /// child processes can inherit the returned handle. If 
        /// lpSecurityAttributes is NULL, the named pipe gets a default 
        /// security descriptor and the handle cannot be inherited. 
        /// The ACLs in the default security descriptor for a named pipe 
        /// grant full control to the LocalSystem account, administrators, 
        /// and the creator owner. They also grant read access to members 
        /// of the Everyone group and the anonymous account.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the server end of a named pipe instance. If the 
        /// function fails, the return value is INVALID_HANDLE_VALUE.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr CreateNamedPipe
            (
                string lpName,
                PipeOpenFlags dwOpenMode,
                PipeModeFlags dwPipeMode,
                uint nMaxInstances,
                uint nOutBufferSize,
                uint nInBufferSize,
                uint nDefaultTimeOut,
                IntPtr lpSecurityAttributes
            );

        /// <summary>
        /// Creates an anonymous pipe, and returns handles to the read 
        /// and write ends of the pipe.
        /// </summary>
        /// <param name="hReadPipe">Pointer to a variable that receives 
        /// the read handle for the pipe.</param>
        /// <param name="hWritePipe">Pointer to a variable that receives 
        /// the write handle for the pipe.</param>
        /// <param name="lpPipeAttributes">Pointer to a 
        /// SECURITY_ATTRIBUTES structure that determines whether the 
        /// returned handle can be inherited by child processes. 
        /// If lpPipeAttributes is NULL, the handle cannot be inherited.
        /// </param>
        /// <param name="nSize">Size of the buffer for the pipe, in 
        /// bytes. The size is only a suggestion; the system uses the 
        /// value to calculate an appropriate buffering mechanism. If 
        /// this parameter is zero, the system uses the default buffer 
        /// size.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool CreatePipe
            (
                ref IntPtr hReadPipe,
                ref IntPtr hWritePipe,
                IntPtr lpPipeAttributes,
                uint nSize
            );

        /// <summary>
        /// Enables a debugger to attach to an active process and 
        /// debug it.
        /// </summary>
        /// <param name="dwProcessId"> Identifier of the process to be 
        /// debugged. The debugger is granted debugging access to the 
        /// process as if it created the process with the 
        /// DEBUG_ONLY_THIS_PROCESS flag.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool DebugActiveProcess
            (
                uint dwProcessId
            );

        /// <summary>
        /// Stops the debugger from debugging the specified process.
        /// </summary>
        /// <param name="dwProcessId">Identifier of the process 
        /// to stop debugging.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Included in: Windows XP/2003.</remarks>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool DebugActiveProcessStop
            (
                uint dwProcessId
            );

        /// <summary>
        /// The DebugBreak function causes a breakpoint exception to 
        /// occur in the current process. This allows the calling thread 
        /// to signal the debugger to handle the exception.
        /// </summary>
        [DllImport(DllName, SetLastError = false)]
        public static extern void DebugBreak();

        /// <summary>
        /// The DebugBreakProcess function causes a breakpoint exception 
        /// to occur in the specified process. This allows the calling 
        /// thread to signal the debugger to handle the exception.
        /// </summary>
        /// <param name="Process">Handle to the process.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Included in: Windows XP/2003.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DebugBreakProcess
            (
                IntPtr Process
            );

        /// <summary>
        /// Sets the action to be performed when the debugging thread 
        /// exits.
        /// </summary>
        /// <param name="KillOnExit">If this parameter is TRUE, the 
        /// debug thread will kill the process being debugged on exit. 
        /// Otherwise, the debug thread will detach from the process 
        /// being debugged on exit.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Included in: Windows XP/2003.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DebugSetProcessKillOnExit
            (
                bool KillOnExit
            );

        /// <summary>
        /// Decrypts an encrypted file or directory.
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that specifies the name of the file to decrypt.
        /// </para>
        /// <para>The caller must have the FILE_READ_DATA, FILE_WRITE_DATA, 
        /// FILE_READ_ATTRIBUTES, FILE_WRITE_ATTRIBUTES, and SYNCHRONIZE 
        /// access rights.</para></param>
        /// <param name="dwReserved">Reserved; must be zero.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DecryptFile
            (
                string lpFileName,
                int dwReserved
            );

        /// <summary>
        /// Defines, redefines, or deletes MS-DOS device names.
        /// </summary>
        /// <param name="dwFlags">Controllable aspects of the 
        /// DefineDosDevice function.</param>
        /// <param name="lpDeviceName">Pointer to an MS-DOS device 
        /// name string specifying the device the function is defining, 
        /// redefining, or deleting. The device name string must not 
        /// have a trailing colon, unless a drive letter (C or D, for 
        /// example) is being defined, redefined, or deleted. In no 
        /// case is a trailing backslash allowed.</param>
        /// <param name="lpTargetPath">Pointer to a path string that 
        /// will implement this device. The string is an MS-DOS path 
        /// string unless the DDD_RAW_TARGET_PATH flag is specified, 
        /// in which case this string is a path string.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DefineDosDevice
            (
                DosDeviceFlags dwFlags,
                string lpDeviceName,
                string lpTargetPath
            );

        /// <summary>
        /// Deletes an existing file.
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that specifies the file to be deleted.</para>
        /// <para>In the ANSI version of this function, the name is 
        /// limited to MAX_PATH characters. To extend this limit to 
        /// 32,767 wide characters, call the Unicode version of the 
        /// function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95:  This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>If an application attempts to delete a file that 
        /// does not exist, the DeleteFile function fails.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DeleteFile
            (
                string lpFileName
            );

        /// <summary>
        /// Unmounts the volume from the specified volume mount point.
        /// </summary>
        /// <param name="lpszVolumeMountPoint"> Pointer to a string that 
        /// indicates the volume mount point to be unmounted. This may be 
        /// a root directory (X:\, in which case the DOS drive letter 
        /// assignment is removed) or a directory on a volume (X:\mnt\). 
        /// A trailing backslash is required.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DeleteVolumeMountPoint
            (
                string lpszVolumeMountPoint
            );

        /// <summary>
        /// The DisableThreadLibraryCalls function disables 
        /// the DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications 
        /// for the specified dynamic-link library (DLL). 
        /// This can reduce the size of the working set 
        /// for some applications.
        /// </summary>
        /// <param name="hModule">Handle to the DLL module for 
        /// which the DLL_THREAD_ATTACH and DLL_THREAD_DETACH 
        /// notifications are to be disabled. The LoadLibrary, 
        /// LoadLibraryEx, or GetModuleHandle function returns 
        /// this handle. Note that you cannot call GetModuleHandle 
        /// with NULL because this returns the base address of the 
        /// executable image, not the DLL image. </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DisableThreadLibraryCalls
            (
                IntPtr hModule
            );

        /// <summary>
        /// Disconnects the server end of a named pipe instance from a 
        /// client process.
        /// </summary>
        /// <param name="hNamedPipe">Handle to an instance of a named 
        /// pipe. This handle must be created by the CreateNamedPipe 
        /// function.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool DisconnectNamedPipe
            (
                IntPtr hNamedPipe
            );

        /// <summary>
        /// The DuplicateHandle function duplicates an object handle. 
        /// The duplicate handle refers to the same object as the 
        /// original handle. Therefore, any changes to the object are 
        /// reflected through both handles. For example, the current 
        /// file mark for a file handle is always the same for both 
        /// handles.
        /// </summary>
        /// <param name="hSourceProcessHandle"><para>Handle to the 
        /// process with the handle to duplicate.</para>
        /// <para>The handle must have the PROCESS_DUP_HANDLE 
        /// access right.</para></param>
        /// <param name="hSourceHandle">Handle to duplicate. This is 
        /// an open object handle that is valid in the context of the 
        /// source process.</param>
        /// <param name="hTargetProcessHandle">Handle to the process that 
        /// is to receive the duplicated handle. The handle must have 
        /// the PROCESS_DUP_HANDLE access right.</param>
        /// <param name="lpTargetHandle"><para> Pointer to a variable 
        /// that receives the value of the duplicate handle. This handle 
        /// value is valid in the context of the target process.</para>
        /// <para>If lpTargetHandle is NULL, the function duplicates 
        /// the handle, but does not return the duplicate handle value 
        /// to the caller. This behavior exists only for backward 
        /// compatibility with previous versions of this function. 
        /// You should not use this feature, as you will lose system 
        /// resources until the target process terminates.</para></param>
        /// <param name="dwDesiredAccess"><para>Access requested for the 
        /// new handle. For the flags that can be specified for each 
        /// object type, see the following Remarks section.</para>
        /// <para>This parameter is ignored if the dwOptions parameter 
        /// speies the DUPLICATE_SAME_ACCESS flag. Otherwise, the 
        /// flags that can be specified depend on the type of object 
        /// whose handle is to be duplicated.</para></param>
        /// <param name="bInheritHandle">Indicates whether the handle 
        /// is inheritable.</param>
        /// <param name="dwOptions">Optional actions.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool DuplicateHandle
            (
                IntPtr hSourceProcessHandle,
                IntPtr hSourceHandle,
                IntPtr hTargetProcessHandle,
                ref IntPtr lpTargetHandle,
                uint dwDesiredAccess,
                bool bInheritHandle,
                DuplicateHandleFlags dwOptions
            );

        /// <summary>
        /// The EncryptFile function encrypts a file or directory. 
        /// All data streams in a file are encrypted. All new files 
        /// created in an encrypted directory are encrypted.
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that specifies the name of the file or directory to 
        /// encrypt.</para>
        /// <para>The caller must have the FILE_READ_DATA, FILE_WRITE_DATA, 
        /// FILE_READ_ATTRIBUTES, FILE_WRITE_ATTRIBUTES, and SYNCHRONIZE 
        /// access rights.</para></param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool EncryptFile
            (
                string lpFileName
            );

        /// <summary>
        /// The EnumSystemCodePages function enumerates the code pages 
        /// that are either installed on or supported by a system. 
        /// The dwFlags parameter determines whether the function 
        /// enumerates installed or supported code pages. The function 
        /// enumerates the code pages by passing code page identifiers, 
        /// one at a time, to the specified application defined callback 
        /// function. This continues until all of the installed or supported 
        /// code page identifiers have been passed to the callback function, 
        /// or the callback function returns FALSE.
        /// </summary>
        /// <param name="lpCodePageEnumProc">Pointer to an application 
        /// defined callback function. The EnumSystemCodePages function 
        /// enumerates code pages by making repeated calls to this callback 
        /// function.</param>
        /// <param name="dwFlags">Specifies the code pages to enumerate.
        /// </param>
        /// <returns>If the function succeeds, the return values 
        /// is a nonzero value.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool EnumSystemCodePages
            (
                CodePageEnumProc lpCodePageEnumProc,
                CodePageEnumFlags dwFlags
            );

        /// <summary>
        /// Ends a process and all its threads.
        /// </summary>
        /// <param name="uExitCode">Exit code for the process and 
        /// all threads terminated as a result of this call.</param>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern void ExitProcess
            (
                uint uExitCode
            );

        /// <summary>
        /// The FatalAppExit function displays a message box and 
        /// terminates the application when the message box is closed. 
        /// If the system is running with a debug version of 
        /// kernel32.dll, the message box gives the user the 
        /// opportunity to terminate the application or to cancel 
        /// the message box and return to the application that called 
        /// FatalAppExit.</summary>
        /// <param name="uAction">Reserved; must be zero.</param>
        /// <param name="lpMessageText">Pointer to a null-terminated 
        /// string that is displayed in the message box.</param>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern void FatalAppExit
            (
                uint uAction,
                string lpMessageText
            );

        /// <summary>
        /// The FatalExit function transfers execution control to 
        /// the debugger. The behavior of the debugger thereafter is 
        /// specific to the type of debugger used.
        /// </summary>
        /// <param name="ExitCode">Error code associated with the exit.
        /// </param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void FatalExit
            (
                int ExitCode
            );

        /// <summary>
        /// Converts a file time to system time format.
        /// </summary>
        /// <param name="lpFileTime"><para>Pointer to a FILETIME 
        /// structure containing the file time to convert to system date 
        /// and time format.</para>
        /// <para>This value must be less than 0x8000000000000000. 
        /// Otherwise, the function fails.</para></param>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// to receive the converted file time.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FileTimeToSystemTime
            (
                ref FILETIME lpFileTime,
                ref SYSTEMTIME lpSystemTime
            );

        /// <summary>
        /// Sets the character attributes for a specified number 
        /// of character cells, beginning at the specified coordinates 
        /// in a screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_WRITE access right.</param>
        /// <param name="wAttribute">Attributes to use when writing to the 
        /// console screen buffer.</param>
        /// <param name="nLength">Number of character cells to be set to the 
        /// specified color attributes.</param>
        /// <param name="dwWriteCoord">A COORD structure that specifies the 
        /// console screen buffer coordinates of the first cell whose attributes 
        /// are to be set.</param>
        /// <param name="lpNumberOfAttrsWritten">Pointer to a variable that receives 
        /// the number of character cells whose attributes were actually set.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error 
        /// information, call GetLastError.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FillConsoleOutputAttribute
            (
                IntPtr hConsoleOutput,
                short wAttribute,
                int nLength,
                COORD dwWriteCoord,
                ref int lpNumberOfAttrsWritten
            );

        /// <summary>
        /// Writes a character to the console screen buffer a specified number 
        /// of times, beginning at the specified coordinates.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_WRITE access right.</param>
        /// <param name="cCharacter">Character to write to the console screen 
        /// buffer.</param>
        /// <param name="nLength">Number of character cells to which the 
        /// character should be written.</param>
        /// <param name="dwWriteCoord">A COORD structure that specifies the 
        /// console screen buffer coordinates of the first cell to which the 
        /// character is to be written.</param>
        /// <param name="lpNumberOfCharsWritten">Pointer to a variable that 
        /// receives the number of characters actually written to the console 
        /// screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        /// <remarks>If the number of characters to write to extends beyond the 
        /// end of the specified row in the console screen buffer, characters 
        /// are written to the next row. If the number of characters to write 
        /// to extends beyond the end of the console screen buffer, the characters 
        /// are written up to the end of the console screen buffer.
        /// The attribute values at the positions written are not changed.
        /// This function uses either Unicode characters or 8-bit characters 
        /// from the console's current code page. The console's code page defaults 
        /// initially to the system's OEM code page. To change the console's code 
        /// page, use the SetConsoleCP or SetConsoleOutputCP functions, or use the 
        /// chcp or mode con cp select= commands.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FillConsoleOutputCharacter
            (
                IntPtr hConsoleOutput,
                char cCharacter,
                int nLength,
                COORD dwWriteCoord,
                ref int lpNumberOfCharsWritten
            );

        /// <summary>
        /// The FindClose function closes the specified search handle. 
        /// The FindFirstFile, FindFirstFileEx, and FindNextFile functions 
        /// use the search handle to locate files with names that match 
        /// a given name.
        /// </summary>
        /// <param name="hFindFile">File search handle. This handle must 
        /// have been previously opened by the FindFirstFile function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindClose
            (
                IntPtr hFindFile
            );

        /// <summary>
        /// <para>The FindFirstFile function searches a directory for a 
        /// file whose name matches the specified file name. FindFirstFile 
        /// examines subdirectory names as well as file names.</para>
        /// <para>To specify additional attributes to be used in the search, 
        /// use the FindFirstFileEx function.</para>
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that specifies a valid directory or path and file name, 
        /// which can contain wildcard characters (* and ?). If the string 
        /// ends with a wildcard, a period, or a directory name, the user 
        /// must have access to the root and all subdirectories on the path.
        /// </para>
        /// <para>In the ANSI version of this function, the name is limited 
        /// to MAX_PATH characters. To extend this limit to 32,767 wide 
        /// characters, call the Unicode version of the function and prepend 
        /// "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed MAX_PATH 
        /// characters.</para></param>
        /// <param name="lpFindFileData">Pointer to the WIN32_FIND_DATA 
        /// structure that receives information about the found file or 
        /// subdirectory.</param>
        /// <returns>If the function succeeds, the return value is a search 
        /// handle used in a subsequent call to FindNextFile or FindClose.
        /// If the function fails, the return value is INVALID_HANDLE_VALUE.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr FindFirstFile
            (
                string lpFileName,
                out WIN32_FIND_DATA lpFindFileData
            );

        /// <summary>
        /// The FindFirstVolume function returns the name of a volume 
        /// on a computer. FindFirstVolume is used to begin scanning 
        /// the volumes of a computer.
        /// </summary>
        /// <param name="lpszVolumeName">Pointer to a buffer that receives 
        /// a null-terminated string that specifies the unique volume name 
        /// of the first volume found.</param>
        /// <param name="cchBufferLength">Length of the buffer to receive 
        /// the name, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is a search 
        /// handle used in a subsequent call to the FindNextVolume and 
        /// FindVolumeClose functions. If the function fails to find any 
        /// volumes, the return value is the INVALID_HANDLE_VALUE error code.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr FindFirstVolume
            (
                StringBuilder lpszVolumeName,
                int cchBufferLength
            );

        /// <summary>
        /// The FindFirstVolumeMountPoint function returns the name of 
        /// a volume mount point on the specified volume. 
        /// FindFirstVolumeMountPoint is used to begin scanning the 
        /// volume mount points on a volume.
        /// </summary>
        /// <param name="lpszRootPathName">Unique volume name of the volume 
        /// to scan for volume mount points. A trailing backslash is required.
        /// </param>
        /// <param name="lpszVolumeMountPoint">Pointer to a buffer that 
        /// receives the name of the first volume mount point found.</param>
        /// <param name="cchBufferLength">Length of the buffer that receives 
        /// the volume mount point name, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is a search 
        /// handle used in a subsequent call to the FindNextVolumeMountPoint 
        /// and FindVolumeMountPointClose functions.
        /// If the function fails to find a volume mount point on the volume, 
        /// the return value is the INVALID_HANDLE_VALUE error code.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr FindFirstVolumeMountPoint
            (
                string lpszRootPathName,
                StringBuilder lpszVolumeMountPoint,
                int cchBufferLength
            );

        /// <summary>
        /// Continues a file search from a previous call to the 
        /// FindFirstFile function.
        /// </summary>
        /// <param name="hFindFile">Search handle returned by a 
        /// previous call to the FindFirstFile function.</param>
        /// <param name="lpFindFileData">Pointer to the WIN32_FIND_DATA 
        /// structure that receives information about the found file 
        /// or subdirectory. The structure can be used in subsequent 
        /// calls to FindNextFile to see the found file or directory.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindNextFile
            (
                IntPtr hFindFile,
                ref WIN32_FIND_DATA lpFindFileData
            );

        /// <summary>
        /// The FindNextVolume function continues a volume search started 
        /// by a call to the FindFirstVolume function. FindNextVolume finds 
        /// one volume per call.
        /// </summary>
        /// <param name="hFindVolume">Volume search handle returned by a 
        /// previous call to the FindFirstVolume function.</param>
        /// <param name="lpszVolumeName">Pointer to a string that receives 
        /// the unique volume name found.</param>
        /// <param name="cchBufferLength">Length of the buffer that receives 
        /// the name, in TCHARs. </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindNextVolume
            (
                IntPtr hFindVolume,
                StringBuilder lpszVolumeName,
                int cchBufferLength
            );

        /// <summary>
        /// The FindNextVolumeMountPoint function continues a volume mount 
        /// point search started by a call to the FindFirstVolumeMountPoint 
        /// function. FindNextVolumeMountPoint finds one volume mount point 
        /// per call.
        /// </summary>
        /// <param name="hFindVolumeMountPoint">Mount-point search handle 
        /// returned by a previous call to the FindFirstVolumeMountPoint 
        /// function.</param>
        /// <param name="lpszVolumeMountPoint">Pointer to a string that 
        /// receives the name of the volume mount point found.</param>
        /// <param name="cchBufferLength">Length of the buffer that 
        /// receives the names, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindNextVolumeMountPoint
            (
                IntPtr hFindVolumeMountPoint,
                StringBuilder lpszVolumeMountPoint,
                int cchBufferLength
            );

        /// <summary>
        /// Function determines the location of a resource with the 
        /// specified type and name in the specified module.
        /// </summary>
        /// <param name="hModule">Handle to the module whose executable 
        /// file contains the resource. A value of NULL specifies the 
        /// module handle associated with the image file that the 
        /// operating system used to create the current process.
        /// </param>
        /// <param name="lpName">Specifies the name of the resource.
        /// </param>
        /// <param name="lpType">Specifies the resource type.</param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true,
            CharSet = CharSet.Ansi, EntryPoint = "FindResourceA")]
        public static extern IntPtr FindResource
            (
                IntPtr hModule,
                string lpName,
                ResourceTypes lpType
            );

        /// <summary>
        /// Function determines the location of a resource with the 
        /// specified type and name in the specified module.
        /// </summary>
        /// <param name="hModule">Handle to the module whose executable 
        /// file contains the resource. A value of NULL specifies the 
        /// module handle associated with the image file that the 
        /// operating system used to create the current process.
        /// </param>
        /// <param name="lpName">Specifies the name of the resource.
        /// </param>
        /// <param name="lpType">Specifies the resource type.</param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true,
            CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource
            (
                IntPtr hModule,
                string lpName,
                string lpType
            );

        /// <summary>
        /// The FindVolumeClose function closes the specified volume 
        /// search handle. The FindFirstVolume and FindNextVolume 
        /// functions use this search handle to locate volumes.
        /// </summary>
        /// <param name="hFindVolume">Volume search handle to close. 
        /// This handle must have been previously opened by the 
        /// FindFirstVolume function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindVolumeClose
            (
                IntPtr hFindVolume
            );

        /// <summary>
        /// The FindVolumeMountPointClose function closes the specified 
        /// mount-point search handle. The FindFirstVolumeMountPoint 
        /// and FindNextVolumeMountPoint functions use this search handle 
        /// to locate volume mount points on a specified volume.
        /// </summary>
        /// <param name="hFindVolumeMountPoint">Mount-point search handle 
        /// to close. This handle must have been previously opened by the 
        /// FindFirstVolumeMountPoint function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FindVolumeMountPointClose
            (
                IntPtr hFindVolumeMountPoint
            );

        /// <summary>
        /// The FlushConsoleInputBuffer function flushes the console 
        /// input buffer. All input records currently in the input buffer 
        /// are discarded.
        /// </summary>
        /// <param name="hConsoleInput">Handle to the console input buffer. 
        /// The handle must have the GENERIC_WRITE access right.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FlushConsoleInputBuffer
            (
                int hConsoleInput
            );

        /// <summary>
        /// The FormatMessage function formats a message string. The 
        /// function requires a message definition as input. The message 
        /// definition can come from a buffer passed into the function. 
        /// It can come from a message table resource in an 
        /// already-loaded module. Or the caller can ask the function to 
        /// search the system's message table resource(s) for the 
        /// message definition. The function finds the message 
        /// definition in a message table resource based on a message 
        /// identifier and a language identifier. The function copies 
        /// the formatted message text to an output buffer, processing 
        /// any embedded insert sequences if requested.
        /// </summary>
        /// <param name="dwFlags"> Formatting options, and how to 
        /// interpret the lpSource parameter. The low-order byte of 
        /// dwFlags specifies how the function handles line breaks 
        /// in the output buffer. The low-order byte can also specify 
        /// the maximum width of a formatted output line.</param>
        /// <param name="lpSource">Location of the message definition.
        /// </param>
        /// <param name="dwMessageId">Message identifier for the 
        /// requested message. This parameter is ignored if dwFlags 
        /// includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="dwLanguageId">Language identifier for the 
        /// requested message.</param>
        /// <param name="lpBuffer"><para>Pointer to a buffer that 
        /// receives null-terminated string that specifies the formatted 
        /// message. If dwFlags includes FORMAT_MESSAGE_ALLOCATE_BUFFER, 
        /// the function allocates a buffer using the LocalAlloc function, 
        /// and places the pointer to the buffer at the address specified 
        /// in lpBuffer.</para>
        /// <para>This buffer cannot be larger than 64K bytes.</para>
        /// </param>
        /// <param name="nSize"><para>If the 
        /// FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set, this 
        /// parameter specifies the maximum number of TCHARs that can 
        /// be stored in the output buffer. If 
        /// FORMAT_MESSAGE_ALLOCATE_BUFFER is set, this parameter 
        /// specifies the minimum number of TCHARs to allocate for an 
        /// output buffer.</para>
        /// <para>The output buffer cannot be larger than 64K bytes.
        /// </para></param>
        /// <param name="Arguments"><para>Pointer to an array of values that 
        /// are used as insert values in the formatted message. A %1 in 
        /// the format string indicates the first value in the Arguments 
        /// array; a %2 indicates the second argument; and so on.</para>
        /// <para>Windows Me/98/95:  No single insertion string may exceed 
        /// 1023 characters in length.</para></param>
        /// <returns>If the function succeeds, the return value is 
        /// the number of TCHARs stored in the output buffer, 
        /// excluding the terminating null character. If the function 
        /// fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint FormatMessage
            (
                FormatMessageFlags dwFlags,
                IntPtr lpSource,
                uint dwMessageId,
                uint dwLanguageId,
                IntPtr lpBuffer,
                uint nSize,
                IntPtr Arguments
            );

        /// <summary>
        /// Flushes the buffers of the specified file and causes all 
        /// buffered data to be written to the file.
        /// </summary>
        /// <param name="hFile">Handle to an open file.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FlushFileBuffers
            (
                IntPtr hFile
            );

        /// <summary>
        /// Writes to the disk a byte range within a mapped view of 
        /// a file.
        /// </summary>
        /// <param name="lpBaseAddress">Pointer to the base address 
        /// of the byte range to be flushed to the disk representation 
        /// of the mapped file.</param>
        /// <param name="dwNumBytesToFlush">Number of bytes to flush. 
        /// If dwNumberOfBytesToFlush is zero, the file is flushed from 
        /// the base address to the end of the mapping.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FlushViewOfFile
            (
                IntPtr lpBaseAddress,
                int dwNumBytesToFlush
            );

        /// <summary>
        /// The FormatMessage function formats a message string. The 
        /// function requires a message definition as input. The message 
        /// definition can come from a buffer passed into the function. 
        /// It can come from a message table resource in an 
        /// already-loaded module. Or the caller can ask the function to 
        /// search the system's message table resource(s) for the 
        /// message definition. The function finds the message 
        /// definition in a message table resource based on a message 
        /// identifier and a language identifier. The function copies 
        /// the formatted message text to an output buffer, processing 
        /// any embedded insert sequences if requested.
        /// </summary>
        /// <param name="dwFlags"> Formatting options, and how to 
        /// interpret the lpSource parameter. The low-order byte of 
        /// dwFlags specifies how the function handles line breaks 
        /// in the output buffer. The low-order byte can also specify 
        /// the maximum width of a formatted output line.</param>
        /// <param name="lpSource">Location of the message definition.
        /// </param>
        /// <param name="dwMessageId">Message identifier for the 
        /// requested message. This parameter is ignored if dwFlags 
        /// includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="dwLanguageId">Language identifier for the 
        /// requested message.</param>
        /// <param name="lpBuffer"><para>Pointer to a buffer that 
        /// receives null-terminated string that specifies the formatted 
        /// message. If dwFlags includes FORMAT_MESSAGE_ALLOCATE_BUFFER, 
        /// the function allocates a buffer using the LocalAlloc function, 
        /// and places the pointer to the buffer at the address specified 
        /// in lpBuffer.</para>
        /// <para>This buffer cannot be larger than 64K bytes.</para>
        /// </param>
        /// <param name="nSize"><para>If the 
        /// FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set, this 
        /// parameter specifies the maximum number of TCHARs that can 
        /// be stored in the output buffer. If 
        /// FORMAT_MESSAGE_ALLOCATE_BUFFER is set, this parameter 
        /// specifies the minimum number of TCHARs to allocate for an 
        /// output buffer.</para>
        /// <para>The output buffer cannot be larger than 64K bytes.
        /// </para></param>
        /// <param name="Arguments"><para>Pointer to an array of values that 
        /// are used as insert values in the formatted message. A %1 in 
        /// the format string indicates the first value in the Arguments 
        /// array; a %2 indicates the second argument; and so on.</para>
        /// <para>Windows Me/98/95:  No single insertion string may exceed 
        /// 1023 characters in length.</para></param>
        /// <returns>If the function succeeds, the return value is 
        /// the number of TCHARs stored in the output buffer, 
        /// excluding the terminating null character. If the function 
        /// fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint FormatMessage
            (
                FormatMessageFlags dwFlags,
                uint lpSource,
                uint dwMessageId,
                uint dwLanguageId,
                StringBuilder lpBuffer,
                uint nSize,
                uint Arguments
            );

        /// <summary>
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FreeConsole();

        /// <summary>
        /// The FreeLibrary function decrements the reference count 
        /// of the loaded dynamic-link library (DLL). When the reference 
        /// count reaches zero, the module is unmapped from the address 
        /// space of the calling process and the handle is no longer valid.
        /// </summary>
        /// <param name="hModule">Handle to the loaded DLL module.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool FreeLibrary
            (
                IntPtr hModule
            );

        /// <summary>
        /// The FreeLibraryAndExitThread function decrements 
        /// the reference count of a loaded dynamic-link library 
        /// (DLL) by one, just as FreeLibrary does, then calls 
        /// ExitThread to terminate the calling thread. 
        /// The function does not return.
        /// </summary>
        /// <param name="hModule">Handle to the DLL module whose 
        /// reference count the function decrements.</param>
        /// <param name="dwExitCode">Exit code for the calling thread.</param>
        /// <remarks>This function does not return a value. 
        /// Invalid module handles are ignored.</remarks>
        [DllImport(DllName, SetLastError = false)]
        public static extern void FreeLibraryAndExitThread
            (
                IntPtr hModule,
                int dwExitCode
            );

        /// <summary>
        /// Sends a specified signal to a console process group that 
        /// shares the console associated with the calling process.
        /// </summary>
        /// <param name="dwCtrlEvent">Type of signal to generate.</param>
        /// <param name="dwProcessGroupId">Identifier of the process group 
        /// to receive the signal. A process group is created when the 
        /// CREATE_NEW_PROCESS_GROUP flag is specified in a call to the 
        /// CreateProcess function. The process identifier of the new process 
        /// is also the process group identifier of a new process group. 
        /// The process group includes all processes that are descendants 
        /// of the root process. Only those processes in the group that 
        /// share the same console as the calling process receive the signal. 
        /// In other words, if a process in the group creates a new console, 
        /// that process does not receive the signal, nor do its descendants.
        /// If this parameter is zero, the signal is generated in all processes 
        /// that share the console of the calling process.</param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GenerateConsoleCtrlEvent
            (
                ConsoleEvent dwCtrlEvent,
                int dwProcessGroupId
            );

        /// <summary>
        /// The GetBinaryType function determines whether a file is 
        /// executable, and if so, what type of executable file it is. 
        /// That last property determines which subsystem an executable 
        /// file runs under.
        /// </summary>
        /// <param name="lpApplicationName"><para>Pointer to a 
        /// null-terminated string that contains the full path of 
        /// the file whose binary type is to be determined.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para></param>
        /// <param name="lpBinaryType">Pointer to a variable to receive 
        /// information about the executable type of the file specified 
        /// by lpApplicationName.</param>
        /// <returns>If the file is executable, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetBinaryType
            (
                string lpApplicationName,
                out ExecutableKind lpBinaryType
            );

        /// <summary>
        /// The GetCompressedFileSize function retrieves the actual 
        /// number of bytes of disk storage used to store a specified 
        /// file. If the file is located on a volume that supports 
        /// compression and the file is compressed, the value obtained 
        /// is the compressed size of the specified file. If the file 
        /// is located on a volume that supports sparse files and the 
        /// file is a sparse file, the value obtained is the sparse 
        /// size of the specified file.
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that specifies the name of the file.</para>
        /// <para>Do not specify the name of a file on a nonseeking device, 
        /// such as a pipe or a communications device, as its file size has 
        /// no meaning.</para></param>
        /// <param name="lpFileSizeHigh">Pointer to a variable that receives 
        /// the high-order DWORD of the compressed file size.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// low-order DWORD of the actual number of bytes of disk storage 
        /// used to store the specified file, and if lpFileSizeHigh is 
        /// non-NULL, the function puts the high-order DWORD of that 
        /// actual value into the DWORD pointed to by that parameter. 
        /// This is the compressed file size for compressed files, the 
        /// actual file size for noncompressed files.
        /// If the function fails, and lpFileSizeHigh is NULL, the return 
        /// value is INVALID_FILE_SIZE.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint GetCompressedFileSize
            (
                string lpFileName,
                out int lpFileSizeHigh
            );

        /// <summary>
        /// Retrieves the text for the specified console alias 
        /// and executable.
        /// </summary>
        /// <param name="lpSource">Pointer to a null-terminated string 
        /// that specifies the console alias whose text is to be 
        /// retrieved.</param>
        /// <param name="lpTargetBuffer">Pointer to a buffer that receives 
        /// the text associated with the console alias.</param>
        /// <param name="TargetBufferLength">Size of the buffer pointed 
        /// to by lpTargetBuffer, in bytes.</param>
        /// <param name="lpExeName">Pointer to a null-terminated string 
        /// that specifies the name of the executable file.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetConsoleAlias
            (
                string lpSource,
                StringBuilder lpTargetBuffer,
                int TargetBufferLength,
                string lpExeName
            );

        /// <summary>
        /// Retrieves all defined console aliases for the specified 
        /// executable.
        /// </summary>
        /// <param name="lpAliasBuffer">Pointer to a buffer that 
        /// receives the aliases.</param>
        /// <param name="AliasBufferLength">Size of the buffer pointed 
        /// to by lpAliasBuffer, in bytes.</param>
        /// <param name="lpExeName">Pointer to a string that specifies 
        /// the executable file whose aliases are to be retrieved.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetConsoleAliases
            (
                byte[] lpAliasBuffer,
                int AliasBufferLength,
                string lpExeName
            );

        /// <summary>
        /// Retrieves the required size for the buffer used by the 
        /// GetConsoleAliases function.
        /// </summary>
        /// <param name="lpExeName">Name of the executable file whose 
        /// console aliases are to be retrieved.</param>
        /// <returns>Size of the buffer required to store all console 
        /// aliases defined for this executable file, in bytes.
        /// </returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern int GetConsoleAliasesLength
            (
                string lpExeName
            );

        /// <summary>
        /// Retrieves the names of all executable files with console 
        /// aliases defined. 
        /// </summary>
        /// <param name="lpExeNameBuffer">Pointer to a buffer that 
        /// receives the names of the executable files.</param>
        /// <param name="ExeNameBufferLength">Size of the buffer pointed 
        /// to by lpExeNameBuffer, in bytes.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetConsoleAliasExes
            (
                byte[] lpExeNameBuffer,
                int ExeNameBufferLength
            );

        /// <summary>
        /// Retrieves the required size for the buffer used by the 
        /// GetConsoleAliasExes function.
        /// </summary>
        /// <returns>Size of the buffer required to store the names of 
        /// all executable files that have console aliases defined, in 
        /// bytes.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern int GetConsoleAliasExesLength();

        /// <summary>
        /// The GetConsoleCP function retrieves the input code page 
        /// used by the console associated with the calling process. 
        /// A console uses its input code page to translate keyboard 
        /// input into the corresponding character value.
        /// </summary>
        /// <returns><para>The return value is a code that identifies 
        /// the code page.</para>
        /// <para>Japanese and Korean Windows Me/98/95: The GetConsoleCP 
        /// function returns the VM code page, because the OEM code page 
        /// can be either 437 or DBCS.</para>
        /// <para>Windows Me/98/95: On all language versions besides 
        /// Japanese and Korean, GetConsoleCP returns the OEM code 
        /// page.</para></returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint GetConsoleCP();

        /// <summary>
        /// Retrieves information about the size and visibility of the 
        /// cursor for the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="lpConsoleCursorInfo">Pointer to a CONSOLE_CURSOR_INFO 
        /// structure that receives information about the console's cursor.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetConsoleCursorInfo
            (
                IntPtr hConsoleOutput,
                ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
            );

        /// <summary>
        /// Retrieves the display mode of the current console.
        /// </summary>
        /// <param name="lpModeFlags">Display mode of the console.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetConsoleDisplayMode
            (
                out ConsoleDisplayMode lpModeFlags
            );

        /// <summary>
        /// Undocumented function.
        /// </summary>
        /// <param name="hConsole"></param>
        /// <param name="bUnknown"></param>
        /// <param name="dwTableSize"></param>
        /// <param name="pFontTable"></param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetConsoleFontInfo
            (
                IntPtr hConsole,
                bool bUnknown,
                int dwTableSize,
                CONSOLE_FONT_INFO[] pFontTable
            );

        /// <summary>
        /// Retrieves the size of the font used by the specified console 
        /// screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="nFont">Index of the font whose size is to be retrieved. 
        /// This index is obtained by calling the GetCurrentConsoleFont 
        /// function.</param>
        /// <returns>If the function succeeds, the return value is a COORD 
        /// structure that contains the width and height of each character 
        /// in the font. The X member contains the width, while the Y member 
        /// contains the height. If the function fails, the width and the height 
        /// are zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern COORD GetConsoleFontSize
            (
                IntPtr hConsoleOutput,
                int nFont
            );

        /// <summary>
        /// Retrieves the current input mode of a console's input buffer 
        /// or the current output mode of a console screen buffer.
        /// </summary>
        /// <param name="hConsoleHandle">Handle to a console input buffer 
        /// or a console screen buffer. The handle must have the GENERIC_READ 
        /// access right.</param>
        /// <param name="lpMode">Pointer to a variable that receives the 
        /// current mode of the specified buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetConsoleMode
            (
                IntPtr hConsoleHandle,
                ref ConsoleModeFlags lpMode
            );

        /// <summary>
        /// The GetConsoleOutputCP function retrieves the output code 
        /// page used by the console associated with the calling 
        /// process. A console uses its output code page to translate 
        /// the character values written by the various output functions 
        /// into the images displayed in the console window.
        /// </summary>
        /// <returns><para>The return value is a code that identifies 
        /// the code page.</para>
        /// <para>Japanese and Korean Windows Me/98/95: The 
        /// GetConsoleOutputCP function returns the VM code page, 
        /// because the OEM code page can be either 437 or DBCS.</para>
        /// <para>Windows Me/98/95: On all language versions besides 
        /// Japanese and Korean, GetConsoleOutputCP returns the OEM 
        /// code page.</para></returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint GetConsoleOutputCP();

        /// <summary>
        /// Retrieves a list of the processes attached to the current 
        /// console.
        /// </summary>
        /// <param name="lpdwProcessList">Pointer to a buffer that 
        /// receives an array of process identifiers. The total size 
        /// of the output buffer required will be less than 64K.</param>
        /// <param name="dwProcessCount">Maximum number of process 
        /// identifiers that can be stored in the lpdwProcessList 
        /// buffer.</param>
        /// <returns>The return value is the number of processes that 
        /// are attached to the current console. If the return value 
        /// is less than or equal to dwProcessCount, it is also the 
        /// number of process identifiers stored in the lpdwProcessList 
        /// buffer. If the return value is greater than dwProcessCount, 
        /// the lpdwProcessList buffer is too small to hold all the 
        /// valid process identifiers. The function will have stored 
        /// no identifiers in the buffer. In this situation, use the 
        /// return value to allocate a buffer that is large enough 
        /// to store the entire list, and call the function again.
        /// If the return value is zero, the function has failed, 
        /// because every console has at least one process associated 
        /// with it. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetConsoleProcessList
            (
                int[] lpdwProcessList,
                int dwProcessCount
            );

        /// <summary>
        /// Retrieves the title bar string for the current console window.
        /// </summary>
        /// <param name="lpConsoleTitle">Pointer to a buffer that receives 
        /// a null-terminated string containing the text that appears in 
        /// the title bar of the console window. The total size of the 
        /// buffer required will be less than 64K.</param>
        /// <param name="nSize">Size of the buffer pointed to by the 
        /// lpConsoleTitle parameter, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// length of the string copied to the buffer, in TCHARs.
        /// If the function fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetConsoleTitle
            (
                StringBuilder lpConsoleTitle,
                int nSize
            );

        /// <summary>
        /// Retrieves the window handle used by the console associated 
        /// with the calling process.
        /// </summary>
        /// <returns>The return value is a handle to the window used by 
        /// the console associated with the calling process or NULL if 
        /// there is no such associated console.</returns>
        /// <remarks>Included in: Windows 2000/XP/2003.</remarks>
        [DllImport(DllName, SetLastError = false)]
        public static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Retrieves information about the current console font.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="bMaximumWindow">If this parameter is TRUE, font 
        /// information is retrieved for the maximum window size. 
        /// If this parameter is FALSE, font information is retrieved for 
        /// the current window size.</param>
        /// <param name="lpConsoleCurrentFont">Pointer to a CONSOLE_FONT_INFO 
        /// structure that receives the requested font information.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetCurrentConsoleFont
            (
                IntPtr hConsoleOutput,
                bool bMaximumWindow,
                ref CONSOLE_FONT_INFO lpConsoleCurrentFont
            );

        /// <summary>
        /// Retrieves a pseudo handle for the current process.
        /// </summary>
        /// <returns>Pseudo handle to the current process.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Retrieves the process identifier of the calling process.
        /// </summary>
        /// <returns>Process identifier of the calling process.</returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint GetCurrentProcessId();

        /// <summary>
        /// Retrieves the current power state of the specified device.
        /// </summary>
        /// <param name="hDevice">Handle to an object on the device, 
        /// such as a file or socket, or a handle to the device itself.</param>
        /// <param name="pfOn">Pointer to the variable that receives 
        /// the power state. This value is TRUE if the device is in the 
        /// working state. Otherwise, it is FALSE.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks><para>An application can use GetDevicePowerState to 
        /// determine whether a device is in the working state or a low-power 
        /// state. If the device is in a low-power state, accessing the device 
        /// may cause it to either queue or fail any I/O requests, or transition 
        /// the device into the working state. The exact behavior depends on 
        /// the implementation of the device.</para>
        /// <para>To ensure maximum battery life on a laptop computer, use 
        /// GetDevicePowerState to reduce power consumption. For example, 
        /// if a disk is currently powered down, accessing the disk will cause 
        /// it to spin up, resulting in increased power consumption and reduced 
        /// battery life.</para>
        /// <para>Applications should defer or limit access to devices 
        /// wherever possible while the system is running on battery power. 
        /// To determine whether the system is running on battery power, 
        /// and the remaining battery life, use the GetSystemPowerStatus 
        /// function.</para></remarks>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool GetDevicePowerState
            (
                IntPtr hDevice,
                out bool pfOn
            );

        /// <summary>
        /// <para>The GetDiskFreeSpace function retrieves information 
        /// about the specified disk, including the amount of free space 
        /// on the disk.</para>
        /// <para>The GetDiskFreeSpace function cannot report volume 
        /// sizes that are greater than 2 gigabytes (GB). To ensure that 
        /// your application works with large capacity hard drives, use 
        /// the GetDiskFreeSpaceEx function.</para>
        /// </summary>
        /// <param name="lpRootPathName"><para>Pointer to a 
        /// null-terminated string that specifies the root directory 
        /// of the disk for which information is to be returned. 
        /// If this parameter is NULL, the function uses the root of the 
        /// current disk. If this parameter is a UNC name, it must 
        /// include a trailing backslash (for example, 
        /// \\MyServer\MyShare\). Furthermore, a drive specification 
        /// must have a trailing backslash (for example, C:\).</para>
        /// <para>Windows 95:  The initial release of Windows 95 does 
        /// not support UNC paths. Therefore, you must temporarily map 
        /// the UNC path to a drive letter, query the free disk space 
        /// on the drive, then remove the temporary mapping. Windows 95 
        /// OSR2 and later support UNC paths.</para></param>
        /// <param name="lpSectorsPerCluster">Pointer to a variable that 
        /// receives the number of sectors per cluster.</param>
        /// <param name="lpBytesPerSector">Pointer to a variable that 
        /// receives the number of bytes per sector.</param>
        /// <param name="lpNumberOfFreeClusters"><para>Pointer to a 
        /// variable for the total number of free clusters on the disk 
        /// that are available to the user associated with the calling 
        /// thread.</para>
        /// <para>If per-user disk quotas are in use, this value may be 
        /// less than the total number of free clusters on the disk.
        /// </para></param>
        /// <param name="lpTotalNumberOfClusters"><para>Pointer to a 
        /// variable for the total number of clusters on the disk that 
        /// are available to the user associated with the calling thread.
        /// </para>
        /// <para>If per-user disk quotas are in use, this value may be 
        /// less than the total number of clusters on the disk.</para>
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetDiskFreeSpace
            (
                string lpRootPathName,
                ref uint lpSectorsPerCluster,
                ref uint lpBytesPerSector,
                ref uint lpNumberOfFreeClusters,
                ref uint lpTotalNumberOfClusters
            );

        /// <summary>
        /// The GetDiskFreeSpaceEx function retrieves information about 
        /// the amount of space available on a disk volume: the total 
        /// amount of space, the total amount of free space, and the 
        /// to amount of free space available to the user associated 
        /// with the calling thread.
        /// </summary>
        /// <param name="lpDirectoryName"><para>Pointer to a 
        /// null-terminated string that specifies a directory on the 
        /// disk of interest. If this parameter is NULL, the function 
        /// uses the root of the current disk. If this parameter is a 
        /// UNC name, it must include a trailing backslash (for example, 
        /// \\MyServer\MyShare\).</para>
        /// <para>Note that this parameter does not have to specify 
        /// the root directory on a disk. The function accepts any 
        /// directory on the disk.</para></param>
        /// <param name="lpFreeBytesAvailable"><para>Pointer to a 
        /// variable that receives the total number of free bytes on 
        /// the disk that are available to the user associated with 
        /// the calling thread. This parameter can be NULL.</para>
        /// <para>If per-user quotas are in use, this value may be 
        /// less than the total number of free bytes on the disk.
        /// </para></param>
        /// <param name="lpTotalNumberOfBytes"><para>Pointer to a 
        /// variable that receives the total number of bytes on the 
        /// disk that are available to the user associated with the 
        /// calling thread. This parameter can be NULL.</para>
        /// <para>If per-user quotas are in use, this value may be less 
        /// than the total number of bytes on the disk.</para></param>
        /// <param name="lpTotalNumberOfFreeBytes">Pointer to a variable 
        /// that receives the total number of free bytes on the disk. 
        /// This parameter can be NULL.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetDiskFreeSpaceEx
            (
                string lpDirectoryName,
                ref ulong lpFreeBytesAvailable,
                ref ulong lpTotalNumberOfBytes,
                ref ulong lpTotalNumberOfFreeBytes
            );

        /// <summary>
        /// Retrieves the application-specific portion 
        /// of the search path used to locate DLLs for the application.
        /// </summary>
        /// <param name="nBufferLength">Size of the output buffer, 
        /// in characters.</param>
        /// <param name="lpBuffer">Pointer to a buffer 
        /// that receives the application-specific portion of 
        /// the search path.</param>
        /// <returns>If the function succeeds, the return value 
        /// is the length of the string copied to lpBuffer, in characters, 
        /// not including the terminating null character. If the return 
        /// value is greater than nBufferLength, it specifies the size 
        /// of the buffer required for the path.
        /// If the function fails, the return value is zero.
        /// </returns>
        /// <remarks>Requires Windows XP SP1.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetDllDirectory
            (
                int nBufferLength,
                StringBuilder lpBuffer
            );

        /// <summary>
        /// Determines whether a disk drive is a removable, fixed, 
        /// CD-ROM, RAM disk, or network drive.
        /// </summary>
        /// <param name="lpRootPathName">Pointer to a null-terminated 
        /// string that specifies the root directory of the disk to return 
        /// information about. A trailing backslash is required. If this 
        /// parameter is NULL, the function uses the root of the current 
        /// directory.</param>
        /// <returns>The return value specifies the type of drive.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern DRIVETYPE GetDriveType
            (
                string lpRootPathName
            );

        /// <summary>
        /// Retrieves the termination status of the specified process.
        /// </summary>
        /// <param name="hProcess">Handle to the process. The handle 
        /// must have the PROCESS_QUERY_INFORMATION access right.</param>
        /// <param name="lpExitCode">Pointer to a variable to receive 
        /// the process termination status.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetExitCodeProcess
            (
                IntPtr hProcess,
                ref uint lpExitCode
            );

        /// <summary>
        /// Retrieves file information for a specified file.
        /// </summary>
        /// <param name="hFile"><para>Handle to the file for which 
        /// to obtain information.</para>
        /// <para>This handle should not be a pipe handle. 
        /// The GetFileInformationByHandle function does 
        /// not work with pipe handles.</para></param>
        /// <param name="lpFileInformation">Pointer to a 
        /// BY_HANDLE_FILE_INFORMATION structure that 
        /// receives the file information. The structure 
        /// can be used in subsequent calls to 
        /// GetFileInformationByHandle to see the 
        /// information about the file.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetFileInformationByHandle
            (
                IntPtr hFile,
                out BY_HANDLE_FILE_INFORMATION lpFileInformation
            );

        /// <summary>
        /// Retrieves the size of a specified file.
        /// </summary>
        /// <param name="handle">Handle to the file whose size is to 
        /// be returned. This handle must have been created with either 
        /// the GENERIC_READ or GENERIC_WRITE access right.</param>
        /// <param name="highPart">Pointer to the variable where the 
        /// high-order word of the file size is returned. This parameter 
        /// can be NULL if the application does not require the 
        /// high-order word.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// low-order doubleword of the file size, and, if 
        /// lpFileSizeHigh is non-NULL, the function puts the high-order 
        /// doubleword of the file size into the variable pointed to 
        /// by that parameter. If the function fails and lpFileSizeHigh 
        /// is NULL, the return value is INVALID_FILE_SIZE.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint GetFileSize
            (
                IntPtr handle,
                ref int highPart
            );

        /// <summary>
        /// Retrieves the date and time that a file was created, 
        /// last accessed, and last modified.
        /// </summary>
        /// <param name="hFile">Handle to the files for which to get 
        /// dates and times. The file handle must have been created with 
        /// the GENERIC_READ access right.</param>
        /// <param name="lpCreationTime">Pointer to a FILETIME structure 
        /// to receive the date and time the file was created. 
        /// This parameter can be NULL if the application does not 
        /// require this information.</param>
        /// <param name="lpLastAccessTime">Pointer to a FILETIME 
        /// structure to receive the date and time the file was last 
        /// accessed. The last access time includes the last time the 
        /// file was written to, read from, or, in the case of executable 
        /// files, run. This parameter can be NULL if the application 
        /// does not require this information.</param>
        /// <param name="lpLastWriteTime">Pointer to a FILETIME structure 
        /// to receive the date and time the file was last written to. 
        /// This parameter can be NULL if the application does not 
        /// require this information.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetFileTime
            (
                IntPtr hFile,
                out FILETIME lpCreationTime,
                out FILETIME lpLastAccessTime,
                out FILETIME lpLastWriteTime
            );

        /// <summary>
        /// Retrieves the file type for the specified file.
        /// </summary>
        /// <param name="hFile">Handle to an open file.</param>
        /// <returns>The return value is one of values of 
        /// the FileTypeFlags enum.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern FileTypeFlags GetFileType
            (
                IntPtr hFile
            );

        /// <summary>
        /// The GetKeyState function retrieves the status of the specified 
        /// virtual key. The status specifies whether the key is up, down, 
        /// or toggled (on, off—alternating each time the key is pressed).
        /// </summary>
        /// <param name="nVirtKey"><para>Specifies a virtual key. If the desired 
        /// virtual key is a letter or digit (A through Z, a through z, or 0 
        /// through 9), nVirtKey must be set to the ASCII value of that character. 
        /// For other keys, it must be a virtual-key code.</para>
        /// <para>If a non-English keyboard layout is used, virtual keys with 
        /// values in the range ASCII A through Z and 0 through 9 are used to 
        /// specify most of the character keys. For example, for the German 
        /// keyboard layout, the virtual key of value ASCII O (0x4F) refers to 
        /// the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
        /// </para></param>
        /// <returns><para>The return value specifies the status of the 
        /// specified virtual key, as follows:</para>
        /// <list type="bullet">
        /// <item>If the high-order bit is 1, the key is down; otherwise, 
        /// it is up.</item>
        /// <item>If the low-order bit is 1, the key is toggled. A key, such 
        /// as the CAPS LOCK key, is toggled if it is turned on. The key is 
        /// off and untoggled if the low-order bit is 0. A toggle key's 
        /// indicator light (if any) on the keyboard will be on when the key 
        /// is toggled, and off when the key is untoggled.</item>
        /// </list></returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern short GetKeyState
            (
                VirtualKeys nVirtKey
            );

        /// <summary>
        /// Retrieves the size of the largest possible console window, 
        /// based on the current font and the size of the display.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer.
        /// </param>
        /// <returns>If the function succeeds, the return value is a COORD 
        /// structure that specifies the number of character cell rows 
        /// (X member) and columns (Y member) in the largest possible console 
        /// window. Otherwise, the members of the structure are zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern COORD GetLargestConsoleWindowSize
            (
                IntPtr hConsoleOutput
            );

        /// <summary>
        /// The GetLastError function retrieves the calling thread's 
        /// last-error code value. The last-error code is maintained 
        /// on a per-thread basis. Multiple threads do not overwrite 
        /// each other's last-error code.
        /// </summary>
        /// <returns><para>The return value is the calling thread's 
        /// last-error code value. Functions set this value by calling 
        /// the SetLastError function. The Return Value section of each 
        /// reference page notes the conditions under which the function 
        /// sets the last-error code.</para>
        /// <para>Windows Me/98/95: Functions that are actually 
        /// implemented in 16-bit code do not set the last-error code. 
        /// You should ignore the last-error code when you call these 
        /// functions. They include window management functions, GDI 
        /// functions, and Multimedia functions. For functions that do 
        /// set the last-error code, you should not rely on GetLastError 
        /// returning the same value under Windows Me/98/95 and Windows 
        /// NT.</para></returns>
        [DllImport(DllName)]
        [CLSCompliant(false)]
        public static extern uint GetLastError();

        /// <summary>
        /// The GetLocalTime function retrieves the current local 
        /// date and time.
        /// </summary>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// to receive the current local date and time.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void GetLocalTime
            (
                ref SYSTEMTIME lpSystemTime
            );

        /// <summary>
        /// Retrieves a bitmask representing the currently available disk drives.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a bitmask 
        /// representing the currently available disk drives. Bit position 0 
        /// (the least-significant bit) is drive A, bit position 1 is drive B, 
        /// bit position 2 is drive C, and so on.
        /// If the function fails, the return value is zero.</returns>
        [CLSCompliant(false)]
        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetLogicalDrives();

        /// <summary>
        /// Retrieves a module handle for the specified module if the 
        /// file has been mapped into the address space of the calling 
        /// process.
        /// </summary>
        /// <param name="lpModuleName"><para>Pointer to a null-terminated 
        /// string that contains the name of the module (either a .dll or 
        /// .exe file). If the file name extension is omitted, the 
        /// default library extension .dll is appended. The file name 
        /// string can include a trailing point character (.) to indicate 
        /// that the module name has no extension. The string does not 
        /// have to specify a path. When specifying a path, be sure to 
        /// use backslashes (\), not forward slashes (/). The name is 
        /// compared (case independently) to the names of modules 
        /// currently mapped into the address space of the calling 
        /// process.</para>
        /// <para>If this parameter is NULL, GetModuleHandle returns a 
        /// handle to the file used to create the calling process.
        /// </para></param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the specified module. If the function fails, the 
        /// return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetModuleHandle
            (
                string lpModuleName
            );

        /// <summary>
        /// Retrieves the fully qualified path for the specified module.
        /// </summary>
        /// <param name="hModule">Handle to the module whose path is 
        /// being requested. If this parameter is NULL, GetModuleFileName 
        /// retrieves the path for the current module.</param>
        /// <param name="lpFilename"><para>Pointer to a buffer that 
        /// receives a null-terminated string that specifies the 
        /// fully-qualified path of the module. If the length of the 
        /// path exceeds the size specified by the nSize parameter, 
        /// the function succeeds and the string is truncated to nSize 
        /// characters and null terminated.</para>
        /// <para>The path can have the prefix "\\?\", depending on how 
        /// the module was loaded.</para></param>
        /// <param name="nSize">Size of the lpFilename buffer, in TCHARs.
        /// </param>
        /// <returns>If the function succeeds, the return value is the 
        /// length of the string copied to the buffer, in TCHARs. If the 
        /// buffer is too small to hold the module name, the string is 
        /// truncated to nSize, and the function returns nSize. If the 
        /// function fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint GetModuleFileName
            (
                IntPtr hModule,
                StringBuilder lpFilename,
                uint nSize
            );

        /// <summary>
        /// The GetNamedPipeHandleState function retrieves information 
        /// about a specified named pipe. The information returned can 
        /// vary during the lifetime of an instance of the named pipe.
        /// </summary>
        /// <param name="hNamedPipe">Handle to the named pipe for which 
        /// information is wanted. The handle must have GENERIC_READ 
        /// access to the named pipe.This parameter can also be a handle 
        /// to an anonymous pipe, as returned by the CreatePipe function.
        /// </param>
        /// <param name="lpState">Pointer to a variable that indicates 
        /// the current state of the handle. This parameter can be NULL 
        /// if this information is not needed.</param>
        /// <param name="lpCurInstances">Pointer to a variable that 
        /// receives the number of current pipe instances. This parameter 
        /// can be NULL if this information is not required.</param>
        /// <param name="lpMaxCollectionCount">Pointer to a variable that 
        /// receives the maximum number of bytes to be collected on the 
        /// client's computer before transmission to the server. This 
        /// parameter must be NULL if the specified pipe handle is to 
        /// the server end of a named pipe or if client and server 
        /// processes are on the same computer. This parameter can be 
        /// NULL if this information is not required.</param>
        /// <param name="lpCollectDataTimeout"> Pointer to a variable 
        /// that receives the maximum time, in milliseconds, that can 
        /// pass before a remote named pipe transfers information over 
        /// the network. This parameter must be NULL if the specified 
        /// pipe handle is to the server end of a named pipe or if 
        /// client and server processes are on the same computer. This 
        /// parameter can be NULL if this information is not required.
        /// </param>
        /// <param name="lpUserName"><para>Pointer to a buffer that 
        /// receives the null-terminated string containing the user name 
        /// string associated with the client application. The server 
        /// can only retrieve this information if the client opened the 
        /// pipe with SECURITY_IMPERSONATION access.</para>
        /// <para>This parameter must be NULL if the specified pipe 
        /// handle is to the client end of a named pipe. This parameter 
        /// can be NULL if this information is not required.</para>
        /// </param>
        /// <param name="nMaxUserNameSize">Size of the buffer specified 
        /// by the lpUserName parameter, in TCHARs. This parameter is 
        /// ignored if lpUserName is NULL.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetNamedPipeHandleState
            (
                IntPtr hNamedPipe,
                ref PipeInfoFlags lpState,
                ref uint lpCurInstances,
                ref uint lpMaxCollectionCount,
                ref uint lpCollectDataTimeout,
                StringBuilder lpUserName,
                uint nMaxUserNameSize
            );

        /// <summary>
        /// Retrieves information about the specified named pipe.
        /// </summary>
        /// <param name="hNamedPipe">Handle to the named pipe instance. 
        /// The handle must have GENERIC_READ access to the named pipe.
        /// This parameter can also be a handle to an anonymous pipe, 
        /// as returned by the CreatePipe function.</param>
        /// <param name="lpFlags">Pointer to a variable that indicates 
        /// the type of the named pipe.</param>
        /// <param name="lpOutBufferSize">Pointer to a variable that 
        /// receives the size of the buffer for outgoing data, in bytes. 
        /// If the buffer size is zero, the buffer is allocated as 
        /// needed. This parameter can be NULL if this information is 
        /// not required.</param>
        /// <param name="lpInBufferSize">Pointer to a variable that 
        /// receives the size of the buffer for incoming data, in bytes. 
        /// If the buffer size is zero, the buffer is allocated as needed.
        /// </param>
        /// <param name="lpMaxInstances">Pointer to a variable that 
        /// receives the maximum number of pipe instances that can be 
        /// created. If the variable is set to PIPE_UNLIMITED_INSTANCES, 
        /// the number of pipe instances that can be created is limited 
        /// only by the availability of system resources.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetNamedPipeInfo
            (
                IntPtr hNamedPipe,
                out PipeInfoFlags lpFlags,
                out uint lpOutBufferSize,
                out uint lpInBufferSize,
                out uint lpMaxInstances
            );

        /// <summary>
        /// The GetNativeSystemInfo function retrieves information about 
        /// the current system to an application running under WOW64. 
        /// If the function is called from a 64-bit application, it is 
        /// equivalent to the GetSystemInfo function.
        /// </summary>
        /// <param name="lpSystemInfo">Pointer to a SYSTEM_INFO structure 
        /// that receives the information.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void GetNativeSystemInfo
            (
                ref SYSTEM_INFO lpSystemInfo
            );

        /// <summary>
        /// Undocumented function.
        /// </summary>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern int GetNumberOfConsoleFonts();

        /// <summary>
        /// Retrieves the number of unread input records in the 
        /// console's input buffer.
        /// </summary>
        /// <param name="hConsoleInput">Handle to the console input buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="lpcNumberOfEvents">Pointer to a variable that 
        /// receives the number of unread input records in the console's 
        /// input buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetNumberOfConsoleInputEvents
            (
                IntPtr hConsoleInput,
                ref int lpcNumberOfEvents
            );

        /// <summary>
        /// Retrieves the number of buttons on the mouse used by the 
        /// current console.
        /// </summary>
        /// <param name="lpNumberOfMouseButtons">Pointer to a variable 
        /// that receives the number of mouse buttons.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetNumberOfConsoleMouseButtons
            (
                ref int lpNumberOfMouseButtons
            );

        /// <summary>
        /// <para>Retrieves all the keys and values for the specified 
        /// section of an initialization file.</para>
        /// <para>Windows Me/98/95: The specified profile section must 
        /// not exceed 32K.</para>
        /// </summary>
        /// <param name="lpAppName">Pointer to a null-terminated string 
        /// specifying the name of the section in the initialization 
        /// file.</param>
        /// <param name="lpReturnedString">Pointer to a buffer that 
        /// receives the key name and value pairs associated with the 
        /// named section. The buffer is filled with one or more 
        /// null-terminated strings; the last string is followed by 
        /// a second null character.</param>
        /// <param name="nSize"><para>Size of the buffer pointed to 
        /// by the lpReturnedString parameter, in TCHARs.</para>
        /// <para>Windows Me/98/95:  The maximum buffer size is 32,767 
        /// characters.</para></param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// that specifies the name of the initialization file. If this 
        /// parameter does not contain a full path to the file, the 
        /// system searches for the file in the Windows directory.
        /// </param>
        /// <returns>The return value specifies the number of characters 
        /// copied to the buffer, not including the terminating null 
        /// character. If the buffer is not large enough to contain all 
        /// the key name and value pairs associated with the named 
        /// section, the return value is equal to nSize minus two.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetPrivateProfileSection
            (
                string lpAppName,
                [MarshalAs(UnmanagedType.LPArray)]
                byte[] lpReturnedString,
                int nSize,
                string lpFileName
            );

        /// <summary>
        /// Retrieves the names of all sections in an initialization 
        /// file.
        /// </summary>
        /// <param name="lpszReturnBuffer">Pointer to a buffer that 
        /// receives the section names associated with the named file. 
        /// The buffer is filled with one or more null-terminated 
        /// strings; the last string is followed by a second null 
        /// character.</param>
        /// <param name="nSize">Size of the buffer pointed to by the 
        /// lpszReturnBuffer parameter, in TCHARs.</param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// that specifies the name of the initialization file. 
        /// If this parameter is NULL, the function searches the Win.ini 
        /// file. If this parameter does not contain a full path to the 
        /// file, the system searches for the file in the Windows 
        /// directory.</param>
        /// <returns>The return value specifies the number of characters 
        /// copied to the specified buffer, not including the terminating 
        /// null character. If the buffer is not large enough to contain 
        /// all the section names associated with the specified 
        /// initialization file, the return value is equal to the size 
        /// specified by nSize minus two.</returns>
        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames
            (
                [MarshalAs(UnmanagedType.LPArray)]
                byte[] lpszReturnBuffer,
                int nSize,
                string lpFileName
            );

        /// <summary>
        /// Retrieves a string from the specified section in an 
        /// initialization file.
        /// </summary>
        /// <param name="lpAppName">Pointer to a null-terminated string 
        /// that specifies the name of the section containing the key 
        /// name. If this parameter is NULL, the GetPrivateProfileString 
        /// function copies all section names in the file to the supplied 
        /// buffer.</param>
        /// <param name="lpKeyName">Pointer to the null-terminated string 
        /// specifying the name of the key whose associated string is to 
        /// be retrieved. If this parameter is NULL, all key names in 
        /// the section specified by the lpAppName parameter are copied 
        /// to the buffer specified by the lpReturnedString parameter.
        /// </param>
        /// <param name="lpDefault"><para>Pointer to a null-terminated 
        /// default string. If the lpKeyName key cannot be found in the 
        /// initialization file, GetPrivateProfileString copies the 
        /// default string to the lpReturnedString buffer. This 
        /// parameter cannot be NULL.</para>
        /// <para>Avoid specifying a default string with trailing blank 
        /// characters. The function inserts a null character in the 
        /// lpReturnedString buffer to strip any trailing blanks.</para>
        /// <para>Windows Me/98/95: Although lpDefault is declared as 
        /// a constant parameter, the system strips any trailing blanks 
        /// by inserting a null character into the lpDefault string 
        /// before copying it to the lpReturnedString buffer.
        /// </para></param>
        /// <param name="lpReturnedString"><para>Pointer to the buffer 
        /// that receives the retrieved string.</para>
        /// <para>Windows Me/98/95: The string cannot contain control 
        /// characters (character code less than 32). Strings containing 
        /// control characters may be truncated.</para></param>
        /// <param name="nSize">Size of the buffer pointed to by the 
        /// lpReturnedString parameter, in TCHARs.</param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// that specifies the name of the initialization file. If this 
        /// parameter does not contain a full path to the file, the 
        /// system searches for the file in the Windows directory.
        /// </param>
        /// <returns><para>The return value is the number of characters 
        /// copied to the buffer, not including the terminating null 
        /// character.</para>
        /// <para>If neither lpAppName nor lpKeyName is NULL and the 
        /// supplied destination buffer is too small to hold the 
        /// requested string, the string is truncated and followed 
        /// by a null character, and the return value is equal to nSize 
        /// minus one.</para>
        /// <para>If either lpAppName or lpKeyName is NULL and the 
        /// supplied destination buffer is too small to hold all the 
        /// strings, the last string is truncated and followed by two 
        /// null characters. In this case, the return value is equal to 
        /// nSize minus two.</para></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetPrivateProfileString
            (
                string lpAppName,
                string lpKeyName,
                string lpDefault,
                StringBuilder lpReturnedString,
                int nSize,
                string lpFileName
            );

        /// <summary>
        /// Retrieves the address of an exported function or variable 
        /// from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">Handle to the DLL module that contains 
        /// the function or variable.</param>
        /// <param name="lpProcName">Pointer to a null-terminated string 
        /// that specifies the function or variable name, or the 
        /// function's ordinal value. If this parameter is an ordinal 
        /// value, it must be in the low-order word; the high-order word 
        /// must be zero.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// address of the exported function or variable.
        /// If the function fails, the return value is NULL</returns>
        /// <remarks>The spelling and case of a function name pointed to 
        /// by lpProcName must be identical to that in the EXPORTS 
        /// statement of the source DLL's module-definition (.def) 
        /// file.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetProcAddress
            (
                IntPtr hModule,
                string lpProcName
            );

        /// <summary>
        /// The GetProcessHeap function obtains a handle to the heap 
        /// of the calling process. This handle can then be used in 
        /// subsequent calls to the heap functions.
        /// </summary>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the calling process's heap. If the function fails, 
        /// the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetProcessHeap();

        /// <summary>
        /// Retrieves the process identifier of the specified process.
        /// </summary>
        /// <param name="Process">Handle to the process. The handle must 
        /// have the PROCESS_QUERY_INFORMATION access right.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// process identifier of the specified process. If the function 
        /// fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint GetProcessId
            (
                IntPtr Process
            );

        /// <summary>
        /// Retrieves timing information for the specified process.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose timing 
        /// information is sought. This handle must be created with 
        /// the PROCESS_QUERY_INFORMATION access right.</param>
        /// <param name="lpCreationTime">Pointer to a FILETIME structure 
        /// that receives the creation time of the process.</param>
        /// <param name="lpExitTime">Pointer to a FILETIME structure 
        /// that receives the exit time of the process. If the process 
        /// has not exited, the content of this structure is undefined.
        /// </param>
        /// <param name="lpKernelTime">Pointer to a FILETIME structure 
        /// that receives the amount of time that the process has 
        /// executed in kernel mode. The time that each of the threads 
        /// of the process has executed in kernel mode is determined, 
        /// and then all of those times are summed together to obtain 
        /// this value.</param>
        /// <param name="lpUserTime">Pointer to a FILETIME structure 
        /// that receives the amount of time that the process has 
        /// executed in user mode. The time that each of the threads 
        /// of the process has executed in user mode is determined, 
        /// and then all of those times are summed together to obtain 
        /// this value.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetProcessTimes
            (
                IntPtr hProcess,
                ref FILETIME lpCreationTime,
                ref FILETIME lpExitTime,
                ref FILETIME lpKernelTime,
                ref FILETIME lpUserTime
            );

        /// <summary>
        /// Retrieves the minimum and maximum working set sizes of the 
        /// specified process.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose working 
        /// set sizes will be obtained. The handle must have the 
        /// PROCESS_QUERY_INFORMATION access right.</param>
        /// <param name="lpMinimumWorkingSetSize">Pointer to a variable 
        /// that receives the minimum working set size of the specified 
        /// process, in bytes. The virtual memory manager attempts to 
        /// keep at least this much memory resident in the process 
        /// whenever the process is active.</param>
        /// <param name="lpMaximumWorkingSetSize">Pointer to a variable 
        /// that receives the maximum working set size of the specified 
        /// process, in bytes. The virtual memory manager attempts to 
        /// keep no more than this much memory resident in the process 
        /// whenever the process is active when memory is in short supply.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetProcessWorkingSetSize
            (
                IntPtr hProcess,
                ref uint lpMinimumWorkingSetSize,
                ref uint lpMaximumWorkingSetSize
            );

        /// <summary>
        /// Retrieves the short path form of a specified input path.
        /// </summary>
        /// <param name="lpszLongPath">Pointer to a null-terminated 
        /// path string.</param>
        /// <param name="lpszShortPath">Pointer to a buffer to receive 
        /// the null-terminated short form of the path specified by 
        /// lpszLongPath.</param>
        /// <param name="cchBuffer">Size of the buffer pointed to by 
        /// lpszShortPath, in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is 
        /// the length, in TCHARs, of the string copied to lpszShortPath, 
        /// not including the terminating null character.
        /// If the lpszShortPath buffer is too small to contain the path, 
        /// the return value is the size of the buffer, in TCHARs, 
        /// required to hold the path. Therefore, if the return value 
        /// is greater than cchBuffer, call the function again with 
        /// a buffer that is large enough to hold the path.
        /// If the function fails for any other reason, the return value 
        /// is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetShortPathName
            (
                string lpszLongPath,
                StringBuilder lpszShortPath,
                int cchBuffer
            );

        /// <summary>
        /// Retrieves a handle for the standard input, standard output, 
        /// or standard error device.
        /// </summary>
        /// <param name="nStdHandle">Standard device for which a handle 
        /// is to be returned.</param>
        /// <returns><para>If the function succeeds, the return value 
        /// is a handle to the specified device, or a redirected handle 
        /// set by a previous call to SetStdHandle. The handle has 
        /// GENERIC_READ and GENERIC_WRITE access rights, unless the 
        /// application has used SetStdHandle to set a standard handle 
        /// with lesser access.</para>
        /// <para>If the function fails, the return value is 
        /// INVALID_HANDLE_VALUE. To get extended error information, 
        /// call GetLastError.</para>
        /// <para>If an application does not have associated standard 
        /// handles, such as a service running on an interactive 
        /// desktop, and has not redirected them, the return value 
        /// is NULL.</para></returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr GetStdHandle
            (
                StdHandles nStdHandle
            );

        /// <summary>
        /// Returns information about the current system.
        /// </summary>
        /// <param name="lpSystemInfo">Pointer to a 
        /// SYSTEM_INFO structure that receives the information.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void GetSystemInfo
            (
                ref SYSTEM_INFO lpSystemInfo
            );

        /// <summary>
        /// The GetSystemPowerStatus function retrieves the power status 
        /// of the system. The status indicates whether the system is 
        /// running on AC or DC power, whether the battery is currently 
        /// charging, and how much battery life remains.
        /// </summary>
        /// <param name="lpSystemPowerStatus">Pointer to a SYSTEM_POWER_STATUS 
        /// structure that receives status information.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetSystemPowerStatus
            (
                out SYSTEM_POWER_STATUS lpSystemPowerStatus
            );

        /// <summary>
        /// Retrieves the current system date and time. The system 
        /// time is expressed in Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// to receive the current system date and time.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void GetSystemTime
            (
                ref SYSTEMTIME lpSystemTime
            );

        /// <summary>
        /// The GetSystemTimeAdjustment function determines whether 
        /// the system is applying periodic time adjustments to its 
        /// time-of-day clock at each clock interrupt, along with the 
        /// value and period of any such adjustments. Note that the 
        /// period of such adjustments is equivalent to the time period 
        /// between clock interrupts.
        /// </summary>
        /// <param name="lpTimeAdjustment">Pointer to a value that the 
        /// function sets to the number of 100-nanosecond units added 
        /// to the time-of-day clock at each periodic time adjustment.
        /// </param>
        /// <param name="lpTimeIncrement">Pointer to a value that the 
        /// function sets to the interval between periodic time 
        /// adjustments, in 100-nanosecond units. This interval is the 
        /// time period between a system's clock interrupts.</param>
        /// <param name="lpTimeAdjustmentDisabled"><para>Pointer to a 
        /// value that the function sets to indicate whether periodic 
        /// time adjustment is in effect.</para>
        /// <para>A value of TRUE indicates that periodic time 
        /// adjustment is disabled. At each clock interrupt, the 
        /// system merely adds the interval between clock interrupts 
        /// to the time-of-day clock. The system is free, however, 
        /// to adjust its time-of-day clock using other techniques. 
        /// Such other techniques may cause the time-of-day clock to 
        /// noticeably jump when adjustments are made.</para>
        /// <para>A value of FALSE indicates that periodic time 
        /// adjustment is being used to adjust the time-of-day clock. 
        /// At each clock interrupt, the system adds the time increment 
        /// specified by SetSystemTimeAdjustment's dwTimeIncrement 
        /// parameter to the time-of-day clock. The system will not 
        /// interfere with the time adjustment scheme, and will not 
        /// attempt to synchronize time of day on its own via other 
        /// techniques.</para></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool GetSystemTimeAdjustment
            (
                out uint lpTimeAdjustment,
                out uint lpTimeIncrement,
                out bool lpTimeAdjustmentDisabled
            );

        /// <summary>
        /// The GetSystemTimes function retrieves system timing 
        /// information. On a multiprocessor system, the values 
        /// returned are the sum of the designated times across 
        /// all processors.
        /// </summary>
        /// <param name="lpIdleTime">Pointer to a FILETIME structure 
        /// that receives the amount of time that the system has been 
        /// idle.</param>
        /// <param name="lpKernelTime">Pointer to a FILETIME structure 
        /// that receives the amount of time that the system has spent 
        /// executing in Kernel mode (including all threads in all 
        /// processes, on all processors).</param>
        /// <param name="lpUserTime">Pointer to a FILETIME structure 
        /// that receives the amount of time that the system has spent 
        /// executing in User mode (including all threads in all 
        /// processes, on all processors).</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Included in: Windows XP SP1/2003</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetSystemTimes
            (
                out FILETIME lpIdleTime,
                out FILETIME lpKernelTime,
                out FILETIME lpUserTime
            );

        /// <summary>
        /// The GetTickCount function retrieves the number of 
        /// milliseconds that have elapsed since the system was started. 
        /// It is limited to the resolution of the system timer. 
        /// To obtain the system timer resolution, use the 
        /// GetSystemTimeAdjustment function.
        /// </summary>
        /// <returns>The return value is the number of milliseconds 
        /// that have elapsed since the system was started.</returns>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern uint GetTickCount();

        /// <summary>
        /// Takes a volume mount point or root directory and returns 
        /// the corresponding unique volume name.
        /// </summary>
        /// <param name="lpszVolumeMountPoint">Pointer to a string 
        /// that contains either the path of a volume mount point 
        /// with a trailing backslash (\) or a drive letter indicating 
        /// a root directory in the form "D:\".</param>
        /// <param name="lpszVolumeName">Pointer to a string that receives 
        /// the volume name. This name is a unique volume name of the 
        /// form "\\?\Volume{GUID}\" where GUID is the GUID that identifies 
        /// the volume.</param>
        /// <param name="cchBufferLength">Length of the output buffer, 
        /// in TCHARs. A reasonable size for the buffer to accommodate 
        /// the largest possible volume name is 50 characters.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetVolumeNameForVolumeMountPoint
            (
                string lpszVolumeMountPoint,
                StringBuilder lpszVolumeName,
                int cchBufferLength
            );

        /// <summary>
        /// Retrieves the volume mount point at which the specified 
        /// path is mounted.
        /// </summary>
        /// <param name="lpszFileName">Pointer to the input path string. 
        /// Both absolute and relative file and directory names, such as 
        /// ".", are acceptable in this path.</param>
        /// <param name="lpszVolumePathName">Pointer to a string that 
        /// receives the volume mount point for the input path.</param>
        /// <param name="cchBufferLength">Length of the output buffer, 
        /// in TCHARs.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetVolumePathName
            (
                string lpszFileName,
                StringBuilder lpszVolumePathName,
                int cchBufferLength
            );

        /// <summary>
        /// Retrieves information about a file system and volume 
        /// whose root directory is specified.
        /// </summary>
        /// <param name="lpRootPathName">Pointer to a string that contains 
        /// the root directory of the volume to be described. If this parameter 
        /// is NULL, the root of the current directory is used. A trailing 
        /// backslash is required. For example, you would specify 
        /// \\MyServer\MyShare as \\MyServer\MyShare\, or the C drive as 
        /// "C:\". </param>
        /// <param name="lpVolumeNameBuffer">Pointer to a buffer that 
        /// receives the name of the specified volume.</param>
        /// <param name="nVolumeNameSize">Length of the volume name buffer, 
        /// in TCHARs. This parameter is ignored if the volume name buffer 
        /// is not supplied.</param>
        /// <param name="lpVolumeSerialNumber"><para>Pointer to a variable 
        /// that receives the volume serial number. This parameter can be 
        /// NULL if the serial number is not required.</para>
        /// <para>Windows Me/98/95: If the queried volume is a network drive, 
        /// the serial number will not be returned.</para></param>
        /// <param name="lpMaximumComponentLength"><para>Pointer to a variable 
        /// that receives the maximum length, in TCHARs, of a file name 
        /// component supported by the specified file system. A file name 
        /// component is that portion of a file name between backslashes.
        /// </para>
        /// <para>The value stored in variable pointed to by 
        /// *lpMaximumComponentLength is used to indicate that long names 
        /// are supported by the specified file system. For example, for 
        /// a FAT file system supporting long names, the function stores 
        /// the value 255, rather than the previous 8.3 indicator. 
        /// Long names can also be supported on systems that use the NTFS 
        /// file system.</para></param>
        /// <param name="lpFileSystemFlags">Pointer to a variable that 
        /// receives flags associated with the specified file system. 
        /// </param>
        /// <param name="lpFileSystemNameBuffer">Pointer to a buffer that 
        /// receives the name of the file system (such as FAT or NTFS).
        /// </param>
        /// <param name="nFileSystemNameSize">Length of the file system 
        /// name buffer, in TCHARs. This parameter is ignored if the file 
        /// system name buffer is not supplied.</param>
        /// <returns>If all the requested information is retrieved, the 
        /// return value is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetVolumeInformation
            (
                string lpRootPathName,
                StringBuilder lpVolumeNameBuffer,
                int nVolumeNameSize,
                out int lpVolumeSerialNumber,
                out int lpMaximumComponentLength,
                out FileSystemFlags lpFileSystemFlags,
                StringBuilder lpFileSystemNameBuffer,
                int nFileSystemNameSize
            );

        /// <summary>
        /// The GlobalAlloc function allocates the specified number 
        /// of bytes from the heap. Windows memory management does not 
        /// provide a separate local heap and global heap.
        /// </summary>
        /// <param name="uFlags">Memory allocation attributes.</param>
        /// <param name="dwBytes">Number of bytes to allocate. If this 
        /// parameter is zero and the uFlags parameter specifies 
        /// GMEM_MOVEABLE, the function returns a handle to a memory 
        /// object that is marked as discarded.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the newly allocated memory object. If the function 
        /// fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr GlobalAlloc
            (
                GlobalAllocFlags uFlags,
                uint dwBytes
            );

        /// <summary>
        /// The GlobalAlloc function allocates the specified number 
        /// of bytes from the heap. Windows memory management does not 
        /// provide a separate local heap and global heap.
        /// </summary>
        /// <param name="dwBytes">Number of bytes to allocate.</param>
        /// <returns>Handle to newly allocated memory object.</returns>
        [CLSCompliant(false)]
        public static IntPtr GlobalAlloc(uint dwBytes)
        {
            return GlobalAlloc(GlobalAllocFlags.GMEM_FIXED, dwBytes);
        }

        /// <summary>
        /// Returns information about the specified global memory object.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the GlobalAlloc or 
        /// GlobalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value 
        /// specifies the allocation values and the lock count for 
        /// the memory object. If the function fails, the return value 
        /// is GMEM_INVALID_HANDLE, indicating that the global handle 
        /// is not valid.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern GlobalAllocFlags GlobalFlags
            (
                IntPtr hMem
            );

        /// <summary>
        /// The GlobalFree function frees the specified global memory 
        /// object and invalidates its handle.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the GlobalAlloc or 
        /// GlobalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value 
        /// is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GlobalFree
            (
                IntPtr hMem
            );

        /// <summary>
        /// The GlobalMemoryStatus function obtains information 
        /// about the system's current usage of both physical 
        /// and virtual memory.
        /// </summary>
        /// <param name="lpBuffer">Pointer to a MEMORYSTATUS structure. 
        /// The GlobalMemoryStatus function stores information about 
        /// current memory availability into this structure.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void GlobalMemoryStatus
            (
                ref MEMORYSTATUS lpBuffer
            );

        /// <summary>
        /// Obtains information about the system's current usage of 
        /// both physical and virtual memory.
        /// </summary>
        /// <param name="lpBuffer">Pointer to a MEMORYSTATUSEX structure 
        /// that receives information about current memory availability.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        /// <remarks>Included in: Windows 2000/XP/2003.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx
            (
                ref MEMORYSTATUSEX lpBuffer
            );

        /// <summary>
        /// The GlobalReAlloc function changes the size or attributes 
        /// of a specified global memory object. The size can increase 
        /// or decrease.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object to be 
        /// reallocated. This handle is returned by either the 
        /// GlobalAlloc or GlobalReAlloc function.</param>
        /// <param name="dwBytes">New size of the memory block, in bytes.
        /// If uFlags specifies GMEM_MODIFY, this parameter is ignored.
        /// </param>
        /// <param name="uFlags">Reallocation options. If GMEM_MODIFY is 
        /// specified, this parameter modifies the attributes of the 
        /// memory object, and the dwBytes parameter is ignored. 
        /// Otherwise, this parameter controls the reallocation of the 
        /// memory object.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the reallocated memory object. If the function 
        /// fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr GlobalReAlloc
            (
                IntPtr hMem,
                uint dwBytes,
                GlobalAllocFlags uFlags
            );

        /// <summary>
        /// retrieves the current size of the specified global memory 
        /// object, in bytes.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the GlobalAlloc or 
        /// GlobalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// size of the specified global memory object, in bytes.
        /// If the specified handle is not valid or if the object has 
        /// been discarded, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint GlobalSize
            (
                IntPtr hMem
            );

        /// <summary>
        /// The HeapAlloc function allocates a block of memory from 
        /// a heap. The allocated memory is not movable.
        /// </summary>
        /// <param name="hHeap">Handle to the heap from which the memory 
        /// will be allocated. This handle is returned by the HeapCreate 
        /// or GetProcessHeap function.</param>
        /// <param name="dwFlags"><para>Heap allocation control. 
        /// Specifying any of these values will override the 
        /// corresponding value specified when the heap was created 
        /// with HeapCreate. This parameter can be one or more of the 
        /// following values:</para>
        /// <list type="bullet">
        /// <item>HEAP_GENERATE_EXCEPTIONS</item>
        /// <item>HEAP_NO_SERIALIZE</item>
        /// <item>HEAP_ZERO_MEMORY</item>
        /// </list>
        /// </param>
        /// <param name="dwBytes"><para>Number of bytes to be allocated.
        /// </para>
        /// <para>If the heap specified by the hHeap parameter is a 
        /// "non-growable" heap, dwBytes must be less than 0x7FFF8. 
        /// You create a non-growable heap by calling the HeapCreate 
        /// function with a nonzero value.</para></param>
        /// <returns>If the function succeeds, the return value is a 
        /// pointer to the allocated memory block. If the function fails 
        /// and you have not specified HEAP_GENERATE_EXCEPTIONS, the 
        /// return value is NULL. If the function fails and you have 
        /// specified HEAP_GENERATE_EXCEPTIONS, the function may generate 
        /// exceptions.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr HeapAlloc
            (
                IntPtr hHeap,
                HeapFlags dwFlags,
                uint dwBytes
            );

        /// <summary>
        /// The HeapCompact function attempts to compact a specified 
        /// heap. It compacts the heap by coalescing adjacent free 
        /// blocks of memory and decommitting large free blocks of 
        /// memory.
        /// </summary>
        /// <param name="hHeap">Handle to the heap to be compacted. 
        /// This handle is returned by either the HeapCreate or 
        /// GetProcessHeap function.</param>
        /// <param name="dwFlags"><para>Heap access control. This 
        /// parameter can be the following value:</para>
        /// <list type="bullet">
        /// <item>HEAP_NO_SERIALIZE</item>
        /// </list>
        /// </param>
        /// <returns>If the function succeeds, the return value is the 
        /// size of the largest committed free block in the heap, in 
        /// bytes. If the function fails, the return value is zero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint HeapCompact
            (
                IntPtr hHeap,
                HeapFlags dwFlags
            );

        /// <summary>
        /// The HeapCreate function creates a heap object that can be 
        /// used by the calling process. The function reserves space 
        /// in the virtual address space of the process and allocates 
        /// physical storage for a specified initial portion of this 
        /// block.
        /// </summary>
        /// <param name="flOptions"><para>Heap allocation attributes. 
        /// These options affect subsequent access to the new heap 
        /// through calls to the heap functions. This parameter can be 
        /// one or more of the following values:</para>
        /// <list type="bullet">
        /// <item>HEAP_GENERATE_EXCEPTIONS</item>
        /// <item>HEAP_NO_SERIALIZE</item>
        /// </list>
        /// </param>
        /// <param name="dwInitialSize">Initial size of the heap, 
        /// in bytes. This value determines the initial amount of 
        /// physical storage that is allocated for the heap. The value 
        /// is rounded up to the next page boundary.</param>
        /// <param name="dwMaximumSize"><para>Maximum size of the heap, 
        /// in bytes. The HeapCreate function rounds dwMaximumSize 
        /// up to the next page boundary, and then reserves a block 
        /// of that size in the process's virtual address space for 
        /// the heap. If allocation requests made by the HeapAlloc 
        /// or HeapReAlloc functions exceed the initial amount of 
        /// physical storage specified by dwInitialSize, the system 
        /// allocates additional pages of physical storage for the heap, 
        /// up to the heap's maximum size. However, the heap cannot grow, 
        /// so the maximum size of a memory block in the heap is a bit 
        /// less than 0x7FFF8 bytes. Requests to allocate larger blocks 
        /// will fail, even if the maximum size of the heap is large 
        /// enough to contain the block.</para>
        /// <para>If dwMaximumSize is zero, the heap is growable. 
        /// The heap's size is limited only by available memory. 
        /// Requests to allocate blocks larger than 0x7FFF8 bytes do 
        /// not automatically fail; the system calls the VirtualAlloc 
        /// function to obtain the memory needed for such large blocks. 
        /// Applications that need to allocate large memory blocks 
        /// should set dwMaximumSize to zero.</para></param>
        /// <returns>If the function succeeds, the return value is 
        /// a handle to the newly created heap. If the function fails, 
        /// the return value is is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr HeapCreate
            (
                HeapFlags flOptions,
                uint dwInitialSize,
                uint dwMaximumSize
            );

        /// <summary>
        /// The HeapDestroy function destroys the specified heap object. 
        /// HeapDestroy decommits and releases all the pages of a 
        /// private heap object, and it invalidates the handle to the 
        /// heap.
        /// </summary>
        /// <param name="hHeap">Handle to the heap to be destroyed. 
        /// This handle is returned by the HeapCreate function. 
        /// Do not use the handle to the process heap returned by the 
        /// GetProcessHeap function.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero. If the function fails, the return value is zero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool HeapDestroy
            (
                IntPtr hHeap
            );

        /// <summary>
        /// Frees a memory block allocated from a heap by the HeapAlloc 
        /// or HeapReAlloc function.
        /// </summary>
        /// <param name="hHeap">Handle to the heap whose memory block is 
        /// to be freed. This handle is a returned by either the 
        /// HeapCreate or GetProcessHeap function.</param>
        /// <param name="dwFlags"><para>Heap free options. Specifying the 
        /// following value overrides the corresponding value specified 
        /// in the flOptions parameter when the heap was created by using 
        /// the HeapCreate function:</para>
        /// <list type="bullet">
        /// <item>HEAP_NO_SERIALIZE</item>
        /// </list>
        /// </param>
        /// <param name="lpMem">Pointer to the memory block to be freed. 
        /// This pointer is returned by the HeapAlloc or HeapReAlloc 
        /// function.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero. If the function fails, the return value is zero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool HeapFree
            (
                IntPtr hHeap,
                HeapFlags dwFlags,
                IntPtr lpMem
            );

        /// <summary>
        /// The HeapReAlloc function reallocates a block of memory from 
        /// a heap. This function enables you to resize a memory block 
        /// and change other memory block properties. The allocated 
        /// memory is not movable. 
        /// </summary>
        /// <param name="hHeap">Handle to the heap from which the memory 
        /// is to be reallocated.</param>
        /// <param name="dwFlags"><para>Heap reallocation options:</para>
        /// <list type="bullet">
        /// <item>HEAP_GENERATE_EXCEPTIONS</item>
        /// <item>HEAP_NO_SERIALIZE</item>
        /// <item>HEAP_REALLOC_IN_PLACE_ONLY</item>
        /// <item>HEAP_ZERO_MEMORY</item>
        /// </list>
        /// </param>
        /// <param name="lpMem">Pointer to the block of memory that the 
        /// function reallocates.</param>
        /// <param name="dwBytes">New size of the memory block, in bytes.
        /// A memory block's size can be increased or decreased by using 
        /// this function. If the heap specified by the hHeap parameter 
        /// is a "non-growable" heap, dwBytes must be less than 0x7FFF8. 
        /// You create a non-growable heap by calling the HeapCreate 
        /// function with a nonzero value.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// pointer to the reallocated memory block. If the function 
        /// fails and you have not specified HEAP_GENERATE_EXCEPTIONS, 
        /// the return value is NULL. If the function fails and you have 
        /// specified HEAP_GENERATE_EXCEPTIONS, the function may generate 
        /// exceptions.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr HeapReAlloc
            (
                IntPtr hHeap,
                HeapFlags dwFlags,
                IntPtr lpMem,
                uint dwBytes
            );

        /// <summary>
        /// Retrieves the size of a memory block allocated from a heap 
        /// by the HeapAlloc or HeapReAlloc function.
        /// </summary>
        /// <param name="hHeap">Handle to the heap in which the memory 
        /// block resides.</param>
        /// <param name="dwFlags"><para>Heap size options:</para>
        /// <list type="bullet">
        /// <item>HEAP_NO_SERIALIZE</item>
        /// </list>
        /// </param>
        /// <param name="lpMem">Pointer to the memory block whose size 
        /// the function will obtain.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// size of the allocated memory block, in bytes.
        /// If the function fails, the return value is (SIZE_T) -1.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint HeapSize
            (
                IntPtr hHeap,
                HeapFlags dwFlags,
                IntPtr lpMem
            );

        /// <summary>
        /// The HeapValidate function validates the specified heap. 
        /// The function scans all the memory blocks in the heap, 
        /// and verifies that the heap control structures maintained 
        /// by the heap manager are in a consistent state. You can also 
        /// use the HeapValidate function to validate a single memory 
        /// block within a specified heap, without checking the validity 
        /// of the entire heap.
        /// </summary>
        /// <param name="hHeap">Handle to the heap to be validated.
        /// </param>
        /// <param name="dwFlags"><para>Heap access options. This 
        /// parameter can be the following value:</para>
        /// <list type="bullet">
        /// <item>HEAP_NO_SERIALIZE</item>
        /// </list>
        /// </param>
        /// <param name="lpMem"><para>Pointer to a memory block within 
        /// the specified heap. This parameter may be NULL.</para>
        /// <para>If this parameter is NULL, the function attempts to 
        /// validate the entire heap specified by hHeap.</para>
        /// <para>If this parameter is not NULL, the function attempts 
        /// to validate the memory block pointed to by lpMem. It does 
        /// not attempt to validate the rest of the heap.</para></param>
        /// <returns><para>If the specified heap or memory block is 
        /// valid, the return value is nonzero.</para>
        /// <para>If the specified heap or memory block is invalid, 
        /// the return value is zero. On a system set up for debugging, 
        /// the HeapValidate function then displays debugging messages 
        /// that describe the part of the heap or memory block that is 
        /// invalid, and stops at a hard-coded breakpoint so that you 
        /// can examine the system to determine the source of the 
        /// invalidity. The HeapValidate function does not set the 
        /// thread's last error value.</para></returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool HeapValidate
            (
                IntPtr hHeap,
                HeapFlags dwFlags,
                IntPtr lpMem
            );

        /// <summary>
        /// Determines whether the calling process has read access 
        /// to the memory at the specified address.
        /// </summary>
        /// <param name="lpfn">Pointer to a memory address.</param>
        /// <returns>If the calling process has read access 
        /// to the specified memory, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool IsBadCodePtr
            (
                IntPtr lpfn
            );

        /// <summary>
        /// Verifies that the calling process has read access to 
        /// the specified range of memory.
        /// </summary>
        /// <param name="lp">Pointer to the first byte of the 
        /// memory block.</param>
        /// <param name="ucb">Size of the memory block, in bytes. 
        /// If this parameter is zero, the return value is zero.</param>
        /// <returns>If the calling process has read access to all bytes 
        /// in the specified memory range, the return value is zero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool IsBadReadPtr
            (
                IntPtr lp,
                uint ucb
            );

        /// <summary>
        /// Verifies that the calling process has read access to 
        /// a range of memory pointed to by a string pointer.
        /// </summary>
        /// <param name="lpsz">Pointer to a null-terminated ASCII string.
        /// </param>
        /// <param name="ucchMax">Maximum size of the string, in TCHARs. 
        /// The function checks for read access in all characters up to 
        /// the string's terminating null character or up to the number 
        /// of characters specified by this parameter, whichever is 
        /// smaller. If this parameter is zero, the return value is zero.
        /// </param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool IsBadStringPtrA
            (
                IntPtr lpsz,
                uint ucchMax
            );

        /// <summary>
        /// Verifies that the calling process has read access to 
        /// a range of memory pointed to by a string pointer.
        /// </summary>
        /// <param name="lpsz">Pointer to a null-terminated Unicode
        /// string.</param>
        /// <param name="ucchMax">Maximum size of the string, in TCHARs. 
        /// The function checks for read access in all characters up to 
        /// the string's terminating null character or up to the number 
        /// of characters specified by this parameter, whichever is 
        /// smaller. If this parameter is zero, the return value is zero.
        /// </param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool IsBadStringPtrW
            (
                IntPtr lpsz,
                uint ucchMax
            );

        /// <summary>
        /// Verifies that the calling process has write access to the 
        /// specified range of memory.
        /// </summary>
        /// <param name="lp">Pointer to the first byte of the memory 
        /// block.</param>
        /// <param name="ucb">Size of the memory block, in bytes. 
        /// If this parameter is zero, the return value is zero.</param>
        /// <returns>If the calling process has write access to all bytes 
        /// in the specified memory range, the return value is zero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool IsBadWritePtr
            (
                IntPtr lp,
                uint ucb
            );

        /// <summary>
        /// Determines whether the calling process is being debugged.
        /// </summary>
        /// <returns>If the current process is running in the context 
        /// of a debugger, the return value is nonzero.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsDebuggerPresent();

        /// <summary>
        /// Determines whether the specified processor feature 
        /// is supported by the current computer.
        /// </summary>
        /// <param name="ProcessorFeature"></param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern bool IsProcessorFeaturePresent
            (
                ProcessorFeatures ProcessorFeature
            );

        /// <summary>
        /// Determines whether the specified process is running under 
        /// WOW64.
        /// </summary>
        /// <param name="hProcess">Handle to a process.</param>
        /// <param name="Wow64Process">Pointer to a value that is set 
        /// to TRUE if the process is running under WOW64.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// nonzero value.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool IsWow64Process
            (
                IntPtr hProcess,
                ref bool Wow64Process
            );


        /// <summary>
        /// The LoadLibrary function maps the specified executable module into 
        /// the address space of the calling process.
        /// </summary>
        /// <param name="lpFileName">Pointer to a null-terminated string that 
        /// names the executable module (either a .dll or .exe file). The name 
        /// specified is the file name of the module and is not related to the 
        /// name stored in the library module itself, as specified by the LIBRARY 
        /// keyword in the module-definition (.def) file.</param>
        /// <returns>If the function succeeds, the return value is a handle to 
        /// the module.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr LoadLibrary
            (
                string lpFileName
            );

        /// <summary>
        /// The LoadLibraryEx function maps the specified executable 
        /// module into the address space of the calling process. 
        /// The executable module can be a .dll or an .exe file. 
        /// The specified module may cause other modules to be mapped 
        /// into the address space.
        /// </summary>
        /// <param name="lpFileName"><para>Pointer to a null-terminated 
        /// string that names the executable module (either a .dll or 
        /// an .exe file). The name specified is the file name of the 
        /// executable module. This name is not related to the name 
        /// stored in a library module itself, as specified by the 
        /// LIBRARY keyword in the module-definition (.def) file.
        /// </para>
        /// <para>If the string specifies a path, but the file does not 
        /// exist in the specified directory, the function fails. 
        /// When specifying a path, be sure to use backslashes (\), 
        /// not forward slashes (/).</para>
        /// <para>If the string does not specify a path, and the file 
        /// name extension is omitted, the function appends the default 
        /// library extension .dll to the file name. However, the file 
        /// name string can include a trailing point character (.) to 
        /// indicate that the module name has no extension.</para>
        /// <para>If the string does not specify a path, the function 
        /// uses a standard search strategy to find the file.</para>
        /// <para>If mapping the specified module into the address space 
        /// causes the system to map in other, associated executable 
        /// modules, the function can use either the standard search 
        /// strategy or an alternate search strategy to find those 
        /// modules.</para></param>
        /// <param name="hFile">This parameter is reserved for future use. 
        /// It must be NULL.</param>
        /// <param name="dwFlags">Action to take when loading the module. 
        /// If no flags are specified, the behavior of this function is 
        /// identical to that of the LoadLibrary function.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the mapped executable module. If the function 
        /// fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr LoadLibraryEx
            (
                string lpFileName,
                IntPtr hFile,
                LoadLibraryFlags dwFlags
            );

        /// <summary>
        /// Loads the specified resource into global memory.
        /// </summary>
        /// <param name="hModule">Handle to the module whose executable 
        /// file contains the resource. If hModule is NULL, the system 
        /// loads the resource from the module that was used to create 
        /// the current process.</param>
        /// <param name="hResInfo">Handle to the resource to be loaded. 
        /// This handle is returned by the FindResource or FindResourceEx 
        /// function.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the data associated with the resource. If the 
        /// function fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr LoadResource
            (
                IntPtr hModule,
                IntPtr hResInfo
            );

        /// <summary>
        /// The LocalAlloc function allocates the specified number 
        /// of bytes from the heap. Windows memory management does not 
        /// provide a separate local heap and global heap.
        /// </summary>
        /// <param name="uFlags">Memory allocation attributes.</param>
        /// <param name="dwBytes">Number of bytes to allocate. If this 
        /// parameter is zero and the uFlags parameter specifies 
        /// GMEM_MOVEABLE, the function returns a handle to a memory 
        /// object that is marked as discarded.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the newly allocated memory object. If the function 
        /// fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr LocalAlloc
            (
                LocalAllocFlags uFlags,
                uint dwBytes
            );

        /// <summary>
        /// The LocalAlloc function allocates the specified number 
        /// of bytes from the heap. Windows memory management does not 
        /// provide a separate local heap and global heap.
        /// </summary>
        /// <param name="dwBytes">Number of bytes to allocate.</param>
        /// <returns>Handle to newly allocated memory object.</returns>
        [CLSCompliant(false)]
        public static IntPtr LocalAlloc(uint dwBytes)
        {
            return LocalAlloc(LocalAllocFlags.GMEM_FIXED, dwBytes);
        }

        /// <summary>
        /// Returns information about the specified global memory object.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the LocalAlloc or 
        /// LocalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value 
        /// specifies the allocation values and the lock count for 
        /// the memory object. If the function fails, the return value 
        /// is GMEM_INVALID_HANDLE, indicating that the global handle 
        /// is not valid.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern LocalAllocFlags LocalFlags
            (
                IntPtr hMem
            );

        /// <summary>
        /// The LocalFree function frees the specified global memory 
        /// object and invalidates its handle.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the LocalAlloc or 
        /// LocalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value 
        /// is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr LocalFree
            (
                IntPtr hMem
            );

        /// <summary>
        /// The LocalReAlloc function changes the size or attributes 
        /// of a specified global memory object. The size can increase 
        /// or decrease.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object to be 
        /// reallocated. This handle is returned by either the 
        /// LocalAlloc or LocalReAlloc function.</param>
        /// <param name="dwBytes">New size of the memory block, in bytes.
        /// If uFlags specifies GMEM_MODIFY, this parameter is ignored.
        /// </param>
        /// <param name="uFlags">Reallocation options. If GMEM_MODIFY is 
        /// specified, this parameter modifies the attributes of the 
        /// memory object, and the dwBytes parameter is ignored. 
        /// Otherwise, this parameter controls the reallocation of the 
        /// memory object.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the reallocated memory object. If the function 
        /// fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr LocalReAlloc
            (
                IntPtr hMem,
                uint dwBytes,
                LocalAllocFlags uFlags
            );

        /// <summary>
        /// retrieves the current size of the specified global memory 
        /// object, in bytes.
        /// </summary>
        /// <param name="hMem">Handle to the global memory object. 
        /// This handle is returned by either the LocalAlloc or 
        /// LocalReAlloc function.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// size of the specified global memory object, in bytes.
        /// If the specified handle is not valid or if the object has 
        /// been discarded, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint LocalSize
            (
                IntPtr hMem
            );

        /// <summary>
        /// Locks the specified resource in memory.
        /// </summary>
        /// <param name="hResData">Handle to the resource to be locked. 
        /// The LoadResource function returns this handle. Note that 
        /// this parameter is listed as an HGLOBAL variable only for 
        /// backwards compatibility. Do not pass any value as a 
        /// parameter other than a successful return value from the 
        /// LoadResource function.</param>
        /// <returns>If the loaded resource is locked, the return value 
        /// is a pointer to the first byte of the resource; otherwise, 
        /// it is NULL.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern IntPtr LockResource
            (
                IntPtr hResData
            );

        /// <summary>
        /// Maps a view of a file into the address space of the calling 
        /// process.
        /// </summary>
        /// <param name="hFileMappingObject">Handle to an open handle of 
        /// a file mapping object. The CreateFileMapping and 
        /// OpenFileMapping functions return this handle.</param>
        /// <param name="dwDesiredAccess">Type of access to the file view 
        /// and, therefore, the protection of the pages mapped by the 
        /// file.</param>
        /// <param name="dwFileOffsetHigh">High-order DWORD of the file 
        /// offset where mapping is to begin.</param>
        /// <param name="dwFileOffsetLow">Low-order DWORD of the file 
        /// offset where mapping is to begin. The combination of the 
        /// high and low offsets must specify an offset within the file 
        /// that matches the system's memory allocation granularity, or 
        /// the function fails. That is, the offset must be a multiple 
        /// of the allocation granularity. Use the GetSystemInfo 
        /// function, which fills in the members of a SYSTEM_INFO 
        /// structure, to obtain the system's memory allocation 
        /// granularity.</param>
        /// <param name="dwNumBytesToMap">Number of bytes of the file 
        /// to map. If this parameter is zero, the entire file is mapped.
        /// </param>
        /// <returns>If the function succeeds, the return value is the 
        /// starting address of the mapped view. If the function fails, 
        /// the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr MapViewOfFile
            (
                IntPtr hFileMappingObject,
                FileMappingFlags dwDesiredAccess,
                int dwFileOffsetHigh,
                uint dwFileOffsetLow,
                uint dwNumBytesToMap
            );

        /// <summary>
        /// The MapVirtualKey function translates (maps) a virtual-key code into 
        /// a scan code or character value, or translates a scan code into a 
        /// virtual-key code.
        /// </summary>
        /// <param name="uCode">Specifies the virtual-key code or scan code for 
        /// a key. How this value is interpreted depends on the value of the 
        /// uMapType parameter.</param>
        /// <param name="uMapType">Specifies the translation to perform.</param>
        /// <returns>The return value is either a scan code, a virtual-key code, 
        /// or a character value, depending on the value of uCode and uMapType. 
        /// If there is no translation, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern int MapVirtualKey
            (
                int uCode,
                MapKeyType uMapType
            );

        /// <summary>
        /// The MapVirtualKeyEx function translates (maps) a virtual-key code 
        /// into a scan code or character value, or translates a scan code into 
        /// a virtual-key code. The function translates the codes using the input 
        /// language and an input locale identifier.
        /// </summary>
        /// <param name="uCode">Specifies the virtual-key code or scan code for 
        /// a key. How this value is interpreted depends on the value of the 
        /// uMapType parameter.</param>
        /// <param name="uMapType">Specifies the translation to perform.</param>
        /// <param name="hkl">Input locale identifier to use for translating the 
        /// specified code.</param>
        /// <returns>The return value is either a scan code, a virtual-key code, 
        /// or a character value, depending on the value of uCode and uMapType. 
        /// If there is no translation, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern int MapVirtualKeyEx
            (
                int uCode,
                MapKeyType uMapType,
                IntPtr hkl
            );

        /// <summary>
        /// Moves an existing file or a directory, including its children.
        /// </summary>
        /// <param name="lpExistingFileName"><para>Pointer to a 
        /// null-terminated string that names an existing file 
        /// or directory.</para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="lpNewFileName"><para>Pointer to a 
        /// null-terminated string that specifies the new name 
        /// of a file or directory. The new name must not already 
        /// exist. A new file may be on a different file system 
        /// or drive. A new directory must be on the same drive.
        /// </para>
        /// <para>In the ANSI version of this function, the name 
        /// is limited to MAX_PATH characters. To extend this limit 
        /// to 32,767 wide characters, call the Unicode version of 
        /// the function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool MoveFile
            (
                string lpExistingFileName,
                string lpNewFileName
            );

        /// <summary>
        /// Moves an existing file or directory.
        /// </summary>
        /// <param name="lpExistingFileName"><para>Pointer to a 
        /// null-terminated string that names an existing file 
        /// or directory on the local computer.</para>
        /// <para>If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT, 
        /// the file cannot exist on a remote share because delayed 
        /// operations are performed before the network is available.
        /// </para>
        /// <para>In the ANSI version of this function, the name is 
        /// limited to MAX_PATH characters. To extend this limit to 
        /// 32,767 wide characters, call the Unicode version of the 
        /// function and prepend "\\?\" to the path.</para></param>
        /// <param name="lpNewFileName"><para>Pointer to a 
        /// null-terminated string that specifies the new name of 
        /// lpExistingFileName on the local computer.</para>
        /// <para>When moving a file, the destination can be on a 
        /// different file system or volume. If the destination is 
        /// on another drive, you must set the MOVEFILE_COPY_ALLOWED 
        /// flag in dwFlags.</para>
        /// <para>When moving a directory, the destination must be 
        /// on the same drive.</para>
        /// <para>If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT 
        /// and lpNewFileName is NULL, MoveFileEx registers the 
        /// lpExistingFileName file to be deleted when the system 
        /// restarts. If lpExistingFileName refers to a directory, 
        /// the system removes the directory at restart only if the 
        /// directory is empty.</para></param>
        /// <param name="dwFlags">Move options.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool MoveFileEx
            (
                string lpExistingFileName,
                string lpNewFileName,
                MoveFileFlags dwFlags
            );

        /// <summary>
        /// The MoveFileWithProgress function moves a file or directory. 
        /// MoveFileWithProgress is equivalent to the MoveFileEx function, 
        /// except that MoveFileWithProgress allows you to provide a 
        /// callback function that receives progress notifications.
        /// </summary>
        /// <param name="lpExistingFileName"><para>Pointer to a 
        /// null-terminated string that names an existing file 
        /// or directory on the local computer.</para>
        /// <para>If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT, 
        /// the file cannot exist on a remote share because delayed 
        /// operations are performed before the network is available.
        /// </para>
        /// <para>In the ANSI version of this function, the name is 
        /// limited to MAX_PATH characters. To extend this limit to 
        /// 32,767 wide characters, call the Unicode version of the 
        /// function and prepend "\\?\" to the path.</para>
        /// <para>Windows Me/98/95: This string must not exceed 
        /// MAX_PATH characters.</para></param>
        /// <param name="lpNewFileName"><para>Pointer to a null-terminated 
        /// string containing the new name of the file or directory on the 
        /// local computer.</para>
        /// <para>When moving a file, lpNewFileName can be on a different 
        /// file system or volume. If lpNewFileName is on another drive, 
        /// you must set the MOVEFILE_COPY_ALLOWED flag in dwFlags.
        /// </para>
        /// <para>When moving a directory, lpExistingFileName and 
        /// lpNewFileName must be on the same drive.</para>
        /// <para>If dwFlags specifies MOVEFILE_DELAY_UNTIL_REBOOT and 
        /// lpNewFileName is NULL, MoveFileWithProgress registers 
        /// lpExistingFileName to be deleted when the system restarts. 
        /// The function fails if it cannot access the registry to store 
        /// the information about the delete operation. If lpExistingFileName 
        /// refers to a directory, the system removes the directory at 
        /// restart only if the directory is empty.</para></param>
        /// <param name="lpProgressRoutine">Pointer to a CopyProgressRoutine 
        /// callback function that is called each time another portion of 
        /// the file has been moved. The callback function can be useful 
        /// if you provide a user interface that displays the progress of 
        /// the operation. This parameter can be NULL.</param>
        /// <param name="lpData">Argument to be passed to the 
        /// CopyProgressRoutine callback function. This parameter can 
        /// be NULL.</param>
        /// <param name="dwFlags">Move options.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool MoveFileWithProgress
            (
                string lpExistingFileName,
                string lpNewFileName,
                CopyProgressRoutine lpProgressRoutine,
                IntPtr lpData,
                MoveFileFlags dwFlags
            );

        /// <summary>
        /// Opens an existing named event object.
        /// </summary>
        /// <param name="dwDesiredAccess">Access to the event object.
        /// </param>
        /// <param name="bInheritHandle">If TRUE, a process created 
        /// by the CreateProcess function can inherit the handle.</param>
        /// <param name="lpName">Pointer to a null-terminated string that 
        /// names the event to be opened. Name comparisons are case 
        /// sensitive.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// handle to the event object. If the function fails, the return 
        /// value is NULL.</returns>
        /// <remarks>Terminal Services: The name can have a "Global\" or 
        /// "Local\" prefix to explicitly open an object in the global 
        /// or session name space. The remainder of the name can contain 
        /// any character except the backslash character (\).</remarks>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr OpenEvent
            (
                uint dwDesiredAccess,
                bool bInheritHandle,
                string lpName
            );

        /// <summary>
        /// Opens a named file mapping object.
        /// </summary>
        /// <param name="dwDesiredAccess">Access to the file mapping 
        /// object.</param>
        /// <param name="bInheritHandle">If this parameter is TRUE, 
        /// the new process inherits the handle.</param>
        /// <param name="lpName">Pointer to a string that names the file 
        /// mapping object to be opened.</param>
        /// <returns>If the function succeeds, the return value is an 
        /// open handle to the specified file mapping object. If the 
        /// function fails, the return value is NULL.</returns>
        /// <remarks>Terminal Services: The name can have a "Global\" or 
        /// "Local\" prefix to explicitly open an object in the global 
        /// or session name space. The remainder of the name can contain 
        /// any character except the backslash character (\).</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr OpenFileMapping
            (
                FileMappingFlags dwDesiredAccess,
                bool bInheritHandle,
                string lpName
            );


        /// <summary>
        /// Opens an existing process object.
        /// </summary>
        /// <param name="dwDesiredAccess">Access to the process object. 
        /// This access right is checked against any security descriptor 
        /// for the process.</param>
        /// <param name="bInheritHandle">If this parameter is TRUE, the 
        /// handle is inheritable.</param>
        /// <param name="dwProcessId">Identifier of the process to 
        /// open.</param>
        /// <returns>If the function succeeds, the return value is an 
        /// open handle to the specified process. If the function fails, 
        /// the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr OpenProcess
            (
                ProcessAccessFlags dwDesiredAccess,
                bool bInheritHandle,
                uint dwProcessId
            );

        /// <summary>
        /// Sends a string to the debugger for display.
        /// </summary>
        /// <param name="lpOutputString">Pointer to the null-terminated 
        /// string to be displayed.</param>
        [DllImport(DllName, SetLastError = false)]
        public static extern void OutputDebugString
            (
                string lpOutputString
            );

        /// <summary>
        /// The PeekNamedPipe function copies data from a named or 
        /// anonymous pipe into a buffer without removing it from the 
        /// pipe. It also returns information about data in the pipe.
        /// </summary>
        /// <param name="hNamedPipe">Handle to the pipe. This parameter 
        /// can be a handle to a named pipe instance, as returned by the 
        /// CreateNamedPipe or CreateFile function, or it can be a handle 
        /// to the read end of an anonymous pipe, as returned by the 
        /// CreatePipe function. The handle must have GENERIC_READ access 
        /// to the pipe.</param>
        /// <param name="lpBuffer">Pointer to a buffer that receives data 
        /// read from the pipe. This parameter can be NULL if no data is 
        /// to be read.</param>
        /// <param name="nBufferSize">Size of the buffer specified by the 
        /// lpBuffer parameter, in bytes. This parameter is ignored if 
        /// lpBuffer is NULL.</param>
        /// <param name="lpBytesRead">Pointer to a variable that receives 
        /// the number of bytes read from the pipe. This parameter can be 
        /// NULL if no data is to be read.</param>
        /// <param name="lpTotalBytesAvail">Pointer to a variable that 
        /// receives the total number of bytes available to be read from 
        /// the pipe. This parameter can be NULL if no data is to be read.
        /// </param>
        /// <param name="lpBytesLeftThisMessage">Pointer to a variable 
        /// that receives the number of bytes remaining in this message. 
        /// This parameter will be zero for byte-type named pipes or for 
        /// anonymous pipes. This parameter can be NULL if no data is to 
        /// be read.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool PeekNamedPipe
            (
                IntPtr hNamedPipe,
                byte[] lpBuffer,
                uint nBufferSize,
                ref uint lpBytesRead,
                ref uint lpTotalBytesAvail,
                ref uint lpBytesLeftThisMessage
            );

        /// <summary>
        /// Retrieves the Terminal Services session associated with a 
        /// specified process.
        /// </summary>
        /// <param name="dwProcessId">Specifies a process identifier.
        /// </param>
        /// <param name="pSessionId">Pointer to a variable that receives 
        /// the identifier of the Terminal Services session under which 
        /// the specified process is running. A value of zero identifies 
        /// the terminal server console session.</param>
        /// <returns>If the function succeeds, the return value is a 
        /// nonzero value.</returns>
        /// <remarks><para>If the calling process is not running in 
        /// a Terminal Services environment, the value returned in 
        /// pSessionId is zero.</para>
        /// <para>Included in: Windows 2000/XP/2003.</para></remarks>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool ProcessIdToSessionId
            (
                uint dwProcessId,
                ref uint pSessionId
            );

        /// <summary>
        /// <para>The PulseEvent function sets the specified event 
        /// object to the signaled state and then resets it to the 
        /// nonsignaled state after releasing the appropriate number of 
        /// waiting threads.</para>
        /// <para>Note: This function is unreliable and should not be 
        /// used. It exists mainly for backward compatibility.</para>
        /// </summary>
        /// <param name="hEvent">Handle to the event object.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool PulseEvent
            (
                IntPtr hEvent
            );

        /// <summary>
        /// <para>The QueryDosDevice function retrieves information 
        /// about MS-DOS device names. The function can obtain the 
        /// current mapping for a particular MS-DOS device name. 
        /// The function can also obtain a list of all existing 
        /// MS-DOS device names.</para>
        /// <para>MS-DOS device names are stored as junctions in the 
        /// object name space. The code that converts an MS-DOS path 
        /// into a corresponding path uses these junctions to map 
        /// MS-DOS devices and drive letters. The QueryDosDevice 
        /// function enables an application to query the names of the 
        /// junctions used to implement the MS-DOS device namespace 
        /// as well as the value of each specific junction.</para>
        /// </summary>
        /// <param name="lpDeviceName">Pointer to an MS-DOS device 
        /// name string specifying the target of the query. The device 
        /// name cannot have a trailing backslash. This parameter can 
        /// be NULL. In that case, the QueryDosDevice function will 
        /// store a list of all existing MS-DOS device names into the 
        /// buffer pointed to by lpTargetPath.</param>
        /// <param name="lpTargetPath"><para>Pointer to a buffer that 
        /// will receive the result of the query. The function fills 
        /// this buffer with one or more null-terminated strings. The 
        /// final null-terminated string is followed by an additional NULL.
        /// </para>
        /// <para>If lpDeviceName is non-NULL, the function retrieves 
        /// information about the particular MS-DOS device specified 
        /// by lpDeviceName. The first null-terminated string stored 
        /// into the buffer is the current mapping for the device. 
        /// The other null-terminated strings represent undeleted prior 
        /// mappings for the device.</para>
        /// <para>If lpDeviceName is NULL, the function retrieves a list 
        /// of all existing MS-DOS device names. Each null-terminated 
        /// string stored into the buffer is the name of an existing 
        /// MS-DOS device.</para></param>
        /// <param name="ucchMax">Maximum number of TCHARs that can be 
        /// stored into the buffer pointed to by lpTargetPath.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// number of TCHARs stored into the buffer pointed to by lpTargetPath.
        /// If the function fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int QueryDosDevice
            (
                string lpDeviceName,
                StringBuilder lpTargetPath,
                int ucchMax
            );

        /// <summary>
        /// Retrieves the current value of the high-resolution 
        /// performance counter.
        /// </summary>
        /// <param name="lpPerformanceCount">Pointer to a variable 
        /// that receives the current performance-counter value, 
        /// in counts.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool QueryPerformanceCounter
            (
                out long lpPerformanceCount
            );

        /// <summary>
        /// The QueryPerformanceFrequency function retrieves the 
        /// frequency of the high-resolution performance counter, 
        /// if one exists. The frequency cannot change while the 
        /// system is running.
        /// </summary>
        /// <param name="lpFrequency">Pointer to a variable that 
        /// receives the current performance-counter frequency, 
        /// in counts per second. If the installed hardware does 
        /// not support a high-resolution performance counter, 
        /// this parameter can be zero.</param>
        /// <returns>If the installed hardware supports a 
        /// high-resolution performance counter, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool QueryPerformanceFrequency
            (
                out long lpFrequency
            );

        /// <summary>
        /// <para>The ReadFile function reads data from a file, starting 
        /// at the position indicated by the file pointer. After the 
        /// read operation has been completed, the file pointer is 
        /// adjusted by the number of bytes actually read, unless the 
        /// file handle is created with the overlapped attribute. If the 
        /// file handle is created for overlapped input and output (I/O), 
        /// the application must adjust the position of the file pointer 
        /// after the read operation.</para>
        /// <para>This function is designed for both synchronous and 
        /// asynchronous operation. The ReadFileEx function is designed 
        /// solely for asynchronous operation. It lets an application 
        /// perform other processing during a file read operation.
        /// </para></summary>
        /// <param name="hFile">Handle to the file to be read.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives 
        /// the data read from the file.</param>
        /// <param name="nNumberOfBytesToRead">Number of bytes to be 
        /// read from the file.</param>
        /// <param name="lpNumberOfBytesRead"> Pointer to the variable 
        /// that receives the number of bytes read.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED 
        /// structure.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool ReadFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                int nNumberOfBytesToRead,
                out int lpNumberOfBytesRead,
                IntPtr lpOverlapped
            );

        /// <summary>
        /// <para>The ReadFile function reads data from a file, starting 
        /// at the position indicated by the file pointer. After the 
        /// read operation has been completed, the file pointer is 
        /// adjusted by the number of bytes actually read, unless the 
        /// file handle is created with the overlapped attribute. If the 
        /// file handle is created for overlapped input and output (I/O), 
        /// the application must adjust the position of the file pointer 
        /// after the read operation.</para>
        /// <para>This function is designed for both synchronous and 
        /// asynchronous operation. The ReadFileEx function is designed 
        /// solely for asynchronous operation. It lets an application 
        /// perform other processing during a file read operation.
        /// </para></summary>
        /// <param name="hFile">Handle to the file to be read.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives 
        /// the data read from the file.</param>
        /// <param name="nNumberOfBytesToRead">Number of bytes to be 
        /// read from the file.</param>
        /// <param name="lpNumberOfBytesRead"> Pointer to the variable 
        /// that receives the number of bytes read.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED 
        /// structure.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool ReadFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToRead,
                ref uint lpNumberOfBytesRead,
                ref OVERLAPPED lpOverlapped
            );

        /// <summary>
        /// The ReadProcessMemory function reads data from an area of 
        /// memory in a specified process. The entire area to be read 
        /// must be accessible, or the operation fails.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose memory 
        /// is being read. The handle must have PROCESS_VM_READ access 
        /// to the process.</param>
        /// <param name="lpBaseAddress">Pointer to the base address in 
        /// the specified process from which to read. Before any data 
        /// transfer occurs, the system verifies that all data in the 
        /// base address and memory of the specified size is accessible 
        /// for read access. If this is the case, the function proceeds; 
        /// otherwise, the function fails.</param>
        /// <param name="lpBuffer">Pointer to a buffer that receives the 
        /// contents from the address space of the specified process.
        /// </param>
        /// <param name="nSize">Number of bytes to be read from the 
        /// specified process.</param>
        /// <param name="lpNumberOfBytesRead">Pointer to a variable that 
        /// receives the number of bytes transferred into the specified 
        /// buffer. If lpNumberOfBytesRead is NULL, the parameter is 
        /// ignored.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool ReadProcessMemory
            (
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                byte[] lpBuffer,
                int nSize,
                ref int lpNumberOfBytesRead
            );

        /// <summary>
        /// Hides application from Process List in Windows 9x.
        /// </summary>
        /// <param name="dwProcessId">Pass GetCurrentProcessId() 
        /// here.</param>
        /// <param name="dwUnknown">Pass 1 here.</param>
        /// <remarks>Not implemented in NT family.</remarks>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern void RegisterServiceProcess
            (
                uint dwProcessId,
                uint dwUnknown
            );

        /// <summary>
        /// The ReplaceFile function replaces one file with another file, 
        /// with the option of creating a backup copy of the original file. 
        /// The replacement file assumes the name of the replaced file and 
        /// its identity.
        /// </summary>
        /// <param name="lpReplacedFileName"><para>Pointer to a 
        /// null-terminated string that specifies the name of the 
        /// file that will be replaced by the lpReplacementFileName file.
        /// </para>
        /// <para>This file is opened with the GENERIC_READ, DELETE, 
        /// and SYNCHRONIZE access rights. The sharing mode is 
        /// FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE.
        /// </para>
        /// <para>The caller must have write access to the file to be replaced.
        /// </para></param>
        /// <param name="lpReplacementFileName"><para>Pointer to a 
        /// null-terminated string that specifies the name of the file 
        /// that will replace the lpReplacedFileName file.
        /// </para>
        /// <para>The function attempts to open this file with the 
        /// SYNCHRONIZE, GENERIC_READ, GENERIC_WRITE, DELETE, and 
        /// WRITE_DAC access rights so that it can preserve all 
        /// attributes and ACLs. If this fails, the function attempts 
        /// to open the file with the SYNCHRONIZE, GENERIC_READ, DELETE, 
        /// and WRITE_DAC access rights. No sharing mode is specified.
        /// </para></param>
        /// <param name="lpBackupFileName">Pointer to a null-terminated 
        /// string that specifies the name of the file that will serve 
        /// as a backup copy of the lpReplacedFileName file. If this 
        /// parameter is NULL, no backup file is created.</param>
        /// <param name="dwReplaceFlags">Replacement options.</param>
        /// <param name="lpExclude">Reserved for future use.</param>
        /// <param name="lpReserved">Reserved for future use.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool ReplaceFile
            (
                string lpReplacedFileName,
                string lpReplacementFileName,
                string lpBackupFileName,
                ReplaceFileFlags dwReplaceFlags,
                IntPtr lpExclude,
                IntPtr lpReserved
            );

        /// <summary>
        /// Sets the specified event object to the nonsignaled state.
        /// </summary>
        /// <param name="hEvent">Handle to the event object.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool ResetEvent
            (
                IntPtr hEvent
            );

        /// <summary>
        /// Searches for the specified file in the specified path.
        /// </summary>
        /// <param name="lpPath"><para>Pointer to a null-terminated 
        /// string that specifies the path to be searched for the file. 
        /// If this parameter is NULL, the function searches for a 
        /// matching file in the following directories in the following 
        /// sequence:</para>
        /// <list type="number">
        /// <item>The directory from which the application loaded.</item>
        /// <item>The current directory.</item>
        /// <item>The system directory. Use the GetSystemDirectory 
        /// function to get the path of this directory.</item>
        /// <item>The 16-bit system directory. There is no function that 
        /// retrieves the path of this directory, but it is searched.</item>
        /// <item>The Windows directory. Use the GetWindowsDirectory function 
        /// to get the path of this directory.</item>
        /// <item>The directories that are listed in the PATH environment 
        /// variable.</item>
        /// </list></param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// that specifies the name of the file to search for.</param>
        /// <param name="lpExtension">Pointer to a null-terminated string 
        /// that specifies an extension to be added to the file name when 
        /// searching for the file. The first character of the file name 
        /// extension must be a period (.). The extension is added only 
        /// if the specified file name does not end with an extension.</param>
        /// <param name="nBufferLength"> Size of the buffer that receives 
        /// the valid path and file name, in TCHARs.</param>
        /// <param name="lpBuffer">Pointer to the buffer that receives the 
        /// path and file name of the file found.</param>
        /// <param name="lpFilePart">Pointer to the variable that receives 
        /// the address (within lpBuffer) of the last component of the valid 
        /// path and file name, which is the address of the character 
        /// immediately following the final backslash (\) in the path.</param>
        /// <returns>If the function succeeds, the value returned is the 
        /// length, in TCHARs, of the string copied to the buffer, 
        /// not including the terminating null character. If the return 
        /// value is greater than nBufferLength, the value returned is 
        /// the size of the buffer required to hold the path. 
        /// If the function fails, the return value is zero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern int SearchPath
            (
                string lpPath,
                string lpFileName,
                string lpExtension,
                int nBufferLength,
                StringBuilder lpBuffer,
                out IntPtr lpFilePart
            );

        /// <summary>
        /// The SecureZeroMemory function fills a block of memory 
        /// with zeros. It is designed to be a more secure version 
        /// of ZeroMemory.
        /// </summary>
        /// <param name="ptr">Pointer to the starting address of 
        /// the block of memory to fill with zeros.</param>
        /// <param name="cnt">Size of the block of memory to fill 
        /// with zeros, in bytes.</param>
        /// <remarks>Included in: Windows 2003.</remarks>
        [DllImport(DllName)]
        public static extern void SecureZeroMemory
            (
                IntPtr ptr,
                int cnt
            );

        /// <summary>
        /// Sets the specified screen buffer to be the currently 
        /// displayed console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleActiveScreenBuffer
            (
                IntPtr hConsoleOutput
            );

        /// <summary>
        /// The SetConsoleCP function sets the input code page used 
        /// by the console associated with the calling process. 
        /// A console uses its input code page to translate keyboard 
        /// input into the corresponding character value.
        /// </summary>
        /// <param name="wCodePageID">Identifier of the code page to 
        /// be set. The identifiers of the code pages available on the 
        /// local computer are stored in the registry under the 
        /// following key: 
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool SetConsoleCP
            (
                uint wCodePageID
            );

        /// <summary>
        /// Sets the size and visibility of the cursor for the specified 
        /// console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="lpConsoleCursorInfo">Pointer to a CONSOLE_CURSOR_INFO 
        /// structure that provides the new specifications for the console 
        /// screen buffer's cursor.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleCursorInfo
            (
                IntPtr hConsoleOutput,
                ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
            );

        /// <summary>
        /// Sets the cursor position in the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="dwCursorPosition">A COORD structure that specifies 
        /// the new cursor position. The coordinates are the column and row 
        /// of a screen buffer character cell. The coordinates must be within 
        /// the boundaries of the console screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleCursorPosition
            (
                IntPtr hConsoleOutput,
                COORD dwCursorPosition
            );

        /// <summary>
        /// Undocumented function.
        /// </summary>
        /// <param name="hConsole"></param>
        /// <param name="dwIndex"></param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleFont
            (
                IntPtr hConsole,
                int dwIndex
            );

        /// <summary>
        /// Sets the input mode of a console's input buffer or the output mode 
        /// of a console screen buffer.
        /// </summary>
        /// <param name="hConsoleHandle">Handle to a console input buffer or a 
        /// console screen buffer. The handle must have the GENERIC_READ access 
        /// right.</param>
        /// <param name="dwMode">Input or output mode to be set.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleMode
            (
                IntPtr hConsoleHandle,
                ConsoleModeFlags dwMode
            );

        /// <summary>
        /// The SetConsoleOutputCP function sets the output code page 
        /// used by the console associated with the calling process. 
        /// A console uses its output code page to translate the 
        /// character values written by the various output functions 
        /// into the images displayed in the console window.
        /// </summary>
        /// <param name="wCodePageID">Identifier of the code page to 
        /// set. The identifiers of the code pages available on the 
        /// local computer are stored in the registry under the 
        /// following key:
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool SetConsoleOutputCP
            (
                uint wCodePageID
            );

        /// <summary>
        /// Changes the size of the specified console screen buffer.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="dwSize">A COORD structure that specifies the 
        /// new size of the console screen buffer, in rows and columns. 
        /// The specified width and height cannot be less than the width 
        /// and height of the console screen buffer's window. The specified 
        /// dimensions also cannot be less than the minimum size allowed by 
        /// the system. This minimum depends on the current font size for 
        /// the console (selected by the user) and the SM_CXMIN and SM_CYMIN 
        /// values returned by the GetSystemMetrics function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize
            (
                IntPtr hConsoleOutput,
                COORD dwSize
            );

        /// <summary>
        /// The SetConsoleTextAttribute function sets the attributes of 
        /// characters written to the console screen buffer by the WriteFile 
        /// or WriteConsole function, or echoed by the ReadFile or ReadConsole 
        /// function. This function affects text written after the function 
        /// call.</summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="wAttributes"></param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleTextAttribute
            (
                IntPtr hConsoleOutput,
                ConsoleAttributes wAttributes
            );

        /// <summary>
        /// Sets the title bar string for the current console window.
        /// </summary>
        /// <param name="lpConsoleTitle">Pointer to a null-terminated 
        /// string that contains the string to be displayed in the title 
        /// bar of the console window. The total size must be less than 
        /// 64K.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleTitle
            (
                string lpConsoleTitle
            );

        /// <summary>
        /// Sets the current size and position of a console screen 
        /// buffer's window.
        /// </summary>
        /// <param name="hConsoleOutput">Handle to a console screen buffer. 
        /// The handle must have the GENERIC_READ access right.</param>
        /// <param name="bAbsolute">If this parameter is TRUE, the coordinates 
        /// specify the new upper-left and lower-right corners of the window. 
        /// If it is FALSE, the coordinates are offsets to the current 
        /// window-corner coordinates.</param>
        /// <param name="lpConsoleWindow"></param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetConsoleWindowInfo
            (
                IntPtr hConsoleOutput,
                bool bAbsolute,
                ref SMALL_RECT lpConsoleWindow
            );

        /// <summary>
        /// Adds a directory to the search path used to locate 
        /// DLLs for the application.
        /// </summary>
        /// <param name="lpPathName">Pointer to a null-terminated string 
        /// that specifies the directory to be added to the search path. 
        /// If this parameter is NULL, the default search path is used.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks>Requires Windows XP SP1.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetDllDirectory
            (
                string lpPathName
            );

        /// <summary>
        /// Controls whether the system will handle the specified types 
        /// of serious errors, or whether the process will handle them.
        /// </summary>
        /// <param name="uMode">Process error mode.</param>
        /// <returns>The return value is the previous state of the 
        /// error-mode bit flags.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint SetErrorMode
            (
                ErrorModeFlags uMode
            );

        /// <summary>
        /// Sets the specified event object to the signaled state.
        /// </summary>
        /// <param name="hEvent">Handle to the event object.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetEvent
            (
                IntPtr hEvent
            );

        /// <summary>
        /// Sets the date and time that a file was created, last 
        /// accessed, or last modified.
        /// </summary>
        /// <param name="hFile">Handle to the file for which to set 
        /// the dates and times. The file handle must have been created 
        /// with the FILE_WRITE_ATTRIBUTES access right.</param>
        /// <param name="lpCreationTime">Pointer to a FILETIME structure 
        /// that contains the date and time the file was created. This 
        /// parameter can be NULL if the application does not need to 
        /// set this information.</param>
        /// <param name="lpLastAccessTime">Pointer to a FILETIME 
        /// structure that contains the date and time the file was last 
        /// accessed. The last access time includes the last time the 
        /// file was written to, read from, or (in the case of 
        /// executable files) run. This parameter can be NULL if the 
        /// application does not need to set this information.</param>
        /// <param name="lpLastWriteTime">Pointer to a FILETIME structure 
        /// that contains the date and time the file was last written to.
        /// This parameter can be NULL if the application does not want 
        /// to set this information.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetFileTime
            (
                IntPtr hFile,
                ref FILETIME lpCreationTime,
                ref FILETIME lpLastAccessTime,
                ref FILETIME lpLastWriteTime
            );

        /// <summary>
        /// The SetFileValidData function sets the valid data length 
        /// of the specified file. The file cannot be either compressed 
        /// or sparse.
        /// </summary>
        /// <param name="hFile"><para>Handle to the file. The file must 
        /// have been opened with the GENERIC_WRITE access right.</para>
        /// <para>The file cannot be compressed or sparse.</para></param>
        /// <param name="ValidDataLength">New valid data length for the 
        /// file. This parameter must be a positive value that is greater 
        /// than the current valid data length, but less than the current 
        /// file size.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        /// <remarks>Included in Windows XP.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetFileValidData
            (
                IntPtr hFile,
                long ValidDataLength
            );

        /// <summary>
        /// Sets limits for a job object.
        /// </summary>
        /// <param name="hJob">A handle to the job whose
        /// limits are being set. The CreateJobObject
        /// or OpenJobObject function returns this handle.
        /// The handle must have the JOB_OBJECT_SET_ATTRIBUTES
        /// access right.</param>
        /// <param name="infoType">The information class
        /// for the limits to be set. </param>
        /// <param name="lpJobObjectInfo">The limits or job
        /// state to be set for the job. The format of this
        /// data depends on the value of JobObjectInfoClass.
        /// </param>
        /// <param name="cbJobObjectInfoLength">The size of
        /// the job information being set, in bytes.</param>
        /// <returns>If the function succeeds, the return
        /// value is nonzero.</returns>
        [CLSCompliant(false)]
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetInformationJobObject
            (
                JobObjectHandle hJob,
                JobObjectInfoType infoType,
                IntPtr lpJobObjectInfo,
                uint cbJobObjectInfoLength
            );

        /// <summary>
        /// Sets the last-error code for the calling thread.
        /// </summary>
        /// <param name="dwErrCode">Last-error code for the thread.
        /// </param>
        [DllImport(DllName, SetLastError = false)]
        [CLSCompliant(false)]
        public static extern void SetLastError
            (
                uint dwErrCode
            );

        /// <summary>
        /// Sets the current local time and date.
        /// </summary>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// that contains the current local date and time. The wDayOfWeek 
        /// member of the SYSTEMTIME structure is ignored.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetLocalTime
            (
                ref SYSTEMTIME lpSystemTime
            );

        /// <summary>
        /// The SetNamedPipeHandleState function sets the read mode 
        /// and the blocking mode of the specified named pipe. If the 
        /// specified handle is to the client end of a named pipe and 
        /// if the named pipe server process is on a remote computer, 
        /// the function can also be used to control local buffering.
        /// </summary>
        /// <param name="hNamedPipe"><para>Handle to the named pipe 
        /// instance. This parameter can be a handle to the server end 
        /// of the pipe, as returned by the CreateNamedPipe function, 
        /// or to the client end of the pipe, as returned by the 
        /// CreateFile function. The handle must have GENERIC_WRITE 
        /// access to the named pipe.</para>
        /// <para>This parameter can also be a handle to an anonymous 
        /// pipe, as returned by the CreatePipe function.</para></param>
        /// <param name="lpMode">Pointer to a variable that supplies the 
        /// new mode. The mode is a combination of a read-mode flag and 
        /// a wait-mode flag. This parameter can be NULL if the mode is 
        /// not being set.</param>
        /// <param name="lpMaxCollectionCount">Pointer to a variable that 
        /// specifies the maximum number of bytes collected on the client 
        /// computer before transmission to the server. This parameter 
        /// must be NULL if the specified pipe handle is to the server 
        /// end of a named pipe or if client and server processes are on 
        /// the same machine. This parameter is ignored if the client 
        /// process specifies the FILE_FLAG_WRITE_THROUGH flag in the 
        /// CreateFile function when the handle was created. This 
        /// parameter can be NULL if the collection count is not 
        /// being set.</param>
        /// <param name="lpCollectDataTimeout">Pointer to a variable that 
        /// specifies the maximum time, in milliseconds, that can pass 
        /// before a remote named pipe transfers information over the 
        /// network. This parameter must be NULL if the specified pipe 
        /// handle is to the server end of a named pipe or if client and 
        /// server processes are on the same computer. This parameter is 
        /// ignored if the client process specified the 
        /// FILE_FLAG_WRITE_THROUGH flag in the CreateFile function when 
        /// the handle was created. This parameter can be NULL if the 
        /// collection count is not being set.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool SetNamedPipeHandleState
            (
                IntPtr hNamedPipe,
                ref PipeModeFlags lpMode,
                ref uint lpMaxCollectionCount,
                ref uint lpCollectDataTimeout
            );

        /// <summary>
        /// Sets the minimum and maximum working set sizes for the 
        /// specified process.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose working 
        /// set sizes is to be set. The handle must have the 
        /// PROCESS_SET_QUOTA access right.</param>
        /// <param name="dwMinimumWorkingSetSize">Minimum working set 
        /// size for the process, in bytes. The virtual memory manager 
        /// attempts to keep at least this much memory resident in the 
        /// process whenever the process is active. If both 
        /// dwMinimumWorkingSetSize and dwMaximumWorkingSetSize have 
        /// the value -1, the function temporarily trims the working 
        /// set of the specified process to zero. This essentially 
        /// swaps the process out of physical RAM memory.</param>
        /// <param name="dwMaximumWorkingSetSize">Maximum working set 
        /// size for the process, in bytes. The virtual memory manager 
        /// attempts to keep no more than this much memory resident in 
        /// the process whenever the process is active and memory is in 
        /// short supply.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint SetProcessWorkingSetSize
            (
                IntPtr hProcess,
                uint dwMinimumWorkingSetSize,
                uint dwMaximumWorkingSetSize
            );

        /// <summary>
        /// Sets the handle for the standard input, standard output, 
        /// or standard error device.
        /// </summary>
        /// <param name="nStdHandle">Standard device for which the handle 
        /// to be set.</param>
        /// <param name="hHandle"></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool SetStdHandle
            (
                StdHandles nStdHandle,
                IntPtr hHandle
            );

        /// <summary>
        /// The SetSystemPowerState function suspends the system by 
        /// shutting power down. Depending on the ForceFlag parameter, 
        /// the function either suspends operation immediately or 
        /// requests permission from all applications and device 
        /// drivers before doing so.
        /// </summary>
        /// <param name="fSuspend"><para>If this parameter is TRUE, 
        /// the system is suspended. If the parameter is FALSE, the 
        /// system hibernates.</para>
        /// <para>Windows Me/98/95: Ignored.</para></param>
        /// <param name="fForce">If this parameter is TRUE, the function 
        /// broadcasts a PBT_APMSUSPEND event to each application and 
        /// driver, then immediately suspends operation. If the parameter 
        /// is FALSE, the function broadcasts a PBT_APMQUERYSUSPEND event 
        /// to each application to request permission to suspend operation.
        /// </param>
        /// <returns>If power has been suspended and subsequently restored, 
        /// the return value is nonzero. If the system was not suspended, 
        /// the return value is zero.</returns>
        /// <remarks>The calling process must have the SE_SHUTDOWN_NAME 
        /// privilege. To enable the SE_SHUTDOWN_NAME privilege, use the 
        /// AdjustTokenPrivileges function.</remarks>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetSystemPowerState
            (
                bool fSuspend,
                bool fForce
            );

        /// <summary>
        /// The SetSystemTime function sets the current system time 
        /// and date. The system time is expressed in Coordinated 
        /// Universal Time (UTC).
        /// </summary>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// that contains the current system date and time. The 
        /// wDayOfWeek member of the SYSTEMTIME structure is ignored.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetSystemTime
            (
                ref SYSTEMTIME lpSystemTime
            );

        /// <summary>
        /// The SetSystemTimeAdjustment function enables or disables 
        /// periodic time adjustments to the system's time-of-day clock. 
        /// Such time adjustments are used to synchronize the time of 
        /// day with some other source of time information. When 
        /// periodic time adjustments are enabled, they are applied at 
        /// each clock interrupt.
        /// </summary>
        /// <param name="dwTimeAdjustment">Number of 100-nanosecond 
        /// units added to the time-of-day clock at each clock interrupt 
        /// if periodic time adjustment is enabled.</param>
        /// <param name="bTimeAdjustmentDisabled"><para>Time adjustment 
        /// mode that the system is to use. Periodic system time 
        /// adjustments can be disabled or enabled.</para>
        /// <para>A value of TRUE specifies that periodic time 
        /// adjustment is to be disabled. The system is free to adjust 
        /// time of day using its own internal mechanisms. The value of 
        /// dwTimeAdjustment is ignored. The system's internal adjustment 
        /// mechanisms may cause the time-of-day clock to jump noticeably 
        /// when adjustments are made.</para>
        /// <para>A value of FALSE specifies that periodic time 
        /// adjustment is to be enabled, and will be used to adjust 
        /// the time-of-day clock. The system will not interfere with 
        /// the time adjustment scheme, and will not attempt to 
        /// synchronize time of day on its own. The system will add the 
        /// value of dwTimeAdjustment to the time of day at each clock 
        /// interrupt.</para></param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool SetSystemTimeAdjustment
            (
                uint dwTimeAdjustment,
                bool bTimeAdjustmentDisabled
            );

        /// <summary>
        /// The SetThreadExecutionState function enables applications 
        /// to inform the system that it is in use, thereby preventing 
        /// the system from entering the sleeping power state while 
        /// the application is running.
        /// </summary>
        /// <param name="esFlags">Thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// previous thread execution state. If the function fails, 
        /// the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = false)]
        public static extern ExecutionState SetThreadExecutionState
            (
                ExecutionState esFlags
            );

        /// <summary>
        /// Sets the label of a file system volume.
        /// </summary>
        /// <param name="lpRootPathName">Pointer to a null-terminated 
        /// string specifying the root directory of a file system volume. 
        /// This is the volume the function will label. A trailing backslash 
        /// is required. If this parameter is NULL, the root of the current 
        /// directory is used.</param>
        /// <param name="lpVolumeName">Pointer to a string specifying a name 
        /// for the volume. If this parameter is NULL, the function deletes 
        /// the label from the specified volume.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetVolumeLabel
            (
                string lpRootPathName,
                string lpVolumeName
            );

        /// <summary>
        /// Mounts the specified volume at the specified volume mount point.
        /// </summary>
        /// <param name="lpszVolumeMountPoint">Pointer to a string that 
        /// indicates the volume mount point where the volume is to be 
        /// mounted. This may be a root directory (X:\) or a directory 
        /// on a volume (X:\mnt\). The string must end with a trailing 
        /// backslash ('\').</param>
        /// <param name="lpszVolumeName">Pointer to a string that indicates 
        /// the volume to be mounted. This string must be a unique volume 
        /// name of the form "\\?\Volume{GUID}\" where GUID is a GUID that 
        /// identifies the volume. The \\?\ turns off path parsing and is 
        /// ignored as part of the path, as discussed in Naming a Volume. 
        /// For example, "\\?\C:\myworld\private" is seen as 
        /// "C:\myworld\private".</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetVolumeMountPoint
            (
                string lpszVolumeMountPoint,
                string lpszVolumeName
            );

        /// <summary>
        /// Converts a system time to a file time.
        /// </summary>
        /// <param name="lpSystemTime">Pointer to a SYSTEMTIME structure 
        /// that contains the time to be converted. The wDayOfWeek 
        /// member of the SYSTEMTIME structure is ignored.</param>
        /// <param name="lpFileTime">Pointer to a FILETIME structure 
        /// to receive the converted system time.</param>
        /// <returns>If the function succeeds, the return value is 
        /// nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool SystemTimeToFileTime
            (
                ref SYSTEMTIME lpSystemTime,
                ref FILETIME lpFileTime
            );

        /// <summary>
        /// Terminates the specified process and all of its threads.
        /// </summary>
        /// <param name="hProcess">Handle to the process to terminate.
        /// The handle must have the PROCESS_TERMINATE access right.
        /// </param>
        /// <param name="uExitCode">Exit code to be used by the process 
        /// and threads terminated as a result of this call.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool TerminateProcess
            (
                IntPtr hProcess,
                uint uExitCode
            );

        /// <summary>
        /// The TransactNamedPipe function combines the functions that 
        /// write a message to and read a message from the specified 
        /// named pipe into a single network operation.
        /// </summary>
        /// <param name="hNamedPipe"><para>Handle to the named pipe 
        /// returned by the CreateNamedPipe or CreateFile function.</para>
        /// <para>This parameter can also be a handle to an anonymous 
        /// pipe, as returned by the CreatePipe function.</para></param>
        /// <param name="lpInBuffer">Pointer to the buffer containing the 
        /// data to be written to the pipe.</param>
        /// <param name="nInBufferSize">Size of the input buffer, 
        /// in bytes.</param>
        /// <param name="lpOutBuffer">Pointer to the buffer that receives 
        /// the data read from the pipe.</param>
        /// <param name="nOutBufferSize">Size of the output buffer, 
        /// in bytes.</param>
        /// <param name="lpBytesRead">Pointer to the variable that 
        /// receives the number of bytes read from the pipe. If 
        /// lpOverlapped is NULL, lpBytesRead cannot be NULL.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED 
        /// structure.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool TransactNamedPipe
            (
                IntPtr hNamedPipe,
                byte[] lpInBuffer,
                uint nInBufferSize,
                byte[] lpOutBuffer,
                uint nOutBufferSize,
                ref uint lpBytesRead,
                ref OVERLAPPED lpOverlapped
            );

        /// <summary>
        /// Unmaps a mapped view of a file from the calling process's 
        /// address space.
        /// </summary>
        /// <param name="lpBaseAddress">Pointer to the base address of 
        /// the mapped view of a file that is to be unmapped. This value 
        /// must be identical to the value returned by a previous call 
        /// to the MapViewOfFile or MapViewOfFileEx function.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool UnmapViewOfFile
            (
                IntPtr lpBaseAddress
            );

        /// <summary>
        /// Reserves or commits a region of pages in the virtual 
        /// address space of the calling process. Memory allocated 
        /// by this function is automatically initialized to zero, 
        /// unless MEM_RESET is specified.
        /// </summary>
        /// <param name="lpAddress">Starting address of the region to 
        /// allocate. If the memory is being reserved, the specified 
        /// address is rounded down to the next 64-kilobyte boundary. 
        /// If the memory is already reserved and is being committed, 
        /// the address is rounded down to the next page boundary. 
        /// To determine the size of a page on the host computer, 
        /// use the GetSystemInfo function. If this parameter is NULL, 
        /// the system determines where to allocate the region.</param>
        /// <param name="dwSize">Size of the region, in bytes. If the 
        /// lpAddress parameter is NULL, this value is rounded up to 
        /// the next page boundary. Otherwise, the allocated pages 
        /// include all pages containing one or more bytes in the range 
        /// from lpAddress to (lpAddress+dwSize). This means that a 
        /// 2-byte range straddling a page boundary causes both pages 
        /// to be included in the allocated region.</param>
        /// <param name="flAllocationType">Type of memory allocation.
        /// </param>
        /// <param name="flProtect">Memory protection for the region 
        /// of pages to be allocated.</param>
        /// <returns>If the function succeeds, the return value is 
        /// the base address of the allocated region of pages. If the 
        /// function fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr VirtualAlloc
            (
                IntPtr lpAddress,
                uint dwSize,
                MemoryFlags flAllocationType,
                MemoryProtectionFlags flProtect
            );

        /// <summary>
        /// The VirtualAllocEx function reserves or commits a region 
        /// of memory within the virtual address space of a specified 
        /// process. The function initializes the memory it allocates 
        /// to zero, unless MEM_RESET is used.
        /// </summary>
        /// <param name="hProcess"><para>Handle to a process. The 
        /// function allocates memory within the virtual address space 
        /// of this process.</para>
        /// <para>You must have PROCESS_VM_OPERATION access to the 
        /// process. If you do not, the function fails.</para></param>
        /// <param name="lpAddress"><para>Pointer that specifies a 
        /// desired starting address for the region of pages that you 
        /// want to allocate.</para>
        /// <para>If you are reserving memory, the function rounds this 
        /// address down to the nearest 64-kilobyte boundary.</para>
        /// <para>If you are committing memory that is already reserved, 
        /// the function rounds this address down to the nearest page 
        /// boundary. To determine the size of a page on the host 
        /// computer, use the GetSystemInfo function.</para>
        /// <para>If lpAddress is NULL, the function determines where 
        /// to allocate the region.</para></param>
        /// <param name="dwSize"><para>Size of the region of memory to 
        /// allocate, in bytes.</para>
        /// <para>If lpAddress is NULL, the function rounds dwSize 
        /// up to the next page boundary.</para>
        /// <para>If lpAddress is not NULL, the function allocates 
        /// all pages that contain one or more bytes in the range from 
        /// lpAddress to (lpAddress+dwSize). This means, for example, 
        /// that a 2-byte range that straddles a page boundary causes 
        /// the function to allocate both pages.</para></param>
        /// <param name="flAllocationType"><para>Type of memory 
        /// allocation:</para>
        /// <list type="bullet">
        /// <item>MEM_COMMIT</item>
        /// <item>MEM_RESERVE</item>
        /// <item>MEM_RESET</item>
        /// </list></param>
        /// <param name="flProtect">Memory protection for the region of 
        /// pages to be allocated. If the pages are being committed, 
        /// you can specify any one of the memory protection options, 
        /// along with PAGE_GUARD or PAGE_NOCACHE, as needed.</param>
        /// <returns>If the function succeeds, the return value is the 
        /// base address of the allocated region of pages. If the 
        /// function fails, the return value is NULL.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern IntPtr VirtualAllocEx
            (
                IntPtr hProcess,
                IntPtr lpAddress,
                uint dwSize,
                MemoryFlags flAllocationType,
                MemoryProtectionFlags flProtect
            );

        /// <summary>
        /// The VirtualFree function releases, decommits, or releases 
        /// and decommits a region of pages within the virtual address 
        /// space of the calling process.
        /// </summary>
        /// <param name="lpAddress"><para>Pointer to the base address 
        /// of the region of pages to be freed.</para>
        /// <para>If the dwFreeType parameter is MEM_RELEASE, this 
        /// parameter must be the base address returned by the 
        /// VirtualAlloc function when the region of pages was reserved.
        /// </para></param>
        /// <param name="dwSize"><para>Size of the region of memory 
        /// to be freed, in bytes.</para>
        /// <para>If the dwFreeType parameter is MEM_RELEASE, this 
        /// parameter must be zero. The function frees the entire 
        /// region that was reserved in the initial allocation call 
        /// to VirtualAlloc.</para>
        /// <para>If the dwFreeType parameter is MEM_DECOMMIT, 
        /// the function decommits all memory pages that contain one 
        /// or more bytes in the range from the lpAddress parameter 
        /// to (lpAddress+dwSize). This means, for example, that 
        /// a 2-byte region of memory that straddles a page boundary 
        /// causes both pages to be decommitted. If lpAddress is the 
        /// base address returned by VirtualAlloc and dwSize is zero, 
        /// the function decommits the entire region that was allocated 
        /// by VirtualAlloc. Afterwards, the entire region is in the 
        /// reserved state.</para></param>
        /// <param name="dwFreeType"><para>Type of free operation. This 
        /// parameter can be one of the following values:</para>
        /// <list type="bullet">
        /// <item>MEM_DECOMMIT</item>
        /// <item>MEM_RELEASE</item>
        /// </list>
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualFree
            (
                IntPtr lpAddress,
                uint dwSize,
                MemoryFlags dwFreeType
            );

        /// <summary>
        /// The VirtualFreeEx function releases, decommits, or releases 
        /// and decommits a region of memory within the virtual address 
        /// space of a specified process.
        /// </summary>
        /// <param name="hProcess"><para>Handle to a process. The 
        /// function frees memory within the virtual address space of 
        /// this process.</para>
        /// <para>You must have PROCESS_VM_OPERATION access to this 
        /// process. If you do not, the function fails.</para></param>
        /// <param name="lpAddress"><para>Pointer to the starting address 
        /// of the region of memory to be freed.</para>
        /// <para>If the dwFreeType parameter is MEM_RELEASE, lpAddress 
        /// must be the base address returned by the VirtualAllocEx 
        /// function when the region was reserved.</para></param>
        /// <param name="dwSize"><para>Size of the region of memory to 
        /// free, in bytes.</para>
        /// <para>If the dwFreeType parameter is MEM_RELEASE, dwSize 
        /// must be zero. The function frees the entire region that was 
        /// reserved in the initial allocation call to VirtualAllocEx.
        /// </para>
        /// <para>If dwFreeType is MEM_DECOMMIT, the function decommits 
        /// all memory pages that contain one or more bytes in the range 
        /// from the lpAddress parameter to (lpAddress+dwSize). This 
        /// means, for example, that a 2-byte region of memory that 
        /// straddles a page boundary causes both pages to be decommitted. 
        /// If lpAddress is the base address returned by VirtualAllocEx 
        /// and dwSize is zero, the function decommits the entire region 
        /// that was allocated by VirtualAllocEx. Afterwards, the entire 
        /// region is in the reserved state.</para></param>
        /// <param name="dwFreeType"><para>Type of free operation. This 
        /// parameter can be one of the following values:</para>
        /// <list type="bullet">
        /// <item>MEM_DECOMMIT</item>
        /// <item>MEM_RELEASE</item>
        /// </list>
        /// </param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualFreeEx
            (
                IntPtr hProcess,
                IntPtr lpAddress,
                uint dwSize,
                MemoryFlags dwFreeType
            );

        /// <summary>
        /// Locks the specified region of the process's virtual address 
        /// space into physical memory, ensuring that subsequent access 
        /// to the region will not incur a page fault.
        /// </summary>
        /// <param name="lpAddress">Pointer to the base address of the 
        /// region of pages to be locked.</param>
        /// <param name="dwSize">Size of the region to be locked, in 
        /// bytes. The region of affected pages includes all pages that 
        /// contain one or more bytes in the range from the lpAddress 
        /// parameter to (lpAddress+dwSize). This means that a 2-byte 
        /// range straddling a page boundary causes both pages to be 
        /// locked.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualLock
            (
                IntPtr lpAddress,
                uint dwSize
            );

        /// <summary>
        /// Changes the protection on a region of committed pages in the 
        /// virtual address space of the calling process.
        /// </summary>
        /// <param name="lpAddress"><para>Pointer to the base address 
        /// of the region of pages whose access protection attributes 
        /// are to be changed.</para>
        /// <para>All pages in the specified region must be within 
        /// the same reserved region allocated when calling the 
        /// VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. 
        /// The pages cannot span adjacent reserved regions that were 
        /// allocated by separate calls to VirtualAlloc or 
        /// VirtualAllocEx using MEM_RESERVE.</para></param>
        /// <param name="dwSize">Size of the region whose access 
        /// protection attributes are to be changed, in bytes. The 
        /// region of affected pages includes all pages containing 
        /// one or more bytes in the range from the lpAddress parameter 
        /// to (lpAddress+dwSize). This means that a 2-byte range 
        /// straddling a page boundary causes the protection attributes 
        /// of both pages to be changed.</param>
        /// <param name="flNewProtect">Memory protection. This parameter 
        /// can be one of the memory protection options, along with 
        /// PAGE_GUARD or PAGE_NOCACHE, as needed.</param>
        /// <param name="lpflOldProtect">Pointer to a variable that 
        /// receives the previous access protection value of the first 
        /// page in the specified region of pages. If this parameter is 
        /// NULL or does not point to a valid variable, the function 
        /// fails.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualProtect
            (
                IntPtr lpAddress,
                uint dwSize,
                MemoryProtectionFlags flNewProtect,
                ref MemoryProtectionFlags lpflOldProtect
            );

        /// <summary>
        /// Changes the protection on a region of committed pages in 
        /// the virtual address space of a specified process.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose memory 
        /// protection is to be changed. The handle must have 
        /// PROCESS_VM_OPERATION access.</param>
        /// <param name="lpAddress"><para>Pointer to the base address 
        /// of the region of pages whose access protection attributes 
        /// are to be changed.</para>
        /// <para>All pages in the specified region must be within 
        /// the same reserved region allocated when calling the 
        /// VirtualAlloc or VirtualAllocEx function using MEM_RESERVE. 
        /// The pages cannot span adjacent reserved regions that were 
        /// allocated by separate calls to VirtualAlloc or 
        /// VirtualAllocEx using MEM_RESERVE.</para></param>
        /// <param name="dwSize">Size of the region whose access 
        /// protection attributes are changed, in bytes. The region 
        /// of affected pages includes all pages containing one or 
        /// more bytes in the range from the lpAddress parameter to 
        /// (lpAddress+dwSize). This means that a 2-byte range 
        /// straddling a page boundary causes the protection attributes 
        /// of both pages to be changed.</param>
        /// <param name="flNewProtect">Memory protection. This parameter 
        /// can be one of the memory protection options, along with 
        /// PAGE_GUARD or PAGE_NOCACHE, as needed.</param>
        /// <param name="lpflOldProtect">Pointer to a variable that 
        /// receives  the previous access protection of the first page 
        /// in the specified region of pages. If this parameter is NULL 
        /// or does not point to a valid variable, the function fails.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualProtectEx
            (
                IntPtr hProcess,
                IntPtr lpAddress,
                uint dwSize,
                MemoryProtectionFlags flNewProtect,
                ref MemoryProtectionFlags lpflOldProtect
            );

        /// <summary>
        /// Provides information about a range of pages in the virtual 
        /// address space of the calling process.
        /// </summary>
        /// <param name="lpAddress">Pointer to the base address of the 
        /// region of pages to be queried. This value is rounded down 
        /// to the next page boundary.</param>
        /// <param name="lpBuffer">Pointer to a MEMORY_BASIC_INFORMATION 
        /// structure in which information about the specified page range 
        /// is returned.</param>
        /// <param name="dwLength">Size of the buffer pointed to by the 
        /// lpBuffer parameter, in bytes.</param>
        /// <returns><para>The return value is the actual number of bytes 
        /// returned in the information buffer.</para>
        /// <para>Passing a kernel-mode pointer to this function can 
        /// result in no information being returned, due to security 
        /// issues. In this case, the return value is zero.</para>
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern uint VirtualQuery
            (
                IntPtr lpAddress,
                ref MEMORY_BASIC_INFORMATION lpBuffer,
                uint dwLength
            );

        /// <summary>
        /// Unlocks a specified range of pages in the virtual address 
        /// space of a process, enabling the system to swap the pages 
        /// out to the paging file if necessary.
        /// </summary>
        /// <param name="lpAddress">Pointer to the base address of the 
        /// region of pages to be unlocked.</param>
        /// <param name="dwSize">Size of the region being unlocked, 
        /// in bytes. The region of affected pages includes all pages 
        /// containing one or more bytes in the range from the 
        /// lpAddress parameter to (lpAddress+dwSize). This means that 
        /// a 2-byte range straddling a page boundary causes both pages 
        /// to be unlocked.</param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern bool VirtualUnlock
            (
                IntPtr lpAddress,
                uint dwSize
            );

        /// <summary>
        /// The WaitNamedPipe function waits until either a time-out 
        /// interval elapses or an instance of the specified named pipe 
        /// is available for connection (that is, the pipe's server 
        /// process has a pending ConnectNamedPipe operation on the pipe).
        /// </summary>
        /// <param name="lpNamedPipeName"><para>Pointer to a 
        /// null-terminated string that specifies the name of the named 
        /// pipe. The string must include the name of the computer on 
        /// which the server process is executing. A period may be used 
        /// for the servername if the pipe is local. The following pipe 
        /// name format is used:</para>
        /// <para>\\servername\pipe\pipename</para></param>
        /// <param name="nTimeOut">Number of milliseconds that the 
        /// function will wait for an instance of the named pipe to be 
        /// available.</param>
        /// <returns></returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool WaitNamedPipe
            (
                string lpNamedPipeName,
                PipeWaitFlags nTimeOut
            );

        /// <summary>
        /// <para>The WriteFile function writes data to a file and is 
        /// designed for both synchronous and asynchronous operation. 
        /// The function starts writing data to the file at the position 
        /// indicated by the file pointer. After the write operation has 
        /// been completed, the file pointer is adjusted by the number 
        /// of bytes actually written, except when the file is opened 
        /// with FILE_FLAG_OVERLAPPED. If the file handle was created 
        /// for overlapped input and output (I/O), the application must 
        /// adjust the position of the file pointer after the write 
        /// operation is finished.</para>
        /// <para>This function is designed for both synchronous and 
        /// asynchronous operation. The WriteFileEx function is designed 
        /// solely for asynchronous operation. It lets an application 
        /// perform other processing during a file write operation.
        /// </para></summary>
        /// <param name="hFile">Handle to the file.</param>
        /// <param name="lpBuffer">Pointer to the buffer containing the 
        /// data to be written to the file.</param>
        /// <param name="nNumberOfBytesToWrite">Number of bytes to be 
        /// written to the file.</param>
        /// <param name="lpNumberOfBytesWritten">Pointer to the variable 
        /// that receives the number of bytes written.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED structure.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool WriteFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                int nNumberOfBytesToWrite,
                out int lpNumberOfBytesWritten,
                IntPtr lpOverlapped
            );

        /// <summary>
        /// <para>The WriteFile function writes data to a file and is 
        /// designed for both synchronous and asynchronous operation. 
        /// The function starts writing data to the file at the position 
        /// indicated by the file pointer. After the write operation has 
        /// been completed, the file pointer is adjusted by the number 
        /// of bytes actually written, except when the file is opened 
        /// with FILE_FLAG_OVERLAPPED. If the file handle was created 
        /// for overlapped input and output (I/O), the application must 
        /// adjust the position of the file pointer after the write 
        /// operation is finished.</para>
        /// <para>This function is designed for both synchronous and 
        /// asynchronous operation. The WriteFileEx function is designed 
        /// solely for asynchronous operation. It lets an application 
        /// perform other processing during a file write operation.
        /// </para></summary>
        /// <param name="hFile">Handle to the file.</param>
        /// <param name="lpBuffer">Pointer to the buffer containing the 
        /// data to be written to the file.</param>
        /// <param name="nNumberOfBytesToWrite">Number of bytes to be 
        /// written to the file.</param>
        /// <param name="lpNumberOfBytesWritten">Pointer to the variable 
        /// that receives the number of bytes written.</param>
        /// <param name="lpOverlapped">Pointer to an OVERLAPPED structure.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        [CLSCompliant(false)]
        public static extern byte WriteFile
            (
                IntPtr hFile,
                byte[] lpBuffer,
                uint nNumberOfBytesToWrite,
                ref uint lpNumberOfBytesWritten,
                ref OVERLAPPED lpOverlapped
            );

        /// <summary>
        /// Replaces the keys and values for the specified section in an 
        /// initialization file.
        /// </summary>
        /// <param name="lpAppName">Pointer to a null-terminated string 
        /// specifying the name of the section in which data is written. 
        /// This section name is typically the name of the calling 
        /// application.</param>
        /// <param name="lpString">Pointer to a buffer containing the 
        /// new key names and associated values that are to be written 
        /// to the named section. This string is limited to 65,535 bytes.
        /// </param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// containing the name of the initialization file. If this 
        /// parameter does not contain a full path for the file, the 
        /// function searches the Windows directory for the file. If the 
        /// file does not exist and lpFileName does not contain a full 
        /// path, the function creates the file in the Windows directory. 
        /// The function does not create a file if lpFileName contains 
        /// the full path and file name of a file that does not exist.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool WritePrivateProfileSection
            (
                string lpAppName,
                string lpString,
                string lpFileName
            );

        /// <summary>
        /// Copies a string into the specified section of an 
        /// initialization file.
        /// </summary>
        /// <param name="lpAppName">Pointer to a null-terminated string 
        /// containing the name of the section to which the string will 
        /// be copied. If the section does not exist, it is created. 
        /// The name of the section is case-independent; the string can 
        /// be any combination of uppercase and lowercase letters.
        /// </param>
        /// <param name="lpKeyName">Pointer to the null-terminated string 
        /// containing the name of the key to be associated with a string. 
        /// If the key does not exist in the specified section, it is 
        /// created. If this parameter is NULL, the entire section, 
        /// including all entries within the section, is deleted.
        /// </param>
        /// <param name="lpString"><para>Pointer to a null-terminated 
        /// string to be written to the file. If this parameter is NULL, 
        /// the key pointed to by the lpKeyName parameter is deleted.
        /// </para>
        /// <para>Windows Me/98/95: The system does not support the use 
        /// of the TAB (\t) character as part of this parameter.
        /// </para></param>
        /// <param name="lpFileName">Pointer to a null-terminated string 
        /// that specifies the name of the initialization file.</param>
        /// <returns>If the function successfully copies the string to 
        /// the initialization file, the return value is nonzero.
        /// </returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool WritePrivateProfileString
            (
                string lpAppName,
                string lpKeyName,
                string lpString,
                string lpFileName
            );

        /// <summary>
        /// The WriteProcessMemory function writes data to an area of 
        /// memory in a specified process. The entire area to be written 
        /// to must be accessible, or the operation fails.
        /// </summary>
        /// <param name="hProcess">Handle to the process whose memory is 
        /// to be modified. The handle must have PROCESS_VM_WRITE and 
        /// PROCESS_VM_OPERATION access to the process.</param>
        /// <param name="lpBaseAddress">Pointer to the base address in 
        /// the specified process to which data will be written. Before 
        /// any data transfer occurs, the system verifies that all data 
        /// in the base address and memory of the specified size is 
        /// accessible for write access. If this is the case, the 
        /// function proceeds; otherwise, the function fails.
        /// </param>
        /// <param name="lpBuffer">Pointer to the buffer that contains 
        /// data to be written into the address space of the specified 
        /// process.</param>
        /// <param name="nSize">Number of bytes to be written to the 
        /// specified process.</param>
        /// <param name="lpNumberOfBytesWritten">Pointer to a variable 
        /// that receives the number of bytes transferred into the 
        /// specified process. This parameter is optional. If 
        /// lpNumberOfBytesWritten is NULL, the parameter is ignored.
        /// </param>
        /// <returns>If the function succeeds, the return value 
        /// is nonzero.</returns>
        [DllImport(DllName, SetLastError = true)]
        public static extern bool WriteProcessMemory
            (
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                byte[] lpBuffer,
                int nSize,
                ref int lpNumberOfBytesWritten
            );

        #endregion

        #region Public methods
        #endregion
    }
}
