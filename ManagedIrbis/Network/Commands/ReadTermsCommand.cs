/* ReadTermsCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Network.Commands
{
    /// <summary>
    /// Read terms
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Database} {NumberOfTerms} "
        + "{ReverseOrder} {StartTerm}")]
    public sealed class ReadTermsCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Number of terms to return.
        /// </summary>
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Reverse order?
        /// </summary>
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Start term.
        /// </summary>
        [CanBeNull]
        public string StartTerm { get; set; }

        /// <summary>
        /// Terms.
        /// </summary>
        [NotNull]
        public List<TermInfo> Terms
        {
            get { return _terms; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadTermsCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _terms = new List<TermInfo>();
        }

        #endregion

        #region Private members

        private readonly List<TermInfo> _terms;

        #endregion

        #region Public methods

        #endregion

        #region AbstractCommand members

        /// <summary>
        /// Good return codes.
        /// </summary>
        public override int[] GoodReturnCodes
        {
            get { return new[] { -202, -203, -204 }; }
        }

        /// <summary>
        /// Create client query.
        /// </summary>
        public override ClientQuery CreateQuery()
        {
            ClientQuery result =  base.CreateQuery();
            result.CommandCode = ReverseOrder
                ? CommandCode.ReadTermsReverse
                : CommandCode.ReadTerms;

            string database = Database
                ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisException("database not specified");
            }
            result
                .Add(database)
                .Add(StartTerm)
                .Add(NumberOfTerms);

            return result;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public override ServerResponse Execute
            (
                ClientQuery query
            )
        {
            Code.NotNull(query, "query");

            ServerResponse result = base.Execute(query);
            CheckResponse(result);

            TermInfo[] terms = TermInfo.Parse(result);
            Terms.AddRange(terms);

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <summary>
        /// Verify object state.
        /// </summary>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReadTermsCommand> verifier
                = new Verifier<ReadTermsCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert(NumberOfTerms >= 0, "NumberOfTerms")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
