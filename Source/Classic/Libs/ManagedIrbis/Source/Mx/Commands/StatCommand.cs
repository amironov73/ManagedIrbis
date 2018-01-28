// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* StatCommand.cs -- 
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
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class StatCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StatCommand()
            : base("stat")
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

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            string lastSearch = null;
            if (executive.History.Count != 0)
            {
                lastSearch = executive.History.Peek();
            }

            ConnectedClient client = executive.Provider as ConnectedClient;
            if (ReferenceEquals(client, null))
            {
                return true;
            }

            IIrbisConnection connection = client.Connection;

            string text = null;
            if (arguments.Length != 0)
            {
                text = arguments[0].Text;
            }
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            string[] parts = StringUtility.SplitString
                (
                    text,
                    CommonSeparators.Comma,
                    StringSplitOptions.None
                );
            StatDefinition.Item item = new StatDefinition.Item
            {
                Field = parts.GetOccurrence(0),
                Length = parts.GetOccurrence(1).SafeToInt32(10),
                Count = parts.GetOccurrence(2).SafeToInt32(1000),
                Sort = (StatDefinition.SortMethod) parts.GetOccurrence(3).SafeToInt32()
            };
            StatDefinition definition = new StatDefinition
            {
                SearchQuery = lastSearch,
                Items = { item },
                DatabaseName = connection.Database
            };
            string output = connection.GetDatabaseStat(definition);
            if (!string.IsNullOrEmpty(output))
            {
                executive.WriteLine(output);
            }

            OnAfterExecute();

            return true;
        }

        #endregion

        #region Object members

        #endregion
    }
}
