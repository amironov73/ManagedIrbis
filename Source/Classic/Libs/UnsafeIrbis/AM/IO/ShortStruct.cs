// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ShortStruct.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Unsing directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.IO
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 2)]
    public unsafe struct ShortStruct
    {
        #region Fields

        /// <summary>
        /// Signed value.
        /// </summary>
        [FieldOffset(0)]
        public short SignedValue;

        /// <summary>
        /// Unsigned value.
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public ushort UnsignedValue;


        /// <summary>
        /// Array.
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public fixed byte Array[2];

        #endregion
    }
}
