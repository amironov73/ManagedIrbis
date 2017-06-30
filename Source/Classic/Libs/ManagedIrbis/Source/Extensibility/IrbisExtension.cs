// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisExtension.cs -- 
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.ImportExport;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Extensibility
{
    //
    // EXTRACT FROM OFFICIAL DOCUMENTATION
    // http://sntnarciss.ru/irbis/spravka/wc01000000000.htm
    //
    // Регламентируется формат данных, возвращаемых функцией
    // режима пользователя. В общем случае это список строк
    // (т. е. данных, разделенных символами $0D0A).
    //
    // Конкретное содержание возвращаемых данных определяется
    // кодом возврата функции режима (целое число).
    //
    // Предлагаются следующие коды возврата:
    // отрицательное число – ненормальное завершение режима,
    // возвращаемые данные не учитываются;
    // 0 – нормальное завершение, никакие данные не возвращаются;
    // 1 – выполнена корректировка текущей записи, возвращаемые
    // данные представляют собой текущую запись(полностью)
    // в соответствии с форматом &unifor(‘+0’), а именно:
    //
    // 0
    // <mfn>#<статус записи>
    // 0#<версия записи>
    // <метка поля 1>#<значение поля 1>
    // <метка поля 2>#<значение поля 2>
    // ...
    // <метка поля N>#<значение поля N>
    //
    // При этом данные<mfn>, <статус записи> и <версия записи>
    // при возврате не учитываются.
    //

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class IrbisExtension
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        /// <summary>
        /// Decode the record.
        /// </summary>
        [NotNull]
        protected static MarcRecord DecodeRecord
            (
                [NotNull] string text
            )
        {
            Code.NotNullNorEmpty(text, "text");

            using (StringReader reader = new StringReader(text))
            {
                MarcRecord result = PlainText.ReadRecord(reader)
                    .ThrowIfNull("PlainText.ReadRecord");

                return result;
            }
        }

        /// <summary>
        /// Encode the record.
        /// </summary>
        [NotNull]
        protected static string EncodeRecord
            (
                [NotNull] MarcRecord record
            )
        {
            Code.NotNull(record, "record");

            string result = record.ToPlainText();

            return result;
        }

        /// <summary>
        /// Получение строки, ограниченной нулём,
        /// из буфера ИРБИС.
        /// </summary>
        protected static string GetUtfString
            (
                IntPtr buf
            )
        {
            List<byte> bytes = new List<byte>(32000);
            for (int offset = 0; ; offset++)
            {
                byte b = Marshal.ReadByte(buf, offset);
                if (b == 0)
                {
                    break;
                }
                bytes.Add(b);
            }

            byte[] array = bytes.ToArray();
            string result = IrbisEncoding.Utf8.GetString(array, 0, array.Length);

            return result;
        }

        /// <summary>
        /// Передача результата в ИРБИС.
        /// </summary>
        private static void SetUtfString
            (
                IntPtr buf,
                int bufferSize,
                [NotNull] string result
            )
        {
            byte[] temp = IrbisEncoding.Utf8.GetBytes(result);
            int length = Math.Min(temp.Length, bufferSize);
            Marshal.Copy(temp, 0, buf, length);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Entry point.
        /// </summary>
        public int EntryPoint
            (
                IntPtr inputBuffer,
                IntPtr outputBuffer,
                int outputSize
            )
        {
            string input = GetUtfString(inputBuffer);

            string result = HandleInput(input);

            if (ReferenceEquals(result, null))
            {
                return 0;
            }

            SetUtfString(outputBuffer, outputSize, result);

            return 1;
        }

        /// <summary>
        /// Handle the input.
        /// </summary>
        public abstract string HandleInput
            (
                [NotNull] string input
            );

        #endregion

        #region Object members

        #endregion
    }
}
