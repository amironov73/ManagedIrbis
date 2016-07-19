/* ReadFileTest.cs --
 * Ars Magna project, http://arsmagna.ru 
 */

#region Using directives

using System;
using AM;
using ManagedIrbis;
using ManagedIrbis.Network;
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
        public void TestReadFile()
        {
            string text = Connection.ReadTextFile
                (
                    IrbisPath.MasterFile,
                    "II.MNU"
                );
            Console.Write
                (
                    IrbisFormat.PrepareFormat(text.Substring(0, 50)).Trim()
                );
        }

        [TestMethod]
        public void TestReadFiles()
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
            Console.Write(string.Join(Environment.NewLine, texts));
        }

        #endregion
    }
}
