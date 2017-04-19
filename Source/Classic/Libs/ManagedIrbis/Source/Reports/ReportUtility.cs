// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportUtility.cs -- 
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
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class ReportUtility
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Get PFT formatter for the report context.
        /// </summary>
        [NotNull]
        public static PftFormatter GetFormatter
            (
                [NotNull] this ReportContext context,
                [CanBeNull] string expression
            )
        {
            Code.NotNull(context, "context");

            PftFormatter result = new PftFormatter();
            result.SetEnvironment(context.Provider);
            if (!string.IsNullOrEmpty(expression))
            {
                result.ParseProgram(expression);
            }

            return result;
        }

        /// <summary>
        /// List band types.
        /// </summary>
        [NotNull]
        public static Type[] ListBandTypes()
        {
            Type[] result =
            {
                typeof(CompositeBand),
                typeof(ConditionalBand),
                typeof(FilterBand),
                typeof(GroupBand),
                typeof(SectionBand),
                typeof(SortBand),
                typeof(TableBand),
                typeof(TotalBand)
            };

            return result;
        }

        /// <summary>
        /// List cell types.
        /// </summary>
        [NotNull]
        public static Type[] ListCellTypes()
        {
            Type[] result =
            {
                typeof(IndexCell),
                typeof(PftCell),
                typeof(RawPftCell),
                typeof(RawTextCell),
                typeof(TextCell),
                typeof(TotalCell)
            };

            return result;
        }

        /// <summary>
        /// Set height of the object.
        /// </summary>
        [NotNull]
        public static IAttributable SetHeight
            (
                [NotNull] this IAttributable reportObject,
                int height
            )
        {
            Code.NotNull(reportObject, "reportObject");

            reportObject.Attributes["Height"] = height;

            return reportObject;
        }

        /// <summary>
        /// Set width of the object.
        /// </summary>
        [NotNull]
        public static IAttributable SetWidth
            (
                [NotNull] this IAttributable reportObject,
                int width
            )
        {
            Code.NotNull(reportObject, "reportObject");

            reportObject.Attributes["Width"] = width;

            return reportObject;
        }

        /// <summary>
        /// Set variables for <see cref="PftFormatter"/>.
        /// </summary>
        public static void SetVariables
            (
                [NotNull] this ReportContext context,
                [CanBeNull] PftFormatter formatter
            )
        {
            Code.NotNull(context, "context");

            if (!ReferenceEquals(formatter, null))
            {
                foreach (ReportVariable variable in
                    context.Variables.GetAllVariables())
                {
                    formatter.Context.Variables.SetVariable
                        (
                            variable.Name,
                            variable.Value.NullableToString()
                        );
                }
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
