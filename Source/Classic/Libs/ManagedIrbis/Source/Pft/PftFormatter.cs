// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftFormatter.cs -- local PFT script interpreter
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics;

using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Server;

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
        : IPftFormatter
    {
        #region Events

        #endregion

        #region Properties

        /// <inheritdoc cref="IPftFormatter.SupportsExtendedSyntax" />
        public virtual bool SupportsExtendedSyntax { get { return true; } }

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

        /// <summary>
        /// Elapsed.
        /// </summary>
        public TimeSpan Elapsed { get; set; }

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

        /// <inheritdoc cref="IPftFormatter.FormatRecord(MarcRecord)" />
        public virtual string FormatRecord
            (
                MarcRecord record
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

#if !SILVERLIGHT

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

#endif

            Context.ClearAll();
            Context.Record = record;
            Context.Procedures = Program.Procedures;
            Program.Execute(Context);

            string result = Context.GetProcessedOutput();

#if !SILVERLIGHT

            stopwatch.Stop();
            Elapsed = stopwatch.Elapsed;

#endif

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.FormatRecord(Int32)" />
        public string FormatRecord
            (
                int mfn
            )
        {
            Code.Positive(mfn, "mfn");

            MarcRecord record = Context.Provider.ReadRecord(mfn);
            string result = FormatRecord(record);

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.FormatRecords" />
        public string[] FormatRecords
            (
                int[] mfns
            )
        {
            Code.NotNull(mfns, "mfns");

            string[] result = new string[mfns.Length];
            for (int i = 0; i < mfns.Length; i++)
            {
                MarcRecord record = Context.Provider.ReadRecord(mfns[i]);
                result[i] = FormatRecord(record);
            }

            return result;
        }

        /// <inheritdoc cref="IPftFormatter.ParseProgram" />
        public virtual void ParseProgram
            (
                string source
            )
        {
            Code.NotNull(source, "source");

            string[] pathArray = Context.Provider.GetFileSearchPath();
            source = ServerUtility.ExpandInclusion
                (
                    source,
                    "pft",
                    pathArray
                );
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
