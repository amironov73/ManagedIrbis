/* ReadFileTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;

using AM;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class ReadFileTest
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
        public void ReadFile_Existent()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            string text = connection.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    "II.MNU"
                );
            Write
                (
                    IrbisFormat.PrepareFormat
                        (
                            text.Substring(0, 50)
                        )
                        .Trim()
                );
        }

        [TestMethod]
        public void ReadFile_NonExistent()
        {
            IrbisConnection connection
                = Connection.ThrowIfNull("Connection");

            string text = connection.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    "NOTEXIST.MNU"
                );
            Write
                (
                    IrbisFormat.PrepareFormat
                        (
                            text
                        )
                        .Trim()
                );
        }

        [TestMethod]
        public void ReadFiles()
        {
            string[] texts = Connection.ReadTextFiles
                (
                    new[]
                        {
                            new FileSpecification
                                (
                                    IrbisPath.MasterFile,
                                    "IBIS",
                                    "brief.pft"
                                ), 
                            new FileSpecification
                                (
                                    IrbisPath.MasterFile,
                                    "IBIS",
                                    "briefin.pft"
                                ), 
                        }
                );

            texts[0] = IrbisFormat.PrepareFormat(texts[0].SafeSubstring(0, 50)).Trim();
            texts[1] = IrbisFormat.PrepareFormat(texts[1].SafeSubstring(0, 50)).Trim();
            Write(string.Join(Environment.NewLine, texts));
        }

        #endregion
    }
}
