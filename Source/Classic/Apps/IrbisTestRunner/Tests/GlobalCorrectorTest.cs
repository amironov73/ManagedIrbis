/* GlobalCorrectorTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Gbl;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class GlobalCorrectorTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private void _PortionProcessed
            (
                object sender,
                EventArgs args
            )
        {
            GlobalCorrector corrector = (GlobalCorrector) sender;

            Write
                (
                    "Processed: {0} of {1}|",
                    corrector.Result.RecordsProcessed,
                    corrector.Result.RecordsSupposed
                );
        }

        #endregion

        #region Public methods

        [TestMethod]
        public void GlobalCorrector_ProcessRecordset()
        {
            if (ReferenceEquals(Connection, null))
            {
                return;
            }

            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            GlobalCorrector corrector = new GlobalCorrector
                (
                    Connection,
                    "IBIS",
                    2
                );
            corrector.PortionProcessed += _PortionProcessed;
            int[] recordset = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            corrector.ProcessRecordset
                (
                    recordset,
                    statements
                );

            Write(" done");
        }

        [TestMethod]
        public void GlobalCorrector_ProcessInterval()
        {
            if (ReferenceEquals(Connection, null))
            {
                return;
            }

            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            GlobalCorrector corrector = new GlobalCorrector
                (
                    Connection,
                    "IBIS",
                    2
                );
            corrector.PortionProcessed += _PortionProcessed;
            corrector.ProcessInterval
                (
                    11,
                    20,
                    statements
                );

            Write(" done");
        }

        [TestMethod]
        public void GlobalCorrector_ProcessSearchResult()
        {
            if (ReferenceEquals(Connection, null))
            {
                return;
            }

            string field3000 = string.Format
                (
                    "'{0}'",
                    DateTime.Now
                );

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = GblCode.Add,
                    Format1 = field3000,
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "3000",
                    Parameter2 = "*"
                }, 
            };

            GlobalCorrector corrector = new GlobalCorrector
                (
                    Connection,
                    "IBIS",
                    2
                );
            corrector.PortionProcessed += _PortionProcessed;
            corrector.ProcessSearchResult
                (
                    "T=A$",
                    statements
                );

            Write(" done");
        }

        [TestMethod]
        public void GlobalCorrector_ProcessWholeDatabase()
        {
            if (ReferenceEquals(Connection, null))
            {
                return;
            }

            GblStatement[] statements =
            {
                new GblStatement
                {
                    Command = "///",
                    Format1 = "XXXXXXXXXXX",
                    Format2 = "XXXXXXXXXXX",
                    Parameter1 = "XXXXXXXXXXX",
                    Parameter2 = "XXXXXXXXXXX"
                }, 
            };

            GlobalCorrector corrector = new GlobalCorrector
                (
                    Connection,
                    "IBIS",
                    100
                );
            corrector.PortionProcessed += _PortionProcessed;
            corrector.ProcessWholeDatabase
                (
                    statements
                );

            Write(" done");
        }

        #endregion
    }
}
