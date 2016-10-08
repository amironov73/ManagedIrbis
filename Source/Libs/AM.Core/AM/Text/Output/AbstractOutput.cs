/* AbstractOutput.cs -- абстрактный объект текстового вывода
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace AM.Text.Output
{
    /// <summary>
    /// Абстрактный объект текстового вывода. 
    /// Например, консоль или текстовое окно.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class AbstractOutput
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        public abstract bool HaveError { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static AbstractOutput Null
        {
            get
            {
                if (ReferenceEquals(_null, null))
                {
                    _null = new NullOutput();
                }
                return _null;
            }
        }

#if !UAP

        /// <summary>
        /// 
        /// </summary>
        [NotNull]
        public static AbstractOutput Console
        {
            get
            {
                if (ReferenceEquals(_console, null))
                {
                    _console = new ConsoleOutput();
                }
                return _console;
            }
        }

#endif

        #endregion

        #region Private members

        private static AbstractOutput _null;

        private static AbstractOutput _console;

        #endregion

        #region Public methods

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <returns></returns>
        public abstract AbstractOutput Clear();

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public abstract AbstractOutput Configure
            (
                string configuration
            );

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Возвращает сам объект
        /// вывода.</returns>
        public abstract AbstractOutput Write
            (
                string text
            );

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract AbstractOutput WriteError
            (
                string text
            );

        /// <summary>
        /// 
        /// </summary>
        [StringFormatMethod("format")]
        public AbstractOutput Write
            (
                string format,
                params object[] args
            )
        {
            return Write 
                ( 
                    string.Format
                        (
                            format,
                            args
                        )
                );
        }

        /// <summary>
        /// 
        /// </summary>
        [StringFormatMethod("format")]
        public AbstractOutput WriteError
            (
                string format,
                params object[] args
            )
        {
            return WriteError
                (
                    string.Format
                        (
                            format,
                            args
                        )
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public AbstractOutput WriteLine
            (
                string text
            )
        {
            return Write
                (
                    text
                    + Environment.NewLine
                );
        }

        /// <summary>
        /// 
        /// </summary>
        [StringFormatMethod("format")]
        public AbstractOutput WriteLine
            (
                string format,
                params object[] args
            )
        {
            return Write 
                ( 
                    string.Format
                        (
                            format,
                            args
                        )
                    + Environment.NewLine
                );
        }

        /// <summary>
        /// 
        /// </summary>
        [StringFormatMethod("format")]
        public AbstractOutput WriteErrorLine
            (
                string format,
                params object[] args
            )
        {
            return WriteError
                (
                    string.Format
                        (
                            format,
                            args
                        )
                    + Environment.NewLine
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable"/>
        public virtual void Dispose()
        {
        }
        
        #endregion
    }
}
