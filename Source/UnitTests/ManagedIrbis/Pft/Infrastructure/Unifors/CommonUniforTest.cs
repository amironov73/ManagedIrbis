using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.PlatformAbstraction;

using JetBrains.Annotations;

using ManagedIrbis.Client;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    public abstract class CommonUniforTest
    {
        [NotNull]
        protected IrbisProvider GetProvider()
        {
            string rootPath = Path.GetFullPath(@"..\..\..\..\TestData\Irbis64");
            LocalProvider result = new LocalProvider(rootPath)
            {
                Database = "IBIS"
            };

            return result;
        }

        protected void Execute
            (
                [NotNull] string input,
                [NotNull] string expected
            )
        {
            using (IrbisProvider provider = GetProvider())
            {
                PftContext context = new PftContext(null);
                context.SetProvider(provider);
                Unifor unifor = new Unifor();
                string expression = input;
                unifor.Execute(context, null, expression);
                string actual = context.Text;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
