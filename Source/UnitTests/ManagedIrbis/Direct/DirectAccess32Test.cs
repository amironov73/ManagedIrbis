using System;
using System.IO;

using AM;
using AM.IO;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class DirectAccess32Test
        : Common.CommonUnitTest
    {
        [NotNull]
        private string _GetMasterPath()
        {
            return Path.Combine
                (
                    TestDataPath,
                    "Irbis32/ibis.mst"
                );
        }

        [NotNull]
        private DirectAccess32 _GetAccess()
        {
            return new DirectAccess32(_GetMasterPath(), DirectAccessMode.ReadOnly);
        }
    }
}
