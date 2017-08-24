// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SimpleChapter.cs -- 
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
    public class SimpleChapter
        : ChapterWithRecords
    {
        #region Properties

        /// <summary>
        /// Filter.
        /// </summary>
        [CanBeNull]
        [JsonProperty("filter")]
        public string Filter { get; set; }

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [CanBeNull]
        [JsonProperty("order")]
        public string Order { get; set; }

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
            string result = GetProperty<SimpleChapter, string>
                (
                    chapter => chapter.Format
                );
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException("format not set");
            }

            return result;
        }

        /// <summary>
        /// Get description format from chapter hierarchy.
        /// </summary>
        [NotNull]
        protected virtual string GetOrderFormat()
        {
            string result = GetProperty<SimpleChapter, string>
                (
                    chapter => chapter.Order
                );
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException("order not set");
            }

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
            log.WriteLine("Begin build items {0}", this);

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");

            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                string descriptionFormat = GetDescriptionFormat();
                descriptionFormat = processor.GetText
                    (
                        context,
                        descriptionFormat
                    )
                    .ThrowIfNull("processor.GetText");
                formatter.ParseProgram(descriptionFormat);

                int[] mfns = Records.Select(r => r.Mfn).ToArray();
                string[] formatted = formatter.FormatRecords(mfns);

                for (int i = 0; i < Records.Count; i++)
                {
                    log.Write(".");
                    MarcRecord record = Records[i];
                    string description = formatted[i]
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
                }
                log.WriteLine(" done");
            }

            using (IPftFormatter formatter
                = processor.AcquireFormatter(context))
            {
                string orderFormat = GetOrderFormat();
                orderFormat = processor.GetText
                    (
                        context,
                        orderFormat
                    )
                    .ThrowIfNull("processor.GetText");
                formatter.ParseProgram(orderFormat);

                int[] mfns = Records.Select(r => r.Mfn).ToArray();
                string[] formatted = formatter.FormatRecords(mfns);

                for (int i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    BiblioItem item = Items[i];
                    string order = formatted[i].TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(order)

                    item.Order = order;
                }
                log.WriteLine(" done");
            }

            Items.SortByOrder();

            log.WriteLine("Items: {0}", Items.Count);

            foreach (BiblioChapter chapter in Children)
            {
                chapter.BuildItems(context);
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

            log.WriteLine("End gather records {0}", this);
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

            log.WriteLine("End render {0}", this);
        }

        #endregion

        #region Object members

        #endregion
    }
}
