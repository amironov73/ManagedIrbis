// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Umid.cs -- Unique Material Identifier.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // https://ru.wikipedia.org/wiki/UMID
    //
    // UMID (англ. Unique Material Identifier) - уникальный идентификатор
    // аудиовизуального материала, определённый в SMPTE 330M.
    // Это специальный глобально-уникальный 64-байтный код
    // (пример: 359ABAEB603805D808004602022F7EA5), автоматически
    // генерируемый локально по ходу порождения материала и внедряемый
    // в медиа-файл или медиа-поток. Призван упростить дальнейший поиск,
    // отслеживание, предоставление доступа к медиа-материалу.
    // Основной задачей UMID является идентификация материала
    // в системах хранения данных, в течение всего процесса последующей
    // обработки, вещания/распространения. В частности, для связи материала
    // и соответствующих ему метаданных. Каждый отдельно отснятый фрагмент
    // получает свой UMID.
    //
    // Формат UMID состоит из двух частей по 32 байта каждая:
    //
    // * обязательная базовая часть, которая содержит:
    //   * универсальный идентификатор маркера SMPTE-UMID
    //   * длина UMID
    //   * номер, идентифицирующий копию
    //   * уникальный номер, идентифицирующий фрагмент или материал
    //
    // * сигнатура:
    //   * дата и время создания с точностью до фрейма
    //   * пространственные координаты камеры при съёмке оригинала
    //   * код страны
    //   * код организации
    //   * код, используемый производителем.
    //
    // UMID сыграл большую роль в распространении открытых форматов,
    // таких как MXF (Material Exchange Format) и AAF (Advanced Authoring
    // Format), и сегодня поддерживается ведущими производителями
    // в аудиовизульной индустрии.
    //

    /// <summary>
    ///
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class Umid
    {
        #region Properties

        /// <summary>
        /// Value.
        /// </summary>
        public string Value { get; set; }

        #endregion
    }
}
