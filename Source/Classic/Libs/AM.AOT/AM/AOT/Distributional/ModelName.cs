// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Model.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using JetBrains.Annotations;
using MoonSharp.Interpreter;

#endregion

namespace AM.AOT.Distributional
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]

    public sealed class ModelName
    {
        #region Constants

        /// <summary>
        /// Новостной корпус.
        /// </summary>
        /// <remarks><para>Модель обучена на потоке новостей с 1500
        /// преимущественно русскоязычных новостных сайтов
        /// (всего около 30 миллионов документов,
        /// с сентября 2013 до ноября 2016 включительно).
        /// Размер корпуса - почти 5 миллиардов слов.
        /// Модель знает 194 058 лемм; леммы, встретившиеся
        /// в корпусе менее 200 раз, при обучении игнорировались.</para>
        /// <para>Использован алгоритм Continuous Bag-of-Words.</para>
        /// <para>Размерность векторов 300, размер окна 2.</para>
        /// </remarks>
        public const string News = "news";

        /// <summary>
        /// Национальный Корпус Русского языка.
        /// </summary>
        /// <remarks><para>Обучена на полном НКРЯ. Размер корпуса - 250 миллионов слов.
        /// Модель знает 184 973 леммы; леммы, встретившиеся в корпусе менее 10 раз,
        /// при обучении игнорировались.</para>
        /// <para>Использован алгоритм Continuous Skipgram.</para>
        /// <para>Размерность векторов 300, размер окна 10.</para>
        /// </remarks>
        public const string RusCorpora = "ruscorpora";

        /// <summary>
        /// НКРЯ и дамп русскоязычной Википедии за ноябрь 2016.
        /// </summary>
        /// <remarks><para>Модель обучена на двух корпусах, слитых вместе.
        /// Размер корпуса - 600 миллионов слов. Модель знает 392 339 лемм;
        /// леммы, встретившиеся в корпусе менее 15 раз,
        /// при обучении игнорировались.</para>.
        /// <para>Использован алгоритм Continuous Bag-of-Words.</para>
        /// <para>Размерность векторов 300, размер окна 20.</para>
        /// </remarks>
        public const string RuWiki = "ruwikiruscorpora";

        /// <summary>
        /// Веб-корпус.
        /// </summary>
        /// <remarks><para>Модель обучена на случайном сэмпле из 9 миллионов
        /// русскоязычных веб-страниц, обкачанных в декабре 2014 года.
        /// Размер корпуса - 900 миллионов слов. Модель знает 267 540 лемм;
        /// леммы, встретившиеся в корпусе менее 30 раз,
        /// при обучении игнорировались.</para>
        /// <para>Использован алгоритм Continuous Bag-of-Words.</para>
        /// <para>Размерность векторов 300, размер окна 20.</para>
        /// </remarks>
        public const string Web = "web";

        #endregion
    }
}
