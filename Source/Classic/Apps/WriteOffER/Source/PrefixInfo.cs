using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteOffER
{
    class PrefixInfo
    {
        public string Prefix { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}
