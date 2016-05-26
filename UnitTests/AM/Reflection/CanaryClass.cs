using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.AM.Reflection
{
    class CanaryClass
    {
        #region Fields

        public int Int32Field;

        public bool BooleanField;

        public string StringField;

        #endregion

        #region Properties

        public int Int32Property { get; set; }

        public bool BooleanProperty { get; set; }

        public string StringProperty { get; set; }

        #endregion
    }
}
