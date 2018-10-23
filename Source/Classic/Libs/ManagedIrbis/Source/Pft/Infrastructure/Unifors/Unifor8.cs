// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor8.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Search;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    // Неописанный unifor 8
    // Формат (передаваемая строка):
    //
    // 8<dbn>,<@mfn|/termin/>,<fst>,<tag>,<teq>
    //
    // Передаются пять параметров, разделенные запятой:
    // Первый – имя БД;
    // Второй – или непосредственно MFN с предшествующим
    // символом @ или термин, ссылающийся на документ
    // (термин – заключается в ограничительные символы);
    // Третий – имя FST (IFS не поддерживается)
    // Четвертый - тег из FST
    // Пятый метод индексирования
    // Читает FST, ищет строки с указанным тегом и методом,
    // расформатирует найденную запись
    //

    static class Unifor8
    {
        #region Public methods

        public static void FormatWithFst
            (
                [NotNull] PftContext context,
                [CanBeNull] PftNode node,
                [CanBeNull] string expression
            )
        {

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            IrbisProvider provider = context.Provider;
            string[] parts = StringUtility.SplitString
                (
                    expression,
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            string database = parts[0];
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            if (string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            int mfn = 0;
            string query = null;
            if (parts[1].StartsWith("@"))
            {
                parts = StringUtility.SplitString
                    (
                        parts[1],
                        CommonSeparators.Comma,
                        4
                    );
                string mfnText = parts[0].Substring(1);
                if (!NumericUtility.TryParseInt32(mfnText, out mfn)
                    || mfn <= 0)
                {
                    return;
                }
            }
            else
            {
                string separator = parts[1].Substring(0, 1);
                if (string.IsNullOrEmpty(separator))
                {
                    return;
                }
                int index = parts[1].IndexOf
                    (
                        separator,
                        1
#if !UAP
                        StringComparison.InvariantCulture
#endif
                    );
                if (index < 0)
                {
                    return;
                }

                query = parts[1].Substring(1, index - 1); //-V3057
                parts = StringUtility.SplitString
                    (
                        parts[1].Substring(index + 1),
                        CommonSeparators.Comma,
                        4
                    );
            }

            if (parts.Length != 4)
            {
                return;
            }

            string fstName = parts[1];
            // если FST не задана, берем имя БД
            if (string.IsNullOrEmpty(fstName))
            {
                fstName = provider.Database;
            }
            string tagStr = parts[2];
            string methodStr = parts[3];
            // тег может быть пустым, не числом или 0, значит по тегу не фильтровать
            int tag = tagStr.SafeToInt32();
            int method;

            // метод может быть 0
            if (!NumericUtility.TryParseInt32(methodStr, out method)
                || method < 0)
            {
                return;
            }

            string saveDatabase = provider.Database;
            provider.Database = database;

            try
            {
                if (mfn == 0)
                {
                    TermParameters parameters = new TermParameters
                    {
                        StartTerm = query.TrimEnd('$'),
                        NumberOfTerms = 1
                    };
                    TermInfo[] terms = provider.ReadTerms(parameters);
                    if (terms.Length == 0)
                    {
                        return;
                    }

                    TermLink[] postings = provider.ExactSearchLinks(terms[0].Text);
                    if (postings.Length == 0)
                    {
                        return;
                    }

                    mfn = postings[0].Mfn;
                }

                if (!fstName.Contains("."))
                {
                    fstName += ".FST";
                }

                MarcRecord record = provider.ReadRecord(mfn);
                if (ReferenceEquals(record, null))
                {
                    return;
                }

                FileSpecification specification = new FileSpecification
                {
                    Database = database,
                    Path = IrbisPath.InternalResource,
                    FileName = fstName
                };
                string fstContent = provider.ReadFile(specification);
                if (string.IsNullOrEmpty(fstContent))
                {
                    return;
                }

                // разбор FST напрямую без чтения вложенных файлов и поддержки формата IFS
                string[] lines = fstContent.SplitLines();
                List<string> formatLines = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    parts = StringUtility.SplitString
                        (
                            line.TrimStart(),
                            CommonSeparators.SpaceOrTab,
                            3
                        );
                    if (parts.Length != 3)
                    {
                        continue;
                    }

                    // ищем строки, соответствующие указанным тегу и технике индексирования
                    int lineTag, lineMethod;
                    if (!NumericUtility.TryParseInt32(parts[0], out lineTag)
                        || !NumericUtility.TryParseInt32(parts[1], out lineMethod))
                    {
                        continue;
                    }

                    if (tag > 0 && lineTag != tag || lineMethod != method)
                    {
                        continue;
                    }
                    formatLines.Add(parts[2]);
                }

                if (formatLines.Count == 0)
                {
                    return;
                }

                // после вызова этого unifor в главном контексте сбрасываются флаги постобработки
                context.GetRootContext().PostProcessing = PftCleanup.None;

                StringBuilder builder = new StringBuilder();
                List<string> seen = new List<string>();
                for (int i = 0; i < formatLines.Count; i++)
                {
                    using (PftContextGuard guard = new PftContextGuard(context))
                    {
                        // формат вызывается в контексте без повторений
                        // делаем аналогично RepGroup
                        // создаем копию контекста со ссылкой на тот же буфер
                        // в копии сбрасываем состояние повторяющейся группы и работаем через него
                        // текстовый буфер восстанавливаем, так как он один и тот же
                        PftContext nestedContext = guard.ChildContext;
                        nestedContext.Record = record;
                        nestedContext.Reset();
                        string format = formatLines[i];
                        PftProgram program = PftUtility.CompileProgram(format);
                        program.Execute(nestedContext);
                        string formatted = nestedContext.Text;
                        formatted = formatted.Trim(CommonSeparators.NewLineAndPercent);
                        string[] subLines = StringUtility.SplitString
                            (
                                formatted,
                                CommonSeparators.NewLineAndPercent,
                                StringSplitOptions.RemoveEmptyEntries
                            );
                        foreach (string subLine in subLines)
                        {
                            if (seen.Contains(subLine))
                            {
                                continue;
                            }

                            builder.AppendLine();
                            seen.Add(subLine);
                            builder.Append(subLine);
                        }
                    }
                }

                context.WriteAndSetFlag(node, builder.ToString());

            }
            finally
            {
                context.Provider.Database = saveDatabase;
            }
        }

        #endregion
    }
}
