﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BbkRange.cs -- интервал индексов ББК
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;

using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Интервал индексов ББК вроде такого:
    /// 84.3/5
    /// </summary>
    [PublicAPI]
    public sealed class BbkRange
    {
        #region Properties

        /// <summary>
        /// Начальное значение индекса.
        /// </summary>
        [NotNull]
        public string FirstIndex { get; private set; }

        /// <summary>
        /// Оригинальное значение (со слешем).
        /// </summary>
        [NotNull]
        public string OriginalIndex { get; private set; }

        /// <summary>
        /// Конечное значение индекса.
        /// </summary>
        [NotNull]
        public string LastIndex { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BbkRange
            (
                [NotNull] string originalIndex
            )
        {
            Code.NotNullNorEmpty(originalIndex, "originalIndex");

            OriginalIndex = originalIndex;

            int slashPosition = OriginalIndex.IndexOf('/');
            if (slashPosition < 0)
            {
                FirstIndex = OriginalIndex;
                LastIndex = OriginalIndex;
                return;
            }
            if (slashPosition == 0)
            {
                Log.Error
                    (
                        "BbkRange::Constructor: "
                        + "index can't start with /: "
                        + originalIndex
                    );

                throw new BbkException
                    (
                        "Индекс не может начинаться со слеша"
                    );
            }
            if (OriginalIndex.LastIndexOf('/') != slashPosition)
            {
                Log.Error
                    (
                        "BbkRange::Constructor: "
                        + "more than one /: "
                        + originalIndex
                    );

                throw new BbkException("Индекс не может содержать "
                    + "больше одного слэша");
            }

            int totalLength = OriginalIndex.Length;
            int suffixLength = totalLength - slashPosition - 1;
            if (suffixLength == 0)
            {
                Log.Error
                    (
                        "BbkRange::Constructor: "
                        + "index can't end with /: "
                        + originalIndex
                    );

                throw new BbkException("Индекс не может "
                                      + "заканчиваться слэшом");
            }

            int prefixLenght = slashPosition - suffixLength;
            if (prefixLenght < 0)
            {
                Log.Error
                    (
                        "BbkRange::Constructor: "
                        + "prefix is shorter than suffix: "
                        + originalIndex
                    );

                throw new BbkException("Префикс короче суффикса!");
            }

            FirstIndex = OriginalIndex.Substring
                (
                    0,
                    slashPosition
                );
            LastIndex = OriginalIndex.Substring
                (
                    0,
                    prefixLenght
                )
                + OriginalIndex.Substring
                (
                    slashPosition + 1,
                    suffixLength
                );
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Получение списка всех индексов из диапазона.
        /// </summary>
        public string[] GetAllIndexes()
        {
            List<string> result = new List<string>();

            NumberText first = FirstIndex;
            NumberText last = LastIndex;
            NumberText current = first.Clone();
            while (current <= last)
            {
                result.Add(current.ToString());
                current = current.Increment();
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return OriginalIndex;
        }

        #endregion
    }
}
