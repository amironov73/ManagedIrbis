// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExtensionInfo.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

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
    [XmlRoot("extension")]
    public sealed class ExtensionInfo
        : IHandmadeSerializable,
        IVerifiable
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
        [XmlAttribute("dll")]
        [JsonProperty("dll", NullValueHandling = NullValueHandling.Ignore)]
        public string DllName { get; set; }

        /// <summary>
        /// Exported function name.
        /// </summary>
        /// <remarks>UMFUNCTIONn</remarks>
        [CanBeNull]
        [XmlAttribute("function")]
        [JsonProperty("function", NullValueHandling = NullValueHandling.Ignore)]
        public string FunctionName { get; set; }

        /// <summary>
        /// PFT script name.
        /// </summary>
        /// <remarks>UMPFTn</remarks>
        [CanBeNull]
        [XmlAttribute("pft")]
        [JsonProperty("pft", NullValueHandling = NullValueHandling.Ignore)]
        public string PftName { get; set; }

        /// <summary>
        /// Group number.
        /// </summary>
        /// <remarks>UMGROUPn</remarks>
        [XmlAttribute("group")]
        [JsonProperty("group", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int GroupNumber { get; set; }

        /// <summary>
        /// Name for menu item and tooltip.
        /// </summary>
        /// <remarks>UMNAMEn</remarks>
        [CanBeNull]
        [XmlAttribute("name")]
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Icon name.
        /// </summary>
        /// <remarks>UMICONn</remarks>
        [CanBeNull]
        [XmlAttribute("icon")]
        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string IconName { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [CanBeNull]
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object UserData { get; set; }

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

        #region Public methods

        /// <summary>
        /// Create array of <see cref="ExtensionInfo"/>
        /// from <see cref="IniFile"/> entries.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public static ExtensionInfo[] FromIniFile
            (
                [NotNull] IniFile iniFile
            )
        {
            Code.NotNull(iniFile, "iniFile");

            int count = 0;
            List<ExtensionInfo> result = new List<ExtensionInfo>();
            IniFile.Section section = iniFile.GetSection(ExtensionManager.USERMODE);
            if (!ReferenceEquals(section, null))
            {
                count = section.GetValue(ExtensionManager.UMNUMB, 0);
                for (int i = 0; i < count; i++)
                {
                    ExtensionInfo extension = FromIniFile(section, i);
                    if (!ReferenceEquals(extension, null))
                    {
                        result.Add(extension);
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Create <see cref="ExtensionInfo"/>
        /// from <see cref="IniFile"/> entries.
        /// </summary>
        [CanBeNull]
        public static ExtensionInfo FromIniFile
            (
                [NotNull] IniFile.Section section,
                int index
            )
        {
            Code.NotNull(section, "section");
            Code.Nonnegative(index, "index");

            string dllName = section["UMDLL" + index];
            if (string.IsNullOrEmpty(dllName))
            {
                return null;
            }

            ExtensionInfo result = new ExtensionInfo
            {
                Index = index,
                DllName = dllName,
                FunctionName = section["UMFUNCTION" + index]
                    .ThrowIfNull("UMFUNCTION"),
                PftName = section["UMPFT" + index]
                    .ThrowIfNull("UMPFT"),
                GroupNumber = section.GetValue("UMGROUP" + index, 1),
                Name = section["UMNAME" + index]
                    .ThrowIfNull("UMNAME"),
                IconName = section["UMICON" + index]
                    .ThrowIfNull("UMICON")
            };

            return result;
        }

        /// <summary>
        /// Update the INI-file.
        /// </summary>
        public static void UpdateIniFile
            (
                [NotNull] IniFile iniFile,
                [NotNull] ExtensionInfo[] extensions
            )
        {
            Code.NotNull(iniFile, "iniFile");
            Code.NotNull(extensions, "extensions");

            IniFile.Section section = iniFile.GetOrCreateSection(ExtensionManager.USERMODE);
            section.SetValue(ExtensionManager.UMNUMB, extensions.Length);
            for (int i = 0; i < extensions.Length; i++)
            {
                extensions[i].UpdateIniFile(section);
            }
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

            section["UMDLL" + Index] = DllName
                .ThrowIfNull("DllName");
            section["UMFUNCTION" + Index] = FunctionName
                .ThrowIfNull("FunctionName");
            section["UMPFT" + Index] = PftName
                .ThrowIfNull("PftName");
            section["UMGROUP" + Index] = GroupNumber
                .ToInvariantString();
            section["UMNAME" + Index] = Name
                .ThrowIfNull("Name");
            section["UMICON" + Index] = IconName
                .ThrowIfNull("IconName");
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Index = reader.ReadPackedInt32();
            DllName = reader.ReadNullableString();
            FunctionName = reader.ReadNullableString();
            PftName = reader.ReadNullableString();
            GroupNumber = reader.ReadPackedInt32();
            Name = reader.ReadNullableString();
            IconName = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WritePackedInt32(Index)
                .WriteNullable(DllName)
                .WriteNullable(FunctionName)
                .WriteNullable(PftName)
                .WritePackedInt32(GroupNumber)
                .WriteNullable(Name)
                .WriteNullable(IconName);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ExtensionInfo> verifier
                = new Verifier<ExtensionInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(DllName, "DllName")
                .NotNullNorEmpty(FunctionName, "FunctionName");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
