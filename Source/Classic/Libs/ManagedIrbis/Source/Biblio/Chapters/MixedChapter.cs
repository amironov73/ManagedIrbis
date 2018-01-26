// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MixedChapter.cs -- 
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
using ManagedIrbis.Fields;
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
    public class MixedChapter
        : CumulatingSubChapter
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="MenuSubChapter.BuildItems" />
        public override void BuildItems
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            if (Records.Count == 0)
            {
                return;
            }

            AbstractOutput log = context.Log;
            MarcRecord record = null;

            log.WriteLine("Begin build items {0}", this);
            Items = new ItemCollection();

            SpecialSettings settings = Settings;
            if (ReferenceEquals(settings, null))
            {
                return;
            }

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisProvider provider = context.Provider;
            List<MarcRecord> nonSpec = new List<MarcRecord>();

            string generalFormat = settings.GetSetting("general");
            string orderFormat = settings.GetSetting("order");
            string partFormat = settings.GetSetting("format");
            string normalFormat = settings.GetSetting("normal");
            if (string.IsNullOrEmpty(generalFormat)
                || string.IsNullOrEmpty(orderFormat)
                || string.IsNullOrEmpty(partFormat)
                || string.IsNullOrEmpty(normalFormat))
            {
                return;
            }

            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                generalFormat = processor.GetText(context, generalFormat)
                    .ThrowIfNull("generalFormat");
                formatter.ParseProgram(generalFormat);

                for (int i = 0; i < Records.Count; i++)
                {
                    record = Records[i];
                    BookInfo bookInfo = new BookInfo(provider, record);
                    if (bookInfo.Worksheet != "SPEC")
                    {
                        nonSpec.Add(record);
                        continue;
                    }

                    log.Write("o");
                    string header = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(header))
                    {
                        header = header.Trim();
                    }
                    BiblioItem item = new BiblioItem
                    {
                        Record = record,
                        Chapter = this
                    };
                    Multivolume bookGroup = Groups.FirstOrDefault
                        (
                            g => g.Header == header
                        );
                    if (ReferenceEquals(bookGroup, null))
                    {
                        bookGroup = new Multivolume
                        {
                            Header = header
                        };
                        Groups.Add(bookGroup);
                    }
                    bookGroup.Add(item);
                }

                partFormat = processor.GetText(context, partFormat)
                    .ThrowIfNull("partFormat");
                formatter.ParseProgram(partFormat);

                foreach (Multivolume grp in Groups)
                {
                    foreach (BiblioItem item in grp)
                    {
                        log.Write(":");

                        string description = formatter.FormatRecord(item.Record)
                            .TrimEnd('\u001F');

                        // TODO handle string.IsNullOrEmpty(description)

                        description = BiblioUtility.AddTrailingDot(description);
                        item.Description = description;
                    }
                }

                normalFormat = processor.GetText(context, normalFormat)
                    .ThrowIfNull("normalFormat");
                formatter.ParseProgram(normalFormat);

                foreach (MarcRecord rec in nonSpec)
                {
                    log.Write(".");
                    string description = formatter.FormatRecord(rec)
                        .TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(description)

                    description = BiblioUtility.AddTrailingDot(description);

                    BiblioItem item = new BiblioItem
                    {
                        Chapter = this,
                        Record = rec,
                        Description = description
                    };
                    Multivolume bookGroup = new Multivolume
                    {
                        Header = description,
                        Single = true
                    };
                    Groups.Add(bookGroup);
                    bookGroup.Add(item);
                }

                orderFormat = processor.GetText(context, orderFormat)
                    .ThrowIfNull("orderFormat");
                formatter.ParseProgram(orderFormat);

                foreach (Multivolume bookGroup in Groups)
                {
                    record = bookGroup.First().Record
                        .ThrowIfNull("bookGroup.Record");
                    string order = formatter.FormatRecord(record.Mfn);
                    if (!string.IsNullOrEmpty(order))
                    {
                        order = order.Trim();
                    }
                    bookGroup.Order = order;

                    if (!bookGroup.Single)
                    {
                        foreach (BiblioItem item in bookGroup)
                        {
                            order = formatter.FormatRecord(item.Record.Mfn);
                            if (!string.IsNullOrEmpty(order))
                            {
                                order = order.Trim();
                            }

                            item.Order = order;
                        }
                    }
                }

                processor.ReleaseFormatter(context, formatter);
            }

            Groups = Groups.OrderBy(x => x.Order).ToList();
            Items.Clear();
            foreach (Multivolume bookGroup in Groups)
            {
                OrderGroup(bookGroup);
                BiblioItem item = new BiblioItem
                {
                    Description = bookGroup.Header,
                    Record = new MarcRecord(), // TODO ???
                    UserData = bookGroup
                };
                bookGroup.Item = item;
                Items.Add(item);
            }

            log.WriteLine("End build items {0}", this);
        }


        #endregion

        #region Object members

        #endregion
    }
}

