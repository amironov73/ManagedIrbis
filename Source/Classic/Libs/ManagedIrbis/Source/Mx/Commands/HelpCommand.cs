// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HelpCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class HelpCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public HelpCommand()
            : base("help")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc cref="MxCommand.Execute" />
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            if (!executive.Client.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            string name = null;
            if (arguments.Length != 0)
            {
                name = arguments[0].Text;
            }

            if (string.IsNullOrEmpty(name))
            {
                foreach (MxCommand command in executive.Commands)
                {
                    executive.WriteLine("{0} {1}", command.Name, command.GetShortHelp());
                }
            }
            else
            {
                MxCommand command = executive.GetCommand(name);
                if (ReferenceEquals(command, null))
                {
                    executive.WriteError("Unknown command '{0}'", name);
                }
                else
                {
                    executive.WriteLine("{0}", command.GetLongHelp());
                }
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
