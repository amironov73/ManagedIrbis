/* Require60x.cs -- предметные рубрики
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Предметные рубрики
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Require60x
        : QualityRule
    {
        #region Private members

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
