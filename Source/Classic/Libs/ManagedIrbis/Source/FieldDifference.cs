// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FieldDifference.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Text;
using System.Xml.Serialization;

using AM;
using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [XmlRoot("field")]
    [MoonSharpUserData]
    public class FieldDifference
    {
        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        [XmlAttribute("tag")]
        [JsonProperty("tag")]
        public int Tag { get; set; }

        /// <summary>
        /// Field repeat.
        /// </summary>
        [XmlAttribute("repeat")]
        [JsonProperty("repeat")]
        public int Repeat { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        [XmlAttribute("state")]
        [JsonProperty("state")]
        public FieldState State { get; set; }

        /// <summary>
        /// New field value.
        /// </summary>
        [CanBeNull]
        [XmlElement("newValue")]
        [JsonProperty("newValue")]
        public string NewValue { get; set; }

        /// <summary>
        /// Old field value.
        /// </summary>
        [CanBeNull]
        [XmlElement("oldValue")]
        [JsonProperty("oldValue")]
        public string OldValue { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            char sign;
            switch (State)
            {
                case FieldState.Unchanged:
                    sign = '=';
                    break;

                case FieldState.Edited:
                    sign = '~';
                    break;

                case FieldState.Added:
                    sign = '+';
                    break;

                case FieldState.Removed:
                    sign = '-';
                    break;

                default:
                    throw new ArgumentOutOfRangeException("State");
            }

            string tagAndOcc = string.Format("{0}/{1}", Tag, Repeat)
                .PadRight(8);

            StringBuilder result = StringBuilderCache.Acquire();
            result.AppendFormat
                (
                    "{0} {1}{2}",
                    sign,
                    tagAndOcc,
                    NewValue.ToVisibleString()
                );
            if (State == FieldState.Removed || State == FieldState.Edited)
            {
                result.AppendLine();
                result.AppendFormat
                  (
                      "          {0}",
                      OldValue.ToVisibleString()
                  );
            }

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
