// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BitArrayUtility.cs -- helper methods for BitArray class
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
*/

#region Using directives

using System.Collections;

using UnsafeCode;

using JetBrains.Annotations;

#endregion

namespace UnsafeAM.Collections
{
    /// <summary>
    /// Helper methods for <see cref="BitArray"/> class.
    /// </summary>
    [PublicAPI]
    public static class BitArrayUtility
    {
        #region Public methods

        /// <summary>
        /// Compares two <see cref="BitArray"/>s.
        /// </summary>
        public static bool AreEqual
            (
                [NotNull] BitArray left,
                [NotNull] BitArray right
            )
        {
            Code.NotNull(left, nameof(left));
            Code.NotNull(right, nameof(right));

            if (left.Length != right.Length)
            {
                return false;
            }

            int length = left.Length;
            bool[] leftA = new bool[length];
            ICollection leftCollection = left;
            leftCollection.CopyTo(leftA, 0);
            bool[] rightA = new bool[length];
            ICollection rightCollection = right;
            rightCollection.CopyTo(rightA,0);

            for (int i = 0; i < length; i++)
            {
                if (leftA[i] != rightA[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
