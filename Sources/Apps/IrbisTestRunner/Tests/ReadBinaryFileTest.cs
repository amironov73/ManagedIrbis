/* ReadBinaryFileTest.cs --
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
    class ReadBinaryFileTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        [TestMethod]
        public void TestReadBinaryFile()
        {
            string fileName = "logo.gif";

            FileSpecification file = new FileSpecification
                (
                    IrbisPath.System,
                    fileName
                )
            {
                BinaryFile = true
            };

            byte[] bytes = Connection.ReadBinaryFile(file);

            if (!ReferenceEquals(bytes, null))
            {
                string filePath = Path.Combine
                    (
                        Path.GetTempPath(),
                        fileName
                    );
                File.WriteAllBytes
                    (
                        filePath,
                        bytes
                    );

                Write
                    (
                        "{0} bytes read, see {1}",
                        bytes.Length,
                        filePath
                    );
            }
        }

        #endregion
    }
}
