/* ArbitraryCommandTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class ArbitraryCommandTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private void _TestCommand
            (
                string commandCode,
                params object[] arguments
            )
        {
            // ReSharper disable AssignNullToNotNullAttribute
            ServerResponse response = Connection.ExecuteArbitraryCommand
                (
                    commandCode,
                    arguments
                );

            string fileName = string.Format
                (
                    "code-{0}-dn.packet",
                    commandCode
                );
            string filePath = Path.Combine
                (
                    Path.GetTempPath(),
                    fileName
                );
            File.WriteAllBytes
                (
                    filePath,
                    response.RawAnswer
                );
            fileName = string.Format
                (
                    "code-{0}-up.packet",
                    commandCode
                );
            filePath = Path.Combine
                (
                    Path.GetTempPath(),
                    fileName
                );
            Write
                (
                    "{0} response written to {1} | ",
                    commandCode,
                    filePath
                );
            File.WriteAllBytes
                (
                    filePath,
                    response.RawRequest
                );
            // ReSharper restore AssignNullToNotNullAttribute
        }

        #endregion

        #region Public methods

        [TestMethod]
        public void TestArbitraryCommand()
        {
            //_TestCommand("2", "IBIS");
            //_TestCommand("C", "IBIS", "3", "1", "0");
            //_TestCommand("E", "IBIS", "3");
            //_TestCommand("M", "IBIS", "IBIS", "IBIS");
            _TestCommand("T", "NEWDB", "New database", "0");
        }

        #endregion
    }
}
