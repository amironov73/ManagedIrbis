using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplectList
{
    class ComplectInfo
        // : IComparable<ComplectInfo>
    {
        #region Properties

        public string Title { get; set; }

        public string Year { get; set; }

        public string Issue { get; set; }

        public string Index { get; set; }

        #endregion

        //#region IComparable<T> members

        //public int CompareTo(ComplectInfo other)
        //{
        //    return string.Compare(this.Year, other.Year, StringComparison.OrdinalIgnoreCase);
        //}

        //#endregion
    }
}
