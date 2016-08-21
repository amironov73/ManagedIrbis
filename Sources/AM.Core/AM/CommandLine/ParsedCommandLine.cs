/* ParsedCommandLine.cs -- command line parsing result
 * Ars Magna project, http://arsmagna.ru 
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using AM.Collections;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.CommandLine
{
    /// <summary>
    /// Command line parsing result.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ParsedCommandLine
    {
        #region Properties

        /// <summary>
        /// Positional arguments (if any).
        /// </summary>
        [NotNull]
        public NonNullCollection<string> PositionalArguments
        {
            get; private set;
        }

        /// <summary>
        /// Command line switches (if any).
        /// </summary>
        [NotNull]
        public NonNullCollection<CommandLineSwitch> Switches
        {
            get; private set;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParsedCommandLine()
        {
            PositionalArguments = new NonNullCollection<string>();
            Switches = new NonNullCollection<CommandLineSwitch>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Add switch without value.
        /// </summary>
        [NotNull]
        public ParsedCommandLine AddSwitch
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            CommandLineSwitch item = GetSwitch(name);
            if (ReferenceEquals(item, null))
            {
                item = new CommandLineSwitch(name);
                Switches.Add(item);
            }

            return this;
        }

        /// <summary>
        /// Add switch with value.
        /// </summary>
        [NotNull]
        public ParsedCommandLine AddSwitch
            (
                [NotNull] string name,
                [NotNull] string value
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNullNorEmpty(value, "value");

            CommandLineSwitch item = GetSwitch(name);
            if (ReferenceEquals(item, null))
            {
                item = new CommandLineSwitch(name, value);
                Switches.Add(item);
            }
            else
            {
                item.AddValue(value);
            }

            return this;
        }

        /// <summary>
        /// Add/merge switch.
        /// </summary>
        [NotNull]
        public ParsedCommandLine AddSwitch
            (
                [NotNull] CommandLineSwitch otherSwitch
            )
        {
            Code.NotNull(otherSwitch, "otherSwitch");

            CommandLineSwitch thisSwitch
                = GetSwitch(otherSwitch.Name);
            if (ReferenceEquals(thisSwitch, null))
            {
                Switches.Add(otherSwitch);
            }
            else
            {
                foreach (string value in otherSwitch.Values)
                {
                    thisSwitch.AddValue(value);
                }
            }

            return this;
        }

        /// <summary>
        /// Get positional argument.
        /// </summary>
        [CanBeNull]
        public string GetArgument
            (
                int position,
                [CanBeNull] string defaultValue
            )
        {
            Code.Nonnegative(position, "position");

            string result = (position < PositionalArguments.Count)
                ? PositionalArguments[position]
                : defaultValue;

            return result;
        }

        /// <summary>
        /// Get switch with given name.
        /// </summary>
        [CanBeNull]
        public CommandLineSwitch GetSwitch
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            foreach (CommandLineSwitch item in Switches)
            {
                if (name == item.Name)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Get value of the switch.
        /// </summary>
        [CanBeNull]
        public string GetValue
            (
                [NotNull] string name,
                [CanBeNull] string defaultValue
            )
        {
            Code.NotNullNorEmpty(name, "name");

            CommandLineSwitch found = GetSwitch(name);
            string result = found == null
                ? defaultValue
                : found.Value;

            return result;
        }

        /// <summary>
        /// Get array of values of the switch.
        /// </summary>
        [NotNull]
        public string[] GetValues
            (
                [NotNull] string name,
                [NotNull] string[] defaultValue
            )
        {
            Code.NotNullNorEmpty(name, "name");
            Code.NotNull(defaultValue, "defaultValue");

            CommandLineSwitch found = GetSwitch(name);
            string[] result = found == null
                ? defaultValue
                : found.Values.ToArray();

            return result;
        }

        /// <summary>
        /// Get array of values of the switch.
        /// </summary>
        [NotNull]
        public string[] GetValues
            (
                [NotNull] string name
            )
        {
            string[] result = GetValues
                (
                    name,
                    new string[0]
                );

            return result;
        }

        /// <summary>
        /// Do we have specified switch?
        /// </summary>
        public bool HaveSwitch
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            foreach (CommandLineSwitch item in Switches)
            {
                if (name == item.Name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merge two command lines.
        /// </summary>
        [NotNull]
        public ParsedCommandLine Merge
            (
                [NotNull] ParsedCommandLine other
            )
        {
            Code.NotNull(other, "other");

            PositionalArguments.AddRange(other.PositionalArguments);

            foreach (CommandLineSwitch otherSwitch in other.Switches)
            {
                AddSwitch(otherSwitch);
            }

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

            for (int i = 0; i < Switches.Count; i++)
            {
                if (i != 0)
                {
                    result.Append(" ");
                }
                result.Append
                    (
                        Switches[i]
                    );
            }

            if (PositionalArguments.Count != 0)
            {
                if (result.Length != 0)
                {
                    result.Append(" ");
                }

                for (int i = 0; i < PositionalArguments.Count; i++)
                {
                    if (i != 0)
                    {
                        result.Append(" ");
                    }
                    result.Append
                        (
                            CommandLineUtility.WrapArgumentIfNeeded
                                (
                                    PositionalArguments[i]
                                )
                        );
                }
            }

            return result.ToString();
        }

        #endregion
    }
}
