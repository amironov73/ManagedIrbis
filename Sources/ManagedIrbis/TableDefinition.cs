/* TableDefinition.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Menus;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Commands;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis
{
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TableDefinition
    {
        #region Properties

        public string DatabaseName { get; set; }

        public string Table { get; set; }

        public List<string> Headers
        {
            get { return _headers; }
        }

        public string Mod { get; set; }

        public string SearchQuery { get; set; }

        public int MinMfn { get; set; }

        public int MaxMfn { get; set; }

        public string SequentialQuery { get; set; }

        public List<int> MfnList
        {
            get { return _mfnList; }
        }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private readonly List<string> _headers
            = new List<string>();

        private readonly List<int> _mfnList
            = new List<int>();

        #endregion

        #region Public methods

        #endregion
    }
}
