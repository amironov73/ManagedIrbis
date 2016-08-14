/* ReadPostingsCommand.cs -- read posings for given term
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    //
    // ПАРАМЕТРЫ
    // db_name – имя базы данных
    // ΤΕΡΜ – поисковый термин
    // num_postings – число возвращаемых ссылок.
    // Если данный параметр 0, то возвращаются
    // MAX_POSTINGS_IN_PACKET ссылок.
    // first_posting – возможно 2 варианта значений
    // для данного параметра:
    // 1-й вариант – число больше 0. Это номер 1-й возвращаемой
    // ссылки из общего списка ссылок данного термина;
    // 2-й вариант – если равно 0, то возвращается только
    // число ссылок данного термина.
    //
    // ВОЗВРАТ
    // список строк в следующей последовательности:
    // В 1-й строке – код возврата, который определяется тем,
    // найден ли заданный термин TERM в словаре – если найден
    // код возврата – ZERO, если нет – число меньше 0.
    //
    //  Если термин найден в словаре и ИМЕЕТ ССЫЛКИ (не удален),
    // далее следуют строки в следующем формате:
    // MFN#TAG#OCC#CNT, например:
    //
    // 0
    // 1#200#1#1
    // 3#200#1#1
    //

    /// <summary>
    /// Read postings for given term.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Database} {NumberOfPostings} "
        + "{FirstPosting}")]
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
        /// List of terms.
        /// </summary>
        [CanBeNull]
        public string[] ListOfTerms { get; set; }

        /// <summary>
        /// Postings.
        /// </summary>
        [CanBeNull]
        public TermPosting[] Postings { get; set; }

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
            
            FirstPosting = 1;
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
            // TERM_NOT_EXISTS = -202;
            // TERM_LAST_IN_LIST = -203;
            // TERM_FIRST_IN_LIST = -204;
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
                .Add(Format);

            if (string.IsNullOrEmpty(Term))
            {
                if (ReferenceEquals(ListOfTerms, null))
                {
                    throw new IrbisException("list of terms == null");
                }

                foreach (string term in ListOfTerms)
                {
                    result.AddUtf8(term);
                }
            }
            else
            {
                result.AddUtf8(Term);
            }

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

            Postings = TermPosting.Parse(result);

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
                .Assert(FirstPosting >= 0, "FirstPosting")
                .Assert(NumberOfPostings >= 0, "NumberOfPostings")
                .Assert(base.Verify(throwOnError));

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
