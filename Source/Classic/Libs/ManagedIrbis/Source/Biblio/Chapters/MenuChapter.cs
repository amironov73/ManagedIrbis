// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuChapter.cs -- 
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

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
    public class MenuChapter
        : BiblioChapter
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("format")]
        public string Format { get; set; }

        /// <summary>
        /// Leaf nodes only can contain records.
        /// </summary>
        [JsonProperty("leafOnly")]
        public bool LeafOnly { get; set; }

        /// <summary>
        /// Menu name.
        /// </summary>
        [CanBeNull]
        [JsonProperty("menuName")]
        public string MenuName { get; set; }

        /// <summary>
        /// Order.
        /// </summary>
        [CanBeNull]
        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        /// <summary>
        /// Record selector.
        /// </summary>
        [CanBeNull]
        [JsonProperty("recordSelector")]
        public string RecordSelector { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [CanBeNull]
        [JsonProperty("searchExpression")]
        public string SearchExpression { get; set; }

        /// <summary>
        /// Title format.
        /// </summary>
        [CanBeNull]
        [JsonProperty("titleFormat")]
        public string TitleFormat { get; set; }

        /// <summary>
        /// Special settings for some chapters.
        /// </summary>
        [NotNull]
        [JsonProperty("specialSettings")]
        public List<SpecialSettings> SpecialSettings { get; private set; }

        /// <summary>
        /// Formatter.
        /// </summary>
        [CanBeNull]
        public PftFormatter Formatter { get; private set; }

        /// <summary>
        /// Records.
        /// </summary>
        [CanBeNull]
        public List<MarcRecord> Records { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuChapter()
        {
            SpecialSettings = new List<SpecialSettings>();
        }

        #endregion

        #region Private members

        private MenuSubChapter _CreateChapter
            (
                [NotNull] IrbisTreeFile.Item item
            )
        {
            string key = item.Prefix;
            SpecialSettings settings = SpecialSettings.FirstOrDefault
                (
                    s => s.Name == key
                );
            string value = item.Suffix;

            MarcRecord record = new MarcRecord();
            record.Fields.Add(new RecordField(1, key));
            record.Fields.Add(new RecordField(2, value));
            string title = Formatter
                .ThrowIfNull("Formatter")
                .Format(record);

            MenuSubChapter result = new MenuSubChapter
            {
                Key = key,
                MainChapter = this,
                Title = title,
                Value = value,
                SpecialSettings = settings
            };

            foreach (IrbisTreeFile.Item child in item.Children)
            {
                MenuSubChapter subChapter = _CreateChapter(child);
                result.Children.Add(subChapter);
            }

            return result;
        }

        #endregion

        #region Public methods

        #endregion

        #region BiblioChapter members

        /// <inheritdoc cref="BiblioChapter.GatherRecords" />
        public override void GatherRecords
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine
                (
                    "Begin gather records {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );

            try
            {
                IrbisProvider provider = context.Provider;
                string searchExpression = SearchExpression
                    .ThrowIfNull("SearchExpression");

                PftFormatter formatter = Formatter
                    .ThrowIfNull("Formatter");
                formatter.ParseProgram(searchExpression);
                MarcRecord record = new MarcRecord();
                searchExpression = formatter.Format(record);

                int[] found = provider.Search(searchExpression);
                log.WriteLine("Found: {0} record(s)", found.Length);

                log.Write("Reading records");
                Records = new List<MarcRecord>();
                for (int i = 0; i < found.Length; i++)
                {
                    if (i % 100 == 0)
                    {
                        log.Write(".");
                    }
                    record = provider.ReadRecord(found[i]);
                    Records.Add(record);
                }
                log.WriteLine(" done");

                string recordSelector = RecordSelector
                    .ThrowIfNull("RecordSelector");
                formatter.ParseProgram(recordSelector);

                foreach (BiblioChapter child in Children)
                {
                    child.GatherRecords(context);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine
                (
                    "End gather records {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        /// <inheritdoc cref="BiblioChapter.Initialize" />
        public override void Initialize
            (
                BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
            try
            {
                string menuName = MenuName.ThrowIfNull("MenuName");

                IrbisProvider provider = context.Provider;

                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        provider.Database,
                        menuName
                    );
                MenuFile menu = provider.ReadMenuFile(specification);
                if (ReferenceEquals(menu, null))
                {
                    throw new IrbisException();
                }
                IrbisTreeFile tree = menu.ToTree();
                PftContext pftContext = new PftContext(null);
                PftFormatter formatter = new PftFormatter(pftContext);
                formatter.SetProvider(provider);
                Formatter = formatter;
                string titleFormat = TitleFormat
                    .ThrowIfNull("TitleFormat");
                formatter.ParseProgram(titleFormat);
                foreach (IrbisTreeFile.Item root in tree.Roots)
                {
                    MenuSubChapter chapter = _CreateChapter(root);
                    Children.Add(chapter);
                }

                foreach (BiblioChapter chapter in Children)
                {
                    chapter.Initialize(context);
                }
            }
            catch (Exception exception)
            {
                log.WriteLine("Exception: {0}", exception);
                throw;
            }

            log.WriteLine
                (
                    "End initialize {0}: {1}",
                    GetType().Name,
                    Title.ToVisibleString()
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}
