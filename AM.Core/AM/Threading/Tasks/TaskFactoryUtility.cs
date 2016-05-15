/* TaskFactoryUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Threading.Tasks
{
#if FW45

    /// <summary>
    /// Extensions for <see cref="TaskFactory"/> class.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class TaskFactoryUtility
    {
        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static Task Run
            (
                [NotNull] this TaskFactory factory,
                [NotNull] Action action
            )
        {
            Task result = factory.StartNew
                (
                    action,
                    factory.CancellationToken,
                    factory.CreationOptions
                    | TaskCreationOptions.DenyChildAttach,
                    factory.Scheduler
                );

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static Task<TResult> Run<TResult>
            (
                [NotNull] this TaskFactory factory,
                [NotNull] Func<TResult> action
            )
        {
            Task<TResult> result = factory.StartNew
                (
                    action,
                    factory.CancellationToken,
                    factory.CreationOptions
                    | TaskCreationOptions.DenyChildAttach,
                    factory.Scheduler
                );

            return result;
        }

        #endregion
    }

#endif
}
