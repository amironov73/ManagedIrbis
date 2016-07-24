/* ReadPostingsCommand.cs --
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
    [DebuggerDisplay("{Database} {NumberOfPostings} "
        + "{StartPosting}")]
    public sealed class ReadPostingsCommand
        : AbstractCommand
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// First posting to return.
        /// </summary>
        public int FirstPosting { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        /// <summary>
        /// Number of postings to return.
        /// </summary>
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// Term.
        /// </summary>
        [CanBeNull]
        public string Term { get; set; }

        /// <summary>
        /// Postings.
        /// </summary>
        [NotNull]
        public List<TermPosting> Postings
        {
            get { return _postings; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadPostingsCommand
            (
                [NotNull] IrbisConnection connection
            )
            : base(connection)
        {
            _postings = new List<TermPosting>();
        }

        #endregion

        #region Private members

        private readonly List<TermPosting> _postings;

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
            ClientQuery result = base.CreateQuery();
            result.CommandCode = CommandCode.ReadPostings;

            string database = Database
                ?? Connection.Database;
            if (string.IsNullOrEmpty(database))
            {
                throw new IrbisException("database not specified");
            }
            result
                .Add(database)
                .Add(NumberOfPostings)
                .Add(FirstPosting)
                .Add(Format)
                .Add(Term);

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

            TermPosting[] postings = TermPosting.Parse(result);
            Postings.AddRange(postings);

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
            Verifier<ReadPostingsCommand> verifier
                = new Verifier<ReadPostingsCommand>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .Assert(NumberOfPostings >= 0, "NumberOfPostings")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
