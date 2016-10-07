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
        public static RecordReport MergeReport
            (
                RecordReport first,
                RecordReport second
            )
        {
            RecordReport result = new RecordReport
            {
                Defects = new DefectList
                    (
                        first.Defects.Concat(second.Defects)
                    ),
                Description = first.Description,
                Gold = first.Gold + second.Gold - 1000,
                Mfn = first.Mfn,
                Index = first.Index
            };

            return result;
        }


        /// <summary>
        /// Проверка одной записи
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public RecordReport CheckRecord
            (
                RuleContext context
            )
        {
            RecordReport result = new RecordReport
            {
                Description = context.Connection.FormatRecord
                (
                    context.BriefFormat,
                    context.Record.Mfn
                ),
                Index = context.Record.FM("903"),
                Mfn = context.Record.Mfn
            };
            RuleUtility.RenumberFields
                (
                    context.Record
                );

            result.Gold = 1000;
            int bonus = 0;

            foreach (QualityRule rule in Rules)
            {
                RuleReport oneReport = rule.CheckRecord(context);
                result.Defects.AddRange(oneReport.Defects);
                result.Gold -= oneReport.Damage;
                bonus += oneReport.Bonus;
            }

            if (result.Gold >= 900)
            {
                result.Gold += bonus;
            }

            return result;
        }

        /// <summary>
        /// Получаем правило по его имени.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QualityRule GetRule
            (
                string name
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static RuleSet LoadJson
            (
                string fileName
            )
        {
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

#if !NETCORE

        /// <summary>
        /// Регистрируем все правила из указанной сборки.
        /// </summary>
        /// <param name="assembly"></param>
        public static void RegisterAssembly
            (
                Assembly assembly
            )
        {
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
        }

        /// <summary>
        /// Регистрация встроенных правил.
        /// </summary>
        public static void RegisterBuiltinRules ()
        {
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

#endif

        /// <summary>
        /// Register rule from type.
        /// </summary>
        public static void RegisterRule
            (
                Type ruleType
            )
        {
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
                string name
            )
        {
            if (_registeredRules.ContainsKey(name))
            {
                _registeredRules.Remove(name);
            }
        }

        #endregion
    }
}
