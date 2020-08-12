// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PhoneUtility.cs -- утилиты для работы с номером телефона.
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

/*
 * https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D0%BB%D0%B5%D1%84%D0%BE%D0%BD%D0%BD%D1%8B%D0%B9_%D0%BD%D0%BE%D0%BC%D0%B5%D1%80
 *
 * Телефонный номер (или абонентский номер) — последовательность цифр
 * (реже также букв[1]), присвоенная пользователю или абоненту телефонной
 * сети, зная которую, можно ему позвонить. С технической точки зрения
 * телефонный номер — необходимое условие автоматической коммутации вызова,
 * которое определяет маршрут его прохождения и поиска телефонного
 * оборудования вызываемого пользователя для соединения (в рамках
 * сигнализации). Телефонный номер назначается обслуживающим персоналом
 * АТС или коммутатора, так чтобы каждый пользователь сети имел уникальную
 * идентификацию. При подключении к телефонной сети общего пользования
 * абонентский номер (идентификатор, ID) выделяется компанией-оператором
 * связи при заключении договора об оказании услуг телефонной связи.
 * В свою очередь, регулированием и распределением диапазонов (блоков)
 * номеров в глобальной телефонной сети общего пользования между компаниями,
 * также как и стандартизацией и общим контролем за услугами связи
 * занимаются соответствующие государственные и международные организации.
 *
 * В частности, существует рекомендация ITU-T под номером E.164, определяющая
 * общий международный телекоммуникационный план нумерации, используемый
 * в телефонных сетях общего пользования и некоторых других сетях.
 * Согласно E.164 номера могут иметь максимум 15 цифр и обычно записываются
 * с префиксом «+».
 *
 * https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D0%BB%D0%B5%D1%84%D0%BE%D0%BD%D0%BD%D1%8B%D0%B9_%D0%BF%D0%BB%D0%B0%D0%BD_%D0%BD%D1%83%D0%BC%D0%B5%D1%80%D0%B0%D1%86%D0%B8%D0%B8_%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D0%B8
 *
 * Телефонный план нумерации России — диапазоны телефонных номеров, выделяемых
 * различным пользователям телефонной сети общего пользования в России,
 * специальные номера и другие особенности набора для совершения телефонных
 * вызовов. Все международные номера пользователей данной телефонной сети
 * имеют общее начало +7 - называемый префиксом или телефонным кодом страны.
 *
 * В сети общего пользования России применяются номера с тем же префиксом,
 * что и в Казахстане и Абхазии. Между ними разделен международный код +7,
 * разница состоит в следующих значащих цифрах префикса.
 *
 * Национальные номера абонентов (то есть без префиксов, таких как международный
 * +7 или национальный 8) состоят из 10 цифр, в которые входят зоновый код
 * (3 цифры) и номер абонента (7 цифр).
 *
 */

namespace AM.Net
{
    /// <summary>
    /// Утилиты для работы с номером телефона.
    /// </summary>
    public static class PhoneUtility
    {
        #region Public methods

        /// <summary>
        /// Очистка номера от лишних символов.
        /// Перевод кириллических символов в латиницу.
        /// </summary>
        [NotNull]
        public static string CleanupNumber
            (
                [NotNull] string number
            )
        {
            Code.NotNullNorEmpty(number, "number");

            var result = new StringBuilder(number.Length);
            if (number.FirstChar() == '+')
            {
                result.Append('+');
            }

            if (number.FirstChar() == '8')
            {
                result.Append('+');
                result.Append('7');
                number = number.Substring(1);
            }

            foreach (var c in number)
            {
                if (c.IsArabicDigit())
                {
                    result.Append(c);
                }
            }

            if (result.Length > 10 && result[0] == '7')
            {
                result.Insert(0, '+');
            }

            return result.ToString();
        }

        /// <summary>
        /// Верификация (приблизительная) номера телефона.
        /// </summary>
        public static bool VerifyNumber
            (
                [NotNull] string number
            )
        {
            Code.NotNullNorEmpty(number, "number");

            var result = number.SafeStarts("8")
                         || number.SafeStarts("+7");

            return result;
        }

        #endregion
    }
}
