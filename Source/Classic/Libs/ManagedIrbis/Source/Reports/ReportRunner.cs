// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportRunner.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Client;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ReportRunner
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Run the report.
        /// </summary>
        public void RunReport
            (
                [NotNull] IrbisReport report,
                [NotNull] ReportSettings settings
            )
        {
            Code.NotNull(report, "report");
            Code.NotNull(settings, "settings");

            report.Verify(true);
            settings.Verify(true);

#if CLASSIC || ANDROID

            string[] assemblies = settings.Assemblies.ToArray();
            foreach (string path in assemblies)
            {
                Assembly.LoadFile(path);
            }

#endif

            string providerFullName = settings.RegisterProvider;
            if (!string.IsNullOrEmpty(providerFullName))
            {
                Type providerType = Type.GetType
                    (
                        providerFullName,
                        true
                    );
                string key = providerType.Name;
                ProviderManager.Registry[key] = providerType;
            }

            string driverFullName = settings.DriverName;
            if (!string.IsNullOrEmpty(driverFullName))
            {
                Type driverType = Type.GetType
                    (
                        driverFullName,
                        true
                    );
                string key = driverType.Name;
                DriverManager.Registry[key] = driverType;
            }

            string providerName = settings.ProviderName
                .ThrowIfNull("providerName not specified");
            IrbisProvider provider = ProviderManager.GetProvider
                (
                    providerName,
                    true
                )
                .ThrowIfNull("can't get provider");
            string driverName = settings.DriverName
                .ThrowIfNull("driverName not specified");
            ReportDriver driver = DriverManager.GetDriver
                (
                    driverName,
                    true
                )
                .ThrowIfNull("can't get driver");

            ReportContext context = new ReportContext(provider);

            string filterExpression = settings.Filter;
            if (!string.IsNullOrEmpty(filterExpression))
            {
                provider.Search(filterExpression);
                // TODO set records to context
            }

            report.Render(context);
        }

        #endregion

        #region Object members

        #endregion
    }
}
