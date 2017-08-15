// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuSubChapter.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MenuSubChapter
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Key.
        /// </summary>
        [CanBeNull]
        public string Key { get; set; }

        /// <summary>
        /// Main chapter.
        /// </summary>
        [CanBeNull]
        public MenuChapter MainChapter { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public List<MarcRecord> Records { get; private set; }

        /// <summary>
        /// Special settings associated with the chapter
        /// and its children.
        /// </summary>
        [CanBeNull]
        public SpecialSettings SpecialSettings { get; set; }

        ///// <summary>
        ///// Items.
        ///// </summary>
        //[NotNull]
        //public List<BiblioItem> Items { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuSubChapter()
        {
            Records = new List<MarcRecord>();
            //Items = new List<BiblioItem>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            MarcRecord record = null;
            IPftFormatter formatter = null;

            log.WriteLine("Begin build items {0}", this);
            Items = new ItemCollection();

            try
            {
                if (Records.Count != 0)
                {
                    BiblioProcessor processor = context.Processor
                        .ThrowIfNull("context.Processor");
                    MenuChapter mainChapter = MainChapter
                        .ThrowIfNull("MainChapter");

                    using (formatter = processor.GetFormatter(context))
                    {
                        // TODO use hierarchy for descriptionFormat
                        // and orderFormat

                        string descriptionFormat = mainChapter.Format
                            .ThrowIfNull("mainChapter.Format");
                        descriptionFormat = processor.GetText
                            (
                                context,
                                descriptionFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(descriptionFormat);

                        for (int i = 0; i < Records.Count; i++)
                        {
                            log.Write(".");
                            record = Records[i];
                            string description = formatter.Format(record);

                            // TODO handle string.IsNullOrEmpty(description)

                            BiblioItem item = new BiblioItem
                            {
                                Chapter = this,
                                Record = record,
                                Description = description
                            };
                            Items.Add(item);
                        }
                    }

                    log.WriteLine(string.Empty);

                    using (formatter = processor.GetFormatter(context))
                    {
                        string orderFormat = mainChapter.OrderBy
                            .ThrowIfNull("mainChapter.OrderBy");
                        orderFormat = processor.GetText
                            (
                                context,
                                orderFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(orderFormat);

                        for (int i = 0; i < Items.Count; i++)
                        {
                            log.Write(".");
                            BiblioItem item = Items[i];
                            record = item.Record;
                            string order = formatter.Format(record);

                            // TODO handle string.IsNullOrEmpty(order)

                            item.Order = order;
                        }
                    }

                    log.WriteLine(string.Empty);

                    Items.SortByOrder();
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.BuildItems(context);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format
                    (
                        "Exception: {0}", exception
                    );
                if (!ReferenceEquals(record, null))
                {
                    message = string.Format
                        (
                            "MFN={0} : {1}",
                            record.Mfn,
                            message
                        );
                }

                log.WriteLine(string.Empty);
                log.WriteLine(message);
                throw;
            }

            log.WriteLine("End build items {0}", this);
        }

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather records {0}", this);

            try
            {
                IrbisProvider provider = context.Provider;
                MenuChapter mainChapter = MainChapter.ThrowIfNull();

                // What to do?

            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine("Record count: {0}", Records.Count);

            log.WriteLine("End gather records {0}", this);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = base.ToString()
                + " [:] "
                + Records.Count.ToInvariantString();

            return result;
        }

        #endregion
    }
}
