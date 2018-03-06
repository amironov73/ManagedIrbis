// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CopyFileFlags.cs -- flags for CopyFileEx function.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// Flags that specify how the file is to be copied by
	/// CopyFileEx function.
	/// </summary>
	[Flags]
	public enum CopyFileFlags
	{
		/// <summary>
		/// The copy operation fails immediately if the target file 
		/// already exists.
		/// </summary>
		COPY_FILE_FAIL_IF_EXISTS = 0x00000001,

		/// <summary>
		/// Progress of the copy is tracked in the target file in 
		/// case the copy fails. The failed copy can be restarted 
		/// at a later time by specifying the same values for 
		/// lpExistingFileName and lpNewFileName as those used in 
		/// the call that failed.
		/// </summary>
		COPY_FILE_RESTARTABLE = 0x00000002,

		/// <summary>
		/// ???
		/// </summary>
		COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,

		/// <summary>
		/// <para>An attempt to copy an encrypted file will succeed 
		/// even if the destination copy cannot be encrypted.</para>
		/// <para>Windows 2000/NT and Windows Me/98/95: This value 
		/// is not supported.</para>
		/// </summary>
		COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
	}
}