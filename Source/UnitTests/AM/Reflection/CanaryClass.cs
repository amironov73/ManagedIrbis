using System.ComponentModel;
using AM.Reflection;

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

        [DisplayName("CanaryProperty")]
        [MemberOrder(1)]
        public int Int32Property { get; set; }

        [MemberOrder(2)]
        public bool BooleanProperty { get; set; }

        [MemberOrder(3)]
        public string StringProperty { get; set; }

        [MemberOrder(4)]
        public int NoSetterProperty { get { return 123; } }

        #endregion

        #region Methods

        public void MethodOne()
        {
            // Nothing to do here
        }

        #endregion
    }
}
