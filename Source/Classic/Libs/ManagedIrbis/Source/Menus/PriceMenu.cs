// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PriceMenu.cs -- wrapper for IZC.MNU
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using AM;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion


namespace ManagedIrbis.Menus
{
    //
    // Extract from official documentation
    //
    // При выполнении пополнения записи КСУ, при формировании выходных форм,
    // а также в задаче «Проверка фонда», подсчет цены выбывшего
    // (или проверенного) экземпляра книги и итоговых сумм выбытия
    // осуществляется с учетом индекса изменения цен текущего года
    // относительно цен года приобретения экземпляра.
    //
    // Индексы изменения цен или коэффициенты пересчета цен (КПЦ)
    // по годам представлены в справочнике IZC.mnu, находящемся в БД ЭК.
    // В исходном состоянии IZC.mnu содержит КПЦ, начиная с 1980 года,
    // по состоянию на 2007 год.

    /// <summary>
    /// Wrapper for IZC.MNU.
    /// </summary>
    [PublicAPI]
    public class PriceMenu
    {
        #region Constants

        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "IZC.MNU";

        #endregion

        #region Nested classes

        /// <summary>
        /// Item.
        /// </summary>
        public sealed class Item
            : IComparable<Item>
        {
            #region Properties

            /// <summary>
            /// Date.
            /// </summary>
            public string Date { get; set; }

            /// <summary>
            /// Coefficient.
            /// </summary>
            public decimal Coefficient { get; set; }

            #endregion

            #region Public methods

            /// <summary>
            /// Parse the menu entry.
            /// </summary>
            [NotNull]
            public static Item Parse
                (
                    [NotNull] MenuEntry entry
                )
            {
                Code.NotNull(entry, "entry");

                string date = entry.Code;
                if (string.IsNullOrEmpty(date)
                    || ((date.Length != 4) && (date.Length != 6)))
                {
                    throw new IrbisException();
                }

                string comment = entry.Comment;
                if (string.IsNullOrEmpty(comment))
                {
                    throw new IrbisException();
                }

                decimal coefficient = NumericUtility.ParseDecimal(comment);

                Item result = new Item
                {
                    Date = date,
                    Coefficient = coefficient
                };

                return result;
            }

            #endregion

            #region IComparable<Item> members

            /// <inheritdoc cref="IComparable{T}.CompareTo"/>
            public int CompareTo
                (
                    Item other
                )
            {
                // ReSharper disable ArrangeThisQualifier
                return _Compare(this.Date, other.Date);
                // ReSharper restore ArrangeThisQualifier
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of items.
        /// </summary>
        [NotNull]
        public List<Item> Items { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PriceMenu
            (
                [NotNull] MenuFile menu
            )
        {
            Code.NotNull(menu, "menu");

            Items = new List<Item>();
            foreach (MenuEntry entry in menu.Entries)
            {
                Item item = Item.Parse(entry);
                Items.Add(item);
            }

            Items.Sort(new UniversalComparer<Item>
                (
                    (left, right) => _Compare(left.Date, right.Date)
                ));
        }

        #endregion

        #region Private members

        private static int _Compare(string left, string right)
        {
            return string.Compare
                (
                    left,
                    right,
                    StringComparison.Ordinal
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read IZC.MNU from server connection.
        /// </summary>
        [NotNull]
        public static PriceMenu FromConnection
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] string fileName
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    StandardDatabases.Readers
                );
            MenuFile menu = MenuFile.ReadFromServer(connection, specification)
                .ThrowIfNull("menu");
            PriceMenu result = new PriceMenu(menu);

            return result;
        }

        /// <summary>
        /// Read IZC.MNU from server connection.
        /// </summary>
        [NotNull]
        public static PriceMenu FromConnection
            (
                [NotNull] IIrbisConnection connection
            )
        {
            return FromConnection(connection, DefaultFileName);
        }

        /// <summary>
        /// Read IZC.MNU from the local file.
        /// </summary>
        [NotNull]
        public static PriceMenu FromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            MenuFile menu = MenuFile.ParseLocalFile(fileName);
            PriceMenu result = new PriceMenu(menu);

            return result;
        }

        /// <summary>
        /// Read IZC.MNU from the provider.
        /// </summary>
        [NotNull]
        public static PriceMenu FromProvider
            (
                [NotNull] IrbisProvider provider,
                [NotNull] string fileName
            )
        {
            Code.NotNull(provider, "provider");
            Code.NotNullNorEmpty(fileName, "fileName");

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    StandardDatabases.Readers,
                    fileName
                );
            MenuFile menu = provider.ReadMenuFile(specification)
                .ThrowIfNull("menu");
            PriceMenu result = new PriceMenu(menu);

            return result;
        }

        /// <summary>
        /// Read IZC.MNU from the provider.
        /// </summary>
        [NotNull]
        public static PriceMenu FromProvider
            (
                [NotNull] IrbisProvider provider
            )
        {
            return FromProvider(provider, DefaultFileName);
        }

        /// <summary>
        /// Get coefficient for the date.
        /// </summary>
        public decimal GetCoefficient
            (
                [CanBeNull] string date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                return 1.0m;
            }

            decimal result = 1.0m;
            if (Items.Count != 0)
            {
                result = Items.Min().Coefficient;
            }

            foreach (Item item in Items)
            {
                if (_Compare(date, item.Date) >= 0)
                {
                    result = item.Coefficient;
                }
            }

            return result;
        }

        /// <summary>
        /// Do we have coefficient for the date?
        /// </summary>
        public bool HaveCoefficient
            (
                [CanBeNull] string date
            )
        {
            if (string.IsNullOrEmpty(date))
            {
                return true;
            }

            foreach (Item item in Items)
            {
                if (item.Date.StartsWith(date))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
