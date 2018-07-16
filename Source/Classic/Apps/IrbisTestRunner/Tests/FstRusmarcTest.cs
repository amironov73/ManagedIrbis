/* FstRusmarcTest.cs --
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
using ManagedIrbis.Client;
using ManagedIrbis.Fst;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Testing;

using Newtonsoft.Json;

#endregion

namespace IrbisTestRunner.Tests
{
    [TestClass]
    class FstRusmarcTest
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
        public void FstProcessor_Rusmarc()
        {
            IrbisConnection connection = Connection.ThrowIfNull();
            IrbisProvider provider = new ConnectedClient(connection);

            FileSpecification specification = new FileSpecification
                (
                    IrbisPath.MasterFile,
                    "IBIS",
                    "rmarci.fst"
                );
            FstProcessor processor = new FstProcessor(provider, specification);

            MarcRecord record;
            string fileName = Path.Combine
                (
                    DataPath.ThrowIfNull("DataPath"),
                    "TEST1.ISO"
                );
            using (Stream stream = File.OpenRead(fileName))
            {
                record = Iso2709.ReadRecord
                (
                    stream,
                    IrbisEncoding.Ansi
                )
                .ThrowIfNull("Iso2709.ReadRecord");
            }
            MarcRecord transformed = processor.TransformRecord
                (
                    record,
                    processor.File
                );
            Write(transformed);
        }

        #endregion
    }
}
