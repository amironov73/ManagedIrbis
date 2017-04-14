// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SpaceLayout.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisInterop
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class SpaceLayout
    {
        #region Properties

        /// <summary>
        /// Offset of native record in the space.
        /// </summary>
        [JsonProperty("record")]
        public int RecordOffset { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Search for the record in the space.
        /// </summary>
        public void SearchForRecord
            (
                IntPtr space,
                int mfn,
                int blockSize
            )
        {
            if (RecordOffset != 0)
            {
                return;
            }

            int minAddress = space.ToInt32() - 0x100000;
            int maxAddress = space.ToInt32() + 0x100000;

            for (int offset = 4; offset < blockSize; offset++)
            {
                IntPtr ptr = space.GetPointer32(offset);
                int address = ptr.ToInt32();
                if (address < minAddress || address > maxAddress)
                {
                    continue;
                }

                int canBeMfn = Marshal.ReadInt32(ptr);
                if (canBeMfn != mfn)
                {
                    continue;
                }
                int mustBeZero = Marshal.ReadInt32(ptr, 8);
                if (mustBeZero != 0)
                {
                    continue;
                }
                mustBeZero = Marshal.ReadInt32(ptr, 12);
                if (mustBeZero != 0)
                {
                    continue;
                }

                RecordOffset = offset;
                return;
            } 

            throw new IrbisException("record not found");
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2012()
        {
            SpaceLayout result = new SpaceLayout
            {
                RecordOffset = 577
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2014()
        {
            SpaceLayout result = new SpaceLayout
            {
                RecordOffset = 626
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static SpaceLayout Version2016()
        {
            SpaceLayout result = new SpaceLayout
            {
                RecordOffset = 2034
            };

            return result;
        }

        #endregion
    }
}
