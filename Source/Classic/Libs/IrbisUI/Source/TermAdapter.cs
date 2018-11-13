// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TermAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Search;

using MoonSharp.Interpreter;

#endregion

namespace IrbisUI
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class TermAdapter
    {
        #region Properties

        /// <summary>
        /// Binding source.
        /// </summary>
        [NotNull]
        public BindingSource Source { get; private set; }

        /// <summary>
        /// Current term value.
        /// </summary>
        [NotNull]
        public string CurrentValue
        {
            get
            {
                TermInfo term = (TermInfo)Source.Current;
                if (ReferenceEquals(term, null))
                {
                    return string.Empty;
                }

                string result = term.Text ?? string.Empty;

                return result;
            }
        }

        /// <summary>
        /// NotNull
        /// </summary>
        [NotNull]
        public string FullTerm
        {
            get
            {
                string result = Prefix + CurrentValue;

                return result;
            }
        }

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Prefix.
        /// </summary>
        [NotNull]
        public string Prefix { get; private set; }

        /// <summary>
        /// Portion size;
        /// </summary>
        public int Portion { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TermAdapter
            (
                [NotNull] IrbisConnection connection,
                [NotNull] string prefix
            )
        {
            Code.NotNull(connection, nameof(connection));
            Code.NotNull(prefix, nameof(prefix));

            Source = new BindingSource(EmptyArray<TermInfo>.Value, null);
            Portion = 100;
            Connection = connection;
            Prefix = prefix;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next term.
        /// </summary>
        public bool MoveNext()
        {
            BindingSource termSource = Source;
            CurrencyManager currencyManager = termSource.CurrencyManager;

            termSource.MoveNext();
            if (currencyManager.Position >= currencyManager.Count - 1)
            {
                return Fill();
            }

            return true;
        }

        /// <summary>
        /// Move to next term.
        /// </summary>
        public bool MoveNext(int amount)
        {
            while (amount > 0)
            {
                amount--;
                if (!MoveNext())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Move to previous term.
        /// </summary>
        public bool MovePrevious()
        {
            BindingSource termSource = Source;
            CurrencyManager currencyManager = termSource.CurrencyManager;

            termSource.MovePrevious();
            if (currencyManager.Position < 1)
            {
                return Fill(null, true);
            }

            return true;
        }

        /// <summary>
        /// Move to previous term.
        /// </summary>
        public bool MovePrevious
            (
                int amount
            )
        {
            while (amount > 0)
            {
                amount--;
                if (!MovePrevious())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Fill the adapter.
        /// </summary>
        public bool Fill
            (
                string startTerm = null,
                bool backward = false
            )
        {
            string fullTerm = FullTerm;
            if (!ReferenceEquals(startTerm, null))
            {
                fullTerm = Prefix + startTerm;
            }

            TermParameters parameters = new TermParameters
            {
                Database = Connection.Database,
                StartTerm = fullTerm,
                NumberOfTerms = Portion,
                ReverseOrder = backward
            };
            TermInfo[] terms = Connection.ReadTerms(parameters);
            if (terms.Length <= 1)
            {
                return false;
            }

            if (backward)
            {
                Array.Reverse(terms);
            }

            string prefix = Prefix;
            int prefixLength = prefix.Length;
            if (prefixLength != 0)
            {
                List<TermInfo> goodTerms = new List<TermInfo>(terms.Length);
                foreach (TermInfo term in terms)
                {
                    if (term.Count < 1)
                    {
                        continue;
                    }

                    string termText = term.Text;
                    if (!string.IsNullOrEmpty(termText) && termText.StartsWith(prefix))
                    {
                        term.Text = termText.Substring(prefixLength);
                        goodTerms.Add(term);
                    }
                }

                terms = goodTerms.ToArray();
            }

            if (terms.Length <= 1)
            {
                return false;
            }

            Source.DataSource = terms;
            if (backward)
            {
                Source.Position = Source.Count - 1;
            }
            else
            {
                Source.Position = 0;
            }

            return true;
        }

        #endregion
    }
}
