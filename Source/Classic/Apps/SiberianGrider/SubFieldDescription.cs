/* SubFieldDescription.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace SiberianGrider
{
    /// <summary>
    /// Subfield description.
    /// </summary>
    public class SubFieldDescription
    {
        #region Properties

        /// <summary>
        /// Title of the subfield.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Value of the subfield.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format
                (
                    "Title: {0}, Value: {1}", 
                    Title, 
                    Value
                );
        }

        #endregion
    }
}
