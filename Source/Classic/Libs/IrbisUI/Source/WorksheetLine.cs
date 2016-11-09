/* WorksheetLine.cs -- single line in the worksheet
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

using AM;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Worksheet;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace IrbisUI.Sources
{
    /// <summary>
    /// Single line in the worksheet.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class WorksheetLine
    {
        #region Properties

        /// <summary>
        /// Schema.
        /// </summary>
        [CanBeNull]
        public WorksheetItem Schema { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        [CanBeNull]
        public string Title { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        #endregion
    }
}
