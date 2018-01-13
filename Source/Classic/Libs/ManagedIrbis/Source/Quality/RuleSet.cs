// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RuleSet.cs
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Набор правил.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class RuleSet
    {
        #region Properties

        /// <summary>
        /// Правила, входящие в набор.
        /// </summary>
        [JsonProperty("rules")]
        public QualityRule[] Rules { get; set; }

        #endregion

        #region Private members

        private static readonly Dictionary<string,Type> _registeredRules
            = new Dictionary<string, Type>();

        #endregion

        #region Public methods

        /// <summary>
        /// Merge two reports.
        /// </summary>
        [NotNull]
        public static RecordReport MergeReport
            (
                [NotNull] RecordReport first,
                [NotNull] RecordReport second
            )
        {
            Code.NotNull(first, "first");
            Code.NotNull(second, "second");

            RecordReport result = new RecordReport
            {
                Defects = new DefectList
                    (
                        first.Defects.Concat(second.Defects)
                    ),
                Description = first.Description,
                Quality = first.Quality + second.Quality - 1000,
                Mfn = first.Mfn,
                Index = first.Index
            };

            return result;
        }


        /// <summary>
        /// Проверка одной записи
        /// </summary>
        [NotNull]
        public RecordReport CheckRecord
            (
                [NotNull] RuleContext context
            )
        {
            Code.NotNull(context, "context");

            RecordReport result = new RecordReport
            {
                Description = context.Connection.FormatRecord
                    (
                        context.BriefFormat,
                        context.Record.Mfn
                    ),
                Index = context.Record.FM(903),
                Mfn = context.Record.Mfn
            };
            RuleUtility.RenumberFields
                (
                    context.Record
                );

            result.Quality = 1000;
            int bonus = 0;

            foreach (QualityRule rule in Rules)
            {
                RuleReport oneReport = rule.CheckRecord(context);
                result.Defects.AddRange(oneReport.Defects);
                result.Quality -= oneReport.Damage;
                bonus += oneReport.Bonus;
            }

            if (result.Quality >= 900)
            {
                result.Quality += bonus;
            }

            return result;
        }

        /// <summary>
        /// Получаем правило по его имени.
        /// </summary>
        [CanBeNull]
        public static QualityRule GetRule
            (
                [NotNull] string name
            )
        {
            Type ruleType;
            if (!_registeredRules.TryGetValue
                (
                    name, 
                    out ruleType)
                )
            {
                return null;
            }

            QualityRule result = (QualityRule)Activator.CreateInstance
                (
                    ruleType
                );

            return result;
        }

#if !WINMOBILE && !PocketPC

        /// <summary>
        /// Load set of rules from the specified file.
        /// </summary>
        [NotNull]
        public static RuleSet LoadJson
            (
                [NotNull] string fileName
            )
        {
            Code.NotNullNorEmpty(fileName, "fileName");

            string text = File.ReadAllText(fileName);
            JObject obj = JObject.Parse(text);
            
            RuleSet result = new RuleSet();
            List<QualityRule> rules = new List<QualityRule>();

            foreach (JToken o in obj["rules"])
            {
                string name = o.ToString();
                QualityRule rule = GetRule(name);
                if (rule != null)
                {
                    rules.Add(rule);
                }
            }

            result.Rules = rules.ToArray();

            return result;
        }

        /// <summary>
        /// Регистрируем все правила из указанной сборки.
        /// </summary>
        public static void RegisterAssembly
            (
                [NotNull] Assembly assembly
            )
        {
            Code.NotNull(assembly, "assembly");

#if UAP

            throw new NotImplementedException();

#else

            Type[] types = assembly
                .GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(QualityRule)))
                .ToArray();
            foreach (Type ruleType in types)
            {
                RegisterRule(ruleType);
            }

#endif
        }

        /// <summary>
        /// Регистрация встроенных правил.
        /// </summary>
        public static void RegisterBuiltinRules ()
        {
#if UAP

            // TODO Implement

#else

            RegisterAssembly(Assembly.GetExecutingAssembly());

#endif
        }

#endif

        /// <summary>
        /// Register rule from type.
        /// </summary>
        public static void RegisterRule
            (
                [NotNull] Type ruleType
            )
        {
            Code.NotNull(ruleType, "ruleType");

            string ruleName = ruleType.Name;

            _registeredRules.Add
                (
                    ruleName,
                    ruleType
                );
        }

        /// <summary>
        /// Отменяем регистрацию правила с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        public static void UnregisterRule
            (
                [NotNull] string name
            )
        {
            Code.NotNullNorEmpty(name, "name");

            if (_registeredRules.ContainsKey(name))
            {
                _registeredRules.Remove(name);
            }
        }

        #endregion
    }
}
