﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuSubChapter.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AM;
using AM.Collections;
using AM.Text;
using AM.Text.Output;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft;
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json.Linq;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class MenuSubChapter
        : ChapterWithRecords
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

        /// <inheritdoc cref="BiblioChapter.IsServiceChapter" />
        public override bool IsServiceChapter
        {
            get
            {
                if (Children.Count == 0)
                {
                    return false;
                }

                MenuChapter mainChapter = MainChapter;
                if (ReferenceEquals(mainChapter, null))
                {
                    return Records.Count == 0;
                }

                return mainChapter.LeafOnly && Records.Count == 0;
            }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        [NotNull]
        protected virtual string GetDescriptionFormat()
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                MenuSubChapter subChapter = chapter as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    SpecialSettings settings = subChapter.Settings;
                    if (!ReferenceEquals(settings, null))
                    {
                        string result = settings.GetSetting("format");
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }

                chapter = chapter.Parent;
            }

            return MainChapter
                .ThrowIfNull("MainChapter")
                .Format
                .ThrowIfNull("MainChapter.Format");
        }

        /// <summary>
        /// Get order format from chapter hierarchy.
        /// </summary>
        [NotNull]
        protected virtual string GetOrderFormat()
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                MenuSubChapter subChapter = chapter as MenuSubChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    SpecialSettings settings = subChapter.Settings;
                    if (!ReferenceEquals(settings, null))
                    {
                        string result = settings.GetSetting("order");
                        if (!string.IsNullOrEmpty(result))
                        {
                            return result;
                        }
                    }
                }

                chapter = chapter.Parent;
            }

            return MainChapter
                .ThrowIfNull("MainChapter")
                .OrderBy
                .ThrowIfNull("MainChapter.Format");
        }

        [NotNull]
        internal static string Enhance
            (
                [NotNull] string text
            )
        {
            string result = text
                .Replace (". - ", ". – ");
                // .Replace("№", "\\'B9");

            return result;
        }

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

                    using (IPftFormatter formatter = processor.AcquireFormatter(context))
                    {
                        string descriptionFormat = GetDescriptionFormat();
                        descriptionFormat = processor.GetText
                            (
                                context,
                                descriptionFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(descriptionFormat);
                        //string[] formatted
                        //    = FormatRecords(context, descriptionFormat);

                        for (int i = 0; i < Records.Count; i++)
                        {
                            log.Write(".");
                            record = Records[i];
                            //string description = formatted[i]
                            //    .TrimEnd('\u001F');
                            string description = formatter.FormatRecord(record)
                                .TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(description)

                            description = BiblioUtility.AddTrailingDot(description);

                            BiblioItem item = new BiblioItem
                            {
                                Chapter = this,
                                Record = record,
                                Description = description
                            };
                            Items.Add(item);

                            RecordCollection same = record.UserData as RecordCollection;
                            if (!ReferenceEquals(same, null))
                            {
                                foreach (MarcRecord oneRecord in same)
                                {
                                    string desc = formatter.FormatRecord(oneRecord)
                                        .TrimEnd('\u001F');
                                    desc = BiblioUtility.AddTrailingDot(desc);
                                    oneRecord.Description = desc;
                                }
                            }
                        }

                        log.WriteLine(" done");

                        //string orderFormat = mainChapter.OrderBy
                        //    .ThrowIfNull("mainChapter.OrderBy");
                        string orderFormat = GetOrderFormat();
                        orderFormat = processor.GetText
                            (
                                context,
                                orderFormat
                            )
                            .ThrowIfNull("processor.GetText");
                        formatter.ParseProgram(orderFormat);
                        // formatted = FormatRecords(context, orderFormat);
                        for (int i = 0; i < Items.Count; i++)
                        {
                            log.Write(".");
                            BiblioItem item = Items[i];
                            record = item.Record;
                            //string order = formatted[i].TrimEnd('\u001F');
                            string order = formatter.FormatRecord(record)
                                .TrimEnd('\u001F');

                            // TODO handle string.IsNullOrEmpty(order)

                            order = CleanOrder(order);

                            //item.Order = RichText.Decode(order);
                            item.Order = order;
                        }

                        log.WriteLine(" done");

                        Items.SortByOrder(TrimOrder);

                        log.WriteLine("Items: {0}", Items.Count);
                    }
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

        /// <inheritdoc cref="BiblioChapter.Render" />
        public override void Render
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render {0}", this);

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");
            // ReportDriver driver = context.ReportContext.Driver;

            bool showOrder =
#if WINMOBILE || PocketPC
                false;
#else
                context.Document.CommonSettings.Value<bool?>("showOrder") ?? false;
#endif

            if (Records.Count != 0
                || Duplicates.Count != 0
                || Children.Count != 0)
            {
                RenderTitle(context);

                for (int i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    BiblioItem item = Items[i];
                    int number = item.Number;
                    string description = item.Description.ThrowIfNull("item.Description");

                    description = Enhance(description);

                    ReportBand band = new ParagraphBand
                        (
                            number.ToInvariantString() + ".\\~\\~"
                        );
                    report.Body.Add(band);

                    band.Cells.Add(new SimpleTextCell
                        (
                            // TODO implement properly!!!
                            //RichText.Encode(description, UnicodeRange.Russian)
                            //RichText.Encode2(description, UnicodeRange.Russian)
                            RichText.Encode3(description, UnicodeRange.Russian, "\\f2")
                        ));

                    MarcRecord record = item.Record;

                    // Для отладки: проверить упорядочение
                    if (showOrder)
                    {
                        if (!ReferenceEquals(record, null))
                        {
                            band = new ParagraphBand("MFN " + record.Mfn + " " + item.Order);
                            report.Body.Add(band);
                            report.Body.Add(new ParagraphBand());
                        }
                    }

                    if (!ReferenceEquals(record, null))
                    {
                        RecordCollection sameBooks = record.UserData as RecordCollection;
                        if (!ReferenceEquals(sameBooks, null))
                        {
                            foreach (MarcRecord book in sameBooks)
                            {
                                string text = book.Description;
                                text = RichText.Encode3(text, UnicodeRange.Russian, "\\f2")
                                    .ThrowIfNull();
                                band = new ParagraphBand(text);
                                report.Body.Add(band);
                                //band.Cells.Add(new SimpleTextCell(text));
                            }
                        }
                    }
                }

                log.WriteLine(" done");
            }

            RenderDuplicates(context);

            RenderChildren(context);

            log.WriteLine(string.Empty);
            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="BiblioChapter.ToString" />
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
