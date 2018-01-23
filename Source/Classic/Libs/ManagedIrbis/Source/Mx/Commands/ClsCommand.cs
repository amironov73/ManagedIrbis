// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ClsCommand.cs -- 
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
    public sealed class ClsCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClsCommand()
            : base("cls")
        {
        }

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

            executive.MxConsole.Clear();

            OnAfterExecute();

            return true;
        }

        /// <inheritdoc cref="MxCommand.GetShortHelp" />
        public override string GetShortHelp()
        {
            return "Clear the console";
        }

        #endregion
    }
}
