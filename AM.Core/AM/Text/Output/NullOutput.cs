/* NullOutput.cs -- пустой объект вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Пустой объект вывода.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class NullOutput
        : AbstractOutput
    {
        #region Private members

        #endregion

        #region AbstractOutput members

        /// <summary>
        /// 
        /// </summary>
        public override bool HaveError { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override AbstractOutput Clear()
        {
            HaveError = false;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // Noting to do here
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override AbstractOutput Write
            (
                string text
            )
        {
            // Nothing to do here
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            // Nothing to do here
            HaveError = true;
            return this;
        }

        #endregion
    }
}
