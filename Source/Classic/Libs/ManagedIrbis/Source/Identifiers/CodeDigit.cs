// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CodeDigit.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public struct CodeDigit
    {
        #region Properties

        /// <summary>
        /// Digit.
        /// </summary>
        public char Digit;

        /// <summary>
        /// Value.
        /// </summary>
        public int Value;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeDigit
            (
                char digit,
                int value
            )
        {
            Digit = digit;
            Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find the digit.
        /// </summary>
        [CanBeNull]
        public static CodeDigit? FindDigit
            (
                char digit,
                [NotNull] CodeDigit[] allowedDigits
            )
        {
            foreach (CodeDigit current in allowedDigits)
            {
                if (current.Digit == digit)
                {
                    return current;
                }
            }

            return null;
        }

        /// <summary>
        /// Extract all the digits from the identifier.
        /// </summary>
        [NotNull]
        public static CodeDigit[] ExtractDigits
            (
                [NotNull] string identifier,
                [NotNull] CodeDigit[] allowedDigits
            )
        {
            Code.NotNullNorEmpty(identifier, "identifier");
            Code.NotNull(allowedDigits, "allowedDigits");

            List<CodeDigit> result = new List<CodeDigit>(identifier.Length);

            foreach (char c in identifier)
            {
                CodeDigit? found = FindDigit(c, allowedDigits);
                if (found != null)
                {
                    result.Add(found.Value);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return Digit.ToString();
        }

        #endregion
    }
}
