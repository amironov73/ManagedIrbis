// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisBuffer.cs -- helper class for interop with Irbis DLL
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace OfficialWrapper
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [StructLayout(LayoutKind.Explicit)]
    public sealed class IrbisBuffer
    {
        #region Properties

        /// <summary>
        /// Size.
        /// </summary>
        [FieldOffset(0)]
        public int Size;

        /// <summary>
        /// Capacity.
        /// </summary>
        [FieldOffset(4)]
        public int Capacity;

        /// <summary>
        /// Pointer.
        /// </summary>
        [FieldOffset(8)]
        public IntPtr Pointer;

        /// <summary>
        /// Data.
        /// </summary>
        public byte[] Data
        {
            get
            {
                byte[] result = new byte[Size];
                Marshal.Copy(Pointer, result, 0, Size);
                return result;
                
            }
        }

        #endregion
    }
}
