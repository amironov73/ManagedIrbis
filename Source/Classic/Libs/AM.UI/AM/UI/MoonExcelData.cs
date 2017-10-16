// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MoonExcelData.cs -- 
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
using System.Xml.Serialization;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Scripting;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace AM.UI
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("cell")]
    [MoonSharpUserData]
    public sealed class MoonExcelData
    {
        #region Properties

        /// <summary>
        /// For information only.
        /// </summary>
        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Row number is relative?
        /// </summary>
        [XmlAttribute("rowRelative")]
        [JsonProperty("rowRelative")]
        public bool RowRelative { get; set; }

        /// <summary>
        /// Row number.
        /// </summary>
        [XmlAttribute("row")]
        [JsonProperty("row")]
        public int Row { get; set; }

        /// <summary>
        /// Column number is relative?
        /// </summary>
        [XmlAttribute("columnRelative")]
        [JsonProperty("columnRelative")]
        public bool ColumnRelative { get; set; }

        /// <summary>
        /// Column number.
        /// </summary>
        [XmlAttribute("column")]
        [JsonProperty("column")]
        public int Column { get; set; }

        /// <summary>
        /// Script text.
        /// </summary>
        [CanBeNull]
        [XmlElement("script")]
        [JsonProperty("script")]
        public string ScriptCode { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the script.
        /// </summary>
        [CanBeNull]
        public string Execute
            (
                [NotNull] IIrbisConnection connection,
                [NotNull] IDictionary<string,object> dataDictionary
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(dataDictionary, "dataDictionary");

            string scriptText = ScriptCode;
            if (string.IsNullOrEmpty(scriptText))
            {
                return null;
            }

            using (IrbisScript moonScript = new IrbisScript(connection))
            {
                foreach (KeyValuePair<string, object> pair in dataDictionary)
                {
                    moonScript.SetGlobal(pair.Key, pair.Value);
                }

                DynValue result = moonScript.DoString(ScriptCode);

                return result.CastToString();
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
