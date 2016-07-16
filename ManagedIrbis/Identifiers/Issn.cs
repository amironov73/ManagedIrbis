/* Issn.cs -- ISSN
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

using Newtonsoft.Json;

#endregion

namespace ManagedIrbis.Identifiers
{
    //
    // Международный стандартный серийный номер
    // (англ. International Standard Serial Number — ISSN)
    // — уникальный номер, позволяющий идентифицировать любое
    // периодическое издание независимо от того, где оно издано,
    // на каком языке, на каком носителе. Состоит из восьми цифр.
    // Восьмая цифра — контрольное число, рассчитываемое
    // по предыдущим семи и модулю 11.
    //
    // История создания
    // Стандарт ISO 3297, определяющий правила присвоения ISSN,
    // был введён в 1975 году. Управление процессом присвоения ISSN
    // осуществляется из 75 национальных центров. Их координацию
    // осуществляет Международный центр, расположенный в Париже,
    // при поддержке ЮНЕСКО и правительства Франции.
    //
    // Национальное агентство ISSN в Российской книжной палате
    // В 1989 году в СССР был введён ГОСТ 7.56-89, который
    // с 1 января 2003 года был заменён на ГОСТ 7.56-2002.
    // С 1 января 2016 года в составе РКП начало работу
    // Национальное агентство ISSN в России.
    // Национальное агентство ISSN было учреждено 3 декабря 2015 года,
    // когда между Международным центром ISSN и ИТАР-ТАСС было
    // подписано рабочее соглашение об основании Национального
    // центра ISSN Российской Федерации. В своей работе Национальное
    // агентство руководствуется Международным стандартом ISO 3297-2007,
    // ГОСТ 7.56-2002 и Федеральным законом об обязательном
    // экземпляре документов (77-ФЗ).
    // 
    // См. https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D0%B6%D0%B4%D1%83%D0%BD%D0%B0%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D1%81%D1%82%D0%B0%D0%BD%D0%B4%D0%B0%D1%80%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D0%B5%D1%80%D0%B8%D0%B9%D0%BD%D1%8B%D0%B9_%D0%BD%D0%BE%D0%BC%D0%B5%D1%80
    // https://en.wikipedia.org/wiki/International_Standard_Serial_Number
    //



    /// <summary>
    /// Всё, связанное с ISSN.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public static class Issn
    {
        #region Constants

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}
