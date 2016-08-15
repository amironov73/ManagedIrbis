/* TestTemplate.cs -- IRBIS64 test template
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class WriteFileTest
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
        public void TestWriteFile()
        {
            string fileName = "hello.txt";
            string fileContents = "Hello, IRBIS!\r\nHello!";
            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    fileName
                )
            {
                Contents = fileContents
            };
            Connection.WriteTextFile(specification);
        }

        [TestMethod]
        public void TestWriteFiles()
        {
            string fileName1 = "hello1.txt";
            string fileContents1 = "Hello1, IRBIS!\r\nHello!";
            FileSpecification specification1 = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    fileName1
                )
            {
                Contents = fileContents1
            };
            string fileName2 = "hello2.txt";
            string fileContents2 = "Hello2, IRBIS!\r\nHello!";
            FileSpecification specification2 = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    fileName2
                )
            {
                Contents = fileContents2
            };

            
            Connection.WriteTextFiles
                (
                    specification1,
                    specification2
                );
        }

        #endregion
    }
}
