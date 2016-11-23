/* PftFormatter.cs -- local PFT script interpreter
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Local PFT script interpreter.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class PftFormatter
        : IDisposable
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Контекст форматирования. Во время парсинга не нужен.
        /// </summary>
        public PftContext Context { get; set; }

        /// <summary>
        /// Корневой элемент синтаксического дерева - собственно программа.
        /// </summary>
        public PftProgram Program { get; set; }

        /// <summary>
        /// Нормальный результат расформатирования.
        /// </summary>
        public string Output { get { return Context.Text; } }

        /// <summary>
        /// Поток ошибок.
        /// </summary>
        public string Error { get { return Context.Output.ErrorText; } }

        /// <summary>
        /// Поток предупреждений.
        /// </summary>
        public string Warning { get { return Context.Output.WarningText; } }

        /// <summary>
        /// Have error?
        /// </summary>
        public bool HaveError { get { return Context.Output.HaveError; } }

        /// <summary>
        /// Have warning.
        /// </summary>
        public bool HaveWarning { get { return Context.Output.HaveWarning; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFormatter()
            : this
            (
                new PftContext(null)
            )
        {
            _ownContext = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFormatter
            (
                [NotNull] PftContext context
            )
        {
            Code.NotNull(context, "context");

            Context = context;
            _ownContext = false;
        }

        #endregion

        #region Private members

        private bool _ownContext;

        #endregion

        #region Public methods

        /// <summary>
        /// Check whether connection established.
        /// </summary>
        public PftFormatter CheckConnection ()
        {
            IrbisConnection connection = Context.Connection;
            if (ReferenceEquals(connection, null)
                || !connection.Connected)
            {
                throw new PftNotConnectedException();
            }

            return this;
        }

        /// <summary>
        /// Format the record.
        /// </summary>
        [NotNull]
        public virtual string Format
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            if (ReferenceEquals(Program, null))
            {
                throw new PftException("Program was not set");
            }

            Context.ClearAll();
            Context.Record = record;
            Context.Procedures = Program.Procedures;
            Program.Execute(Context);

            return Context.GetProcessedOutput();
        }

        /// <summary>
        /// Parse the program.
        /// </summary>
        public virtual void ParseProgram
            (
                [NotNull] string text
            )
        {
            Code.NotNull(text, "text");

            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(text);
            PftParser parser = new PftParser(tokens);
            Program = parser.Parse();
        }

        /// <summary>
        /// Set environment.
        /// </summary>
        public void SetEnvironment
            (
                //[NotNull] PftEnvironmentAbstraction environment
                [NotNull] AbstractClient environment
            )
        {
            Code.NotNull(environment, "environment");

            Context.SetEnvironment(environment);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_ownContext)
            {
                Context.Dispose();
            }
        }

        #endregion
    }
}
