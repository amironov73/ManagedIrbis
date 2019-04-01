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
using System.Text.RegularExpressions;
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
using ManagedIrbis.Reports;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

// ReSharper disable ForCanBeConvertedToForeach

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
        [JsonProperty("search")]
        public string SearchExpression { get; set; }

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

        static string Propis(int number)
        {
            int[] array_int = new int[4];
            string[,] array_string = new string[4, 3]
            {
                {" миллиард", " миллиарда", " миллиардов"},
                {" миллион", " миллиона", " миллионов"},
                {" тысяча", " тысячи", " тысяч"},
                {"", "", ""}

            };
            array_int[0] = (number - (number % 1000000000)) / 1000000000;
            array_int[1] = ((number % 1000000000) - (number % 1000000)) / 1000000;
            array_int[2] = ((number % 1000000) - (number % 1000)) / 1000;
            array_int[3] = number % 1000;
            string result = "";
            for (int i = 0; i < 4; i++)
            {
                if (array_int[i] != 0)
                {
                    if (((array_int[i] - (array_int[i] % 100)) / 100) != 0)
                        switch (((array_int[i] - (array_int[i] % 100)) / 100))
                        {
                            case 1: result += " сто"; break;
                            case 2: result += " двести"; break;
                            case 3: result += " триста"; break;
                            case 4: result += " четыреста"; break;
                            case 5: result += " пятьсот"; break;
                            case 6: result += " шестьсот"; break;
                            case 7: result += " семьсот"; break;
                            case 8: result += " восемьсот"; break;
                            case 9: result += " девятьсот"; break;
                        }
                    if (((array_int[i] % 100) - ((array_int[i] % 100) % 10)) / 10 != 1)
                    {
                        switch (((array_int[i] % 100) - ((array_int[i] % 100) % 10)) / 10)
                        {
                            case 2: result += " двадцать"; break;
                            case 3: result += " тридцать"; break;
                            case 4: result += " сорок"; break;
                            case 5: result += " пятьдесят"; break;
                            case 6: result += " шестьдесят"; break;
                            case 7: result += " семьдесят"; break;
                            case 8: result += " восемьдесят"; break;
                            case 9: result += " девяносто"; break;
                        }
                    }
                    switch (array_int[i] % 100)
                    {
                        case 1: if (i == 2) result += " одна"; else result += " один"; break;
                        case 2: if (i == 2) result += " две"; else result += " два"; break;
                        case 3: result += " три"; break;
                        case 4: result += " четыре"; break;
                        case 5: result += " пять"; break;
                        case 6: result += " шесть"; break;
                        case 7: result += " семь"; break;
                        case 8: result += " восемь"; break;
                        case 9: result += " девять"; break;
                        case 10: result += " десять"; break;
                        case 11: result += " одиннадцать"; break;
                        case 12: result += " двенадцать"; break;
                        case 13: result += " тринадцать"; break;
                        case 14: result += " четырнадцать"; break;
                        case 15: result += " пятнадцать"; break;
                        case 16: result += " шестнадцать"; break;
                        case 17: result += " семнадцать"; break;
                        case 18: result += " восемнадцать"; break;
                        case 19: result += " девятнадцать"; break;
                    }

                    if (array_int[i] % 100 >= 10 && array_int[i] % 100 <= 19)
                    {
                        result += " " + array_string[i, 2] + " ";
                    }
                    else
                    {
                        switch (array_int[i] % 100)
                        {
                            case 1: result += " " + array_string[i, 0] + " "; break;
                            case 2:
                            case 3:
                            case 4: result += " " + array_string[i, 1] + " "; break;
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9: result += " " + array_string[i, 2] + " "; break;

                        }
                    }
                }

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

                if (ReferenceEquals(Items, null))
                {
                    Items = new ItemCollection();
                }

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

                Regex fioRegex = new Regex(@"^[А-Я]\.(\s+[А-Я]\.)");

                for (int i = 0; i < Items.Count; i++)
                {
                    log.Write(".");
                    BiblioItem item = Items[i];
                    string order = formatted[i].TrimEnd('\u001F');

                    // TODO handle string.IsNullOrEmpty(order)

                    if (order.StartsWith("["))
                    {
                        order = order.Substring(1);
                    }

                    var firstChar = order.FirstChar();
                    if (char.IsDigit(firstChar))
                    {
                        var numberText = "";
                        while (order.Length != 0)
                        {
                            firstChar = order.FirstChar();
                            if (!char.IsDigit(firstChar) && char.IsWhiteSpace(firstChar))
                            {
                                break;
                            }

                            if (char.IsDigit(firstChar))
                            {
                                numberText += firstChar;
                            }

                            order = order.Substring(1);
                        }

                        numberText = numberText.Trim();
                        var numberValue = int.Parse(numberText);
                        numberText = Propis(numberValue).Trim();
                        numberText = char.ToUpperInvariant(numberText.FirstChar())
                                     + numberText.Substring(1);
                        order = numberText + " " + order;
                    }

                    Match match = fioRegex.Match(order);
                    if (match.Success)
                    {
                        var length = match.Value.Length;
                        order = order.Substring(length).TrimStart();
                    }

                    //item.Order = RichText.Decode(order);
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
            MarcRecord record = null;

            try
            {
                BiblioProcessor processor = context.Processor
                    .ThrowIfNull("context.Processor");
                using (IPftFormatter formatter
                    = processor.AcquireFormatter(context))
                {
                    IrbisProvider provider = context.Provider;
                    RecordCollection records = Records
                        .ThrowIfNull("Records");

                    string searchExpression = SearchExpression
                        .ThrowIfNull("SearchExpression");
                    formatter.ParseProgram(searchExpression);
                    record = new MarcRecord();
                    searchExpression = formatter.FormatRecord(record);

                    int[] found = provider.Search(searchExpression);
                    log.WriteLine("Found: {0} record(s)", found.Length);

                    log.Write("Reading records");

                    // Пробуем не загружать записи,
                    // а предоставить заглушки

                    for (int i = 0; i < found.Length; i++)
                    {
                        log.Write(".");
                        record = new MarcRecord
                        {
                            Mfn = found[i]
                        };
                        records.Add(record);
                        context.Records.Add(record);
                    }

                    log.WriteLine(" done");
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.GatherRecords(context);
                }

            }
            catch (Exception exception)
            {
                string message = string.Format
                    (
                        "Exception: {0}",
                        exception
                    );

                log.WriteLine(message);
                throw;
            }

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

            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");


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
                    string description = item.Description
                        .ThrowIfNull("item.Description");

                    ReportBand band = new ParagraphBand
                        (
                            number.ToInvariantString() + ") "
                        );
                    report.Body.Add(band);
                    band.Cells.Add(new SimpleTextCell(
                            description
                            //RichText.Encode3(description, UnicodeRange.Russian, "\\f2")
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

        #endregion
    }
}
