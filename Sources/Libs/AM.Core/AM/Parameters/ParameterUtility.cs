/* ParameterUtility.cs -- useful routines for parameter parsing/encoding
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.Parameters
{
    /// <summary>
    /// Useful routines for parameter parsing/encoding.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ParameterUtility
    {
        #region Properties

        /// <summary>
        /// Escape character.
        /// </summary>
        public static char EscapeCharacter = '\\';

        /// <summary>
        /// Name separator.
        /// </summary>
        public static char NameSeparator = '=';

        /// <summary>
        /// Value separator.
        /// </summary>
        public static char ValueSeparator = ';';

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Encode parameters to sting representation.
        /// </summary>
        [NotNull]
        public static string Encode
            (
                [NotNull] Parameter[] parameters
            )
        {
            Code.NotNull(parameters, "parameters");

            StringBuilder result = new StringBuilder();

            char[] badNameCharacters = {NameSeparator};
            char[] badValueCharacters = {ValueSeparator};

            foreach (Parameter parameter in parameters)
            {
                result.Append
                    (
                        StringUtility.Mangle
                            (
                                parameter.Name,
                                EscapeCharacter,
                                badNameCharacters
                            )
                    );
                result.Append(NameSeparator);
                result.Append
                    (
                        StringUtility.Mangle
                        (
                            parameter.Value,
                            EscapeCharacter,
                            badValueCharacters
                        )
                    );
                result.Append(ValueSeparator);
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse specified string.
        /// </summary>
        [NotNull]
        public static Parameter[] ParseString
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            List<Parameter> result = new List<Parameter>();
            TextNavigator navigator = new TextNavigator(text);
            navigator.SkipWhitespace();

            while (!navigator.IsEOF)
            {
                while (true)
                {
                    bool flag = false;
                    if (navigator.IsWhiteSpace())
                    {
                        flag = true;
                        navigator.SkipWhitespace();
                    }
                    if (navigator.PeekChar() == ValueSeparator)
                    {
                        flag = true;
                        navigator.SkipChar(ValueSeparator);
                    }
                    if (!flag)
                    {
                        break;
                    }
                }

                string name = navigator.ReadEscapedUntil
                    (
                        EscapeCharacter,
                        NameSeparator
                    );
                if (ReferenceEquals(name, null))
                {
                    break;
                }
                name = name.Trim();

                navigator.SkipWhitespace();

                string value = navigator.ReadEscapedUntil
                    (
                        EscapeCharacter,
                        ValueSeparator
                    );
                Parameter parameter = new Parameter(name, value);
                result.Add(parameter);
            }

            return result.ToArray();
        }

        #endregion
    }
}
