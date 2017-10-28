// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TestingPlatformAbstraction.cs -- 
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

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.PlatformAbstraction
{
    /// <summary>
    /// Testing replacement for <see cref="PlatformAbstractionLayer"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TestingPlatformAbstraction
        : PlatformAbstractionLayer
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public DateTime NowValue { get; set; }

        #endregion

        #region PlatformAbstractionLevel members

        /// <inheritdoc cref="PlatformAbstractionLayer.Now" />
        public override DateTime Now()
        {
            return NowValue;
        }

        /// <inheritdoc cref="PlatformAbstractionLayer.Today" />
        public override DateTime Today()
        {
            return NowValue.Date;
        }

        #endregion
    }
}
