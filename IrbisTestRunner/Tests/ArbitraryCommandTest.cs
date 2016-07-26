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
using ManagedIrbis.Network;
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
                    "code-{0}.packet",
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
                    response.Packet
                );
            Write
                (
                    "{0} response written to {1} | ",
                    commandCode,
                    filePath
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
            _TestCommand("T", "NEW2", "New2", "0");
        }

        #endregion
    }
}
