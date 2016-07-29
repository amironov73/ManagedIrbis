/* FieldText.cs --
 * Ars Magna project, http://arsmagna.ru
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

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldValue
    {
        #region Constants

        #endregion

        #region Properties

        public static bool ThrowOnVerify { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static bool Verify
            (
                [CanBeNull] string text,
                bool throwOnError
            )
        {
            return true;
        }

        public static bool Verify
            (
                [CanBeNull] string text
            )
        {
            return Verify(text, ThrowOnVerify);
        }

        #endregion
    }
}
