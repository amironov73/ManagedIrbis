/* IrbisNetworkUtilty.cs -- 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Network
{
    /// <summary>
    /// Вспомогательные методы для формирования сетевых
    /// пакетов и их парсинга.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public static class IrbisNetworkUtility
    {
        #region Public methods

        /// <summary>
        /// Записываем любой объект (диспетчеризация).
        /// </summary>
        [NotNull]
        public static Stream EncodeAny
            (
                [NotNull] this Stream stream,
                [CanBeNull] object anyObject
            )
        {
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

            if (ReferenceEquals(anyObject, null))
            {
                
            }
            else if (anyObject is bool)
            {
                return stream.EncodeBoolean((bool) anyObject);
            }
            else if (anyObject is byte)
            {
                stream.WriteByte((byte)anyObject);
                return stream;
            }
            else if (anyObject is byte[])
            {
                return stream.EncodeBytes((byte[]) anyObject);
            }
            else if (anyObject is int)
            {
                return stream.EncodeInt32((int) anyObject);
            }
            else if (anyObject is IrbisRecord)
            {
                return stream.EncodeRecord((IrbisRecord) anyObject);
            }
            else if (anyObject is string)
            {
                return stream.EncodeString((string) anyObject);
            }
            else if (anyObject is TextWithEncoding)
            {
                return stream.EncodeTextWithEncoding
                    (
                        (TextWithEncoding) anyObject
                    );
            }
            else
            {
                return stream.EncodeObject(anyObject);
            }

            return stream;

            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }

        /// <summary>
        /// Записываем булево значение в виде 0/1.
        /// </summary>
        [NotNull]
        public static Stream EncodeBoolean
            (
                [NotNull] this Stream stream,
                bool value
            )
        {
            byte b = (byte) (value ? '1' : '0');
            
            stream.WriteByte(b);

            return stream;
        }

        /// <summary>
        /// Записываем буфер.
        /// </summary>
        [NotNull]
        public static Stream EncodeBytes
            (
                [NotNull] this Stream stream,
                [CanBeNull] byte[] bytes
            )
        {
            if (!ReferenceEquals(bytes, null))
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            return stream;
        }

        /// <summary>
        /// Перевод строки.
        /// </summary>
        [NotNull]
        public static Stream EncodeDelimiter
            (
                [NotNull] this Stream stream
            )
        {
            stream.WriteByte((byte)IrbisClientQuery.Delimiter);

            return stream;
        }

        /// <summary>
        /// Записываем целое.
        /// </summary>
        [NotNull]
        public static Stream EncodeInt32
            (
                [NotNull] this Stream stream,
                int value
            )
        {
            string text = value.ToString(CultureInfo.InvariantCulture);
            byte[] bytes = IrbisEncoding.Ansi.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);

            return stream;
        }

        /// <summary>
        /// Записываем целое.
        /// </summary>
        [NotNull]
        public static Stream EncodeInt64
            (
                [NotNull] this Stream stream,
                long value
            )
        {
            string text = value.ToString(CultureInfo.InvariantCulture);
            byte[] bytes = IrbisEncoding.Ansi.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);

            return stream;
        }

        /// <summary>
        /// Записываем произвольные объект.
        /// ToString + кодировка ANSI.
        /// </summary>
        [NotNull]
        public static Stream EncodeObject
            (
                [NotNull] this Stream stream,
                [CanBeNull] object obj
            )
        {
            if (!ReferenceEquals(obj, null))
            {
                string text = obj.ToString();
                return stream.EncodeString(text);
            }

            return stream;
        }

        /// <summary>
        /// Запись в кодировке UTF.
        /// </summary>
        [NotNull]
        public static Stream EncodeRecord
            (
                [NotNull] this Stream stream,
                [CanBeNull] IrbisRecord record
            )
        {
            throw new NotImplementedException();

            //return stream;
        }

        /// <summary>
        /// Записываем строку в кодировке ANSI.
        /// </summary>
        [NotNull]
        public static Stream EncodeString
            (
                [NotNull] this Stream stream,
                [CanBeNull] string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                byte[] bytes = IrbisEncoding.Ansi.GetBytes(text);
                
                stream.Write(bytes, 0, bytes.Length);
            }
            
            return stream;
        }

        /// <summary>
        /// Записываем строку в произвольной кодировке.
        /// </summary>
        public static Stream EncodeTextWithEncoding
            (
                [NotNull] this Stream stream,
                [CanBeNull] TextWithEncoding text
            )
        {
            if (!ReferenceEquals(text, null))
            {
                byte[] bytes = text.ToBytes();

                stream.Write(bytes, 0, bytes.Length);
            }

            return stream;
        }

        /// <summary>
        /// Записываем код АРМ.
        /// </summary>
        [NotNull]
        public static Stream EncodeWorkstation
            (
                [NotNull] this Stream stream,
                IrbisWorkstation workstation
            )
        {
            stream.WriteByte((byte)workstation);

            return stream;
        }

        #endregion
    }
}
