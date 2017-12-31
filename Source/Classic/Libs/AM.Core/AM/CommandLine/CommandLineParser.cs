// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM.Logging;
using AM.Text;

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
                Log.Error
                    (
                        "CommandLineParser::_ParseSwitch: "
                        + "premature end of text"
                    );

                throw new ArgumentException();
            }

            CommandLineSwitch result = new CommandLineSwitch();

            text = text.Substring(1);
            if (text.ContainsCharacter(CommandLineSettings.ValueSeparator))
            {
                char[] separators
                    = { CommandLineSettings.ValueSeparator };

                string[] parts = StringUtility.SplitString(text, separators, 2);

                if (string.IsNullOrEmpty(parts[0])
                    || string.IsNullOrEmpty(parts[1]))
                {
                    Log.Error
                        (
                            "CommandLineParser::_ParseSwitch: "
                            + "empty switch value"
                        );

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
                    Log.Error
                        (
                            "CommandLineParser::Parse: "
                            + "empty argument"
                        );

                    throw new ArgumentException();
                }

                char firstChar = argument[0];
                if (firstChar == CommandLineSettings.ArgumentDelimiter)
                {
                    if (argument.Length == 1)
                    {
                        Log.Error
                            (
                                "CommandLineParser::Parse: "
                                + "premature end of argument"
                            );

                        throw new ArgumentException();
                    }
                    if (argument[argument.Length - 1] != '"')
                    {
                        Log.Error
                            (
                                "CommandLineParser::Parse: "
                                + "unclosed colon"
                            );

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

#if !WINMOBILE && !PocketPC

                        string fileName = trimmed.Substring(1);
                        ParsedCommandLine inner = ParseFile(fileName);
                        result.Merge(inner);

#endif
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

#if !WINMOBILE && !PocketPC

                    string fileName = argument.Substring(1);
                    ParsedCommandLine inner = ParseFile(fileName);
                    result.Merge(inner);

#endif
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

#if !WINMOBILE && !PocketPC

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
                    EncodingUtility.DefaultEncoding
                );
        }

#endif

        #endregion
    }
}
