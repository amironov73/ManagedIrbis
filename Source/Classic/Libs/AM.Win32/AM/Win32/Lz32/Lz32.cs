// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Lz32.cs -- 
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace AM.Win32
{
	/// <summary>
	/// </summary>
	public static class Lz32
	{
		#region Constants

		/// <summary>
		/// DLL name.
		/// </summary>
		public const string DllName = "LZ32.dll";

		/// <summary>
		/// Invalid input handle.
		/// </summary>
		public const int LZERROR_BADINHANDLE = -1;

		/// <summary>
		/// Invalid output handle.
		/// </summary>
		public const int LZERROR_BADOUTHANDLE = -2;

		/// <summary>
		/// Corrupt compressed file format.
		/// </summary>
		public const int LZERROR_READ = -3;

		/// <summary>
		/// Out of space for output file.
		/// </summary>
		public const int LZERROR_WRITE = -4;

		/// <summary>
		/// Insufficient memory for LZFile struct.
		/// </summary>
		public const int LZERROR_GLOBALLOC = -5;

		/// <summary>
		/// Bad global handle.
		/// </summary>
		public const int LZERROR_GLOBLOCK = -6;

		/// <summary>
		/// Input parameter out of acceptable range.
		/// </summary>
		public const int LZERROR_BADVALUE = -7;

		/// <summary>
		/// Compression algorithm not recognized.
		/// </summary>
		public const int LZERROR_UNKNOWNALG = -8;

		#endregion

		#region Interop

		/// <summary>
		/// The GetExpandedName function retrieves the original 
		/// name of a compressed file, if the file was compressed 
		/// by the Lempel-Ziv algorithm.
		/// </summary>
		/// <param name="lpszSource">Pointer to a string that specifies 
		/// the name of a compressed file.</param>
		/// <param name="lpszBuffer">Pointer to a buffer that receives 
		/// the name of the compressed file.</param>
		/// <returns>If the function succeeds, the return value is 1.
		/// If the function fails, the return value is LZERROR_BADVALUE.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int GetExpandedName
			(
			string lpszSource,
			StringBuilder lpszBuffer
			);

		/// <summary>
		/// Closes a file that was opened by using the LZOpenFile function.
		/// </summary>
		/// <param name="hFile"></param>
		[DllImport ( DllName, SetLastError = false )]
		public static extern void LZClose
			(
			int hFile
			);

		/// <summary>
		/// The LZCopy function copies a source file to a destination file. 
		/// If the source file has been compressed by the Lempel-Ziv algorithm, 
		/// this function creates a decompressed destination file. If the 
		/// source file is not compressed, this function duplicates the 
		/// original file.
		/// </summary>
		/// <param name="hfSource">Handle to the source file.</param>
		/// <param name="hfDest">Handle to the destination file.</param>
		/// <returns>If the function succeeds, the return value specifies 
		/// the size, in bytes, of the destination file.</returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int LZCopy
			(
			int hfSource,
			int hfDest
			);

		/// <summary>
		/// Allocates memory for the internal data structures required 
		/// to decompress files, and then creates and initializes them.
		/// </summary>
		/// <param name="hfSource">Handle to the source file.</param>
		/// <returns>If the function succeeds, the return value 
		/// is a new LZ file handle.
		/// If the function fails, the return value is an LZERROR_* code.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int LZInit
			(
			int hfSource
			);

		/// <summary>
		/// Creates, opens, reopens, or deletes the specified file.
		/// </summary>
		/// <param name="lpFileName">Pointer to a string that 
		/// specifies the name of a file.</param>
		/// <param name="lpReOpenBuf"><para>Pointer to the OFSTRUCT 
		/// structure that is to receive information about the 
		/// file when the file is first opened. The structure can 
		/// be used in subsequent calls to the LZOpenFile function 
		/// to see the open file.</para>
		/// <para>The szPathName member of this structure contains 
		/// characters from the original equipment manufacturer 
		/// (OEM) character set.</para></param>
		/// <param name="wStyle">Action to take.</param>
		/// <returns>If the function succeeds and the value specified 
		/// by the wStyle parameter is not OF_READ, the return value 
		/// is a handle identifying the file. If the file is compressed 
		/// and opened with wStyle set to OF_READ, the return value is 
		/// a special file handle.
		/// If the function fails, the return value is an LZERROR_* code. 
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		[CLSCompliant ( false )]
		public static extern int LZOpenFile
			(
			string lpFileName,
			ref OFSTRUCT lpReOpenBuf,
			OpenFileFlags wStyle
			);

		/// <summary>
		/// Reads (at most) the specified number of bytes from 
		/// a file and copies them into a buffer.
		/// </summary>
		/// <param name="hFile">Handle to the source file.</param>
		/// <param name="lpBuffer">Pointer to a buffer that receives 
		/// the bytes read from the file. Ensure that this buffer is 
		/// larger than cbRead.</param>
		/// <param name="cbRead">Count of bytes to be read.</param>
		/// <returns>If the function succeeds, the return value 
		/// specifies the number of bytes read.
		/// If the function fails, the return value is an LZERROR_* code.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int LZRead
			(
			int hFile,
			byte[] lpBuffer,
			int cbRead
			);

		/// <summary>
		/// Moves a file pointer a number of bytes from a starting position.
		/// </summary>
		/// <param name="hFile">Handle to the source file.</param>
		/// <param name="lOffset">Number of bytes by which to move the 
		/// file pointer.</param>
		/// <param name="iOrigin">Starting position of the pointer.</param>
		/// <returns>If the function succeeds, the return value specifies 
		/// the offset from the beginning of the file to the new pointer 
		/// position.
		/// If the function fails, the return value is an LZERROR_* code.
		/// </returns>
		[DllImport ( DllName, SetLastError = true )]
		public static extern int LZSeek
			(
			int hFile,
			int lOffset,
			SeekOrigin iOrigin
			);

		#endregion
	}
}