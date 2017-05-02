// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MenuSpecification.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Menus
{
    //
    // Official documentation says:
    //
    // 1 - ввод через простое меню (неиерархический справочник).
    //
    // Параметр ДОП.ИНФ. имеет следующую структуру:
    // <Menu_file_name>\<SYS|DBN>,<N>\<MnuSort> где:
    // <Menu_file_name> - имя файла справочника (с расширением);
    // <SYS|DBN>,<N> - указывает путь, по которому находится
    // файл справочника. Может принимать следующие значения: 
    // SYS,0 - директория исполняемых модулей; 
    // SYS,N - (N>0) рабочая директория (указываемая в параметре WORKDIR);
    // DBN,N - директория БД ввода (N - любая цифра);
    // <MnuSort> - порядок сортировки справочника:
    // 0-без сортировки;
    // 1-по значениям (по элементам меню);
    // 2-по пояснениям.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MenuSpecification
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// File name (with extension).
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [CanBeNull]
        public string Database { get; set; }

        /// <summary>
        /// Path.
        /// </summary>
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Sort mode.
        /// </summary>
        public int SortMode { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Convert <see cref="FileSpecification"/> to menu specification.
        /// </summary>
        [NotNull]
        public static MenuSpecification FromFileSpecification
            (
                [NotNull] FileSpecification specification
            )
        {
            Code.NotNull(specification, "specification");

            MenuSpecification result = new MenuSpecification
            {
                Database = specification.Database,
                Path = specification.Path,
                FileName = specification.FileName
            };

            return result;
        }

        /// <summary>
        /// Parse the text.
        /// </summary>
        [NotNull]
        public static MenuSpecification Parse
            (
                [CanBeNull] string text
            )
        {
            MenuSpecification result = new MenuSpecification
            {
                Path = IrbisPath.MasterFile
            };

            if (!string.IsNullOrEmpty(text))
            {
                TextNavigator navigator = new TextNavigator(text);
                result.FileName = navigator.ReadUntil('\\');
                if (!navigator.IsEOF)
                {
                    string db = navigator.ReadUntil('\\');

                    // TODO: implement properly

                    result.Database = db; 

                    if (!navigator.IsEOF)
                    {
                        string sortText = navigator.GetRemainingText();
                        int sortMode;
                        int.TryParse(sortText, out sortMode);
                        result.SortMode = sortMode;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Convert menu specification to <see cref="FileSpecification"/>.
        /// </summary>
        [NotNull]
        public FileSpecification ToFileSpecification()
        {
            FileSpecification result = new FileSpecification
                (
                    Path,
                    Database,
                    FileName.ThrowIfNull("FileName")
                );

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            FileName = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Path = (IrbisPath) reader.ReadPackedInt32();
            SortMode = reader.ReadPackedInt32();
        }

        /// <inheritdoc/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Code.NotNull(writer, "writer");

            writer
                .WriteNullable(FileName)
                .WriteNullable(Database)
                .WritePackedInt32((int)Path)
                .WritePackedInt32(SortMode);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<MenuSpecification> verifier
                = new Verifier<MenuSpecification>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(FileName, "FileName");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc/>
        public override string ToString()
        {
            return FileName.ToVisibleString();
        }

        #endregion
    }
}
