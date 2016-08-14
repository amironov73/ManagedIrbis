/* CommandLineSwitch.cs -- command line switch
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Text;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.CommandLine
{
    /// <summary>
    /// Command line switch.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [DebuggerDisplay("{Name.Value}={Value}")]
    public sealed class CommandLineSwitch
    {
        #region Properties

        /// <summary>
        /// Name of the switch.
        /// </summary>
        public NonNullValue<string> Name { get; set; }

        /// <summary>
        /// Value of the switch.
        /// </summary>
        [CanBeNull]
        public string Value { get; set; }

        /// <summary>
        /// Values of the switch.
        /// </summary>
        [NotNull]
        public NonNullCollection<string> Values { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandLineSwitch()
        {
            Values = new NonNullCollection<string>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandLineSwitch
            (
                [NotNull] string name
            )
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandLineSwitch
            (
                [NotNull] string name,
                [CanBeNull] string value
            )
            : this()
        {
            Name = name;
            value = value.EmptyToNull();
            Value = value;
            if (!ReferenceEquals(value, null))
            {
                Values.Add(value);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add value.
        /// </summary>
        [NotNull]
        public CommandLineSwitch AddValue
            (
                [NotNull] string value
            )
        {
            Code.NotNullNorEmpty(value, "value");

            if (string.IsNullOrEmpty(Value))
            {
                Value = value;
            }
            Values.Add(value);

            return this;
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
            StringBuilder result = new StringBuilder();

            if (Values.Count == 0)
            {
                result.Append(CommandLineSettings.SwitchPrefix);
                result.Append(Name.Value);
            }
            else
            {
                bool first = true;
                foreach (string value in Values)
                {
                    if (!first)
                    {
                        result.Append(" ");
                    }
                    result.Append(CommandLineSettings.SwitchPrefix);
                    result.Append(Name.Value);
                    first = false;
                    result.Append(CommandLineSettings.ValueSeparator);
                    result.Append(value);
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
