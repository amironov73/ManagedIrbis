// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* CsCommand.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */


#region Using directives

#if CLASSIC

using System.Reflection;

#endif

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class CsCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsCommand()
            : base("CS")
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region MxCommand members

        /// <inheritdoc/>
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

#if CLASSIC

            if (arguments.Length != 0)
            {
                string argument = arguments[0].Text;
                if (!string.IsNullOrEmpty(argument))
                {
                    MethodInfo method = SharpRunner.CompileSnippet
                        (
                            argument,
                            "MxExecutive executive",
                            err => executive.WriteLine(err)
                        );
                    if (!ReferenceEquals(method, null))
                    {
                        method.Invoke(null, new object[]{executive});
                    }
                }
            }

#endif

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}

