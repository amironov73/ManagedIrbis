/* StatDefinition.cs --
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
    public sealed class StatDefinition
    {
        #region Nested classes

        public enum SortMethod
        {
            None = 0,

            Ascending = 1,

            Descending = 2
        }

        public class Item
        {
            #region Properties

            public string Field { get; set; }

            public int Length { get; set; }

            public int Count { get; set; }

            public SortMethod Sort { get; set; }

            #endregion

            #region Object members

            public override string ToString()
            {
                return string.Format
                    (
                        "{0},{1},{2},{3}",
                        Field,
                        Length,
                        Count,
                        (int) Sort
                    );
            }

            #endregion
        }

        #endregion

        #region Properties

        public string DatabaseName { get; set; }

        public List<Item> Items
        {
            get { return _items; }
        }

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

        private readonly List<Item> _items = new List<Item>();

        private readonly List<int> _mfnList
            = new List<int>();

        #endregion

        #region Public methods

        #endregion

        #region Object members
        
        #endregion
    }
}
