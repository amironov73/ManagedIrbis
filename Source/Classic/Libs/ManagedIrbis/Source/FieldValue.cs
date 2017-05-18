// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldValue.cs -- field value related routines
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: moderate
 * TODO trim value?
 */

#region Using directives

#if FW4
using System.Diagnostics.CodeAnalysis;
#endif

using AM;
using AM.Logging;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Field value related routines.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class FieldValue
    {
        #region Constants

        #endregion

        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        #if FW4
        [ExcludeFromCodeCoverage]
        #endif
        public static bool ThrowOnVerify { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the value valid.
        /// </summary>
        public static bool IsValidValue
            (
                [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                {
                    if (c == SubField.Delimiter
                        || c < ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Field value normalization.
        /// </summary>
        [CanBeNull]
        public static string Normalize
            (
                [CanBeNull] string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            string result = value.Trim();
            return result;
        }

        /// <summary>
        /// Verify subfield value.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string value
            )
        {
            return Verify(value, ThrowOnVerify);
        }

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                [CanBeNull] string value,
                bool throwOnError
            )
        {
            bool result = IsValidValue(value);

            if (!result)
            {
                Log.Trace
                    (
                        "FieldValue::Verify: "
                        + "bad value: "
                        + value.NullableToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                        (
                            "Bad Field.Value: " 
                            + value.NullableToVisibleString()
                        );
                }
            }

            return result;
        }

        #endregion
    }
}
