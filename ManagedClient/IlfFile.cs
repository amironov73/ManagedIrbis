/* IlfFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    //
    // ILF - Архив текстовых файлов Irbis Library Files
    //
    // ILF-файлы – специфические для ИРБИС текстовые файлы,
    // содержащие независимые поименованные разделы.
    //
    // Могут использоваться для хранения основных текстовых
    // ресурсов баз данных: форматов (PFT), рабочих листов (WS),
    // вложенных РЛ (WSS), справочников (MNU),
    // таблиц переформатирования (FST) и др.
    //
    // При этом предлагается следующая структура имен ILF-файлов:
    // <ИМЯ_БД>_<ТИП>.ILF
    // Например:
    // Ibis_pft.ilf – ILF-файл для хранения форматов БД IBIS.
    //
    // С форума:
    //
    // * Сервер ищет файлы сначала в ilf затем в директории БД.
    // * Сервер пересылает клиенту ИРБИС64 файлы по одному.
    // * Клиент ИРБИС64 кэширует скачанные файлы. Так чтобы 
    // заметить изменения, например в рабочих листах,
    // нужно выполнить режим ОБНОВИТЬ КОНТЕКСТ.
    // * Распаковка ILF - это задача исключительно сервера.

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class ILfFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region IHandmadeSerializable members

        public void RestoreFromStream(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public void SaveToStream(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        public bool Verify(bool throwOnError)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
