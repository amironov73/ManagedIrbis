/* CommandLineParser.cs -- simple parsing of the command line
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.CommandLine
{
    /// <summary>
    /// Simple parsing of command line switches
    /// and arguments.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CommandLineParser
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private CommandLineSwitch _ParseSwitch
            (
                string text
            )
        {
            if (text.Length < 2)
            {
                throw new ArgumentException();
            }

            CommandLineSwitch result = new CommandLineSwitch();

            text = text.Substring(1);
            if (text.Contains(CommandLineSettings.ValueSeparator))
            {
                char[] separators
                    = { CommandLineSettings.ValueSeparator };
                string[] parts = text.Split(separators, 2);
                if (string.IsNullOrEmpty(parts[0])
                    || string.IsNullOrEmpty(parts[1]))
                {
                    throw new ArgumentException();
                }
                result.Name = parts[0];
                result.Value = parts[1];
            }
            else
            {
                result.Name = text;
            }


            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse given arguments.
        /// </summary>
        [NotNull]
        public ParsedCommandLine Parse
            (
                [NotNull][ItemNotNull] string[] arguments
            )
        {
            Code.NotNull(arguments, "arguments");

            ParsedCommandLine result = new ParsedCommandLine();

            foreach (string argument in arguments)
            {
                if (string.IsNullOrEmpty(argument))
                {
                    throw new ArgumentException();
                }

                char firstChar = argument[0];
                if (firstChar == CommandLineSettings.ArgumentDelimiter)
                {
                    if (argument.Length == 1)
                    {
                        throw new ArgumentException();
                    }
                    if (argument[argument.Length - 1] != '"')
                    {
                        throw new ArgumentException();
                    }

                    string trimmed = argument.Substring(1, argument.Length - 2);
                    if (trimmed.Length == 0)
                    {
                        result.PositionalArguments.Add(string.Empty);
                        continue;
                    }

                    char secondChar = trimmed[0];
                    if (secondChar == CommandLineSettings.ResponsePrefix)
                    {
                        string fileName = trimmed.Substring(1);
                        ParsedCommandLine inner = ParseFile(fileName);
                        result.Merge(inner);
                    }
                    else if (secondChar == CommandLineSettings.SwitchPrefix)
                    {
                        CommandLineSwitch item = _ParseSwitch(trimmed);
                        result.AddSwitch(item);
                    }
                    else
                    {
                        result.PositionalArguments.Add(argument);
                    }
                }
                else if (firstChar == CommandLineSettings.ResponsePrefix)
                {
                    string fileName = argument.Substring(1);
                    ParsedCommandLine inner = ParseFile(fileName);
                    result.Merge(inner);
                }
                else if (firstChar == CommandLineSettings.SwitchPrefix)
                {
                    CommandLineSwitch item = _ParseSwitch(argument);
                    result.AddSwitch(item);
                }
                else
                {
                    result.PositionalArguments.Add(argument);
                }
            }

            return result;
        }

        /// <summary>
        /// Parse arguments from response file.
        /// </summary>
        [NotNull]
        public ParsedCommandLine ParseFile
            (
                [NotNull] string fileName,
                [NotNull] Encoding encoding
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");
            Code.NotNull(encoding, "encoding");

            string text = File.ReadAllText(fileName, encoding);
            string[] arguments = CommandLineUtility.SplitText(text);
            ParsedCommandLine result = Parse(arguments);

            return result;
        }

        /// <summary>
        /// Parse arguments from response file.
        /// </summary>
        [NotNull]
        public ParsedCommandLine ParseFile
            (
                [NotNull] string fileName
            )
        {
            return ParseFile
                (
                    fileName,
                    Encoding.GetEncoding(0)
                );
        }

        #endregion
    }
}
