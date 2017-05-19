// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExtensionInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Xml.Serialization;

using AM;
using AM.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion


namespace ManagedIrbis.Extensibility
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    // http://sntnarciss.ru/irbis/spravka/wc01000000000.htm
    //
    // Предлагается возможность подключения пользовательских режимов,
    // т.е. режимов, созданных самим пользователем-разработчиком.
    //
    // Это означает, что пользователь может "повесить" собственные
    // режимы обработки (как пакетные, так и интерактивные)
    // в главном меню и на панели инструментов АРМа «Каталогизатор».
    // Предполагается, что режим пользователя представляет собой
    // функцию DLL (созданную по тем же правилам, что и для
    // форматного выхода &unifor('+8...) – Приложение 4).
    //
    // Режимы пользователя описываются через INI-файл
    // (по умолчанию - irbisc.ini) в секции [USERMODE]
    // по следующей схеме.
    //
    // Каждый внешний режим пользователя описывается набором
    // следующих параметров:
    //
    // UMDLLn - имя DLL (в случае нестандартного вызова функций
    // перед именем DLL следует поставить символ*);
    //
    // UMFUNCTIONn - имя функции DLL, реализующей данный режим.
    // Следует иметь в виду, что имя функции DLL надо указывать
    // точно так, как она экспортирована из DLL (т. е. большие
    // и малые буквы различаются);
    //
    // UMPFTn - имя формата, в соответствии с которым осуществляется
    // передача входных данных в функцию (формат применяется
    // к текущей записи);
    //
    // UMNAMEn - название режима на естественном языке
    // (для главного меню и подсказки);
    //
    // UMGROUPn - порядковый номер группы режимов в главном меню;
    //
    // UMICONn – имя иконки для отображения данного режима
    // на панели инструментов (иконка включается в DLL в качестве
    // ресурса типа ICON размером 16х16).
    //
    // n – порядковый номер режим в списке (начиная с 0).
    //
    // Общее количество режимов пользователя указывается
    // в параметре UMNUMB.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class ExtensionInfo
    {
        #region Properties

        /// <summary>
        /// Index (starting from 0).
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public int Index { get; set; }

        /// <summary>
        /// DLL file name (with extension).
        /// </summary>
        /// <remarks>UMDLLn</remarks>
        [CanBeNull]
        [JsonProperty("dll")]
        [XmlAttribute("dll")]
        public string DllName { get; set; }

        /// <summary>
        /// Exported function name.
        /// </summary>
        /// <remarks>UMFUNCTIONn</remarks>
        [CanBeNull]
        [JsonProperty("function")]
        [XmlAttribute("function")]
        public string FunctionName { get; set; }

        /// <summary>
        /// PFT script name.
        /// </summary>
        /// <remarks>UMPFTn</remarks>
        [CanBeNull]
        [JsonProperty("pft")]
        [XmlAttribute("pft")]
        public string PftName { get; set; }

        /// <summary>
        /// Group number.
        /// </summary>
        /// <remarks>UMGROUPn</remarks>
        [JsonProperty("group")]
        [XmlAttribute("group")]
        public int GroupNumber { get; set; }

        /// <summary>
        /// Name for menu item and tooltip.
        /// </summary>
        /// <remarks>UMNAMEn</remarks>
        [CanBeNull]
        [JsonProperty("name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Icon name.
        /// </summary>
        /// <remarks>UMICONn</remarks>
        [CanBeNull]
        [JsonProperty("icon")]
        [XmlAttribute("icon")]
        public string IconName { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExtensionInfo()
        {
            GroupNumber = 1;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Create <see cref="ExtensionInfo"/>
        /// from <see cref="IniFile"/> entries.
        /// </summary>
        [NotNull]
        public static ExtensionInfo FromIniFile
            (
                [NotNull] IniFile.Section section,
                int index
            )
        {
            Code.NotNull(section, "section");
            Code.Nonnegative(index, "index");

            ExtensionInfo result = new ExtensionInfo
            {
                Index = index,
                DllName = section["UMDLL" + index].ThrowIfNull("UMDLL"),
                FunctionName = section["UMFUNCTION" + index]
                    .ThrowIfNull("UMFUNCTION"),
                PftName = section["UMPFT" + index].ThrowIfNull("UMPFT"),
                GroupNumber = section.GetValue("UMGROUP" + index, 0),
                Name = section["UMNAME" + index].ThrowIfNull("UMNAME"),
                IconName = section["UMICON" + index].ThrowIfNull("UMICON")
            };

            return result;
        }

        /// <summary>
        /// Update <see cref="IniFile"/> entries.
        /// </summary>
        public void UpdateIniFile
            (
                [NotNull] IniFile.Section section
            )
        {
            Code.NotNull(section, "section");

            section["UMDLL" + Index] = DllName.ThrowIfNull("DllName");
            section["UMFUNCTION" + Index] = FunctionName
                .ThrowIfNull("FunctionName");
            section["UMPFT" + Index] = PftName.ThrowIfNull("PftName");
            section["UMGROUP" + Index] = GroupNumber.ToInvariantString();
            section["UMNAME" + Index] = Name.ThrowIfNull("Name");
            section["UMICON" + Index] = IconName.ThrowIfNull("IconName");
        }

        #endregion

        #region Object members

        #endregion
    }
}
