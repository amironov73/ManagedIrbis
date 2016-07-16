/* SubFieldInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace ManagedClient.Mapping
{
    /// <summary>
    /// Информация о маппинге.
    /// </summary>
    public sealed class SubFieldInfo
    {
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Повторение подполя
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Метод-маппер.
        /// </summary>
        public Func<SubField, object> Mapper { get; set; }

        #endregion
    }
}
