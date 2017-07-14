// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftHash.cs -- переход на новую строку
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Команда # выполняет те же действия, что и /,
    /// но переход на новую строку является безусловным.
    /// Можно использовать комбинацию /# для создания
    /// одной (и только одной) пустой строки.
    /// Комбинация ## может привести к созданию одной
    /// или двух пустых строк в зависимости от того,
    /// была ли пустой текущая строка перед выполнением
    /// первой команды #.
    /// Использование команды # может вызвать затруднение
    /// в тех случаях, когда выбираемое поле оказывается
    /// пустым. Эта ситуация хорошо иллюстрируется
    /// на следующем примере:
    /// 
    /// Пример:
    /// /#V10/#V20/#V30 ...
    /// 
    /// Если все поля присутствуют в документе,
    /// то в результате поля 10, 20, и 30 будут
    /// располагаться с начала строк и каждому будет
    /// предшествовать одна пустая строка.Однако,
    /// если поле 20 в документе отсутствует,
    /// то между 10 и 30 полями будут вставлены
    /// две пустые строки. Это может оказаться
    /// нежелательным, если действительно требуется,
    /// чтобы между полями была пропущена именно
    /// одна пустая строка, независимо от наличия
    /// или отсутствия некоторых полей.
    /// Таким образом, приведенный выше формат
    /// не приведет к желаемому результату.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftHash
        : PftNode
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHash()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHash
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.Hash);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!context._eatNextNewLine)
            {
                context.WriteLine(this);
            }
            context._eatNextNewLine = false;

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Write" />
        public override void Write
            (
                StreamWriter writer
            )
        {
            // Обрамляем пробелами
            writer.Write(" # ");
        }

        #endregion
    }
}
