using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAdmin
{
    public sealed class DatabaseDescription
    {
        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public int MaxMfn { get; set; }

        public bool ExclusiveLock { get; set; }

        public int LockedRecords { get; set; }

        public int LogicallyDeleted { get; set; }

        public int PhysicallyDeleted { get; set; }

        public int NonactualizedRecords { get; set; }

        #endregion
    }
}
