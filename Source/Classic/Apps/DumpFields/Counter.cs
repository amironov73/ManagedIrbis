using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.Collections;

namespace DumpFields
{
    class Info
    {
        public int N { get; set; }
        public string Title { get; set; }
        public double Years { get; set; }
        public int ExemplarCount { get; set; }
        public int LoanCount { get; set; }
        //public double MeanLoan { get; set; }
    }

    class Counter
    {
        #region Properties

        public string Name { get; private set; }

        #endregion

        #region Construction

        public Counter(string name)
        {
            Name = name;
            _dictionary = new CaseInsensitiveDictionary<Info>();
        }

        #endregion


        #region Private members

        private readonly Dictionary<string, Info> _dictionary;

        #endregion

        #region Public members

        public void Add
            (
                string title,
                int exemplarCount,
                int loanCount,
                double years//,
                //double meanLoan
            )
        {
            if (!_dictionary.TryGetValue(title, out var info))
            {
                info = new Info { Title = title };
                _dictionary.Add(title, info);
            }

            info.N++;
            info.Years = Math.Max(info.Years, years);
            info.ExemplarCount += exemplarCount;
            info.LoanCount += loanCount;
            //info.MeanLoan += meanLoan;
        }

        public Info Get(string title)
        {
            _dictionary.TryGetValue(title, out var result);
            return result;
        }

        public string[] Keys()
        {
            string[] result = _dictionary.Keys.ToArray();
            Array.Sort(result);
            return result;
        }

        public void Save()
        {
            Console.WriteLine(Name);
            using (StreamWriter writer = File.CreateText(Name + ".txt"))
            {
                string[] keys = Keys();
                foreach (string key in keys)
                {
                    Info info = Get(key);
                    double mean = (((double)info.LoanCount) / info.ExemplarCount) / info.Years;
                    writer.WriteLine($"{key}\t{info.N}\t{info.ExemplarCount}\t{info.Years}\t{info.LoanCount}\t{mean}");
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
