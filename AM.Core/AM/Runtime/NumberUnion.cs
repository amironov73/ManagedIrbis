/* NumberUnion.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace AM.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Explicit)]
    public struct NumberUnion
    {
        #region Bytes

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public byte Byte0;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(1)]
        public byte Byte1;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(2)]
        public byte Byte2;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(3)]
        public byte Byte3;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(4)]
        public byte Byte4;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(5)]
        public byte Byte5;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(6)]
        public byte Byte6;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(7)]
        public byte Byte7;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(8)]
        public byte Byte8;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(9)]
        public byte Byte9;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(10)]
        public byte Byte10;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(11)]
        public byte Byte11;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(12)]
        public byte Byte12;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(13)]
        public byte Byte13;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(14)]
        public byte Byte14;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(15)]
        public byte Byte15;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public sbyte SignedByte;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public byte UnsignedByte;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public short SignedInt16;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public ushort UnsignedInt16;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public int SignedInt32;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public uint UnsignedInt32;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public long SignedInt64;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public ulong UnsignedInt64;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public float Single;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public double Double;

        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        public decimal Decimal;
    }
}
