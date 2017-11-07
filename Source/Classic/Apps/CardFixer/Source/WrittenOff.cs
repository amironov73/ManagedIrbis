/* WrittenOff.cs
 */

#region Using directives

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using JetBrains.Annotations;

#endregion

namespace CardFixer
{
    [PublicAPI]
    public class WrittenOff
    {
        #region Properties

        [PrimaryKey]
        [MapField("number")]
        public string Number { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Public methods

        #endregion

        #region Object members

        public override string ToString()
        {
            return Number;
        }

        #endregion
    }
}
