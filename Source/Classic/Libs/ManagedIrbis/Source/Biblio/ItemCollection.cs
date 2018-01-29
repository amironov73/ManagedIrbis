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

        private static bool _IsOfficial
            (
                [NotNull] MarcRecord record
            )
        {
            // Официальные документы имеют характер n или 67

            string[] character =
            {
                record.FM(900, 'c'),
                record.FM(900, '2'),
                record.FM(900, '3'),
                record.FM(900, '4'),
                record.FM(900, '5'),
                record.FM(900, '6')
            };

            return character.Contains("n")
                   || character.Contains("N")
                   || character.Contains("67");
        }

        private static bool _IsForeign
            (
                [NotNull] MarcRecord record
            )
        {
            // У иностранных книг язык не rus
            // Если язык не указан, считаем, что rus

            string language = record.FM(101) ?? "rus";

            return !language.SameString("rus");
        }

        private static int _Comparison
            (
                BiblioItem x,
                BiblioItem y
            )
        {
            MarcRecord xrec = x.Record;
            MarcRecord yrec = y.Record;
            if (!ReferenceEquals(xrec, null) && !ReferenceEquals(yrec, null))
            {
                // Поднимаем официальные документы

                bool xup = _IsOfficial(xrec);
                bool yup = _IsOfficial(yrec);
                if (xup != yup)
                {
                    return xup ? -1 : 1;
                }

                // Опускаем иностранные документы

                bool xdown = _IsForeign(xrec);
                bool ydown = _IsForeign(yrec);
                if (xdown != ydown)
                {
                    return xdown ? 1 : -1;
                }
            }

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
