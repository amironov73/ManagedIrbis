// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportContext.cs -- 
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
using System.Xml.Serialization;

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

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public class ReportContext
    {
        #region Properties

        /// <summary>
        /// Abstract client.
        /// </summary>
        [NotNull]
        public AbstractClient Client { get; set; }

        /// <summary>
        /// Current record.
        /// </summary>
        [CanBeNull]
        public MarcRecord CurrentRecord { get; internal set; }

        /// <summary>
        /// Text driver.
        /// </summary>
        [NotNull]
        public ReportDriver Driver { get; internal set; }

        /// <summary>
        /// Record index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Records.
        /// </summary>
        [NotNull]
        public NonNullCollection<MarcRecord> Records { get; private set; }

        /// <summary>
        /// Output.
        /// </summary>
        [NotNull]
        public ReportOutput Output { get; private set; }

        /// <summary>
        /// Variables.
        /// </summary>
        [NotNull]
        public ReportVariableManager Variables { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        public object UserData { get; set; }


        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportContext
            (
                [NotNull] AbstractClient client
            )
        {
            Code.NotNull(client, "client");

            Variables = new ReportVariableManager();
            Records = new NonNullCollection<MarcRecord>();
            Output = new ReportOutput();
            Driver = new PlainTextDriver();
            Client = client;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the context.
        /// </summary>
        [NotNull]
        public ReportContext Clone()
        {
            ReportContext result = (ReportContext)MemberwiseClone();

            return result;
        }

        /// <summary>
        /// Clone the context with new record list.
        /// </summary>
        [NotNull]
        public ReportContext Clone
            (
                [NotNull] IEnumerable<MarcRecord> records
            )
        {
            Code.NotNull(records, "records");

            ReportContext result = (ReportContext) MemberwiseClone();
            result.Records = new NonNullCollection<MarcRecord>();
            result.Records.AddRange(records);

            return result;
        }

        /// <summary>
        /// Push the context.
        /// </summary>
        [NotNull]
        public ReportContext Push()
        {
            ReportContext result = (ReportContext) MemberwiseClone();
            result.Output = new ReportOutput();

            return result;
        }

        /// <summary>
        /// Set text driver for the context.
        /// </summary>
        [NotNull]
        public ReportContext SetDriver
            (
                [NotNull] ReportDriver driver
            )
        {
            Code.NotNull(driver, "driver");

            Driver = driver;

            return this;
        }

        #endregion

        #region Object members

        #endregion
    }
}
