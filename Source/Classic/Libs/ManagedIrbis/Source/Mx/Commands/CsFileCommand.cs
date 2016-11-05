/* CsFileCommand.cs -- 
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
    public sealed class CsFileCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CsFileCommand()
            : base("CSFile")
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
                    MethodInfo main = SharpRunner.CompileFile
                        (
                            argument,
                            err => executive.WriteLine(err)
                        );

                    if (!ReferenceEquals(main, null))
                    {
                        main.Invoke(null, null);
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

