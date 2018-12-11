// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MiddleStruct.cs --
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
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 4)]
    public unsafe struct MiddleStruct
    {
        #region Fields

        /// <summary>
        /// Signed value.
        /// </summary>
        [FieldOffset(0)]
        public int SignedValue;

        /// <summary>
        /// Unsigned value.
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public uint UnsignedValue;

        /// <summary>
        /// Float value.
        /// </summary>
        [FieldOffset(0)]
        public float FloatValue;

        /// <summary>
        /// Array.
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public fixed byte Array[4];

        #endregion
    }
}
