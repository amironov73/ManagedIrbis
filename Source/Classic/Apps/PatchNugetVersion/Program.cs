using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;


namespace PatchNugetVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                return;
            }

            string assemblyPath = Path.GetFullPath(args[0]);

            Assembly assembly = Assembly.LoadFile (assemblyPath);
            Version version = assembly.GetName().Version;
            string newVersion = string.Format
                (
                    "<version>{0}</version>",
                    version
                );

            string text = File.ReadAllText(args[1]);
            Regex regex = new Regex(@"<version>.+?</version>");
            text = regex.Replace(text, newVersion);
            File.WriteAllText(args[1],text);
        }
    }
}
