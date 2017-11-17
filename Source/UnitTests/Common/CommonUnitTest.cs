using System;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Common
{
    [TestClass]
    public class CommonUnitTest
    {
        /// <summary>
        /// Папка, в которой расположена UnitTests.dll.
        /// </summary>
        [NotNull]
        public string UnitTestDllPath
        {
            get
            {
                Assembly assembly = typeof(CommonUnitTest).Assembly;
                string codeBase = assembly.CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string asmPath = Uri.UnescapeDataString(uri.Path);
                string result = Path.GetDirectoryName(asmPath);
                Assert.IsNotNull(result, "UnitTestDllPath");

                return result;
            }
        }

        /// <summary>
        /// Папка с данными для тестов.
        /// </summary>
        [NotNull]
        public string TestDataPath
        {
            get
            {
                string result = Path.Combine
                    (
                        UnitTestDllPath,
                        @"..\..\..\..\TestData"
                    );
                result = Path.GetFullPath(result);

                return result;
            }
        }

        /// <summary>
        /// Корневая папка с тестовыми данными для ИРБИС64.
        /// </summary>
        [NotNull]
        public string Irbis64RootPath
        {
            get
            {
                string result = Path.Combine
                    (
                        TestDataPath,
                        "Irbis64"
                    );

                return result;
            }
        }
    }
}
