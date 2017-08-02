// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TbuFile.cs -- 
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
using AM.Logging;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Tables
{
    //
    // Official documentation
    //
    // Файл с расширением TBU представляет из себя файл описания формы. Он содержит три секции:
    // [FormatCode]
    // [Tab]
    // [Header]
    //
    // В секции FormatCode указывается кодировка данных.
    // Обычно это кодировка WIN.
    // После указания кодировки должен следовать
    // признак логического конца секции ***** (5 звезд).
    // Таким образом в частном случае секция FormatCode
    // практически всегда имеет вид:
    // [FormatCode]
    // WIN
    // *****
    //
    // Секция Tab задает начало форматирования документа.
    // Обычно в этой секции указываются строки,
    // инициализирующие размер страницы и начало тела документа.
    // Концом этой секции считается объявление следующей секции Header.
    //
    // Секция Header содержит строки, которыми будет закрыты данные,
    // сформированные из файла с расширением SRW.
    // Обычно это команды закрытия заголовочной части формы.
    //

    //
    // Пример секции Tab:
    //
    // [Tab]
    // \paperw11907\paperh16727\margl1134\margr1134\margt567\margb1134 
    // {\b\fs24
    // БИБЛИОГРАФИЧЕСКИЙ УКАЗАТЕЛЬ КНИГ, ПОСТУПИВШИХ В БИБЛИОТЕКУ
    // \par
    // }
    // {}
    // {\b\fs32 \qc
    //

    //
    // Пример секции Header:
    //
    // [Header]
    // \b0
    // }
    // {}
    // \par }
    //
    /// <summary>
    /// Файл описания формы ИРБИС64.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TbuFile
    {
        #region Properties

        /// <summary>
        /// FormatCode section.
        /// </summary>
        [CanBeNull]
        [XmlElement("encoding")]
        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        /// <summary>
        /// Table section.
        /// </summary>
        [CanBeNull]
        [XmlElement("table")]
        [JsonProperty("table")]
        public string Table { get; set; }

        /// <summary>
        /// Header section.
        /// </summary>
        [CanBeNull]
        [XmlElement("header")]
        [JsonProperty("header")]
        public string Header { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        #endregion
    }
}
