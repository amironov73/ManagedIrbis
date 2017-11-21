using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using JetBrains.Annotations;
using ManagedIrbis;
using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Quality.Rules
{
    public class RuleTest
        : Common.CommonUnitTest
    {
        [NotNull]
        protected RuleContext GetContext()
        {
            IrbisConnection connection = new IrbisConnection();
            MarcRecord record = new MarcRecord();
            RuleContext result = new RuleContext
            {
                Connection = connection,
                BriefFormat = "@brief",
                Record = record
            };

            return result;
        }
    }
}
