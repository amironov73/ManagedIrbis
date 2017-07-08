// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFormatter.cs -- local PFT script interpreter
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM.Logging;

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

            Log.Trace("PftFormatter::Constructor");

            Context = context;
            _ownContext = false;
        }

        #endregion

        #region Private members

        private bool _ownContext;

        #endregion

        #region Public methods

        /// <summary>
        /// Format the record.
        /// </summary>
        [NotNull]
        public virtual string Format
            (
                [CanBeNull] MarcRecord record
            )
        {
            Log.Trace("PftFormatter::Format");

            if (ReferenceEquals(Program, null))
            {
                Log.Error
                    (
                        "PftFormatter::Format: "
                        + "program was not set"
                    );

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
                [NotNull] string source
            )
        {
            Code.NotNull(source, "source");

            Program = PftUtility.CompileProgram(source);
        }

        /// <summary>
        /// Set the provider.
        /// </summary>
        public void SetProvider
            (
                [NotNull] IrbisProvider provider
            )
        {
            Code.NotNull(provider, "provider");

            Context.SetProvider(provider);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Log.Trace("PftFormatter::Dispose");

            if (_ownContext)
            {
                Context.Dispose();
            }
        }

        #endregion
    }
}
