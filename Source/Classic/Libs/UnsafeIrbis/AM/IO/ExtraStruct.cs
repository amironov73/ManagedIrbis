// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExtraStruct.cs --
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
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 16)]
    public unsafe struct ExtraStruct
    {
        #region Fields

        /// <summary>
        /// Decimal value.
        /// </summary>
        [FieldOffset(0)]
        public decimal DecimalValue;

        /// <summary>
        /// Array.
        /// </summary>
        [FieldOffset(0)]
        [CLSCompliant(false)]
        public fixed byte Array[16];

        #endregion
    }
}
