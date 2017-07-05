// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CountingMemoryStream.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.IO
{
    /// <summary>
    /// <see cref="MemoryStream"/> that counts its reached size.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CountingMemoryStream
        : MemoryStream
    {
        #region Properties

        /// <summary>
        /// Reached size, bytes.
        /// </summary>
        public int ReachedSize { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CountingMemoryStream()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CountingMemoryStream
            (
                int capacity
            ) 
            : base(capacity)
        {
        }

        #endregion

        #region MemoryStream members

        /// <inheritdoc cref="MemoryStream.ToArray" />
        public override byte[] ToArray()
        {
            byte[] result = base.ToArray();
            ReachedSize = Math.Max(result.Length, ReachedSize);

            return result;
        }

        #endregion
    }
}
