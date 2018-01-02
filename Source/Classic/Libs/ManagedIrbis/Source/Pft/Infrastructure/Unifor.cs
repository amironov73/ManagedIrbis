// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Unifor.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using AM;
using AM.Collections;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Unifors;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Unifor.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Unifor
        : IFormatExit
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        [NotNull]
        public static CaseInsensitiveDictionary<Action<PftContext, PftNode, string>> Registry
        {
            get; private set;
        }

        /// <summary>
        /// Throw an exception on empty UNIFOR?
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        /// <summary>
        /// Throw an exception on unknown key?
        /// </summary>
        public static bool ThrowOnUnknown { get; set; }

        #endregion

        #region Construction

        static Unifor()
        {
            ThrowOnEmpty = false;
            ThrowOnUnknown = false;

            Registry = new CaseInsensitiveDictionary<Action<PftContext, PftNode, string>>();

            RegisterActions();
        }

        #endregion

        #region Private members

        private static void RegisterActions()
        {
            Registry.Add("0", Unifor0.FormatAll);
            Registry.Add("1", Unifor1.GetElement);
            Registry.Add("2", Unifor2.GetMaxMfn);
            Registry.Add("3", Unifor3.PrintDate);
            Registry.Add("4", Unifor4.FormatPreviousVersion);
            // "5" unknown
            Registry.Add("6", Unifor6.ExecuteNestedFormat);
            Registry.Add("7", Unifor7.FormatDocuments);
            // "8" unknown
            Registry.Add("9", Unifor9.RemoveDoubleQuotes);
            Registry.Add("A", UniforA.GetFieldRepeat);
            Registry.Add("B", UniforB.Convolution);
            Registry.Add("C", UniforC.CheckIsbn);
            Registry.Add("D", UniforD.FormatDocumentDB);
            Registry.Add("E", UniforE.GetFirstWords);
            Registry.Add("F", UniforF.GetLastWords);
            Registry.Add("G", UniforG.GetPart);
            Registry.Add("H", UniforH.ExtractAngleBrackets);
            Registry.Add("I", UniforI.GetIniFileEntry);
            Registry.Add("J", UniforJ.GetTermRecordCountDB);
            Registry.Add("K", UniforK.GetMenuEntry);
            Registry.Add("L", UniforL.ContinueTerm);
            Registry.Add("M", UniforM.Sort);
            Registry.Add("O", UniforO.AllExemplars);
            Registry.Add("P", UniforP.UniqueField);
            Registry.Add("Q", UniforQ.ToLower);
            Registry.Add("R", UniforR.RandomNumber);
            Registry.Add("S", UniforS.Add);
            Registry.Add("S0", UniforS.Clear);
            Registry.Add("SA", UniforS.Arabic);
            Registry.Add("SX", UniforS.Roman);
            Registry.Add("T", UniforT.Transliterate);
            Registry.Add("U", UniforU.Cumulate);
            Registry.Add("V", UniforU.Decumulate);
            Registry.Add("W", UniforU.Check);
            Registry.Add("X", UniforX.RemoveAngleBrackets);
            Registry.Add("Y", UniforY.FreeExemplars);
            Registry.Add("Z", UniforZ.GenerateExemplars);
            Registry.Add("+0", UniforPlus0.FormatAll);
            Registry.Add("+1", UniforPlus1.ClearGlobals);
            Registry.Add("+1A", UniforPlus1.AddGlobals);
            Registry.Add("+1G", UniforPlus1.DistinctGlobals);
            Registry.Add("+1I", UniforPlus1.DistinctList);
            Registry.Add("+1K", UniforPlus1.DecodeGlobals);
            Registry.Add("+1M", UniforPlus1.MultiplyGlobals);
            Registry.Add("+1O", UniforPlus1.DecodeList);
            Registry.Add("+1R", UniforPlus1.ReadGlobal);
            Registry.Add("+1S", UniforPlus1.SubstractGlobals);
            Registry.Add("+1T", UniforPlus1.SortGlobals);
            Registry.Add("+1V", UniforPlus1.SortList);
            Registry.Add("+1W", UniforPlus1.WriteGlobal);
            Registry.Add("+2", UniforPlus2.System);
            Registry.Add("+3D", UniforPlus3.UrlDecode);
            Registry.Add("+3E", UniforPlus3.UrlEncode);
            Registry.Add("+3U", UniforPlus3.ConvertToUtf);
            Registry.Add("+3W", UniforPlus3.ConvertToAnsi);
            Registry.Add("+3+", UniforPlus3.ReplacePlus);
            Registry.Add("+4", UniforPlus4.GetField);
            Registry.Add("+5", UniforPlus5.GetEntry);
            Registry.Add("+6", UniforPlus6.GetRecordStatus);
            Registry.Add("+7", UniforPlus7.ClearGlobals);
            Registry.Add("+7A", UniforPlus7.UnionGlobals);
            Registry.Add("+7G", UniforPlus7.DistinctGlobal);
            Registry.Add("+7M", UniforPlus7.MultiplyGlobals);
            Registry.Add("+7R", UniforPlus7.ReadGlobal);
            Registry.Add("+7S", UniforPlus7.SubstractGlobals);
            Registry.Add("+7T", UniforPlus7.SortGlobal);
            Registry.Add("+7U", UniforPlus7.AppendGlobal);
            Registry.Add("+7W", UniforPlus7.WriteGlobal);
            Registry.Add("+8", UniforPlus8.ExecuteNativeMethod);
            Registry.Add("+90", UniforPlus9.GetIndex);
            Registry.Add("+91", UniforPlus9.GetFileName);
            Registry.Add("+92", UniforPlus9.GetDirectoryName);
            Registry.Add("+93", UniforPlus9.GetExtension);
            Registry.Add("+94", UniforPlus9.GetDrive);
            Registry.Add("+95", UniforPlus9.StringLength);
            Registry.Add("+96", UniforPlus9.Substring);
            Registry.Add("+97", UniforPlus9.ToUpper);
            Registry.Add("+98", UniforPlus9.ReplaceCharacter);
            Registry.Add("+9A", UniforPlus9.GetFileSize);
            Registry.Add("+9C", UniforPlus9.GetFileContent);
            Registry.Add("+9E", UniforPlus9.FormatFileSize);
            Registry.Add("+9F", UniforPlus9.GetCharacter);
            Registry.Add("+9G", UniforPlus9.SplitWords);
            Registry.Add("+9I", UniforPlus9.ReplaceString);
            Registry.Add("+9L", UniforPlus9.FileExist);
            Registry.Add("+9S", UniforPlus9.FindSubstring);
            Registry.Add("+9T", UniforPlus9.PrintNumbers);
            Registry.Add("+9V", UniforPlus9.GetGeneration);
            Registry.Add("+D", UniforPlusD.GetDatabaseName);
            Registry.Add("+E", UniforPlusE.GetFieldIndex);
            Registry.Add("+F", UniforPlusF.CleanRtf);
            Registry.Add("+I", UniforPlusI.BuildLink);
            Registry.Add("+N", UniforPlusN.GetFieldCount);
            Registry.Add("+R", UniforPlusR.TrimAtLastDot);
            Registry.Add("+S", UniforPlusS.DecodeTitle);
            Registry.Add("+@", UniforPlusAt.FormatJson);
            Registry.Add("++0", UniforPlusPlus0.FormatAll);
            Registry.Add("+\\", UniforPlusBackslash.ConvertBackslashes);
            Registry.Add("!", UniforBang. CleanDoubleText);
            Registry.Add("=", UniforEqual.CompareWithMask);
            Registry.Add("[", UniforSquareBracket.CleanContextMarkup);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Find action for specified expression.
        /// </summary>
        public static Action<PftContext, PftNode, string> FindAction
            (
                [NotNull] ref string expression
            )
        {
            var keys = Registry.Keys;
            int bestMatch = 0;
            Action<PftContext, PftNode, string> result = null;

            StringComparison comparison = StringUtility.GetCaseInsensitiveComparison();
            foreach (string key in keys)
            {
                if (key.Length > bestMatch
                    && expression.StartsWith(key, comparison))
                {
                    bestMatch = key.Length;
                    result = Registry[key];
                }
            }

            if (bestMatch != 0)
            {
                expression = expression.Substring(bestMatch);
            }

            return result;
        }

        // ================================================================

        #endregion

        #region IFormatExit members

        /// <inheritdoc cref="IFormatExit.Name" />
        public string Name { get { return "unifor"; } }

        /// <inheritdoc cref="IFormatExit.Execute" />
        public void Execute
            (
                PftContext context,
                PftNode node,
                string expression
            )
        {
            Code.NotNull(context, "context");

            if (string.IsNullOrEmpty(expression))
            {
                Log.Error
                    (
                        "Unifor::Execute: "
                        + "empty expression: "
                        + this
                    );

                if (ThrowOnEmpty)
                {
                    throw new PftSemanticException
                        (
                            "Unifor::Execute: "
                            + "empty expression: "
                            + this
                        );
                }

                return;
            }

            Action<PftContext, PftNode, string> action
                = FindAction(ref expression);

            if (ReferenceEquals(action, null))
            {
                Log.Error
                    (
                        "Unifor::Execute: "
                        + "unknown action="
                        + expression.ToVisibleString()
                    );

                if (ThrowOnUnknown)
                {
                    throw new PftException
                        (
                            "Unknown unifor: "
                            + expression.ToVisibleString()
                        );
                }
            }
            else
            {
                action
                    (
                        context,
                        node,
                        expression
                    );
            }
        }

        #endregion
    }
}
