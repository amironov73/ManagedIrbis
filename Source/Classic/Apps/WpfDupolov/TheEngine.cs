using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;
using ManagedIrbis.Search;

namespace WpfDupolov
{
    class TheEngine
    {
        public MainWindow Window { get; private set; }

        public IIrbisConnection Connection { get; private set; }

        public string Prefix { get; private set; }

        public string CurrentItem { get; private set; }

        public bool Cancel { get; set; }

        public bool NeedDump { get; set; }

        // Поля, остающиеся в записи
        private static readonly int[] ResiduaryTags = { 907, 999 };

        public TheEngine
            (
                MainWindow window,
                IIrbisConnection connection,
                string prefix
            )
        {
            Window = window;
            Connection = connection;
            Prefix = prefix;
        }

        public void AppendLog(string format, params object[] args)
        {
            string line = string.Format(format, args);
            Window.AppendLog(line);
        }

        public void Process()
        {
            string database = Connection.Database;

            AppendLog("База данных: {0}, префикс: {1}", database, Prefix);

            TermParameters parameters = new TermParameters
            {
                Database = database,
                StartTerm = Prefix,
                NumberOfTerms = 1000
            };

            bool first = true;
            while (true)
            {
                if (Cancel)
                {
                    AppendLog("Пользователь прервал выполнение");
                    break;
                }

                TermInfo[] terms = Connection.ReadTerms(parameters);
                if (terms.Length == 0)
                {
                    break;
                }

                Window.SetStatus(terms[0].Text);

                int start = first ? 0 : 1;
                for (int i = start; i < terms.Length; i++)
                {
                    if (Cancel)
                    {
                        break;
                    }

                    TermInfo term = terms[i];
                    if (!term.Text.SafeStarts(Prefix))
                    {
                        break;
                    }
                    if (term.Count > 1)
                    {
                        DumpTerm(term.Text);
                    }
                }

                if (terms.Length < 2)
                {
                    break;
                }
                string lastTerm = terms.Last().Text;
                if (!lastTerm.SafeStarts(Prefix))
                {
                    break;
                }
                parameters.StartTerm = lastTerm;
                first = false;
            }

            AppendLog("Отключение от сервера");
            Connection.Dispose();
        }

        private void DumpTerm
            (
                string termText
            )
        {
            if (termText.SameString(Prefix))
            {
                return;
            }

            string expression = "\"" + termText + "\"";
            int count;

            try
            {
                count = Connection.SearchCount(expression);
            }
            catch (Exception ex)
            {
                AppendLog("Сбой при поиске: {0}", expression);
                AppendLog(ex.Message);
                return;
            }

            if (count > 3)
            {
                AppendLog
                    (
                        "{0} МНОГО ЗАПИСЕЙ: {1}",
                        termText,
                        count
                    );
                return;
            }

            if (count <= 1)
            {
                return;
            }

            MarcRecord[] records = Connection.SearchRead(expression);
            AppendLog
                (
                    "{0} MFN={1}",
                    termText,
                    StringUtility.Join(", ", records.Select(r => r.Mfn))
                );

            if (NeedDump)
            {
                for (int i = 1; i < records.Length; i++)
                {
                    FieldDifference[] diff = RecordComparator.FindDifference2
                        (
                            records[0],
                            records[i],
                            ResiduaryTags
                        )
                        .ToArray();

                    FieldDifference[] modified = diff
                        .Where(line => line.State != FieldState.Unchanged)
                        .ToArray();

                    if (modified.Length == 0)
                    {
                        AppendLog("Различий между записями нет");
                    }

                    foreach (FieldDifference line in diff)
                    {
                        AppendLog(line.ToString());
                    }

                    AppendLog(string.Empty);
                }

                AppendLog(new string('=', 70));
                AppendLog(string.Empty);
            }
        }
    }
}
