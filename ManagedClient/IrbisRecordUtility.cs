/* IrbisRecordUtility.cs --
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

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisRecordUtility
    {
        #region Constants

        #endregion

        #region Properties
        
        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert the <see cref="IrbisRecord"/> to JSON.
        /// </summary>
        [NotNull]
        public static string ToJson
            (
                [NotNull] this IrbisRecord record
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
