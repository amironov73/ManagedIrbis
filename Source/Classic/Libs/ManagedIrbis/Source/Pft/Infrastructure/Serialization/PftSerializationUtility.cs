// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftSerializationUtility.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class PftSerializationUtility
    {
        #region Public methods

        /// <summary>
        /// Verify deserialized <see cref="PftProgram"/>.
        /// </summary>
        public static void VerifyDeserializedProgram
            (
                [NotNull] PftProgram ethalon,
                [NotNull] PftProgram deserialized
            )
        {
            Code.NotNull(ethalon, "ethalon");
            Code.NotNull(deserialized, "deserialized");

            ethalon.CompareNode(deserialized);
        }

        #endregion
    }
}
