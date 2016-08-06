/* BitArrayUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace AM.Collections
{
    [PublicAPI]
    [MoonSharpUserData]
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
            Code.NotNull(left, "left");
            Code.NotNull(right, "right");

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
