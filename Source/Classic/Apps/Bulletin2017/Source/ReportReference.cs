// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportReference.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace Bulletin2017
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportReference
        : IVerifiable
    {
        #region Properties

        [XmlAttribute("default")]
        [JsonProperty("default")]
        public bool Default { get; set; }

        [CanBeNull]
        [XmlAttribute("id")]
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [CanBeNull]
        [XmlAttribute("title")]
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        #endregion

        #region IVerifiable members

        public bool Verify(bool throwOnError)
        {
            Verifier<ReportReference> verifier
                = new Verifier<ReportReference>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Id, "Id")
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return Title.ToVisibleString();
        }

        #endregion
    }
}
