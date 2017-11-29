using AM.Text;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    public class CommonMxCommandTest
        : Common.CommonUnitTest
    {
        [NotNull]
        protected MxExecutive GetExecutive()
        {
            MxExecutive result = new MxExecutive();

            return result;
        }
    }
}
