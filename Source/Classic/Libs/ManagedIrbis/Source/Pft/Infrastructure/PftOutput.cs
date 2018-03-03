// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftParser.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Выходные потоки форматтера.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftOutput
    {
        #region Properties

        /// <summary>
        /// Родительский буфер. Может быть <c>null</c>.
        /// </summary>
        [CanBeNull]
        public PftOutput Parent { get { return _parent; } }

        /// <summary>
        /// Основной (обычный) поток.
        /// </summary>
        [NotNull]
        public TextBuffer Normal { get { return _normal; } }

        /// <summary>
        /// Поток предупреждений.
        /// </summary>
        [NotNull]
        public TextBuffer Warning { get { return _warning; } }

        /// <summary>
        /// Поток ошибок.
        /// </summary>
        [NotNull]
        public TextBuffer Error { get { return _error; } }

        /// <summary>
        /// Накопленный текст основного потока.
        /// </summary>
        [NotNull]
        public string Text { get { return Normal.ToString(); } }

        /// <summary>
        /// Накопленный текст потока предупреждений.
        /// </summary>
        [NotNull]
        public string WarningText { get { return Warning.ToString(); } }

        /// <summary>
        /// Накопленный текст потока ошибок.
        /// </summary>
        [NotNull]
        public string ErrorText { get { return Error.ToString(); } }

        /// <summary>
        /// Накоплен ли текст в основном потоке?
        /// </summary>
        public bool HaveText { get { return _HaveText(_normal); } }

        /// <summary>
        /// Были ли предупреждения?
        /// </summary>
        public bool HaveWarning { get { return _HaveText(_warning); } }

        /// <summary>
        /// Были ли ошибки?
        /// </summary>
        public bool HaveError { get { return _HaveText(_error); } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftOutput()
            : this(null)
        {
        }

        //=================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftOutput
            (
               [CanBeNull] PftOutput parent
            )
        {
            _parent = parent;
            _normal = new TextBuffer();
            _warning = new TextBuffer();
            _error = new TextBuffer();
        }

        #endregion

        #region Private members

        private readonly PftOutput _parent;

        private readonly TextBuffer _normal;
        private readonly TextBuffer _warning;
        private readonly TextBuffer _error;

        private static bool _HaveText
            (
               [NotNull] TextBuffer writer
            )
        {
            return writer.Length != 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка основного потока.
        /// </summary>
        [NotNull]
        public PftOutput ClearText()
        {
            _normal.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Очистака потока предупреждений.
        /// </summary>
        [NotNull]
        public PftOutput ClearWarning()
        {
            _warning.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Очистка потока ошибок.
        /// </summary>
        [NotNull]
        public PftOutput ClearError()
        {
            _error.Clear();

            return this;
        }

        //=================================================

        /// <summary>
        /// Получить (воображаемую) позицию курсора по горизонтали.
        /// </summary>
        public int GetCaretPosition()
        {
            return _normal.Column;
        }

        //=================================================

        /// <summary>
        /// Пустая ли последняя строка в основном буфере?
        /// </summary>
        public bool HaveEmptyLine()
        {
            bool result = _normal.Column == 1;

            return result;
        }

        //=================================================

        /// <summary>
        /// Предваряется явным переводом строки?
        /// </summary>
        public bool PrecededByEmptyLine()
        {
            bool result = _normal.PrecededByNewLine();

            return result;
        }

        //=================================================

        /// <summary>
        /// Временный переход к новому буферу.
        /// </summary>
        [NotNull]
        public PftOutput Push()
        {
            PftOutput result = new PftOutput(this);

            return result;
        }

        //=================================================

        /// <summary>
        /// Возврат к старому буферу с дописыванием
        /// в конец текста, накопленного в новом
        /// веременном буфере.
        /// </summary>
        [NotNull]
        public string Pop()
        {
            if (!ReferenceEquals(Parent, null))
            {
                string warningText = WarningText;
                if (!string.IsNullOrEmpty(warningText))
                {
                    Parent.Warning.Write(warningText);
                }

                string errorText = ErrorText;
                if (!string.IsNullOrEmpty(errorText))
                {
                    Parent.Error.Write(errorText);
                }
            }

            return ToString();
        }

        //=================================================

        /// <summary>
        /// Удалить последнюю строку в буфере, если она пустая.
        /// </summary>
        public PftOutput RemoveEmptyLines()
        {
            _normal.RemoveEmptyLines();

            return this;
        }

        /// <summary>
        /// Write text.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public PftOutput Write
            (
                [CanBeNull] string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.Write(format, arg);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write text.
        /// </summary>
        [NotNull]
        public PftOutput Write
            (
                [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.Write(value);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        [StringFormatMethod("format")]
        public PftOutput WriteLine
            (
                [CanBeNull] string format,
                params object[] arg
            )
        {
            if (!string.IsNullOrEmpty(format))
            {
                Normal.WriteLine(format, arg);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        public PftOutput WriteLine
            (
               [CanBeNull] string value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                Normal.WriteLine(value);
            }

            return this;
        }

        //=================================================

        /// <summary>
        /// Write line.
        /// </summary>
        [NotNull]
        public PftOutput WriteLine()
        {
            Normal.WriteLine();

            return this;
        }

        //=================================================

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Normal.ToString();
        }

        #endregion
    }
}
