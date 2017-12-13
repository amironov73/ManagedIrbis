// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ItemCollection.cs -- 
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

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ItemCollection
        : NonNullCollection<BiblioItem>,
        IVerifiable
    {
        #region Properties

        #endregion

        #region Private members

        private static void ReadDigit
            (
                [NotNull] TextNavigator navigator,
                [NotNull] StringBuilder text
            )
        {
            char c = navigator.PeekChar();
            if (char.IsDigit(c))
            {
                navigator.ReadChar();
                text.Append(c);
            }
        }

        [CanBeNull]
        private static string _TrimOrder
            (
                [CanBeNull] string text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            StringBuilder result = new StringBuilder();
            TextNavigator navigator = new TextNavigator(text);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);
            ReadDigit(navigator, result);

            while (navigator.IsDigit())
            {
                navigator.ReadChar();
            }

            result.Append(navigator.GetRemainingText());

            return result.ToString();
        }

        private static int _Comparison
            (
                BiblioItem x,
                BiblioItem y
            )
        {
            return NumberText.Compare
                (
                    x.Order,
                    y.Order
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sort items by <see cref="BiblioItem.Order"/> field.
        /// </summary>
        public void SortByOrder()
        {
            List<BiblioItem> list = this.ToList();
            foreach (BiblioItem item in list)
            {
                item.Order = _TrimOrder(item.Order);
            }
            list.Sort(_Comparison);
            Clear();
            AddRange(list);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ItemCollection> verifier
                = new Verifier<ItemCollection>(this, throwOnError);

            foreach (BiblioItem item in this)
            {
                verifier.VerifySubObject(item, "item");
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion
    }
}
