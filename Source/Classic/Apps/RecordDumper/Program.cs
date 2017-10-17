// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Json;
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;
using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace RecordDumper
{
    class Program
    {
        private const string FirstWidth = "1.5cm";
        private const string SecondWidth = "5cm";
        private const string ThirdWidth = "15cm";

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine
                    (
                        "RecordDumper <connection-string> "
                        + "<search-expression> <output-file>"
                    );

                return;
            }

            string connectionString = args[0];
            string searchExpression = args[1];
            string outputFileName = args[2];

            try
            {
                JObject config
                    = JsonUtility.ReadObjectFromFile("dumperConfig.json");
                int etalonTag = config["etalonTag"].Value<int>();
                int[] excludeTags = config["excludeTags"]
                    .Values<int>().ToArray();
                string[] excludePages = config["excludePages"]
                    .Values<string>().ToArray();

                using (IrbisProvider provider = ProviderManager
                    .GetAndConfigureProvider(connectionString))
                using (StreamWriter writer = TextWriterUtility.Create
                    (
                        outputFileName,
                        IrbisEncoding.Utf8
                    ))
                {
                    FileSpecification specification = new FileSpecification
                        (
                            IrbisPath.MasterFile,
                            provider.Database,
                            "WS31.OPT"
                        );
                    IrbisOpt opt = IrbisOpt.LoadFromServer
                        (
                            provider,
                            specification
                        );
                    if (ReferenceEquals(opt, null))
                    {
                        throw new IrbisException("Can't load OPT file!");
                    }

                    int[] found = provider.Search(searchExpression);
                    Console.WriteLine("Found: {0}", found.Length);

                    foreach (int mfn in found)
                    {
                        MarcRecord record = provider.ReadRecord(mfn);
                        if (ReferenceEquals(record, null))
                        {
                            continue;
                        }
                        string title = record.FM(etalonTag);
                        if (string.IsNullOrEmpty(title))
                        {
                            continue;
                        }

                        string wsName = opt.SelectWorksheet(opt.GetWorksheet(record));
                        if (string.IsNullOrEmpty(wsName))
                        {
                            continue;
                        }
                        specification = new FileSpecification
                            (
                                IrbisPath.MasterFile,
                                provider.Database,
                                wsName + ".ws"
                            );
                        WsFile worksheet = WsFile.ReadFromServer
                            (
                                provider,
                                specification
                            );
                        if (ReferenceEquals(worksheet, null))
                        {
                            continue;
                        }

                        Console.WriteLine("MFN={0}: {1}", mfn, title);
                        writer.WriteLine("<h3>{0}</h3>", title);

                        string description = provider.FormatRecord(record, "@");
                        writer.WriteLine(description);

                        RecordField[] fields = record.Fields
                            .Where(field => !field.Tag.OneOf(excludeTags))
                            .ToArray();

                        writer.WriteLine
                            (
                                "<table border=\"1\" "
                                + "cellpadding=\"3\" "
                                + "cellspacing=\"0\" "
                                +">"
                            );
                        writer.WriteLine
                            (
                                "<tr bgcolor=\"black\">"
                                + "<th style=\"color:white;text-align:left;\">Поле</th>"
                                + "<th style=\"color:white;text-align:left;\">Подполе</th>"
                                + "<th style=\"color:white;text-align:left;\">Значение</th>"
                                + "</tr>"
                            );
                        foreach (WorksheetPage page in worksheet.Pages)
                        {
                            if (page.Name.OneOf(excludePages))
                            {
                                continue;
                            }

                            int[] tags = page.Items
                                .Select(item => item.Tag)
                                .Select(NumericUtility.ParseInt32)
                                .ToArray();
                            if (!fields.Any(field => field.Tag.OneOf(tags)))
                            {
                                continue;
                            }
                            writer.WriteLine
                                (
                                    "<tr><td colspan=\"3\"><b>Вкладка «{0}»</b></td></tr>",
                                    page.Name
                                );

                            foreach (WorksheetItem item in page.Items)
                            {
                                if (string.IsNullOrEmpty(item.Tag))
                                {
                                    continue;
                                }

                                int tag = NumericUtility.ParseInt32(item.Tag);
                                if (tag <= 0)
                                {
                                    continue;
                                }

                                RecordField[] itemFields = fields.GetField(tag);
                                for (int i = 0; i < itemFields.Length; i++)
                                {
                                    RecordField field = itemFields[i];
                                    title = item.Title;
                                    title = StringUtility.Sparse(title);
                                    if (i != 0)
                                    {
                                        title = string.Format
                                            (
                                                "(повторение {0})",
                                                i + 1
                                            );
                                    }
                                    int rowspan = 1;
                                    if (string.IsNullOrEmpty(field.Value))
                                    {
                                        rowspan = 1 + field.SubFields.Count;
                                    }
                                    writer.WriteLine
                                        (
                                            "<tr><td rowspan=\"{0}\" \"width=\"{1}\"><b>{2}</b></td><td colspan=\"2\"><b>{3}</b></td></tr>",
                                            rowspan,
                                            FirstWidth,
                                            field.Tag,
                                            HtmlText.Encode(title)
                                        );

                                    if (!string.IsNullOrEmpty(field.Value))
                                    {
                                        writer.WriteLine
                                            (
                                                "<tr><td colspan=\"2\">&nbsp;</td><td>{0}</td></tr>",
                                                HtmlText.Encode(field.Value)
                                            );
                                    }

                                    if (item.EditMode == "5")
                                    {
                                        string inputInfo = item.InputInfo
                                            .ThrowIfNull("item.InputInfo");

                                        // Поле с подполями
                                        specification = new FileSpecification
                                            (
                                                IrbisPath.MasterFile,
                                                provider.Database,
                                                inputInfo
                                            );
                                        WssFile wss = WssFile.ReadFromServer
                                            (
                                                provider,
                                                specification
                                            );
                                        if (ReferenceEquals(wss, null))
                                        {
                                            Console.WriteLine
                                                (
                                                    "Can't load: " + inputInfo
                                                );
                                        }
                                        else
                                        {
                                            foreach (WorksheetItem line in wss.Items)
                                            {
                                                char code = line.Tag.FirstChar();
                                                SubField subField = field.GetFirstSubField(code);
                                                if (!ReferenceEquals(subField, null))
                                                {
                                                    writer.WriteLine
                                                        (
                                                            "<tr>"
                                                            + "<td width=\"{0}\"><b>{1}</b>: {2}</td>"
                                                            + "<td width=\"{3}\">{4}</td></tr>",
                                                            SecondWidth,
                                                            CharUtility.ToUpperInvariant(code),
                                                            HtmlText.Encode(line.Title),
                                                            ThirdWidth,
                                                            HtmlText.Encode(subField.Value)
                                                        );
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        writer.WriteLine("</table>");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
