// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
#if !WINMOBILE && !PocketPC
    [DebuggerDisplay("{Name.Value}={Value}")]
#endif
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
        public string Value
        {
            get { return _value; }
            set { _SetValue(value); }
        }

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
            Value = value;
        }

        #endregion

        #region Private members

        private string _value;

        private string _FormatValue
            (
                string value
            )
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(CommandLineSettings.SwitchPrefix);
            builder.Append(Name.Value);
            builder.Append(CommandLineSettings.ValueSeparator);
            builder.Append(value);

            string result = CommandLineUtility.WrapArgumentIfNeeded
                (
                    builder.ToString()
                );

            return result;
        }

        private void _SetValue
            (
                string value
            )
        {
            value = value.EmptyToNull();

            if (!string.IsNullOrEmpty(value))
            {
                _value = value;

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

            _SetValue(value);

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
                    result.Append(_FormatValue(value));
                    first = false;
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
