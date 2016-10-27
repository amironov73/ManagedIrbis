/* MxExecutive.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;
using ManagedIrbis.Client;
using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MxExecutive
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public ConnectedClient Client { get; private set; }

        /// <summary>
        /// Commands.
        /// </summary>
        [NotNull]
        public NonNullCollection<MxCommand> Commands { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MxExecutive()
        {
            Client = new ConnectedClient();
            Commands = new NonNullCollection<MxCommand>();

            _FillStandardCommands();
        }

        #endregion

        #region Private members

        private void _FillStandardCommands()
        {
            
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Write to console.
        /// </summary>
        public void Write
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Console.Write(format, arguments);
        }

        /// <summary>
        /// Write to console.
        /// </summary>
        public void WriteLine
            (
                [NotNull] string format,
                params object[] arguments
            )
        {
            Console.WriteLine(format, arguments);
        }

        #endregion

        #region Object members

        #endregion
    }
}
