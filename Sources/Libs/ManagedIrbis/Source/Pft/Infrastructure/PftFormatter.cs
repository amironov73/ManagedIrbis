/* PftFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Infrastructure;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Локальный форматтер: интерпретатор PFT-скриптов.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftFormatter
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
                new PftContext(null, null)
            )
        {
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
            Context._SetFormatter(this);
        }

        #endregion

        #region Private members

        private string _InlineEvaluator
            (
                [NotNull] Match match
            )
        {
            string formatName = match.Value;
            formatName = formatName
                .Substring
                (
                    1,
                    formatName.Length - 2
                );
            CheckConnection();
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    formatName
                );
            string result = Context.Connection.ReadTextFile(specification);

            return result;
        }

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
        public string Format
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            Context.ClearAll();
            Context.Record = record;
            Program.Execute(Context);
            
            return Context.Text;
        }

        /// <summary>
        /// Resolve inline format.
        /// </summary>
        public string ResolveInline
            (
                string input
            )
        {
            string result = Regex.Replace
                (
                    input,
                    "\x1C.*?\x1D",
                    _InlineEvaluator
                );
            return result;
        }

        #endregion

        #region IDisposable members

        /// <inhreritdoc/>
        public void Dispose()
        {
            // TODO Do something?
        }

        #endregion
    }
}
