/* MarcRecordUtility.cs -- extensions for MarcRecord
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Extension methods for <see cref="MarcRecord"/>.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class MarcRecordUtility
    {
        #region Constants

        #endregion

        #region Properties
        
        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert the <see cref="MarcRecord"/> to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string result = JObject.FromObject(record)
                .ToString(Formatting.None);

            return result;
        }

        #endregion
    }
}
