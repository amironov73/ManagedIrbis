/* GblExecutive.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Executes GBL statements
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class GblExecutive
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        [NotNull]
        public IrbisConnection Connection { get; private set; }

        /// <summary>
        /// Record.
        /// </summary>
        [NotNull]
        public MarcRecord Record { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblExecutive
            (
                [NotNull] IrbisConnection connection,
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(connection, "connection");
            Code.NotNull(record, "record");

            Connection = connection;
            Record = record;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [CanBeNull]
        public RecordField GetField
            (
                [NotNull] string fieldSpecification
            )
        {
            Code.NotNull(fieldSpecification, "fieldSpecification");

            return null;
        }

        public bool IsSubField
            (
                [NotNull] string specification
            )
        {
            Code.NotNull(specification, "specification");

            return specification.Contains("^");
        }

        #endregion
    }
}
