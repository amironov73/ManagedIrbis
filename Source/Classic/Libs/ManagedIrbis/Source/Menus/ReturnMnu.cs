// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReturnMnu.cs -- wrapper for RETURN.MNU
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Menus
{
    /// <summary>
    /// Wrapper for RETURN.MNU.
    /// </summary>
    [PublicAPI]
    public sealed class ReturnMnu
    {
        #region Constants

        /// <summary>
        /// Default file name.
        /// </summary>
        public const string DefaultFileName = "RETURN.MNU";

        #endregion

        #region Nested classes

        /// <summary>
        /// Item.
        /// </summary>
        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Date.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Comment.
            /// </summary>
            [CanBeNull]
            public string Comment { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item()
            {
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Item
                (
                    [NotNull] MenuEntry entry
                )
            {
                Code.NotNull(entry, "entry");

                string code = entry.Code.ThrowIfNull("entry.Code");
                Comment = entry.Comment;
                if (code.StartsWith("@"))
                {
                    Date = DateTime.ParseExact
                        (
                            code.Substring(1),
                            "dd.MM.yyyy",
                            CultureInfo.InvariantCulture
                        );
                }
                else
                {
                    Date = DateTime.Today.AddDays(NumericUtility.ParseInt32(code));
                }
            }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString()
            {
                return string.Format
                    (
                        "{0} {1}",
                        Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Comment
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Items.
        /// </summary>
        [NotNull]
        public List<Item> Items { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReturnMnu
            (
                [NotNull] MenuFile menu
            )
        {
            Code.NotNull(menu, "menu");

            Items = new List<Item>();
            foreach (MenuEntry entry in menu.Entries)
            {
                Items.Add(new Item(entry));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read RETURN.MNU from server connection.
        /// </summary>
        [NotNull]
        public static ReturnMnu FromConnection
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
            ReturnMnu result = new ReturnMnu(menu);

            return result;
        }

        /// <summary>
        /// Read RETURN.MNU from server connection.
        /// </summary>
        [NotNull]
        public static ReturnMnu FromConnection
            (
                [NotNull] IIrbisConnection connection
            )
        {
            return FromConnection(connection, DefaultFileName);
        }

        /// <summary>
        /// Read RETURN.MNU from the local file.
        /// </summary>
        [NotNull]
        public static ReturnMnu FromFile
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            MenuFile menu = MenuFile.ParseLocalFile(fileName);
            ReturnMnu result = new ReturnMnu(menu);

            return result;
        }

        /// <summary>
        /// Read RETURN.MNU from the provider.
        /// </summary>
        [NotNull]
        public static ReturnMnu FromProvider
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
            ReturnMnu result = new ReturnMnu(menu);

            return result;
        }

        /// <summary>
        /// Read RETURN.MNU from the provider.
        /// </summary>
        [NotNull]
        public static ReturnMnu FromProvider
            (
                [NotNull] IrbisProvider provider
            )
        {
            return FromProvider(provider, DefaultFileName);
        }

        #endregion
    }
}
