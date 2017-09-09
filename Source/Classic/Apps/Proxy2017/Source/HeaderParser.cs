// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* HeaderParser.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Proxy2017
{
    /// <summary>
    /// Простой парсер заголовка запроса/ответа,
    /// разбирающий его на строки.
    /// </summary>
    class HeaderParser
    {
        private readonly byte[] _data;

        private int _offset;

        // ====================================================================

        public HeaderParser(byte[] data)
        {
            _data = data;
            _offset = 0;
        }

        // ====================================================================

        public string NextString()
        {
            int i;
            for (i = _offset; i < _data.Length; i++)
            {
                if (_data[i] == 0x0A)
                {
                    break;
                }
            }

            string result = Encoding.Default.GetString
                (
                    _data,
                    _offset,
                    i - _offset
                );

            _offset = i + 1;
            if (_offset > _data.Length)
            {
                _offset = _data.Length;
            }

            return result;
        }

        // ====================================================================

        public byte[] NextBytes()
        {
            int remainder = _data.Length - _offset;
            byte[] result = new byte[remainder];
            Array.Copy(_data, _offset, result, 0, remainder);
            return result;
        }

        // ====================================================================

        public string[] SplitLines
            (
                byte[] buffer,
                bool utf
            )
        {
            string[] result = null;
            try
            {
                Encoding encoding = utf
                                        ? Encoding.UTF8
                                        : Encoding.Default;
                string text = encoding.GetString(buffer);
                text = text.Replace("\r\n", "\r");
                result = text.Split(new[] { '\r', '\n' });
            }
            catch //(Exception ex)
            {
                // IrbisProxy.WriteException(ex);
            }

            return result;
        }
    }
}
