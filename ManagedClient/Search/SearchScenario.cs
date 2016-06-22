/* SearchScenario.cs -- search scenario
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Search
{
    //
    // Для описания одного вида поиска по словарю служат следующие параметры
    // Имя параметра Содержание
    // ItemAdvNN правила автоматического расширения поиска
    // ItemDictionTypeNN тип словаря
    // ItemF8ForNN имя формата (без расширения)
    // ItemHintNN текст подсказки/предупреждения
    // ItemLogicNN логические операторы
    // ItemMenuNN имя файла справочника
    // ItemModByDicAutoNN не задействован
    // ItemModByDicNN способ Корректировки по словарю
    // ItemNameNN название поиска
    // ItemNumb общее количество поисков по словарю
    // ItemPftNN имя формата показа документов
    // ItemPrefNN префикс инверсии
    // ItemTrancNN исходное положение переключателя Усечение
    //
    // где NN - порядковый номер вида поиска по словарю
    // в общем списке (начиная с 0).


    /// <summary>
    /// Search scenario
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class SearchScenario
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Название поиска.
        /// </summary>
        /// <remarks>Например: ItemName5=Заглавие</remarks>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Префикс для соответствующих терминов в словаре.
        /// </summary>
        /// <remarks>Например: ItemPref5=Т=</remarks>
        [CanBeNull]
        public string Prefix { get; set; }

        /// <summary>
        /// Тип словаря для соответствующего поиска.
        /// </summary>
        /// <remarks>Например: ItemDictionType8=1</remarks>
        public DictionaryType DictionaryType { get; set; }

        /// <summary>
        /// Имя файла справочника.
        /// </summary>
        /// <remarks>Например: ItemMenu8=str.mnu</remarks>
        [CanBeNull]
        public string MenuName { get; set; }

        /// <summary>
        /// Имя формата (без расширения).
        /// </summary>
        /// <remarks>Параметр  служит для указания имени формата
        /// (без расширения), который используется при показе
        /// термина словаря полностью (Приложение 4 п.13).
        /// Используется только в ИРБИС32, т.е. в ИРБИС64
        /// не используется.
        /// Используется для длинных терминов (больше 30 символов).
        /// Например: ItemF8For5=!F8TIT.
        /// </remarks>
        [CanBeNull]
        public string OldFormat { get; set; }

        /// <summary>
        /// Способ Корректировки по словарю.
        /// </summary>
        [CanBeNull]
        public string Correction { get; set; }

        /// <summary>
        /// Исходное положение переключателя "Усечение".
        /// </summary>
        /// <remarks>Параметр  определяет исходное положение
        /// переключателя "Усечение" для данного вида поиска
        /// (0 - нет; 1 - да) - действует только в АРМе "Каталогизатор".
        /// </remarks>
        public bool Truncation { get; set; }

        /// <summary>
        /// Текст подсказки/предупреждения.
        /// </summary>
        [CanBeNull]
        public string Hint { get; set; }

        /// <summary>
        /// Параметр пока не задействован.
        /// </summary>
        [CanBeNull]
        public string ModByDicAuto { get; set; }

        /// <summary>
        /// Логические операторы.
        /// </summary>
        public SearchLogicType Logic { get; set; }

        /// <summary>
        /// Правила автоматического расширения поиска
        /// на основе авторитетного файла или тезауруса.
        /// </summary>
        /// <remarks>Параметр имеет следующую структуру:
        /// Dbname,Prefix,Format.
        /// </remarks>
        [CanBeNull]
        public string Advance { get; set; }

        /// <summary>
        /// Имя формата показа документов.
        /// </summary>
        [CanBeNull]
        public string Format { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        /// <summary>
        /// Restore object state from the stream.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            
        }

        /// <summary>
        /// Save object state to the stream.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
        }

        #endregion

        #region Object members

        #endregion
    }
}
