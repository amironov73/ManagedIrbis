using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchRuntimeVersion
{
    class Program
    {
        private static readonly string[] runtimeVersions =
        {
            "1.0",   "v1.0.3705",
            "1.1",   "v1.1.4322",
            "2.0",   "v2.0.50727",
            "3.0",   "v2.0.50727",
            "3.5",   "v2.0.50727",
            "4.0",   "v4.0",
            "4.0.1", "v4.0",
            "4.0.2", "v4.0",
            "4.0.3", "v4.0",
            "4.5",   "v4.0",
            "4.5.1", "v4.0",
            "4.5.2", "v4.0",
            "4.6",   "v4.0",
            "4.6.1", "v4.0",
            "4.7",   "v4.0",
            "4.7.1", "v4.0"
        };

        private static readonly string[] skuIds =
        {
            "4.0",   ".NETFramework,Version=v4.0",
            "4.0.1", ".NETFramework,Version=v4.0.1",
            "4.0.2", ".NETFramework,Version=v4.0.2",
            "4.0.3", ".NETFramework,Version=v4.0.3",
            "4.5",   ".NETFramework,Version=v4.5",
            "4.5.1", ".NETFramework,Version=v4.5.1",
            "4.5.2", ".NETFramework,Version=v4.5.2",
            "4.6",   ".NETFramework,Version=v4.6",
            "4.6.1", ".NETFramework,Version=v4.6.1",
            "4.7",   ".NETFramework,Version=v4.7",
            "4.7.1", ".NETFramework,Version=v4.7.1"
        };

        static void ProcessFile
            (
                string fileName,
                string runtimeValue,
                string skuValue
            )
        {
            Console.WriteLine(fileName);
            XDocument document = XDocument.Load(fileName);
            XElement element = document.XPathSelectElement("/configuration/startup/supportedRuntime");
            if (ReferenceEquals(element, null))
            {
                return;
            }

            XAttribute version = element.Attribute("version")
                ?? new XAttribute("version", runtimeValue);
            version.Value = runtimeValue;

            XAttribute sku = element.Attribute("sku");
            if (string.IsNullOrEmpty(skuValue))
            {
                if (!ReferenceEquals(sku, null))
                {
                    sku.Remove();
                }
            }
            else
            {
                if (ReferenceEquals(sku, null))
                {
                    sku = new XAttribute("sku", skuValue);
                }
                sku.Value = skuValue;
            }

            document.Save(fileName);
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("PatchRuntimeVersion <file.config> <version>");

                return;
            }

            string ver = args[1];
            string foundRuntime = null;
            for (int i = 0; i < runtimeVersions.Length; i += 2)
            {
                if (runtimeVersions[i] == ver)
                {
                    foundRuntime = runtimeVersions[i + 1];
                    break;
                }
            }

            if (string.IsNullOrEmpty(foundRuntime))
            {
                Console.WriteLine("Unknown version: {0}", ver);
                return;
            }

            string foundSku = null;
            if (foundRuntime == "v4.0")
            {
                for (int i = 0; i < skuIds.Length; i += 2)
                {
                    if (skuIds[i] == ver)
                    {
                        foundSku = skuIds[i + 1];
                        break;
                    }
                }
            }

            string directory = Path.GetDirectoryName(args[0]) ?? ".";
            string pattern = Path.GetFileName(args[0]) ?? "*.exe.config";
            string[] files = Directory.GetFiles(directory, pattern);
            foreach (string file in files)
            {
                ProcessFile(file, foundRuntime, foundSku);
            }
        }
    }
}
