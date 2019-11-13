using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ManagedIrbis;

namespace Chronicle
{
    class Entry
    {
        public int Index;

        public int Mfn;

        public string Description;

        public string Cleaned;

        public string Udc, Bbk;

        public string TranslatedUdc;

        public string Language;

        public MarcRecord Record;

        public List<string> Authors = new List<string>();
        public List<string> Collectives = new List<string>();
        public List<string> Geo = new List<string>();
        public List<string> Isbn = new List<string>();
        public List<string> Publishers = new List<string>();
        public List<string> Series = new List<string>();

        public override string ToString()
        {
            return $"{nameof(Index)}: {Index}, {nameof(Mfn)}: {Mfn}, {nameof(Cleaned)}: {Cleaned}";
        }
    }
}
