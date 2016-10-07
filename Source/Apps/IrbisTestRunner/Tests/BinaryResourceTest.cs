/* BinaryResourceTest.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status:
 */

#region Using directives

using System;
using System.IO;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Testing;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class BinaryResourceTest
        : AbstractTest
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        private void _TestBinaryResource
            (
                int mfn
            )
        {
            IrbisConnection connection = Connection
                .ThrowIfNull("Connection");

            MarcRecord record = connection.ReadRecord(mfn);

            BinaryResource[] resources
                = BinaryResource.Parse(record);
            for (int i = 0; i < resources.Length; i++)
            {
                BinaryResource resource = resources[i];

                string fileName = string.Format
                    (
                        "res{0:00000}-{1}.{2}",
                        mfn,
                        (i + 1),
                        resource.Kind
                    );
                string filePath = Path.Combine
                    (
                        Path.GetTempPath(),
                        fileName
                    );
                byte[] array = resource.Decode();
                File.WriteAllBytes
                    (
                        filePath,
                        array
                    );

                Write
                    (
                        "{0} ",
                        filePath
                    );
            }
        }

        #endregion

        #region Public methods

        [TestMethod]
        public void BinaryResource_Read()
        {
            _TestBinaryResource(18);
            _TestBinaryResource(23);
            _TestBinaryResource(25);
        }

        #endregion
    }
}
