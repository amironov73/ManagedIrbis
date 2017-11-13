using System.ComponentModel;

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
        public int Int32Property { get; set; }

        public bool BooleanProperty { get; set; }

        public string StringProperty { get; set; }

        #endregion

        #region Methods

        public void MethodOne()
        {
            // Nothing to do here
        }

        #endregion
    }
}
