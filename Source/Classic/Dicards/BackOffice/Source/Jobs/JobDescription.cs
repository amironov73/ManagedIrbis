// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* JobDescription.cs -- совмещенное описание задания и триггера
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace BackOffice.Jobs
{
    /// <summary>
    /// Совмещенное описание задания и триггера.
    /// </summary>
    public sealed class JobDescription
    {
        #region Properties
        
        /// <summary>
        /// Идентификатор задания.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Группа.
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }
        
        /// <summary>
        /// Словесное описание задания.
        /// </summary>
        [JsonProperty ("description")]
        public string Description { get; set; }
        
        /// <summary>
        /// Задание разрешено?
        /// </summary>
        [JsonProperty ("enabled")]
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Тип задания (полностью специфицированный,
        /// с указанием сборки).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        
        /// <summary>
        /// Cron-выражение.
        /// </summary>
        [JsonProperty("cron")]
        public string CronExpression { get; set; }
        
        #endregion

        #region Public methods

        /// <summary>
        /// Загрузка массива заданий из файла.
        /// </summary>
        public static JobDescription[] ReadJobs
            (
                string fileName
            )
        {
            var text = File.ReadAllText(fileName);
            var rootObject = JObject.Parse(text);
            var firstProperty = (JProperty) rootObject.First;
            if (ReferenceEquals(firstProperty, null))
            {
                throw new ApplicationException("Bad jobs file format");
            }
            
            var array = (JArray) firstProperty.Value; 
            var result = array.ToObject<JobDescription[]>();

            return result;
        }

        #endregion
        
        #region Object members

        /// <inheritdoc />
        public override string ToString()
        {
            return Name ?? "(null)";
        }

        #endregion
    }
}
