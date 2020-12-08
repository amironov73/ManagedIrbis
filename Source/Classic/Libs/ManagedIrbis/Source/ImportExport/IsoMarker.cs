// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IsoMarker.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Runtime.InteropServices;

#endregion

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

namespace ManagedIrbis.ImportExport
{
    /// <summary>
    /// Маркер записи.
    /// </summary>
    /// <remarks>
    /// <para>Маркер располагается в начале каждой записи. Является обязательным.
    /// Не повторяется.</para>
    /// <para><b>Определение.</b> Маркер записи - это область записи, содержащая 
    /// общую информацию, используемую при обработке записей, подготавливаемых 
    /// в соответствии с положениями ISO 2709.</para>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct IsoMarker
    {
        /// <summary>
        /// Длина маркера.
        /// </summary>
        public const int MarkerLength = 24;

        /// <summary>
        /// Длина записи.
        /// </summary>
        /// <remarks><b>Определение</b>. Пять десятичных цифр, при необходимости
        /// выравниваемых вправо начальными нулями, указывают количество символов
        /// в записи, включая маркер записи, справочник и поля переменной длины.
        /// Определяется автоматически, когда запись окончательно сформирована
        /// для обмена.</remarks>
        [FieldOffset(0)]
        public byte len0;

        /// <summary>
        /// Продолжение.
        /// </summary>

        [FieldOffset(1)]
        public byte len1;
        /// <summary>
        /// Продолжение.
        /// </summary>

        [FieldOffset(2)]
        public byte len2;
        /// <summary>
        /// Продолжение.
        /// </summary>

        [FieldOffset(3)]
        public byte len3;
        /// <summary>
        /// Продолжение.
        /// </summary>
        [FieldOffset(4)]
        public byte len4;

        /// <summary>
        /// Статус записи.
        /// </summary>
        [FieldOffset(5)]
        public byte RecordStatus;

        /// <summary>
        /// Тип записи.
        /// </summary>
        [FieldOffset(6)]
        public byte RecordType;

        /// <summary>
        /// Библиографический указатель.
        /// </summary>
        [FieldOffset(7)]
        public byte BibliographicalIndex;

        /// <summary>
        /// Не используется.
        /// </summary>
        [FieldOffset(8)]
        public byte unused0;

        /// <summary>
        /// Не используется.
        /// </summary>
        [FieldOffset(9)]
        public byte unused1;

        /// <summary>
        /// Длина индикатора.
        /// </summary>
        [FieldOffset(10)]
        public byte IndicatorLength;

        /// <summary>
        /// Длина идентификатора.
        /// </summary>
        [FieldOffset(11)]
        public byte IdentifierLength;

        /// <summary>
        /// Базовый адрес.
        /// </summary>
        [FieldOffset(12)]
        public byte base0;

        /// <summary>
        /// Продолжение.
        /// </summary>
        [FieldOffset(13)]
        public byte base1;

        /// <summary>
        /// Продолжение.
        /// </summary>
        [FieldOffset(14)]
        public byte base2;

        /// <summary>
        /// Продолжение.
        /// </summary>
        [FieldOffset(15)]
        public byte base3;

        /// <summary>
        /// Продолжение.
        /// </summary>
        [FieldOffset(16)]
        public byte base4;

        /// <summary>
        /// Уровень описания.
        /// </summary>
        [FieldOffset(17)]
        public byte BibliographicalLevel;

        /// <summary>
        /// Правила каталогизации.
        /// </summary>
        [FieldOffset(18)]
        public byte CataloguisationRules;

        /// <summary>
        /// Наличие связанной записи.
        /// </summary>
        [FieldOffset(19)]
        public byte RelatedRecord;

        /// <summary>
        /// "Длина длины".
        /// </summary>
        [FieldOffset(20)]
        public byte LengthOfLength;

        /// <summary>
        /// "Длина смещения".
        /// </summary>
        [FieldOffset(21)]
        public byte OffsetLength;

        /// <summary>
        /// Всегда 0.
        /// </summary>
        [FieldOffset(22)]
        public byte zero0;

        /// <summary>
        /// Всегда 0.
        /// </summary>
        [FieldOffset(23)]
        public byte zero1;
    }
}
