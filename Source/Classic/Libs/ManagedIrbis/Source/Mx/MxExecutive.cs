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
using AM.ConsoleIO;
using AM.IO;
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

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
        public IrbisProvider Client { get; internal set; }

        /// <summary>
        /// Commands.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxCommand> Commands { get; private set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string DescriptionFormat { get; set; }

        /// <summary>
        /// Order expression.
        /// </summary>
        [CanBeNull]
        public string OrderFormat { get; set; }

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
        /// Search history.
        /// </summary>
        [NotNull]
        public Stack<string> History { get; private set; }

        /// <summary>
        /// Stack of databases.
        /// </summary>
        [NotNull]
        public Stack<string> Databases { get; private set; }

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
            DescriptionFormat = "@brief";

            Client = new NullProvider();
            Commands = new NonNullCollection<MxCommand>();
            Records = new NonNullCollection<MxRecord>();
            History = new Stack<string>();
            Databases = new Stack<string>();

            _CreateStandardCommands();
            _InitializeCommands();
        }

        #endregion

        #region Private members

#if !WINMOBILE && !PocketPC

        private void _CancelKeyPress
            (
                object sender,
                ConsoleCancelEventArgs e
            )
        {
            StopFlag = true;
        }

#endif

        private void _CreateStandardCommands()
        {
            Commands.AddRange
                (
                    new MxCommand[]
                    {
                        new AliasCommand(),
                        new ClsCommand(),
                        new ConnectCommand(),
                        new CsCommand(),
                        new CsFileCommand(),
                        new DbCommand(),
                        new DirCommand(),
                        new DisconnectCommand(),
                        new ExitCommand(),
                        new ExportCommand(),
                        new FormatCommand(),
                        new HistoryCommand(),
                        new InfoCommand(),
                        new LimitCommand(),
                        new ListCommand(),
                        new ListDbCommand(),
                        new ListUsersCommand(),
                        new NopCommand(),
                        new PingCommand(),
                        new PrintCommand(),
                        new RefineCommand(),
                        new SearchCommand(),
                        new SortCommand(),
                        new StoreCommand(),
                        new TypeCommand(),
                        new VerCommand()
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

            string[] parts = StringUtility.SplitString
                (
                    line,
                    CommonSeparators.SpaceOrTab,
                    2
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
                Log.TraceException
                    (
                        "MxExecutive::_ExecuteLine",
                        exception
                    );

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

            string text = FileUtility.ReadAllText(fileName, IrbisEncoding.Utf8);
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

        /// <summary>
        /// Read one line.
        /// </summary>
        [NotNull]
        public string ReadLine()
        {
            ConsoleColor saveColor = ConsoleInput.ForegroundColor;
            try
            {
                ConsoleInput.ForegroundColor = ConsoleColor.White;
                return ConsoleInput.ReadLine();
            }
            finally
            {
                ConsoleInput.ForegroundColor = saveColor;
            }
        }

        /// <summary>
        /// REPL
        /// </summary>
        public void Repl()
        {
#if !WINMOBILE && !PocketPC && !UAP

            Console.CancelKeyPress += _CancelKeyPress;
            Console.Title = string.Format
                (
                    "mx64 v{0}", Version
                );

#endif

            while (!StopFlag)
            {
                string line = ConsoleInput.ReadLine();
                ExecuteLine(line);
            }
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void Write
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            ConsoleInput.Write(string.Format(format, arguments));
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
            ConsoleInput.WriteLine(string.Format(format, arguments));
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                ConsoleColor color,
                [NotNull] string text
            )
        {
            ConsoleColor saveColor = ConsoleInput.ForegroundColor;
            try
            {
                ConsoleInput.ForegroundColor = color;
                ConsoleInput.WriteLine(text);
            }
            finally
            {
                ConsoleInput.ForegroundColor = saveColor;
            }
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                int verbosityLevel,
                [NotNull] string text
            )
        {
            if (verbosityLevel <= VerbosityLevel)
            {
                WriteLine(text);
            }
        }

        /// <summary>
        /// Write message
        /// </summary>
        public void WriteMessage
            (
                [NotNull] string text
            )
        {
            ConsoleColor saveColor = ConsoleInput.ForegroundColor;
            try
            {
                ConsoleInput.ForegroundColor = ConsoleColor.Blue;
                WriteLine(3, text);
            }
            finally
            {
                ConsoleInput.ForegroundColor = saveColor;
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
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
