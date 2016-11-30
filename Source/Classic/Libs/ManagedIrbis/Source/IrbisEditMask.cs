// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisEditMask.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM.IO;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis
{
    //
    // ! Наличие символа «!» означает, что в EditText недостающие
    //   символы предваряются пробелами, а отсутствие символа «!»
    //   означает, что пробелы размещаются в конце.
    // > Символ «>» означает, что все последующие за ним символы
    //   должны вводиться в верхнем регистре, пока не кончится маска
    //   или пока не встретится символ «<».
    // < Символ «<» означает, что все последующие за ним символы
    //   должны вводиться в нижнем регистре, пока не кончится маска
    //   или пока не встретится символ «>».
    // <> Символы «<>» означают, что анализ регистра не производится.
    // \ Символ «\» означает, что следующий за ним символ является
    //   буквенным, а не специальным, характерным для маски.
    //   Например, символ «>» после символа «\» воспримется
    //   как знак >, а не как символ, указывающий на верхний регистр.
    // L Символ «L» означает, что в данной позиции должна бытъ буква.
    // l Символ «l» означает, что в данной позиции может быть только
    //   буква или ничего.
    // A Символ «А» означает, что в данной позиции должна быть
    //   буква или цифра.
    // a Символ «а» означает, что в данной позиции может быть буква,
    //   или цифра, или ничего.
    // C Символ «С» означает, что в данной позиции должен быть любой
    //   символ.
    // c Символ «с» означает, что в данной позиции может быть любой
    //   символ или ничего.
    // 0 Символ «0» означает, что в данной позиции должна быть цифра.
    // 9 Символ «9» означает, что в данной позиции может быть цифра
    //   или ничего.
    // # Символ «#» означает, что в данной позиции может быть цифра,
    //   знак «+», знак «-» или ничего.
    // : Символ «:» используется для разделения часов, минут и секунд.
    // / Символ «/» используется для разделения месяцев, дней
    //   и годов в датах.
    // " " Символ « » означает автоматическую вставку в текст пробела.



    /// <summary>
    /// Маска ввода в ИРБИС (Delphi)
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class IrbisEditMask
        : IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Маска
        /// </summary>
        [CanBeNull]
        public string Mask { get { return _mask; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisEditMask()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public IrbisEditMask
            (
                [NotNull] string mask
            )
        {
            Code.NotNull(mask, "mask");
        }

        #endregion

        #region Private members

        private string _mask;

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение текста с маской.
        /// </summary>
        public bool Match
            (
                [NotNull] string text
            )
        {
            return false;
        }

        #endregion

        #region IHandmadeSerializable

        /// <summary>
        /// Просим объект восстановить свое состояние из потока.
        /// </summary>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            _mask = reader.ReadNullableString();
        }

        /// <summary>
        /// Просим объект сохранить себя в потоке.
        /// </summary>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Mask);
        }

        #endregion
    }
}
