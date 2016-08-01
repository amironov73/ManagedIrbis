/* SubFieldValue.cs --
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
using AM;
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
    public static class SubFieldValue
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
            [CanBeNull] string value
            )
        {
            return Verify(value, ThrowOnVerify);
        }

        public static bool Verify
            (
                [CanBeNull] string value,
                bool throwOnError
            )
        {
            bool result = true;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.IndexOf(SubField.Delimiter) >= 0)
                {
                    result = false;
                }
            }

            if (!result && throwOnError)
            {
                throw new VerificationException("SubField.Value");
            }

            return result;
        }

        #endregion
    }
}
