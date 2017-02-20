// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Bits.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Linq;

#endregion

namespace Proxy2017
{
    static class Bits
    {
        public static byte[] Not(byte[] bytes)
        {
            return bytes.Select(b => (byte)~b).ToArray();
        }

        public static byte[] And(byte[] A, byte[] B)
        {
            int length = A.Length;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = (byte)(A[i] & B[i]);
            }
            return result;
        }

        public static byte[] Or(byte[] A, byte[] B)
        {
            int length = A.Length;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = (byte)(A[i] | B[i]);
            }
            return result;
        }

        public static bool GE(byte[] A, byte[] B)
        {
            int length = A.Length;
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                int a = A[i], b = B[i];
                result[i] = a == b ? 0 : a < b ? 1 : -1;
            }

            return result
                .SkipWhile(c => c == 0)
                .FirstOrDefault() >= 0;
        }

        public static bool LE(byte[] A, byte[] B)
        {
            int length = A.Length;
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                int a = A[i], b = B[i];
                result[i] = a == b ? 0 : a < b ? 1 : -1;
            }

            return result
                .SkipWhile(c => c == 0)
                .FirstOrDefault() <= 0;
        }

        public static byte[] GetBitMask(int sizeOfBuff, int bitLen)
        {
            var maskBytes = new byte[sizeOfBuff];
            var bytesLen = bitLen / 8;
            var bitsLen = bitLen % 8;
            for (int i = 0; i < bytesLen; i++)
            {
                maskBytes[i] = 0xff;
            }
            if (bitsLen > 0) maskBytes[bytesLen] = (byte)~Enumerable
                .Range(1, 8 - bitsLen).Select(n => 1 << n - 1)
                .Aggregate((a, b) => a | b);
            return maskBytes;
        }
    }
}
