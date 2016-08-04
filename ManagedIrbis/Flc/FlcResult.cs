/* FlcResult.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// Result of formal checking.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("flc-result")]
    public sealed class FlcResult
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Can continue?
        /// </summary>
        [JsonIgnore]
        public bool CanContinue
        {
            get
            {
                return Status == FlcStatus.OK
                    || Status == FlcStatus.Warning;
            }
        }

        /// <summary>
        /// Status.
        /// </summary>
        [XmlAttribute("status")]
        [JsonProperty("status")]
        public FlcStatus Status { get; set; }

        /// <summary>
        /// Messages.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        [XmlElement("message")]
        [JsonProperty("messages")]
        public NonNullCollection<string> Messages
        {
            get { return _messages; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FlcResult()
        {
            _messages = new NonNullCollection<string>();
        }

        #endregion

        #region Private members

        private readonly NonNullCollection<string> _messages;

        private void _AddMessage
            (
                string message
            )
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            message = message.Trim();
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Messages.Add(message);
        }

        private bool _ParseLine
            (
                string line
            )
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            string code = line.Substring(0, 1);
            string message = line.Substring(1, line.Length - 1);

            if (code != "0" && code != "1" && code != "2")
            {
                _AddMessage(line);
                return false;
            }

            if (code == "1")
            {
                Status = FlcStatus.Error;
            }
            else if (code == "2")
            {
                if (Status == FlcStatus.OK)
                {
                    Status = FlcStatus.Warning;
                }
            }
            _AddMessage(message);

            return true;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the text.
        /// </summary>
        [NotNull]
        public static FlcResult Parse
            (
                [CanBeNull] string text
            )
        {
            FlcResult result = new FlcResult();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }
            string[] lines = text.SplitLines();
            int index;
            for (index = 0; index < lines.Length; index++)
            {
                if (!result._ParseLine(lines[index]))
                {
                    break;
                }
            }
            for (; index < lines.Length; index++)
            {
                result.Messages.Add(lines[index]);
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore the object state from the specified stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Status = (FlcStatus) reader.ReadPackedInt32();
            Messages.Clear();
            Messages.AddRange(reader.ReadStringArray());
        }

        /// <summary>
        /// Save the object stat to the specified stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32((int)Status)
                .WriteArray(Messages.ToArray());
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Status: {0}, Messages: {1}",
                    Status,
                    string.Join
                    (
                        Environment.NewLine,
                        Messages.ToArray()
                    )
                );
        }

        #endregion
    }
}
