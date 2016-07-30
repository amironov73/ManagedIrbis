/* GblParameter.cs -- parameter for GBL file
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
using System.Xml.Serialization;
using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Gbl
{
    //
    // Первая строка файла задания – это число, задающее
    // количество параметров, используемых в операторах корректировки.
    // 
    // Последующие пары строк, число пар должно быть равно
    // количеству параметров, используются программой
    // глобальной корректировки.
    // 
    // Первая строка пары - значение параметра или пусто,
    // если пользователю предлагается задать его значение
    // перед выполнением корректировки. В этой строке можно
    // задать имя файла меню (с расширением MNU)
    // или имя рабочего листа подполей (с расширением Wss),
    // которые будут поданы для выбора значения параметра.
    // Вторая строка пары – наименование параметра,
    // которое появится в названии столбца, задающего параметр.
    //

    /// <summary>
    /// Parameter for GBL file.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [XmlRoot("gbl-parameter")]
    [DebuggerDisplay("[{Name}] {Value}")]
    public sealed class GblParameter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя параметра.
        /// </summary>
        [CanBeNull]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Значение параметра.
        /// </summary>
        [CanBeNull]
        [JsonProperty("value")]
        [XmlAttribute("value")]
        public string Value { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse specifiedStream.
        /// </summary>
        [NotNull]
        public static GblParameter ParseStream
            (
                [NotNull] TextReader reader
            )
        {
            GblParameter result = new GblParameter
            {
                Value = reader.RequireLine(),
                Name = reader.RequireLine()
            };

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from given stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <summary>
        /// Save object state to given stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members


        /// <summary>
        /// Verify object state.
        /// </summary>
        public bool Verify
            (
                bool throwOnError
            )
        {
            bool result = !string.IsNullOrEmpty(Name)
                          && !string.IsNullOrEmpty(Value);

            if (!result && throwOnError)
            {
                throw new VerificationException();
            }

            return result;
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" />
        /// that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" />
        /// that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format
                (
                    "Name: {0}, Value: {1}",
                    Name,
                    Value
                );
        }

        #endregion
    }
}
