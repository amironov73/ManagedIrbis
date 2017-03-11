// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MxExecutive.cs -- 
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;
using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Mx.Commands;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MxExecutive
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public ConnectedClient Client { get; private set; }

        /// <summary>
        /// Commands.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxCommand> Commands { get; private set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        /// <summary>
        /// Search limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxRecord> Records { get; private set; }

        /// <summary>
        /// Stop repl.
        /// </summary>
        public bool StopFlag { get; set; }

        /// <summary>
        /// Verbosity level.
        /// </summary>
        public int VerbosityLevel { get; set; }

        /// <summary>
        /// Get version of the executive.
        /// </summary>
        public static Version Version
        {
            get
            {
#if CLASSIC

                Assembly assembly = typeof (MxExecutive).Assembly;
                Version result = assembly.GetName().Version;

                return result;

#else

                return new Version (1, 0);

#endif
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxExecutive()
        {
            VerbosityLevel = 3;
            Format = "@brief";

            Client = new ConnectedClient();
            Commands = new NonNullCollection<MxCommand>();
            Records = new NonNullCollection<MxRecord>();

            _CreateStandardCommands();
            _InitializeCommands();
        }

        #endregion

        #region Private members

        private void _CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            StopFlag = true;
        }

        private void _CreateStandardCommands()
        {
            Commands.AddRange
                (
                    new MxCommand[]
                    {
                        new AliasCommand(),
                        new ConnectCommand(),
                        new CsCommand(),
                        new CsFileCommand(),
                        new DisconnectCommand(),
                        new ExitCommand(),
                        new FormatCommand(),
                        new LimitCommand(),
                        new NopCommand(),
                        new PrintCommand(),
                        new SearchCommand(),
                        new SortCommand(),
                        new StoreCommand(),
                    }
                );
        }

        private void _DisposeCommands()
        {
            foreach (MxCommand command in Commands)
            {
                command.Dispose();
            }
        }

        private bool _ExecuteLine
            (
                [NotNull] TextNavigator navigator
            )
        {
            navigator.SkipWhitespace();

            string line = navigator.ReadLine();
            if (string.IsNullOrEmpty(line))
            {
                return true;
            }

            if (line.StartsWith("#"))
            {
                // Comment, ignore it
                return true;
            }

            string[] parts = line.Split
                (
                    new[] {' ', '\t'},
                    2,
                    StringSplitOptions.RemoveEmptyEntries
                );
            string commandName = parts[0];
            string commandArgument = null;
            if (parts.Length != 1)
            {
                commandArgument = parts[1];
            }

            MxCommand command = _FindCommand(commandName);
            if (ReferenceEquals(command, null))
            {
                WriteLine("Unknown command: '{0}'", commandName);
                return false;
            }

            MxArgument[] arguments =
            {
                new MxArgument
                {
                    Text = commandArgument
                }
            };

            bool result = false;

            try
            {
                result = command.Execute
                    (
                        this,
                        arguments
                    );
            }
            catch (Exception exception)
            {
                WriteLine("Exception: {0}", exception);

                return result;
            }

            return result;
        }

        [CanBeNull]
        private MxCommand _FindCommand
            (
                string name
            )
        {
            return Commands.FirstOrDefault
                (
                    cmd => cmd.Name.SameString(name)
                );
        }

        private void _InitializeCommands()
        {
            foreach (MxCommand command in Commands)
            {
                command.Initialize(this);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Print banner.
        /// </summary>
        public void Banner()
        {
            WriteLine("mx64 version {0}", Version);
            WriteLine(string.Empty);
        }

        /// <summary>
        /// Execute initialization script.
        /// </summary>
        public bool ExecuteInitScript()
        {
            return true;
        }

        /// <summary>
        /// Execute script file.
        /// </summary>
        public bool ExecuteFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName, IrbisEncoding.Utf8);
            bool result = ExecuteLine(text);

            return result;
        }

        /// <summary>
        /// Execute line of the script.
        /// </summary>
        public bool ExecuteLine
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            TextNavigator navigator = new TextNavigator(text);

            while (!navigator.IsEOF)
            {
                if (!_ExecuteLine(navigator))
                {
                    return false;
                }
            }

            return true;
        }

#if !ANDROID

        /// <summary>
        /// REPL
        /// </summary>
        public void Repl()
        {
            Console.CancelKeyPress += _CancelKeyPress;
            Console.Title = string.Format
                (
                    "mx64 v{0}", Version
                );

            while (!StopFlag)
            {
                string line = Console.ReadLine();
                ExecuteLine(line);
            }
        }

#endif

        /// <summary>
        /// Write to console.
        /// </summary>
        public void Write
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Console.Write(format, arguments);
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Console.WriteLine(format, arguments);
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                int verbosityLevel,
                [NotNull] string format,
                params object[] arguments
            )
        {
            if (verbosityLevel <= VerbosityLevel)
            {
                WriteLine(format, arguments);
            }
        }

#endregion

#region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            _DisposeCommands();

            Client.Dispose();
        }

#endregion

#region Object members

#endregion
    }
}
