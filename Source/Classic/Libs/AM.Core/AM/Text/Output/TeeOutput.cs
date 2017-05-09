// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TeeOutput.cs -- расщепление (повтор) потока вывода
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Расщепление (повтор) потока вывода.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class TeeOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Подчинённые потоки
        /// </summary>
        public List<AbstractOutput> Output
        {
            get { return _output; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TeeOutput()
        {
            _output = new List<AbstractOutput>();
        }

        /// <summary>
        /// Создание объекта с заранее установленным
        /// списком.
        /// </summary>
        public TeeOutput
            (
                params AbstractOutput[] children
            )
        {
            _output = new List<AbstractOutput>
                (
                    children
                );
        }

        #endregion

        #region Private members

        private readonly List<AbstractOutput> _output;
        private bool _haveError;

        #endregion

        #region AbstractOutput members

        /// <summary>
        /// Есть ошибка?
        /// </summary>
        public override bool HaveError
        {
            get { return _haveError; }
            set
            {
                _haveError = value;
                foreach (AbstractOutput output in Output)
                {
                    output.HaveError = value;
                }
            }
        }

        /// <summary>
        /// Очистка.
        /// </summary>
        public override AbstractOutput Clear()
        {
            _haveError = false;
            foreach (AbstractOutput output in Output)
            {
                output.Clear();
            }
            return this;
        }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            foreach (AbstractOutput output in Output)
            {
                output.Configure(configuration);
            }
            return this;
        }

        /// <summary>
        /// Вывод.
        /// </summary>
        public override AbstractOutput Write
            (
                string text
            )
        {
            foreach (AbstractOutput output in Output)
            {
                output.Write(text);
            }
            return this;
        }

        /// <summary>
        /// Вывод.
        /// </summary>
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            _haveError = true;
            foreach (AbstractOutput output in Output)
            {
                output.WriteError(text);
            }
            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            foreach (AbstractOutput output in Output)
            {
                output.Dispose();
            }
            base.Dispose();
        }

        #endregion
    }
}
