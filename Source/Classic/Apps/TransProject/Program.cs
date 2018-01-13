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
using System.IO;
using System.Xml.Linq;

#endregion

// ReSharper disable UseNullPropagation
// ReSharper disable ForCanBeConvertedToForeach

namespace TransProject
{
    //
    // Пересаживает файлы исходников из одного проекта в другой.
    // При необходимости может ещё добавлять префикс пути к каждому файлу.
    // Область для пересадки сидит между комментариями
    // <!-- BEGIN --> и <!-- END -->
    //

    class Program
    {
        private const string BeginText = "<!-- BEGIN -->";
        private const string EndText = "<!-- END -->";

        private static string _sourceName, _targetName, _prefix;

        static int _FindLine(string[] array, string textToFind)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(textToFind))
                {
                    return i;
                }
            }

            return -1;
        }

        static string[] _ExtractLines(string[] array, int start, int end)
        {
            List<string> result = new List<string>();
            for (int i = start; i <= end; i++)
            {
                result.Add(array[i]);
            }

            return result.ToArray();
        }

        static string[] _ProcessLines(string[] source, string[] target, int start, int end)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < start; i++)
            {
                result.Add(target[i]);
            }

            for (int i = 0; i < source.Length; i++)
            {
                string line = source[i];
                if (!string.IsNullOrEmpty(_prefix)
                    && line.Contains("<Compile Include="))
                {
                    //
                    // Внимание! Корректно обрабатываются только строки вида
                    // <Compile Include="Program.cs" />
                    // <Compile Include="Properties\AssemblyInfo.cs" />
                    //

                    int indent = 0;
                    while (line[indent] == ' ')
                    {
                        indent++;
                    }

                    XDocument xdoc = XDocument.Parse(line);
                    XElement compile = xdoc.Element("Compile");
                    if (!ReferenceEquals(compile, null))
                    {
                        XAttribute include = compile.Attribute("Include");
                        if (!ReferenceEquals(include, null))
                        {
                            string path = include.Value;
                            XElement link = new XElement("Link", path);
                            compile.Add(link);
                            include.Value = _prefix + path;

                            string output = xdoc.ToString();
                            string[] lines = output.Split('\r', '\n');
                            foreach (string s in lines)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    line = new string(' ', indent) + s;
                                    result.Add(line);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result.Add(line);
                }
            }

            for (int i = end + 1; i < target.Length; i++)
            {
                result.Add(target[i]);
            }

            return result.ToArray();
        }

        // ===================================================================

        static int Main(string[] args)
        {
            if (args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("TransProject <source.csproj> <target.csproj> [prefix]");

                return -1;
            }

            _sourceName = args[0];
            _targetName = args[1];
            _prefix = null;
            if (args.Length > 2)
            {
                _prefix = args[2];
            }

            try
            {
                // Считываем файл-донор

                string[] sourceLines = File.ReadAllLines(_sourceName);
                int startLine = _FindLine(sourceLines, BeginText);
                if (startLine < 0)
                {
                    Console.WriteLine("Bad source file");
                    return -1;
                }

                int endLine = _FindLine(sourceLines, EndText);
                if (endLine < 0)
                {
                    Console.WriteLine("Bad source file");
                    return -1;
                }

                if (endLine < startLine)
                {
                    Console.WriteLine("Bad source file");
                    return -1;
                }

                sourceLines = _ExtractLines(sourceLines, startLine, endLine);

                // Считываем файл-акцептор

                string[] targetLines = File.ReadAllLines(_targetName);
                startLine = _FindLine(targetLines, BeginText);
                if (startLine < 0)
                {
                    Console.WriteLine("Bad target file");
                    return -1;
                }

                endLine = _FindLine(targetLines, EndText);
                if (endLine < 0)
                {
                    Console.WriteLine("Bad target file");
                    return -1;
                }

                if (endLine < startLine)
                {
                    Console.WriteLine("Bad target file");
                    return -1;
                }

                // Делаем пересадку

                targetLines = _ProcessLines(sourceLines, targetLines, startLine, endLine);
                File.WriteAllLines(_targetName, targetLines);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                return -1;
            }

            return 0;
        }
    }
}
