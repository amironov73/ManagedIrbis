using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM;
using ManagedIrbis;

namespace Dundee
{
    static class Utility
    {
        class IndexComparer : IComparer<string>
        {
            static string Trim(string x)
            {
                int index = x.IndexOf('\t');
                if (index >= 0)
                {
                    x = x.Substring(0, index);
                }

                return x;
            }

            public int Compare(string x, string y)
            {
                x = Trim(x);
                y = Trim(y);
                return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public static IrbisConnection GetConnection()
        {
            return Program.Connection.ThrowIfNull("connection");
        }

        public static string ConvertMfnToIndex(int mfn)
        {
            return GetConnection().FormatRecord("v903", mfn);
        }

        public static int[] CountLoans(string index)
        {
            string[] files = Directory.GetFiles("/Archive", "*.txt", SearchOption.TopDirectoryOnly);
            Array.Sort(files);
            List<int> result = new List<int>(files.Length);
            foreach (string file in files)
            {
                result.Add(CountLoans(file, index));
            }

            while (result.Count != 0)
            {
                if (result[0] != 0)
                {
                    break;
                }

                result.RemoveAt(0);
            }

            return result.ToArray();
        }

        static int CountLoans(string fileName, string index)
        {
            string[] lines = File.ReadAllLines(fileName);
            IndexComparer comparer = new IndexComparer();
            int found = Array.BinarySearch(lines, index, comparer);
            if (found < 0)
            {
                return 0;
            }

            string line = lines[found];
            int i = line.IndexOf('\t');
            line = line.Substring(i+1);

            return FastNumber.ParseInt32(line);
        }
    }
}
