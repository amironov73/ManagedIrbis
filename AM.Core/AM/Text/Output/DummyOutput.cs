/* DummyOutput.cs
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
    /// Выходной поток, который не даёт закрыться
    /// другому потоку.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class DummyOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Внутренний поток.
        /// </summary>
        [NotNull]
        public AbstractOutput Inner { get { return _inner; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DummyOutput
            (
                [NotNull] AbstractOutput inner
            )
        {
            if (ReferenceEquals(inner, null))
            {
                throw new ArgumentNullException("inner");
            }
            _inner = inner;
        }

        #endregion

        #region Private members

        private readonly AbstractOutput _inner;

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }
        public override AbstractOutput Clear()
        {
            Inner.Clear();
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            Inner.Configure(configuration);
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            Inner.Write(text);
            return this;
        }

        public override AbstractOutput WriteError
            (
                string text
            )
        {
            Inner.WriteError(text);
            return this;
        }

        #endregion
    }
}
